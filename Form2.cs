using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace D_01_Bag
{
    public partial class Form2 : Form
    {
        Form1 form1;
        int index0;
        public Form2(Form1 form, int index)
        {
            InitializeComponent();
            form1 = form;
            index0 = index;
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            PointF origin = new PointF(40, this.Height - 80);
            g.DrawLine(new Pen(Brushes.Black, 2), origin, new PointF(this.Width, this.Height - 80));
            g.DrawLine(new Pen(Brushes.Black, 2), origin, new PointF(40, 0));
            PointF p1;
            PointF p2;
            for (int i = 1; i * 20 + 40 < this.Width; i++)
            {
                p1 = new PointF(40 + i * 20, this.Height - 80);
                p2 = new PointF(40 + i * 20, this.Height - 85);
                g.DrawLine(new Pen(Brushes.Black, 1), p1, p2);
                if (i % 5 == 0)
                {
                    p2 = new PointF(30 + i * 20, this.Height - 75);
                    g.DrawString((i * 20).ToString(), new Font("黑体", 8, FontStyle.Bold), new SolidBrush(Color.Black), p2);
                }
            }
            for (int i = 1; i * 20 + 40 < this.Height; i++)
            {
                g.DrawLine(new Pen(Brushes.Black, 1), new PointF(40, this.Height - 80 - 20 * i), new PointF(45, this.Height - 80 - i * 20));
                if (i % 5 == 0)
                {
                    p2 = new PointF(15, this.Height - 85 - 20 * i);
                    g.DrawString((i * 20).ToString(), new Font("黑体", 8, FontStyle.Bold), new SolidBrush(Color.Black), p2);
                }
            }
            p2 = new PointF(30, this.Height - 75);
            g.DrawString("0", new Font("黑体", 8, FontStyle.Bold), new SolidBrush(Color.Black), p2);
            p2 = new PointF(this.Width - 30, this.Height - 77);
            g.DrawString("x", new Font("黑体", 8, FontStyle.Bold), new SolidBrush(Color.Black), p2);
            p2 = new PointF(30, 5);
            g.DrawString("y", new Font("黑体", 8, FontStyle.Bold), new SolidBrush(Color.Black), p2);
            data_Set_Block data_Block = form1.get_Data_Block(index0);
            int item_Count = data_Block.get_Item_Count();
            for (int i = 0; i < item_Count; i++)
            {
                item_Set item = data_Block.get_Item(i);
                for(int j = 0; j < 3; j++)
                {
                    draw_Point(item.get_Profit(j), item.get_Weight(j));
                }
            }
        }
        private void draw_Point(int x, int y)
        {
            Graphics g = this.CreateGraphics();
            int new_X = 40 + x;
            int new_Y = this.Height - 80 - y;
            g.FillEllipse(new SolidBrush(Color.YellowGreen), new_X, new_Y, 4, 4);
        }
    }
}
