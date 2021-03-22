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
        public Form1()
        {
            InitializeComponent();
            data_Set_File_Path = "";
            temp_Data = "";
            file_Lines_Count = 0;
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
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Read Error!");
            }
        }
           
        private void cut_Data_Set()
        {
            file_Lines_Count--;
            int first_Line_End_Index = temp_Data.IndexOf('\n');
            string temp = "";
            temp_Data = temp_Data.Remove(0, first_Line_End_Index + 1);
            StreamWriter sw = new StreamWriter("test.txt");
            sw.Write(temp_Data);

            
            if ( file_Lines_Count % 6 != 0)
            {
                MessageBox.Show("File Error!");
                return;
            }
            group_Counts = file_Lines_Count / 6;
            StreamReader sr = new StreamReader("test.txt");
            for (int i = 0; i < group_Counts; i++)
            {
                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //处理d和c

                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //处理profit

                temp = sr.ReadLine();
                temp = sr.ReadLine();
                //处理weight


            }
            File.Delete("test.txt");
        }


    }
}
