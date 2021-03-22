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
        private string data_Set_File_Path;
        private string temp_Data;
        private int file_Lines_Count;
        private int group_Counts;
        private List<data_Set_Block> data_Sets; 
        public Form1()
        {
            InitializeComponent();
            data_Set_File_Path = "";
            temp_Data = "";
            file_Lines_Count = 0;
            data_Sets = new List<data_Set_Block>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            data_Set_File_Path = openFileDialog1.FileName;
            textBox1.Text = data_Set_File_Path;
            read_Data_Set();
            cut_Data_Set();
        }

        private void read_Data_Set()
        {
            temp_Data = "";
            String line = "";
            file_Lines_Count = 0;
            try
            {
                using (StreamReader sr = new StreamReader(data_Set_File_Path))
                {
                    
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            temp_Data += line+"\n";
                            file_Lines_Count++;
                        } 
                    }
                    temp_Data = temp_Data.Substring(0, temp_Data.Length - 1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Read Error!");
                MessageBox.Show(e.Message);
            }
        }
           
        private void cut_Data_Set()
        {
            file_Lines_Count -= 2;
            MessageBox.Show(file_Lines_Count.ToString());
            int first_Line_End_Index = temp_Data.IndexOf('\n');
            temp_Data = temp_Data.Remove(0, first_Line_End_Index + 1);
            int last_Line_Start_Index = temp_Data.LastIndexOf('\n');
            temp_Data = temp_Data.Substring(0, last_Line_Start_Index);

            string temp = "";
            StreamWriter sw = new StreamWriter("test.txt");

            sw.Write(temp_Data);
            sw.Close();
            
            if ( file_Lines_Count % 6 != 0)
            {
                MessageBox.Show("File Error!");
                return;
            }
            group_Counts = file_Lines_Count / 6;
            StreamReader sr = new StreamReader("test.txt");
            for (int i = 0; i < group_Counts; i++)
            {
                data_Set_Block temp_Set;
                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //处理d和c
                string[] blocks = temp.Split(",");
                string d_Str = blocks[0].Split("*")[1];
                string c_Str = blocks[1].Split(" ").Last();

                c_Str = c_Str.Substring(0, c_Str.Length - 1);

                int temp_d = Convert.ToInt32(d_Str);
                int temp_c = Convert.ToInt32(c_Str);
                temp_Set = new data_Set_Block(temp_d, temp_c );

                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //处理profit
                temp = temp.Substring(0, temp.Length - 1);
                string[] profit_Array_Str = temp.Split(",");
                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //处理weight
                temp = temp.Substring(0, temp.Length - 1);
                string[] weight_Array_Str = temp.Split(",");

                int[] profit_Array = new int[profit_Array_Str.Length];
                int[] weight_Array = new int[weight_Array_Str.Length];
                for (int j = 0; j < profit_Array_Str.Length; j++)
                {
                    profit_Array[j] = Convert.ToInt32(profit_Array_Str[j]);
                    weight_Array[j] = Convert.ToInt32(weight_Array_Str[j]);
                }
                temp_Set.init_Item_Sets(profit_Array, weight_Array);
                //temp_Set.find_Max_Result_Dynamic_Programming();
                data_Sets.Add(temp_Set);
                listBox1.Items.Add(openFileDialog1.SafeFileName + "-" + i.ToString());
            }
            sr.Close();
            File.Delete("test.txt");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            data_Sets[index].find_Max_Result_Dynamic_Programming();
            textBox2.Text = data_Sets[index].get_Dynamic_Result().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            data_Sets[index].find_Max_Result_Dynamic_Programming();
        }
    }
}
