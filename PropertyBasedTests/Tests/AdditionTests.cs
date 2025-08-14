using FsCheck.Xunit;

namespace PropertyBasedTests.Tests;

public class AdditionTests
{
    private static int Add(int x, int y) => x + y;
    
    // Changing the order of the numbers doesn’t change the result.
    // a + b = b + a
    [Property]
    public bool Add_IsCommutative(int a, int b)
    {
        return Add(a, b) == Add(b, a);
    }
    
    // The grouping of numbers doesn’t affect the result
    // (a + b) + c = a + (b + c)
    [Property]
    public bool Add_IsAssociative(int a, int b, int c)
    {
        return Add(Add(a, b), c) == Add(a, Add(b, c));
    }
    
    // There’s a number that, when added to any other number, leaves it unchanged.
    // a + 0 = a
    [Property]
    public bool Add_HasIdentityElementZero(int a)
    {
        return Add(a, 0) == Add(0, a);
    }
}