using System;
using System.Collections.Generic;
using System.Text;

namespace D_01_Bag
{
    public class item_Set
    {
        //价值数组
        private int[] profit;
        //重量数组
        private int[] weight;
        //第三组数据的价值重量比
        private double radio;
        //三个物品的选择情况
        private int[] selected;
        public item_Set()
        {
            profit = new int[3];
            weight = new int[3];
            selected = new int[3];
            radio = 0.0;
        }

        //初始化价值重量比
        public void init_Radio()
        {
            radio = (profit[2] * 1.0) / (weight[2]);
        }

        //获取第三个物品的价值重量比
        public double get_Radio()
        {
            return radio;
        }

        //初始化价值数组
        public void init_Profit(int p1,int p2,int p3)
        {
            profit[0] = p1;
            profit[1] = p2;
            profit[2] = p3;
        }

        //初始化重量数组
        public void init_Weight(int w1, int w2, int w3)
        {
            weight[0] = w1;
            weight[1] = w2;
            weight[2] = w3;
        }

        //获取第i个物品价值
        public int get_Profit(int index)
        {
            return profit[index];
        }

        //获取第i个物品重量
        public int get_Weight(int index)
        {
            return weight[index];
        }

        //设置第i个物品被选中
        public void set_Selected(int i)
        {
            selected = new int[3];
            selected[i] = 1;
        }

        //获取选中情况
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
