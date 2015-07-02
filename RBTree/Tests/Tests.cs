using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RBTree;



namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();

            tree.put(1, 25);
            tree.put(2, 28);
            tree.put(3, 53);
            tree.put(4, 54);
            tree.put(100, 100);
            tree.put(200, 777);
        

            Assert.IsTrue(tree.min() == 1, "Tree min fail.");
            Assert.IsTrue(tree.max() == 200, "Tree max fail.");
            Assert.IsTrue(tree.contains(3), "Tree contains fail.");
            Assert.IsTrue(tree.size() == 6, "Tree size fail.");
            Assert.IsFalse(tree.isEmpty(), "Tree empty fail.");
            Assert.IsTrue(tree.get(2) == 28, "Tree get fail.");

            Assert.IsTrue(tree.floor(99) == 4, "Tree floor fail.");
            Assert.IsTrue(tree.ceiling(199) == 200, "Tree ceiling fail.");

            tree.deleteMin();
            tree.deleteMax();
            Assert.IsTrue(tree.min() != 1, "Tree delete min fail.");
            Assert.IsTrue(tree.max() != 4, "Tree delete max fail.");
        }
    }
}
