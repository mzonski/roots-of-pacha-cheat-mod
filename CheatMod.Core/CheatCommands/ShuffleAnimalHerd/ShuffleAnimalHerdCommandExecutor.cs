using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CheatMod.Core.Extensions;
using CheatMod.Core.Managers;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.CheatCommands.ShuffleAnimalHerd;

public class ShuffleAnimalHerdCommandExecutor : CheatCommandExecutor<ShuffleAnimalHerdCommand>
{
    public ShuffleAnimalHerdCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private void DespawnAnimalFromHerd(StateChange state, AnimalHerd herd, Rarity? desiredRarity)
    {
        var animals = desiredRarity.HasValue
            ? herd.AnimalsOfThisHerd.Where(a => desiredRarity.Value != a.Variant.Rarity).ToList()
            : herd.AnimalsOfThisHerd.Where(a => !a.Variant.IsEpicOrLegendary).ToList();

        AnimalEntity animal;
        if (animals.Count == 0)
        {
            Manager.Logger.Log("No detected animals under condition, removing random");
            animal = herd.AnimalsOfThisHerd.GetRandomElement();
        }
        else
        {
            animal = animals.GetRandomElement();
        }

        Manager.Logger.Log(
            $"Removing animal {animal.NameOrSpeciesName} {animal.Variant.Name} {Enum.GetName(typeof(Sex), animal.Sex)}");

        var iD = animal.ID;
        animal.Entity.RemoveAndDestroy().Forget();
        state.EntitiesToDestroy.Add(iD);
    }


    private void AppendRareAnimalData(StateChange state, string herdId, AnimalHerd herd, Rarity? desiredRarity,
        Sex? desiredSex, bool? isAdult)
    {
        var variantIndex = 0;
        if (desiredRarity.HasValue)
        {
            var variant = herd.Animal.Variants.Where(v => v.Rarity == desiredRarity.Value).ToList();
            if (variant.Count > 0)
            {
                var selectedVariant = variant.GetRandomElement();
                variantIndex = herd.Animal.Variants.IndexOf(selectedVariant);
                Manager.Logger.Log($"Picked variant: {selectedVariant.Name}");
            }
        }
        else
        {
            var rareVariants = herd.Animal.Variants
                .Where(v => v.Rarity is Rarity.Epic or Rarity.Rare or Rarity.Legendary).ToList();
            if (rareVariants.Count > 0)
            {
                var randomVariant = rareVariants.GetRandomElement();
                variantIndex = herd.Animal.Variants.IndexOf(randomVariant);
                Manager.Logger.Log($"Random variant {randomVariant.Name}");
            }
        }

        var sex = desiredSex ?? new[] { Sex.Male, Sex.Female }.GetRandomElement();

        var animalStats = new AnimalStats
        {
            Quality = 4,
            Production = 4,
            Speed = 4,
            Generation = 1
        };

        var player = GameObject.FindObjectOfType<PlayerEntity>();
        var birthDate = !isAdult.HasValue || isAdult.Value
            ? Mathf.Min(0, player.CurrentDay - (herd.Animal.AdultAge + 1))
            : player.CurrentDay;

        var newAnimalToHerd = new AnimalData
        {
            ID = Guid.NewGuid().ToBase64String(),
            Animal = herd.Animal,
            Sex = sex,
            HerdId = herdId,
            Birthdate = (short)birthDate,
            VariantIndex = (byte)variantIndex,
            ProductionProgress = 1000,
            Stats = animalStats
        };

        Manager.Logger.Log(
            $"Spawning animal {herd.Animal.Name} {herd.Animal.Variants[variantIndex].Name} {Enum.GetName(typeof(Sex), sex)}");

        state.Entities.Add(newAnimalToHerd);
    }

    private void ReplaceAnimalWithRare(StateChange state, KeyValuePair<string, AnimalHerd> herd, Rarity? desiredRarity,
        Sex? desiredSex, bool? isAdult)
    {
        DespawnAnimalFromHerd(state, herd.Value, desiredRarity);
        AppendRareAnimalData(state, herd.Key, herd.Value, desiredRarity, desiredSex, isAdult);
    }

    public override void Execute(ShuffleAnimalHerdCommand command)
    {
        try
        {
            Manager.Logger.Log("Replacing animals");

            var animalHerds = new Dictionary<string, AnimalHerd>();
            var playerCoords = CommandHelper.GetPlayerCurrentCoords();

            foreach (var entityData in Game.Current.Entities)
            {
                if (entityData.Type != EntityType.AnimalHerd) continue;
                var animalHerd = GuidManager.ResolveGuid(entityData.ID)?.GetComponent<AnimalHerd>();

                if (animalHerd?.AnimalsOfThisHerd == null) continue;

                var animals = animalHerd.AnimalsOfThisHerd.ToList();
                foreach (var animal in animals)
                {
                    var distanceFromPlayer = Vector2.Distance(playerCoords, animal.transform.position);
                    if (!(distanceFromPlayer < command.Range)) continue;
                    animalHerds.Add(entityData.ID, animalHerd);
                    break;
                }
            }

            Manager.Logger.Log($"[Replace] Found {animalHerds.Count} herds in range");

            foreach (var herd in animalHerds)
            {
                var updateHerdDataMethod =
                    herd.Value.GetType().GetMethod("UpdateHerdData", BindingFlags.NonPublic | BindingFlags.Instance);

                var state = new StateChange();

                ReplaceAnimalWithRare(state, herd, command.Rarity, command.Sex, command.IsAdult);

                Network.RaiseEventLocalAndToOthers(new CreateOrUpdateEntitiesEvent(state.Entities));

                if (state.Entities.Count > 0)
                    Network.RaiseEventLocalAndToOthers(new CreateOrUpdateEntitiesEvent(state.Entities));

                updateHerdDataMethod!.Invoke(herd.Value, null);

                foreach (var animalEntity in herd.Value.AnimalsOfThisHerd)
                    animalEntity.OnNewDay();

                Manager.Logger.Log($"Herd: {herd.Value.Animal.Name} reinitialized");
            }
        }
        catch (Exception ex)
        {
            Manager.Logger.Log("[Spawn animals] Failed: " + ex.Message);
            Manager.Logger.Log(ex.StackTrace);
        }
    }
}