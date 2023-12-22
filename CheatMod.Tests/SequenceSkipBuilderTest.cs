using CheatMod.Core.Utils;

namespace CheatMod.Tests;

public class SequenceSkipBuilderTest
{
    [Fact]
    public void SkipNextEdges_Should_Skip_Edges_And_Items_In_Between()
    {
        var list = Enumerable.Range(1, 6);
        var builder = new SequenceSkipBuilder<int>(list);

        builder.SkipNextEdges(start => start == 2, end => end == 4);
        var result = builder.Build();

        Assert.Equal(new List<int> { 1, 5, 6 }, result);
    }

    [Fact]
    public void SkipNextEdges_Should_Include_First_Last_Found_Value()
    {
        var list = Enumerable.Range(1, 10);
        var builder = new SequenceSkipBuilder<int>(list);

        builder.SkipNextEdges(start => start == 1, end => end == 10,
            RangeBoundaryOptions.IncludeStart | RangeBoundaryOptions.IncludeEnd);
        var result = builder.Build();

        Assert.Equal(new List<int> { 1, 10 }, result);
    }

    [Fact]
    public void SkipNextEdges_Without_Start_And_End_Conditions_Should_Throw_Exception()
    {
        var list = Enumerable.Range(1, 5);
        var builder = new SequenceSkipBuilder<int>(list);

        Assert.Throws<ArgumentException>(() => builder.SkipNextEdges(start: null, end: null));
    }

    [Fact]
    public void SkipNextEdges_With_Invalid_Conditions_Should_Throw_Exception()
    {
        var list = Enumerable.Range(1, 6);
        var builder = new SequenceSkipBuilder<int>(list);

        Assert.Throws<InvalidOperationException>(() => builder.SkipNextEdges(x => x == 10, x => x == 11));
    }

    [Fact]
    public void ReplaceNext_Should_Replace_Item()
    {
        var list = Enumerable.Range(1, 5);
        var builder = new SequenceSkipBuilder<int>(list);

        builder.ReplaceNext(x => x == 3, 100);
        var result = builder.Build();

        Assert.Equal(new List<int> { 1, 2, 100, 4, 5 }, result);
    }


    [Fact]
    public void ReplaceNext_NonExistent_Item_Should_Throw_Exception()
    {
        var list = Enumerable.Range(1, 5);
        var builder = new SequenceSkipBuilder<int>(list);

        Assert.Throws<InvalidOperationException>(() => builder.ReplaceNext(x => x == 10, 100));
    }

    [Fact]
    public void SkipFirst_Should_Skip_Items_Range_That_Meets_Condition()
    {
        var list = Enumerable.Range(1, 5);
        var builder = new SequenceSkipBuilder<int>(list);

        builder.SkipFirst(x => x is > 1 and < 4);
        var result = builder.Build();

        Assert.Equal(new List<int> { 1, 4, 5 }, result);
    }

    [Fact]
    public void SkipUntil_Should_Skip_Until_Item_Matched()
    {
        var list = Enumerable.Range(1, 10);
        var builder = new SequenceSkipBuilder<int>(list);

        builder.SkipUntil(x => x == 5);
        var result = builder.Build();

        Assert.Equal(new List<int> { 5, 6, 7, 8, 9, 10 }, result);
    }

    [Fact]
    public void SkipUntil_Not_Found_Item_Should_Skip_All()
    {
        var list = Enumerable.Range(1, 5);
        var builder = new SequenceSkipBuilder<int>(list);

        builder.SkipUntil(x => x == 10);
        var result = builder.Build();

        Assert.Empty(result);
    }

    [Fact]
    public void Mixing_Methods_Works_As_Expected()
    {
        var list = Enumerable.Range(1, 100);
        var builder = new SequenceSkipBuilder<int>(list);

        builder
            .ReplaceNext(item => item == 1, -999)
            .SkipNextEdges(start => start == 3, end => end == 7)
            .SkipSingle(item => item == 9)
            .ReplaceNext(item => item == 11, 999)
            .SkipNextEdges(start => start > 12, end => end <= 50)
            .SkipFirst(item => item is > 51 and <= 95)
            .ReplaceNext(item => item == 99, 101)
            .ReplaceNext(item => item == 100, 2137);
        var result = builder.Build();

        Assert.Equal(new List<int> { -999, 2, 8, 10, 999, 12, 51, 96, 97, 98, 101, 2137 }, result);
    }
}