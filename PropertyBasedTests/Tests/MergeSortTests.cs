using FsCheck.Xunit;
using MergeSort;

namespace PropertyBasedTests.Tests;

public class MergeSortTests()
{

    [Property]
    public bool MergeSortGcFriendly_SortsElementsCorrectly(int[] original)
    {
        var clone = new int[original.Length];
        original.CopyTo(clone, 0);
            
        MergeSortGcFriendly.Sort(clone);
        Array.Sort(original);
            
        return clone.SequenceEqual(original);
    }
        
    [Property]
    public bool MergeSortParallel_SortsElementsCorrectly(int[] original)
    {
        var clone = new int[original.Length];
        original.CopyTo(clone, 0);

        MergeSortParallel.Sort(clone);
        MergeSortGcFriendly.Sort(original);
            
        return clone.SequenceEqual(original);
    }
}