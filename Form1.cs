using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D_01_Bag
{
    public partial class Form1 : Form
    {
        //数据集文件路径
        private string data_Set_File_Path;      

        //中间存储字符串
        private string temp_Data;

        //处理后的文件行数
        private int file_Lines_Count;       
        
        //文件中数据集的数量
        private int group_Counts;            
        
        //当前文件中的全部数据集
        private List<data_Set_Block> data_Sets;     
        
        public Form1()
        {
            InitializeComponent();
            data_Set_File_Path = "";
            temp_Data = "";
            file_Lines_Count = 0;
            data_Sets = new List<data_Set_Block>();
        }

        //获取当前文件中第i个数据集
        public data_Set_Block get_Data_Block(int i)
        {
            return data_Sets[i];
        }

        //呼出文件选择窗口并将文件绝对路径进行存储
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            data_Set_File_Path = openFileDialog1.FileName;
            textBox1.Text = data_Set_File_Path;
            read_Data_Set();
            cut_Data_Set();
        }

        //从选中文件中读取源数据
        private void read_Data_Set()
        {
            //清空中间数据
            temp_Data = "";

            //读取文件按行读取
            String line = "";

            //文件行数计数器清零
            file_Lines_Count = 0;

            try
            {
                //创建流读取对象sr
                using (StreamReader sr = new StreamReader(data_Set_File_Path))
                {
                    //当文件读取完毕结束读取
                    while ((line = sr.ReadLine()) != null)
                    {
                        //如果是空行则不加到中间数据
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            temp_Data += line+"\n";
                            file_Lines_Count++;
                        } 
                    }
                    //去掉最后多出来的一个换行符
                    temp_Data = temp_Data.Substring(0, temp_Data.Length - 1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Read Error!");
                MessageBox.Show(e.Message);
            }
        }
           
        //在文件数据全部读取到中间数据字符串后写入中间数据文件再进行切割
        private void cut_Data_Set()
        {

            //清空数据集和列表1
            data_Sets.Clear();
            listBox1.Items.Clear();

            //文件行数要去掉第一行和最后一行的开始结束符
            file_Lines_Count -= 2;

            //去掉第一行
            int first_Line_End_Index = temp_Data.IndexOf('\n');
            temp_Data = temp_Data.Remove(0, first_Line_End_Index + 1);

            //去掉最后一行
            int last_Line_Start_Index = temp_Data.LastIndexOf('\n');
            temp_Data = temp_Data.Substring(0, last_Line_Start_Index);

            //写入中间数据文件
            string temp = "";
            StreamWriter sw = new StreamWriter("test.txt");
            sw.Write(temp_Data);
            sw.Close();

            //检测是否是6的倍数，每个数据集在文件中应该为6行
            if ( file_Lines_Count % 6 != 0)
            {
                MessageBox.Show("File Error!");
                return;
            }

            //通过文件行数来计算数据集的数量
            group_Counts = file_Lines_Count / 6;

            StreamReader sr = new StreamReader("test.txt");
            for (int i = 0; i < group_Counts; i++)
            {
                data_Set_Block temp_Set;
                //奇数行为提示信息，不需要进行处理，仅对偶数行进行处理
                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //提取d和c的字符串
                string[] blocks = temp.Split(",");
                string d_Str = blocks[0].Split("*")[1];
                string c_Str = blocks[1].Split(" ").Last();

                //去掉结尾的字符
                c_Str = c_Str.Substring(0, c_Str.Length - 1);

                //将d和c的字符串转为整型
                int temp_d = Convert.ToInt32(d_Str);
                int temp_c = Convert.ToInt32(c_Str);

                //初始化当前数据集
                temp_Set = new data_Set_Block(temp_d, temp_c );

                //读取profit行的字符串
                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //切割profit行字符串
                temp = temp.Substring(0, temp.Length - 1);
                string[] profit_Array_Str = temp.Split(",");

                //读取weight行的字符串
                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //切割weight行字符串
                temp = temp.Substring(0, temp.Length - 1);
                string[] weight_Array_Str = temp.Split(",");

                //初始化profit和weight数组
                int[] profit_Array = new int[profit_Array_Str.Length];
                int[] weight_Array = new int[weight_Array_Str.Length];
                for (int j = 0; j < profit_Array_Str.Length; j++)
                {
                    //对应转换
                    profit_Array[j] = Convert.ToInt32(profit_Array_Str[j]);
                    weight_Array[j] = Convert.ToInt32(weight_Array_Str[j]);
                }

                //初始化数据集的profit和weight数组
                temp_Set.init_Item_Sets(profit_Array, weight_Array);
                //加入数据集列表中
                data_Sets.Add(temp_Set);
                //在listbox中添加当前数据集选项
                listBox1.Items.Add(openFileDialog1.SafeFileName + "-" + i.ToString());
            }
            sr.Close();
            //删除中间文件
            File.Delete("test.txt");
            
        }

        //动态规划法求解问题
        private void button2_Click(object sender, EventArgs e)
        {
            //获取当前选中下标
            int index = listBox1.SelectedIndex;
            //获取当前时间
            DateTime dt = DateTime.Now;
            //启动动态规划法求解
            data_Sets[index].find_Max_Result_Dynamic_Programming();
            //获取结束时间
            DateTime dtt = DateTime.Now;
            //求使用时间
            textBox3.Text = Convert.ToString((dtt - dt));
            //获取动态规划法最终结果
            textBox2.Text = data_Sets[index].get_Dynamic_Result().ToString();
            //显示结果
            richTextBox1.Text = data_Sets[index].get_Dynamic_Result_Str();
        }

        //回溯法求解问题
        private void button3_Click(object sender, EventArgs e)
        {
            //获取当前选中下标
            int index = listBox1.SelectedIndex;
            //获取当前时间
            DateTime dt = DateTime.Now;
            //启动回溯法求解
            data_Sets[index].find_Max_Result_Recall();
            //获取j结束时间
            DateTime dtt = DateTime.Now;
            //求解使用时间
            textBox3.Text = Convert.ToString((dtt - dt));
            //显示结果
            textBox2.Text = data_Sets[index].get_Recall_Result().ToString();
            int count = data_Sets[index].get_Selected_Array().Length;
            //清空richtextBox
            richTextBox1.Text = "";
            //获取结果数组
            int[] selected_Array = data_Sets[index].get_Selected_Array();
            for(int i = 0; i < count; i++)
            {
                //显示结果
                richTextBox1.Text += selected_Array[i].ToString() + "→";
            }
            //去掉最后一个箭头
            richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.Text.Length - 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this, listBox1.SelectedIndex);
            form2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            third_Sort third = new third_Sort(data_Sets[listBox1.SelectedIndex]);
            third.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            out_Put_Data out_Obj = new out_Put_Data(data_Sets[listBox1.SelectedIndex]);
            folderBrowserDialog1.ShowDialog();
            out_Obj.out_To_Txt(folderBrowserDialog1.SelectedPath + "//" + openFileDialog1.SafeFileName.Split(".")[0] + "_result"+listBox1.SelectedIndex+".txt");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            out_Put_Data out_Obj = new out_Put_Data(data_Sets[listBox1.SelectedIndex]);
            folderBrowserDialog1.ShowDialog();
            out_Obj.out_To_Excel(folderBrowserDialog1.SelectedPath + "//" + openFileDialog1.SafeFileName.Split(".")[0] + "_result" + listBox1.SelectedIndex+".xlsx");
        }
    }
}
