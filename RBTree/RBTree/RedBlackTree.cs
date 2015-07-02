using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBTree
{
    /// <summary>
    ///     A Left Leaning Red Black Tree implementation based upon that by
    ///     Robert Sedgewick of Princeton University.
    /// </summary>
    /// <typeparam name="Key">The search key used by the tree's Nodes</typeparam>
    /// <typeparam name="Value">The value stored in the tree's Nodes</typeparam>
    public class RedBlackTree<Key, Value> 
        where Key: IComparable<Key>
    {
        private const bool RED = true;
        private const bool BLACK = false;

        private Node root; //the root of the tree

        /// <summary>
        ///     Class representing the Nodes of the tree.
        /// </summary>
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

            return default(Value);            //If never found, return null
        }

        /// <summary>
        ///     Method to see if the tree contains a given item.
        /// </summary>
        /// <param name="key">The sought object.</param>
        /// <returns>True if the object is in the tree; false otherwise.</returns>
        public bool contains(Key key)
        {
            return !(get(key).Equals(default(Value)));
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
        
        /// <summary>
        ///     Public function to delete the node in the RB tree with the smallest key
        /// </summary>
        public void deleteMin()
        {
            if (isEmpty())
                throw new InvalidOperationException("Tree Underflow");

            //If the root's children are both black, temporarily set root to red
            if (!isRed(root.Left) && !isRed(root.Right))
                root.Color = RED;

            //call to actually perform deletion
            root = deleteMin(root);

            //restore root to black
            if (!isEmpty())
                root.Color = BLACK;
        }

        /// <summary>
        ///     Delete the node with the minimum key based on a given root.
        /// </summary>
        /// <param name="x">The given root</param>
        /// <returns>The new root for this subtree</returns>
        private Node deleteMin(Node x)
        {
            //If there is nothing smaller than the given root node,
            //then it is the smallest key and should be deleted
            if (x.Left == null)
                return null;

            if (!isRed(x.Left) && !isRed(x.Left.Left))
                x = moveRedLeft(x);

            //recursive call to perform deletion
            x.Left = deleteMin(x.Left);

            return balance(x);
        }

        /// <summary>
        ///     Public function to delete the node in the RB tree with the largest key
        /// </summary>
        public void deleteMax()
        {
            if (isEmpty())
                throw new InvalidOperationException("Tree Underflow");

            if(!isRed(root.Left) && !isRed(root.Right))
            {
                root.Color = RED;
            }

            root = deleteMax(root);

            if (!isEmpty())
                root.Color = BLACK;
        }

        /// <summary>
        ///     Delete the node with the maximum key based on a given root.
        /// </summary>
        /// <param name="x">The given root</param>
        /// <returns>The new root for this subtree</returns>
        private Node deleteMax(Node x)
        {
            if (isRed(x.Left))
                x = rotateRight(x);

            if (x.Right == null)
                return null;

            if (!isRed(x.Right) && !isRed(x.Right.Left))
                x = moveRedRight(x);

            x.Right = deleteMax(x.Right);

            return balance(x);
        }
        
        /// <summary>
        ///     Delete the key from the red black tree.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        private void delete(Key key)
        {
            if (!contains(key))
            {
                throw new ArgumentException("The key is not within the search tree.");
            }

            if(!isRed(root.Left) && !isRed(root.Right))
            {
                root.Color = RED;
            }

            root = delete(root, key);

            if (!isEmpty())
                root.Color = BLACK;
        }

        /// <summary>
        ///     Delete the node with the given key from the subtree.
        /// </summary>
        /// <param name="h">The root of the given subtree</param>
        /// <param name="key">The key to delete</param>
        /// <returns>The new root of the subtree</returns>
        private Node delete(Node h, Key key)
        {
            if (key.CompareTo(h.Key) < 0)
            {
                if (!isRed(h.Left) && !isRed(h.Left.Left))
                    h = moveRedLeft(h);

                h.Left = delete(h.Left, key);
            }
            else
            {
                if(isRed(h.Left))
                    h = rotateRight(h);
                if(key.CompareTo(h.Key) ==  0 && h.Right == null)
                    return null;
                if (!isRed(h.Right) && !isRed(h.Right.Left))
                    h = moveRedRight(h);
                if (key.CompareTo(h.Key) == 0)
                {
                    Node x = min(h.Right);
                    h.Key = x.Key;
                    h.Val = x.Val;
                    h.Right = deleteMin(h.Right);
                }
                else
                    h.Right = delete(h.Right, key);
            }

            return balance(h);
        }

        #endregion

        #region RedBlackTreeHelperMethods
            
        /// <summary>
        ///     Rotate the subtree at h to the right
        ///     (AVL Tree style)
        /// </summary>
        /// <param name="h">The root of this subtree</param>
        /// <returns>The new root to the subtree</returns>
        private Node rotateRight(Node h)
        {
            Node x = h.Left;
            h.Left = x.Right;
            x.Right = h;

            x.Color = x.Right.Color;
            x.Right.Color = RED;

            x.N = h.N;
            h.N = size(h.Left) + size(h.Right) + 1;

            return x;
        }

        /// <summary>
        ///     Rotate the subtree at h to the left
        ///     (AVL Tree style)
        /// </summary>
        /// <param name="h">The root of this subtree</param>
        /// <returns>The new root to the subtree</returns>
        private Node rotateLeft(Node h)
        {
            Node x = h.Right;
            h.Right = x.Left;
            x.Left = h;

            x.Color = x.Left.Color;
            x.Left.Color = RED;

            x.N = h.N;
            h.N = size(h.Left) + size(h.Right) + 1;

            return x;
        }

        /// <summary>
        ///     Flip the colors of a root, h, and its two children.
        ///     Precondition: h and its children must not be null and
        ///     h must have opposite color of its two children.
        /// </summary>
        /// <param name="h">The root of this subtree</param>
        private void flipColors(Node h)
        {
            h.Color = !h.Color;
            h.Left.Color = !h.Left.Color;
            h.Right.Color = !h.Right.Color;
        }

        /// <summary>
        ///     Make h.left or one of its children red.
        ///     Precondition: h is red and h.left and h.left.left are black
        /// </summary>
        /// <param name="h">The root of the subtree</param>
        /// <returns>The new root of the subtree</returns>
        private Node moveRedLeft(Node h)
        {
            flipColors(h);

            if (isRed(h.Right.Left))
            {
                h.Right = rotateRight(h.Right);
                h = rotateLeft(h);
                flipColors(h);
            }

            return h;
        }

        /// <summary>
        ///     Make h.right or one of its children black.
        ///     Precondition: h is red and both h.right and h.right.left are black.
        /// </summary>
        /// <param name="h">The root of the subtree</param>
        /// <returns>The new root of the subtree</returns>
        private Node moveRedRight(Node h)
        {
            flipColors(h);

            if (isRed(h.Left.Left))
            {
                h = rotateRight(h);
                flipColors(h);
            }

            return h;
        }

        /// <summary>
        ///     Restore red-black tree balance
        /// </summary>
        /// <param name="h">The root of this subtree</param>
        /// <returns>The new root of the subtree</returns>
        private Node balance(Node h)
        {
            if (isRed(h.Right))
                h = rotateLeft(h);
            if (isRed(h.Left) && isRed(h.Left.Left))
                h = rotateRight(h);
            if (isRed(h.Left) && isRed(h.Right))
                flipColors(h);

            h.N = size(h.Left) + size(h.Right) + 1;

            return h;
        }
        #endregion

        #region UtilityMethods
        /// <summary>
        ///     Find the height of the entire tree.
        /// </summary>
        /// <returns>The height of the tree.</returns>
        public int height() { return height(root); }

        /// <summary>
        ///     Find the height of the given subtree.
        /// </summary>
        /// <param name="x">The root of this subtree.</param>
        /// <returns>The height of the subtree or -1 if the root is null.</returns>
        private int height(Node x)
        {
            if (x == null)
                return -1;
            else
                return 1 + Math.Max(height(x.Left), height(x.Right));
        }
        #endregion

        #region OrderedSymbolTableMethods

        /// <summary>
        ///     Find the smallest Key within the tree.
        /// </summary>
        /// <returns>The smallest Key in this tree or Key's default value otherwise.</returns>
        public Key min()
        {
            if (isEmpty()) 
                return default(Key);
            else
                return min(root).Key;
        }

        /// <summary>
        ///     Find the Node with the smallest Key in the given subtree.
        /// </summary>
        /// <param name="x">The root of the subtree</param>
        /// <returns>The Node with the smallest Key in the subtree.</returns>
        private Node min(Node x)
        {
            if (x.Left == null) 
                return x;
            else 
                return min(x.Left);
        }

        /// <summary>
        ///     Find the largest Key in the tree.
        /// </summary>
        /// <returns>The largest key in this tree.</returns>
        public Key max()
        {
            if (isEmpty()) 
                return default(Key);
            else    
                return max(root).Key;
        }

        /// <summary>
        ///     Find the Node with the largest Key in the given subtree.
        /// </summary>
        /// <param name="x">The root of the subtree.</param>
        /// <returns>The Node with the largest Key in the subtree.</returns>
        private Node max(Node x)
        {
            if (x.Right == null) 
                return x;
            else    
                return max(x.Right);
        }

        /// <summary>
        ///     Find the largest key less than or equal to the given key
        /// </summary>
        /// <param name="key">The key used for comparisons</param>
        /// <returns>The largest key less than or equal to the given key</returns>
        public Key floor(Key key)
        {
            Node x = floor(root, key);
            if (x == null)
                return default(Key);
            else
                return x.Key;
        }

        /// <summary>
        ///     The Node whose Key is the largest Key which is less than or equal
        ///     to the given key in the given subtree.
        /// </summary>
        /// <param name="x">The Node of the subtree.</param>
        /// <param name="key">The key to be compared to.</param>
        /// <returns>The Node with the largest key less than or equal to key.</returns>
        private Node floor(Node x, Key key)
        {
            if (x == null)
                return null;

            int cmp = key.CompareTo(x.Key);

            if (cmp == 0)
                return x;
            else if (cmp < 0)
                return floor(x.Left, key);
            else
            {
                //This preserves the fact that the returned value is always
                //<= the key given
                Node t = floor(x.Right, key);
                if (t != null)
                    return t;
                else
                    return x;
            }
        }

        /// <summary>
        ///     Find the largest key less than or equal to the given key
        /// </summary>
        /// <param name="key">The key used for comparisons</param>
        /// <returns>The largest key less than or equal to the given key</returns>
        public Key ceiling(Key key)
        {
            Node x = ceiling(root, key);
            if (x == null)
                return default(Key);
            else
                return x.Key;
        }

        /// <summary>
        ///     The Node whose Key is the smallest Key which is greater than or equal
        ///     to the given key in the given subtree.
        /// </summary>
        /// <param name="x">The Node of the subtree.</param>
        /// <param name="key">The key to be compared to.</param>
        /// <returns>The Node with the smallest key greater than or equal to key.</returns>
        private Node ceiling(Node x, Key key)
        {
            if (x == null)
                return null;

            int cmp = key.CompareTo(x.Key);

            if (cmp == 0)
                return x;
            else if (cmp > 0)
                return ceiling(x.Right, key);
            else
            {
                //This preserves the fact that the returned value is always
                //>= the key given
                Node t = ceiling(x.Left, key);
                if (t != null)
                    return t;
                else
                    return x;
            }
        }

        /// <summary>
        ///     Find the Node of rank k in the tree. 
        ///     i.e. Its index in the sorted list of Nodes in the tree.
        /// </summary>
        /// <param name="k">The rank to find.</param>
        /// <returns>The key to the Node with the given rank.</returns>
        public Key select(int k)
        {
            if (k < 0 || k >= size())
                return default(Key);
            else
            {
                Node x = select(root, k);
                return x.Key;
            }
        }

        /// <summary>
        ///     Find the node of rank k in the given subtree.
        /// </summary>
        /// <param name="x">The root of the subtree.</param>
        /// <param name="k">The rank to find.</param>
        /// <returns>The node with the given rank.</returns>
        private Node select(Node x, int k)
        {
            int t = size(x.Left);

            //if the left subtree of the tree has more elements than k, then
            //the key is within this part of the tree
            if (t > k)
                return select(x.Left, k);
            //if there are not enough elements in the left side, then it must be
            //in the right side of the tree. The rank must be adjusted for this.
            else if (t < k)
                return select(x.Right, k - t - 1);
            else //we are at that rank already
                return x;
        }

        /// <summary>
        ///     Find the number of Keys less than the provided key.
        /// </summary>
        /// <param name="key">The provided key.</param>
        /// <returns>The number of Keys less than key.</returns>
        public int rank(Key key)
        {
            return rank(key, root);
        }

        /// <summary>
        ///     Find the number of Keys less than the provided key in the given
        ///     subtree.
        /// </summary>
        /// <param name="key">The provided key.</param>
        /// <param name="x">The root of the given subtree.</param>
        /// <returns>The number of Keys less than key in this subtree.</returns>
        private int rank(Key key, Node x)
        {
            if (x == null) return 0;

            int cmp = key.CompareTo(x.Key);

            if (cmp < 0)
                return rank(key, x.Left);
            else if (cmp > 0)
                return 1 + size(x.Left) + rank(key, x.Right);
            else
                return size(x.Left);
        }

        #endregion

        #region RangeCountAndSearchMethods

        /// <summary>
        ///     Retrieve an enumerable list of all keys in the tree.
        /// </summary>
        /// <returns>An enumerable list of all keys in this tree.</returns>
        public IEnumerable<Key> keys()
        {
            return keys(min(), max());
        }

        /// <summary>
        ///     Get an enumerable list of all keys between lo and hi in the tree.
        /// </summary>
        /// <param name="lo">The lower bound of keys to add to the list.</param>
        /// <param name="hi">The upper bound of keys to add to the list.</param>
        /// <returns>An enumerable list of all keys between lo and hi in the tree.</returns>
        private IEnumerable<Key> keys(Key lo, Key hi)
        {
            Queue<Key> queue = new Queue<Key>();
            keys(root, queue, lo, hi);
            return queue;
        }

        /// <summary>
        ///     Fill the provided enumerable list with all keys
        ///     in the given subtree between two points.
        /// </summary>
        /// <param name="x">The root of the given subtree.</param>
        /// <param name="queue">The provided enumerable list.</param>
        /// <param name="lo">The lower bound of keys to add to the list.</param>
        /// <param name="hi">The upper bound of keys to add to the list.</param>
        private void keys(Node x, Queue<Key> queue, Key lo, Key hi)
        {
            if (x == null)
                return;

            int cmplo = lo.CompareTo(x.Key);
            int cmphi = hi.CompareTo(x.Key);

            //if lo is lower than this Node's key, add keys to the left
            if (cmplo < 0)
                keys(x.Left, queue, lo, hi);
            //if this key is within range, add it
            if (cmplo <= 0 && cmphi >= 0)
                queue.Enqueue(x.Key);
            //if hi is higher than this Node's key, add keys to the right
            if (cmphi > 0)
                keys(x.Right, queue, lo, hi);
        }

        /// <summary>
        ///     Find the number of keys in the tree between lo and hi.
        /// </summary>
        /// <param name="lo">The lower bound of keys to count.</param>
        /// <param name="hi">The upper bound of keys to count.</param>
        /// <returns>The number of keys in the tree between lo and hi.</returns>
        public int size(Key lo, Key hi)
        {
            if (lo.CompareTo(hi) > 0) 
                return 0;
            if (contains(hi)) 
                return rank(hi) - rank(lo) + 1;
            else 
                return rank(hi) - rank(lo);
        }

        #endregion

        #region IntegrityCheckMethods
            //TODO: Implement Integrity Check Methods
        #endregion
    }
}
