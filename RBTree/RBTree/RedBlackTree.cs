using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBTree
{
    class RedBlackTree<Key, Value> 
        where Key: IComparable<Key> 
        where Value: Nullable
    {
        private static const bool RED = true;
        private static const bool BLACK = false;

        private Node root; //the root of the tree

        //Node helper class
        private class Node
        {
            private Key key;            //Key

            public Key Key
            {
                get { return key; }
                set { key = value; }
            }

            private Value val;          //Data in Node

            public Value Val
            {
                get { return val; }
                set { val = value; }
            }

            private Node left, right;   //links to left and right subtrees

            public Node Right
            {
                get { return right; }
                set { right = value; }
            }

            public Node Left
            {
                get { return left; }
                set { left = value; }
            }
            private bool color;         //Color of Node (Red or Black)

            public bool Color
            {
                get { return color; }
                set { color = value; }
            }
            private int n;              //Subtree count

            public int N
            {
                get { return n; }
                set { n = value; }
            }

            public Node(Key key, Value val, bool color, int n)
            {
                this.key = key;
                this.val = val;
                this.color = color;
                this.n = n;
            }


        }


        #region NodeHelperMethods
        /// <summary>
        ///     Returns whether or not the node is red
        /// </summary>
        /// <param name="x">A Node in the Red-Black Tree</param>
        /// <returns>True if the node is red; false if it is black or null</returns>
        private bool isRed(Node x)
        {
            if (x == null) return false;
            return (x.Color == RED);
        }

        /// <summary>
        ///     The number of nodes in subtree rooted at x
        /// </summary>
        /// <param name="x">A node to act as the root</param>
        /// <returns>The number of subtrees starting at x; 0 if null</returns>
        private int size(Node x)
        {
            if (x == null) return 0;
            return x.N;
        }
        #endregion

        #region SizeMethods

        /// <summary>
        ///     Gets the number of key-value pairs in the tree
        /// </summary>
        /// <returns>The number of nodes in the entire tree beginning at root</returns>
        public int size()
        {
            return size(root);
        }

        /// <summary>
        ///     Is this tree empty?
        /// </summary>
        /// <returns>True if the tree is empty; false otherwise</returns>
        public bool isEmpty()
        {
            return root == null;
        }
        #endregion

        #region StandardBSTSearch

        /// <summary>
        ///     The public function for searching the tree.
        /// </summary>
        /// <param name="key">The thing being searched for</param>
        /// <returns>The sought object if it is in the tree, null otherwise</returns>
        public Value get(Key key) { return get(root, key); }

        /// <summary>
        ///     Standard Binary Search Tree search method implemented with recursion
        /// </summary>
        /// <param name="x">The node the search is at</param>
        /// <param name="key">The value being searched for</param>
        /// <returns></returns>
        private Value get(Node x, Key key)
        {
            while(x != null)
            {
                int cmp = key.CompareTo(x.Key);

                if (cmp < 0)        //when less then, go to the left
                    x = x.Left;
                else if (cmp > 0)   //when greater than, go to the right
                    x = x.Right;
                else 
                    return x.Val;   //If neither, then you found it
            }

            return null;            //If never found, return null
        }

        /// <summary>
        ///     Method to see if the tree contains a given item.
        /// </summary>
        /// <param name="key">The sought object.</param>
        /// <returns>True if the object is in the tree; false otherwise.</returns>
        public bool contains(Key key)
        {
            return get(key) != null;
        }

        #endregion

        #region RedBlackInsertion

        /// <summary>
        ///     Insert the key-value pair; overwrite the old value with the new
        ///     if the key is already in the tree.
        /// </summary>
        /// <param name="key">The key being inserted.</param>
        /// <param name="val">The value being inserted.</param>
        public void put(Key key, Value val)
        {
            root = put(root, key, val);
            root.Color = BLACK;
        }

        /// <summary>
        ///     Insert the key-value pair into the subtree with root x.
        /// </summary>
        /// <param name="x">The root of the subtree.</param>
        /// <param name="key">The key being inserted.</param>
        /// <param name="val">The value being inserted.</param>
        /// <returns>The new root of the subtree.</returns>
        private Node put(Node x, Key key, Value val)
        {
            //create a new node if at a leaf
            if (x == null) return new Node(key, val, RED, 1);

            //use recursion to figure out where the item needs to go
            //this is basically a BST search
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
                x.Left = put(x.Left, key, val);
            else if (cmp > 0)
                x.Right = put(x.Right, key, val);
            else
                x.Val = val;

            //Adjust the tree to fix right-leaning subtrees
            if (isRed(x.Right) && !isRed(x.Left))
                x = rotateLeft(x);

            if (isRed(x.Left) && isRed(x.Left.Left))
                x = rotateRight(x);

            if (isRed(x.Left) && isRed(x.Right))
                flipColors(x);

            //calculate new size of node
            x.N = size(x.Left) + size(x.Right) + 1;

            return x;
        }

        #endregion

        #region RedBlackDeletion
        //TODO: Implement the deletion methods
        #endregion

        #region RedBlackTreeHelperMethods
            //TODO: Implement the helper methods
        #endregion
    }
}
