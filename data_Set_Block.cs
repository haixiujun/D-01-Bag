using System;
using System.Collections.Generic;
using System.Text;

namespace D_01_Bag
{
    class data_Set_Block
    {
        private int item_Count;
        private int bag_Cubage;
        private item_Set[] item_Sets;

        public data_Set_Block(int iC,int bC)
        {
            item_Count = iC;
            bag_Cubage = bC;
            item_Sets = new item_Set[item_Count];
        }

        public void init_Item_Sets(int[] profit,int[] weight)
        {
            for(int i = 0; i < item_Count; i++)
            {
                item_Sets[i].init_Profit(profit[i * 3], profit[i * 3 + 1], profit[i * 3 + 2]);
                item_Sets[i].init_Weight(weight[i * 3], weight[i * 3+1], weight[i * 3+2]);
            }
        }




    }
}
