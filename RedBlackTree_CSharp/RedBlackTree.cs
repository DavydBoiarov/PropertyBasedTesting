using System;
using System.Collections.Generic;

public enum NodeColor
{
    Red,
    Black
}

public class Node<T> where T : IComparable<T>
{
    public T Value { get; set; }
    public NodeColor Color { get; set; }
    public Node<T> Parent { get; set; }
    public Node<T> Left { get; set; }
    public Node<T> Right { get; set; }

    public Node(T value)
    {
        Value = value;
        Color = NodeColor.Red; // New nodes are always red initially
        Parent = null;
        Left = null;
        Right = null;
    }

    // Helper to check if a node is NIL (null)
    public bool IsNil()
    {
        return this == null;
    }
}

public class RedBlackTree<T> where T : IComparable<T>
{
    private Node<T> _root;
    private readonly Node<T> _nil; // Sentinel node for leaves

    public RedBlackTree()
    {
        // Initialize a sentinel NIL node. All 'null' references will effectively point to this,
        // making checks for null and color consistent.
        _nil = new Node<T>(default(T)) { Color = NodeColor.Black }; // Default value for T, color is Black
        _root = _nil; // Initially, the root is NIL
    }

    public Node<T> Root
    {
        get { return _root; }
    }

    /// <summary>
    /// Inserts a new value into the Red-Black Tree.
    /// </summary>
    /// <param name="value">The value to insert.</param>
    public void Add(T value)
    {
        Node<T> z = new Node<T>(value); // New nodes are red by default
        z.Left = _nil; // New nodes always have NIL children
        z.Right = _nil;

        Node<T> y = _nil;
        Node<T> x = _root;

        // Traverse the tree to find the correct insertion point
        while (x != _nil)
        {
            y = x;
            if (z.Value.CompareTo(x.Value) < 0)
            {
                x = x.Left;
            }
            else
            {
                x = x.Right;
            }
        }

        z.Parent = y; // Set the parent of the new node

        // Insert the new node as a child of y
        if (y == _nil)
        {
            _root = z; // Tree was empty, z is the new root
        }
        else if (z.Value.CompareTo(y.Value) < 0)
        {
            y.Left = z;
        }
        else
        {
            y.Right = z;
        }

        // The new node is red, potentially violating Red-Black properties.
        // Call fixup to restore properties.
        InsertFixup(z);
    }

    /// <summary>
    /// Performs a left rotation on the given node x.
    /// </summary>
    /// <param name="x">The node to rotate around.</param>
    private void RotateLeft(Node<T> x)
    {
        Node<T> y = x.Right; // y becomes x's right child
        x.Right = y.Left;   // y's left subtree becomes x's right subtree

        if (y.Left != _nil)
        {
            y.Left.Parent = x;
        }

        y.Parent = x.Parent; // y's parent becomes x's parent

        if (x.Parent == _nil)
        {
            _root = y; // y is the new root
        }
        else if (x == x.Parent.Left)
        {
            x.Parent.Left = y;
        }
        else
        {
            x.Parent.Right = y;
        }

        y.Left = x; // x becomes y's left child
        x.Parent = y;
    }

    /// <summary>
    /// Performs a right rotation on the given node x.
    /// </summary>
    /// <param name="x">The node to rotate around.</param>
    private void RotateRight(Node<T> x)
    {
        Node<T> y = x.Left; // y becomes x's left child
        x.Left = y.Right;   // y's right subtree becomes x's left subtree

        if (y.Right != _nil)
        {
            y.Right.Parent = x;
        }

        y.Parent = x.Parent; // y's parent becomes x's parent

        if (x.Parent == _nil)
        {
            _root = y; // y is the new root
        }
        else if (x == x.Parent.Right)
        {
            x.Parent.Right = y;
        }
        else
        {
            x.Parent.Left = y;
        }

        y.Right = x; // x becomes y's right child
        x.Parent = y;
    }

    /// <summary>
    /// Restores Red-Black Tree properties after an insertion.
    /// </summary>
    /// <param name="z">The newly inserted node (which is red).</param>
    private void InsertFixup(Node<T> z)
    {
        // Loop while z's parent is red (violates property 4: no two consecutive red nodes)
        while (z.Parent.Color == NodeColor.Red)
        {
            if (z.Parent == z.Parent.Parent.Left) // Case 1: z's parent is a left child
            {
                Node<T> y = z.Parent.Parent.Right; // y is z's uncle

                if (y.Color == NodeColor.Red) // Case 1.1: Uncle is red (recoloring)
                {
                    z.Parent.Color = NodeColor.Black;
                    y.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    z = z.Parent.Parent; // Move z up the tree
                }
                else // Case 1.2: Uncle is black (rotation required)
                {
                    if (z == z.Parent.Right) // Case 1.2.1: z is a right child (left rotation)
                    {
                        z = z.Parent;
                        RotateLeft(z);
                    }
                    // Case 1.2.2: z is a left child (right rotation + recoloring)
                    z.Parent.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    RotateRight(z.Parent.Parent);
                }
            }
            else // Case 2: z's parent is a right child (symmetric to Case 1)
            {
                Node<T> y = z.Parent.Parent.Left; // y is z's uncle

                if (y.Color == NodeColor.Red) // Case 2.1: Uncle is red (recoloring)
                {
                    z.Parent.Color = NodeColor.Black;
                    y.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    z = z.Parent.Parent; // Move z up the tree
                }
                else // Case 2.2: Uncle is black (rotation required)
                {
                    if (z == z.Parent.Left) // Case 2.2.1: z is a left child (right rotation)
                    {
                        z = z.Parent;
                        RotateRight(z);
                    }
                    // Case 2.2.2: z is a right child (left rotation + recoloring)
                    z.Parent.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    RotateLeft(z.Parent.Parent);
                }
            }
        }

        _root.Color = NodeColor.Black; // Ensure the root is always black
    }

    /// <summary>
    /// Performs an in-order traversal of the tree (for testing/display).
    /// </summary>
    public void InOrderTraversal()
    {
        Console.Write("In-Order Traversal: ");
        InOrderTraversal(_root);
        Console.WriteLine();
    }

    private void InOrderTraversal(Node<T> node)
    {
        if (node != _nil)
        {
            InOrderTraversal(node.Left);
            Console.Write($"{node.Value}({node.Color}) ");
            InOrderTraversal(node.Right);
        }
    }

    /// <summary>
    /// Searches for a value in the tree.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>The node if found, otherwise null.</returns>
    public Node<T> Search(T value)
    {
        Node<T> current = _root;
        while (current != _nil)
        {
            int comparison = value.CompareTo(current.Value);
            if (comparison == 0)
            {
                return current;
            }
            else if (comparison < 0)
            {
                current = current.Left;
            }
            else
            {
                current = current.Right;
            }
        }
        return null; // Not found
    }
}