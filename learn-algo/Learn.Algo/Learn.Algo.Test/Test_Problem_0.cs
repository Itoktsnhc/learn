using Learn.Algo.Problem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Learn.Algo.Test
{
    [TestClass]
    public class Test_Problem_0
    {
        private readonly Solution _sln = new Solution();

        [TestMethod]
        public void TestHammingWeight()
        {
            var res = _sln.HammingWeight(3);
            Assert.AreEqual(res, 2);
        }
    }
}
