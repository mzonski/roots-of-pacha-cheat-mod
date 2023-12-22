using System;
using System.Collections.Generic;

namespace CheatMod.Core.Utils
{
    [Flags]
    public enum RangeBoundaryOptions
    {
        IncludeStart = 0,
        IncludeEnd = 1,
    }

    public interface ISequenceSkipBuilder<T>
    {
        ISequenceSkipBuilder<T> SkipNextEdges(
            Func<T, bool> startItem, Func<T, bool> endItem,
            RangeBoundaryOptions? options = null, int repeat = 1);

        ISequenceSkipBuilder<T> SkipSingle(Func<T, bool> item);
        ISequenceSkipBuilder<T> SkipFirst(Func<T, bool> item);
        ISequenceSkipBuilder<T> ReplaceNext(Func<T, bool> item, T newElement);
        ISequenceSkipBuilder<T> SkipUntil(Func<T, bool> item);
        ISequenceSkipBuilder<T> MoveIterator(int index);
        IEnumerable<T> Build();
    }

    public class SequenceSkipBuilder<T> : ISequenceSkipBuilder<T>
    {
        private int Iterator { get; set; }
        private readonly List<T> _list;
        private readonly List<(int Head, int Tail)> _skipIndexes;
        private readonly Dictionary<int, T> _replacedIndexes;

        public SequenceSkipBuilder(IEnumerable<T> listItems)
        {
            Iterator = 0;
            _list = new List<T>(listItems);
            _skipIndexes = new List<(int, int)>();
            _replacedIndexes = new Dictionary<int, T>();
        }

        public ISequenceSkipBuilder<T> SkipNextEdges(
            Func<T, bool> start,
            Func<T, bool> end,
            RangeBoundaryOptions? options = null,
            int repeat = 1)
        {
            _ = start ?? end ?? throw new ArgumentException("Both start and end conditions cannot be null");
            if (repeat == 0) return this;

            var startIndex = int.MinValue;
            var endIndex = int.MinValue;

            for (var i = Iterator; i < _list.Count; i++)
            {
                if (startIndex == int.MinValue && start is not null && start(_list[i]))
                {
                    startIndex = i;
                }

                if ((start is null || startIndex != int.MinValue) && end is not null && end(_list[i]))
                {
                    endIndex = i;
                }
            }

            if (startIndex == int.MinValue || endIndex == int.MinValue)
                throw new InvalidOperationException($"Invalid skip conditions at pos [{Iterator}]");

            if (options.HasValue)
            {
                if (options.Value.HasFlag(RangeBoundaryOptions.IncludeStart))
                    startIndex++;

                if (options.Value.HasFlag(RangeBoundaryOptions.IncludeEnd))
                    endIndex--;
            }

            _skipIndexes.Add((startIndex, endIndex));
            Iterator = endIndex;

            if (repeat > 0)
                return SkipNextEdges(start, end, options, repeat - 1);

            return this;
        }

        public ISequenceSkipBuilder<T> SkipFirst(Func<T, bool> item)
        {
            _ = item ?? throw new ArgumentException("Both item condition cannot be null");

            int? start = null;
            int? end = null;
            for (var i = Iterator; i < _list.Count; i++)
            {
                if (item(_list[i]))
                {
                    start ??= i;
                    end = i;
                }
                else
                {
                    if (!start.HasValue)
                    {
                        Iterator++;
                        continue;
                    }

                    if (end.Value == start.Value)
                    {
                        _skipIndexes.Add((start.Value, start.Value));
                        Iterator = i;
                        break;
                    }

                    if (start.HasValue && end.HasValue)
                    {
                        _skipIndexes.Add((start.Value, end.Value));
                        Iterator = end.Value + 1;
                        break;
                    }
                }
            }

            return this;
        }


        public ISequenceSkipBuilder<T> SkipSingle(Func<T, bool> item)
        {
            for (var i = Iterator; i < _list.Count; i++)
            {
                if (!item(_list[i])) continue;
                _skipIndexes.Add((i, i));
                Iterator = i + 1;
                break;
            }

            return this;
        }

        public ISequenceSkipBuilder<T> SkipUntil(Func<T, bool> item)
        {
            var start = Iterator;
            int? end = _list.Count - 1;
            for (var i = Iterator; i < _list.Count; i++)
            {
                if (item(_list[i]))
                {
                    end = i - 1;
                    break;
                }

                Iterator++;
            }

            _skipIndexes.Add((start, end.Value));

            return this;
        }

        public ISequenceSkipBuilder<T> ReplaceNext(Func<T, bool> item, T newElement)
        {
            var foundIndex = -1;
            for (var i = Iterator; i < _list.Count; i++)
            {
                if (item(_list[i]))
                {
                    foundIndex = i;
                    Iterator++;
                    break;
                }

                Iterator++;
            }

            if (foundIndex == -1)
                throw new InvalidOperationException("Cannot find item in a list");

            _replacedIndexes[foundIndex] = newElement;

            return this;
        }

        public ISequenceSkipBuilder<T> MoveIterator(int index)
        {
            Iterator = index;
            return this;
        }

        public List<(int, int)> Debug()
        {
            return _skipIndexes;
        }

        public IEnumerable<T> Build()
        {
            var currentIndex = 0;
            var skipListIndex = 0;

            do
            {
                if (skipListIndex < _skipIndexes.Count &&
                    currentIndex >= _skipIndexes[skipListIndex].Head &&
                    currentIndex <= _skipIndexes[skipListIndex].Tail)
                {
                    currentIndex = _skipIndexes[skipListIndex].Tail + 1;
                    skipListIndex++;
                }
                else
                {
                    if (_replacedIndexes.TryGetValue(currentIndex, out var replacedValue))
                        yield return replacedValue;
                    else
                        yield return _list[currentIndex];
                    currentIndex++;
                }
            } while (currentIndex < _list.Count);
        }
    }
}