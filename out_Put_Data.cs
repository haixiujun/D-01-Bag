using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace D_01_Bag
{
    class out_Put_Data
    {
        private string file_Path;
        private data_Set_Block data_Set_Block;
        private int[] selected;
        public out_Put_Data(data_Set_Block data)
        {
            data_Set_Block = data;
            
        }

        public void out_To_Txt(string path)
        {
            file_Path = path;
            test_File_Exist_And_Create();
            write_To_Txt();
        }

        public void out_To_Excel(string path)
        {
            file_Path = path;
            test_File_Exist_And_Create();
            write_To_Excel();

        }


        private void write_To_Excel()
        {
            int lines = data_Set_Block.get_Item_Count();
            string line = "";
            selected = data_Set_Block.get_Selected_Array();
            XSSFWorkbook sheets = new XSSFWorkbook();
            ISheet sheet0 = sheets.CreateSheet("Result");
            sheet0.CreateRow(0).CreateCell(0).SetCellValue("Selected Index");
            sheet0.GetRow(0).CreateCell(1).SetCellValue("Selected Data");
            for (int i = 0; i < lines; i++)
            {
                if (selected[i] == -1)
                {
                    sheet0.CreateRow(i + 1).CreateCell(0).SetCellValue("-1");
                    sheet0.GetRow(i + 1).CreateCell(1).SetCellValue("Null");
                }
                else
                {
                    sheet0.CreateRow(i + 1).CreateCell(0).SetCellValue(selected[i].ToString());
                    string temp = "(" + data_Set_Block.get_Item(i).get_Profit(selected[i] - 1).ToString() + "," + data_Set_Block.get_Item(i).get_Weight(selected[i] - 1).ToString() + ")";
                    sheet0.GetRow(i + 1).CreateCell(1).SetCellValue(temp);
                }
                
            }
            using (FileStream stream = new FileStream(file_Path, FileMode.Create, FileAccess.Write))
            {
                sheets.Write(stream);
            }
        }

        private void test_File_Exist_And_Create()
        {
            if (!File.Exists(file_Path))
            {
                FileStream fs1 = new FileStream(file_Path, FileMode.Create, FileAccess.Write);
                fs1.Close();
            }
        }

        private void write_To_Txt()
        {
            int lines = data_Set_Block.get_Item_Count();
            string line = "";
            selected = data_Set_Block.get_Selected_Array();
            StreamWriter sr = new StreamWriter(file_Path);
            for (int i = 0; i < lines; i++)
            {
                line = "";
                if (selected[i] == -1)
                {
                    line += "Not Selected!";
                }
                else
                {
                    line += get_Base_Data(i);
                }
                sr.WriteLine(line);
            }
            sr.Close();
        }

        private string get_Base_Data(int index)
        {
            string ret = "[";
            for (int i = 0; i < 3; i++)
            {
                ret += "(";
                ret += data_Set_Block.get_Item(index).get_Profit(i).ToString() + ",";
                ret += data_Set_Block.get_Item(index).get_Weight(i).ToString() + "),";
            }
            ret = ret.Substring(0, ret.Length - 1);
            ret += "]" + " Selected Index:" + selected[index].ToString() + " Selected_Profit:" + data_Set_Block.get_Item(index).get_Profit(selected[index] - 1).ToString();
            ret += " Selected Weight:" + data_Set_Block.get_Item(index).get_Weight(selected[index] - 1).ToString();
            return ret;
         }

    }
}
