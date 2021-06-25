using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Algo.Problem
{
    public partial class Solution
    {
        #region 401 https://leetcode-cn.com/problems/binary-watch/
        public IList<string> ReadBinaryWatch(int t)
        {
            /*
             * 相当于求一个二进制数 最多10位 其中1的个数=t
             * 然后满足时间需求的情况下的所有结果
            */
            //TODO
            var res = new List<string>();
            void Dfs(int idx, int k)
            {

            }
            Dfs(0, t);

            return res;
        }
        #endregion

        #region 剑指 Offer 15 https://leetcode-cn.com/problems/er-jin-zhi-zhong-1de-ge-shu-lcof/
        public int HammingWeight(uint n)
        {
            var res = 0;
            while (n != 0)
            {
                n &= n - 1;
                res++;
            }
            return res;
        }
        #endregion
    }
}
