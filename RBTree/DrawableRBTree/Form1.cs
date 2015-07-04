using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawableRBTree
{
    public partial class Form1 : Form
    {
        int index;
        DrawableRedBlackTree<int, int> tree;
        public Form1()
        {
            InitializeComponent();
            tree = new DrawableRedBlackTree<int, int>();
            index = 0;
            panel1.AutoScrollMinSize = new Size(5000, 5000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tree.put(index, 0);
            index++;
            label1.Text = tree.size().ToString();
            panel1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                tree.deleteMax();
                index--;
            }
            catch(InvalidOperationException ioe)
            {
                index = 0;
            }
            label1.Text = tree.size().ToString();
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            tree.draw(e.Graphics, new Point(panel1.Width / 2, 0));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 50; i++)
            {
                tree.put(index, 0);
                index++;
            }
            label1.Text = tree.size().ToString();
            panel1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    tree.deleteMax();
                }

                index -= 50;
                
            } catch(InvalidOperationException ioe)
            {
                index = 0;
            }

            
            panel1.Invalidate();
            label1.Text = tree.size().ToString();
        }

        public void ScrollToLeft()
        {
            using (Control c = new Control() { Parent = panel1, Dock = DockStyle.Left })
            {
                panel1.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }
    }
}
