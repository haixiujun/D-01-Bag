using System;
using System.Collections.Generic;
using System.Text;

namespace D_01_Bag
{
    public class item_Set
    {
        private int[] profit;
        private int[] weight;
        private int[] selected;
        public item_Set()
        {
            profit = new int[3];
            weight = new int[3];
            selected = new int[3];
        }
        public void init_Profit(int p1,int p2,int p3)
        {
            profit[0] = p1;
            profit[1] = p2;
            profit[2] = p3;
        }

        public void init_Weight(int w1, int w2, int w3)
        {
            weight[0] = w1;
            weight[1] = w2;
            weight[2] = w3;
        }

        public int get_Profit(int index)
        {
            return profit[index];
        }

        public int get_Weight(int index)
        {
            return weight[index];
        }

        public void set_Selected(int i)
        {
            selected = new int[3];
            selected[i] = 1;
        }
        public int get_Selected()
        {
            for (int i = 0; i < 3; i++)
            {
                if (selected[i] == 1)
                {
                    return i;
                }
            }
            return -1;
        }
            
    }
}
