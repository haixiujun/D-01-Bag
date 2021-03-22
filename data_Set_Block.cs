using System;
using System.Collections.Generic;
using System.Text;

namespace D_01_Bag
{
    public class data_Set_Block
    {
        private int item_Count;
        private int bag_Cubage;
        private int best_Result;
        private item_Set[] item_Sets;
        private int[] selected_Items;
        private int[] temp_Selected;
        private int[,] dynamic_Result_Array;
        public data_Set_Block(int iC,int bC)
        {
            item_Count = iC;
            bag_Cubage = bC;
            item_Sets = new item_Set[item_Count];
            selected_Items = new int[item_Count];
            temp_Selected = new int[item_Count];
            best_Result = 0;
        }

        public int get_Item_Count()
        {
            return item_Count;
        }

        public item_Set get_Item(int i)
        {
            return item_Sets[i];
        }

        public int get_Dynamic_Result()
        {
            return dynamic_Result_Array[item_Count, bag_Cubage];
        }

        public int get_Recall_Result()
        {
            return best_Result;
        }

        public void init_Item_Sets(int[] profit,int[] weight)
        {
            for(int i = 0; i < item_Count; i++)
            {
                item_Sets[i] = new item_Set();
                item_Sets[i].init_Profit(profit[i * 3], profit[i * 3 + 1], profit[i * 3 + 2]);
                item_Sets[i].init_Weight(weight[i * 3], weight[i * 3+1], weight[i * 3+2]);
            }
        }

        public void find_Max_Result_Dynamic_Programming()
        {
            dynamic_Result_Array = new int[item_Count + 1, bag_Cubage + 1];
            int num1 = 0;
            int num2 = 0;
            for (int row = 1; row < item_Count + 1; row ++)
            {

                for(int col = 1; col < bag_Cubage + 1; col++)
                {
                    for(int i = 0; i < 3; i++)
                    {
                        if (col >= item_Sets[row-1].get_Weight(i))
                        {
                            num1 = dynamic_Result_Array[row - 1, col];
                            num2 = dynamic_Result_Array[row - 1, col - item_Sets[row - 1].get_Weight(i)] + item_Sets[row - 1].get_Profit(i);
                            num1 = Math.Max(num1, num2);
                            dynamic_Result_Array[row,col] = Math.Max(num1, dynamic_Result_Array[row, col]);
                        }
                        else
                        {
                            dynamic_Result_Array[row, col] = Math.Max(dynamic_Result_Array[row, col],dynamic_Result_Array[row-1, col]);
                        }
                    }  
                }
            }

        }

        public void find_Max_Result_Recall()
        {
            for(int i = 0; i < item_Count; i++)
            {
                selected_Items[i] = -1;
            }
            back_Trace(0, 0, 0);
        }

        private void back_Trace(int group_Id,int profit_Now,int weight_Now)
        {
            if (group_Id == item_Count)
            {
                if (profit_Now > best_Result)
                {
                    for(int i = 0; i < item_Count; i++)
                    {
                        selected_Items[i] = temp_Selected[i];
                    }
                    best_Result = profit_Now;
                }
                return;
            }
            else
            {
                
                for(int i = 0; i < 3; i++)
                {
                    temp_Selected[group_Id] = i;
                    if (weight_Now+ item_Sets[group_Id].get_Weight(i) <= bag_Cubage)
                    {
                        back_Trace(group_Id + 1, profit_Now + item_Sets[group_Id].get_Profit(i), weight_Now + item_Sets[group_Id].get_Weight(i));
                    }
                }
                temp_Selected[group_Id] = -1;
                back_Trace(group_Id + 1, profit_Now, weight_Now);
            }
        }
        

    }
}
