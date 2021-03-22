using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace D_01_Bag
{
    public partial class third_Sort : Form
    {
        private data_Set_Block data_Set;
        public third_Sort(data_Set_Block data)
        {
            data_Set = data;
            InitializeComponent();
        }

        private void third_Sort_Shown(object sender, EventArgs e)
        {
            int count = data_Set.get_Item_Count();
            List<item_Set> list = new List<item_Set>();
            for(int i = 0; i < count; i++)
            {
                list.Add(data_Set.get_Item(i));
            }
            list.Sort(new item_Sort());
            foreach(item_Set i in list)
            {
                dataGridView1.Rows.Add(i.get_Profit(2), i.get_Weight(2), i.get_Radio());
            }
        }

        public class item_Sort : IComparer<item_Set>
        {
            public int Compare([AllowNull] item_Set x, [AllowNull] item_Set y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return 1;
                }

                if (y == null)
                {
                    return -1;
                }
                double num1 = x.get_Radio();
                double num2 = y.get_Radio();
                if (num1 > num2)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}
