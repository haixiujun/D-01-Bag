using System;
using System.Collections.Generic;
using System.Text;

namespace D_01_Bag
{
    public class data_Set_Block
    {
        //程序运行时间
        private double process_Time;
        //数据集中的数据组数
        private int item_Count;
        //背包容量
        private int bag_Cubage; 
        //最优解
        private int best_Result;
        //数据组集合
        private item_Set[] item_Sets;
        //回溯法选择情况集
        private int[] selected_Items_Recall;
        //选择情况——最终结果,即返回的结果
        private int[] temp_Selected;
        //动态规划法的选择情况集合
        private int[] selected_Items_Dynamic;
        //动态规划法的数组
        private int[,] dynamic_Result_Array;

        //初始化数据集
        public data_Set_Block(int iC,int bC)
        {
            item_Count = iC;
            bag_Cubage = bC;
            item_Sets = new item_Set[item_Count];
            selected_Items_Recall = new int[item_Count];
            selected_Items_Dynamic = new int[item_Count];
            temp_Selected = new int[item_Count];
            best_Result = 0;
        }

        //获取选择结果
        public int[] get_Selected_Array()
        {
            return temp_Selected;
        }

        //获取程序运行时间
        public double get_Process_Time()
        {
            return process_Time;
        }

        //动态规划法选择情况克隆到最终结果
        private void clone_To_Selected_Dy()
        {
            for(int i = 0; i < item_Count; i++)
            {
                temp_Selected[i] = selected_Items_Dynamic[i];
            }
        }

        //回溯法选择情况克隆到最终结果
        private void clone_To_Selected_Rc()
        {
            for(int i = 0; i < item_Count; i++)
            {
                temp_Selected[i] = selected_Items_Recall[i];
            }
        }

        //获取当前数据集共有多少组数据
        public int get_Item_Count()
        {
            return item_Count;
        }

        //获取当前的第i组数据
        public item_Set get_Item(int i)
        {
            return item_Sets[i];
        }

        //获取动态规划法的最优结果
        public int get_Dynamic_Result()
        {
            return dynamic_Result_Array[item_Count, bag_Cubage];
        }

        //获取回溯法的最优结果
        public int get_Recall_Result()
        {
            return best_Result;
        }

        //初始化价值和重量数组
        public void init_Item_Sets(int[] profit,int[] weight)
        {
            for(int i = 0; i < item_Count; i++)
            {
                item_Sets[i] = new item_Set();
                item_Sets[i].init_Profit(profit[i * 3], profit[i * 3 + 1], profit[i * 3 + 2]);
                item_Sets[i].init_Weight(weight[i * 3], weight[i * 3+1], weight[i * 3+2]);
                item_Sets[i].init_Radio();
            }
        }

        //动态规划法获取最优数据
        public void find_Max_Result_Dynamic_Programming()
        {
            DateTime start_Time = DateTime.Now;
            //首先初始化结果数组
            dynamic_Result_Array = new int[item_Count + 1, bag_Cubage + 1];
            //存储几个选择的各自结果
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
                            //当前格子选取最大的
                            dynamic_Result_Array[row,col] = Math.Max(num1, dynamic_Result_Array[row, col]);
                        }
                        else
                        {
                            dynamic_Result_Array[row, col] = Math.Max(dynamic_Result_Array[row, col],dynamic_Result_Array[row-1, col]);
                        }
                    }  
                }
            }
            //通过矩阵获得选择情况
            get_Result_Dynamic();
            //将选择情况克隆
            clone_To_Selected_Dy();
            best_Result = get_Dynamic_Result();
            DateTime end_Time = DateTime.Now;
            process_Time = (end_Time - start_Time).TotalSeconds;

        }

        //回溯法求解最大结果
        public void find_Max_Result_Recall()
        {
            DateTime start_Time = DateTime.Now;
            //初始化选择数组
            for (int i = 0; i < item_Count; i++)
            {
                selected_Items_Recall[i] = -1;
            }
            //开始进行回溯
            back_Trace(0, 0, 0);
            //将选择情况克隆
            clone_To_Selected_Rc();
            DateTime end_Time = DateTime.Now;
            process_Time = (end_Time - start_Time).TotalSeconds;
        }

        //递归函数
        private void back_Trace(int group_Id,int profit_Now,int weight_Now)
        {
            //递归出口是当前深度到达最深
            if (group_Id == item_Count)
            {
                //当前结果大于当前最优
                if (profit_Now > best_Result)
                {
                    for(int i = 0; i < item_Count; i++)
                    {
                        selected_Items_Recall[i] = temp_Selected[i];
                    }
                    best_Result = profit_Now;
                }
                return;
            }
            else
            {
                //在当前组分别进行下一步
                for(int i = 0; i < 3; i++)
                {
                    temp_Selected[group_Id] = i;
                    if (weight_Now+ item_Sets[group_Id].get_Weight(i) <= bag_Cubage)
                    {
                        back_Trace(group_Id + 1, profit_Now + item_Sets[group_Id].get_Profit(i), weight_Now + item_Sets[group_Id].get_Weight(i));
                    }
                }
                //不选择当前组进行下一步
                temp_Selected[group_Id] = -1;
                back_Trace(group_Id + 1, profit_Now, weight_Now);
            }
        }

        //求解选择情况
        private void get_Result_Dynamic()
        {
            int col = bag_Cubage;
            for(int i = item_Count; i > 0&&col>0; i--)
            {
                //当前价值等于背包容量-1时
                if(dynamic_Result_Array[i,col]== dynamic_Result_Array[i, col-1])
                {
                    col--;
                    i++;
                }
                //当前价值等于当前物品组不选并且同背包容量时
                else if(dynamic_Result_Array[i, col] == dynamic_Result_Array[i - 1, col])
                {
                    selected_Items_Dynamic[i-1] = -1;
                }
                else
                {
                    //计算当前物品组选择的情况
                    for (int j = 0; j < 3; j++)
                    {
                        int temp_Weight = item_Sets[i-1].get_Weight(j);
                        int temp_Profit = item_Sets[i-1].get_Profit(j);
                        //选中当前物品组的第i个数据时
                        if ((dynamic_Result_Array[i, col] - temp_Profit) == dynamic_Result_Array[i - 1, col - temp_Weight])
                        {
                            col -= temp_Weight;
                            selected_Items_Dynamic[i-1] = j+1;
                            j = 4;
                        }
                    }
                }
            }

        }

        //获取动态规划法的结果（路径）字符串
        public string get_Dynamic_Result_Str()
        {
            string temp = "";
            for(int i = 0; i < item_Count-1; i++)
            {
                temp += (selected_Items_Dynamic[i].ToString()+"→");
            }
            temp += selected_Items_Dynamic[item_Count - 1];
            return temp;
        }
    }
}
