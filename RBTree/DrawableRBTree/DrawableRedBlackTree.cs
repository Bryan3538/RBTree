using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBTree;
using System.Drawing;
using System.Windows.Forms;

namespace DrawableRBTree
{
    class DrawableRedBlackTree<Key, Value> : RedBlackTree<Key, Value>
        where Key: IComparable<Key>
    {
        private int radius = 5;
        private int horizontalSpacing = 20;
        private int verticalSpacing = 30;
        private Pen linePen;

        public DrawableRedBlackTree()
        {
            linePen = new Pen(Color.Black, 2);
            radius = 5;
            horizontalSpacing = 20;
            verticalSpacing = 30;
        }
        public int Radius
        {
            get { return radius; }
            set 
            { 
                if(radius > 0)
                    radius = value; 
            }
        }

        public int HorizontalSpacing
        {
            get { return horizontalSpacing; }
            set 
            {
                if (value > 0)
                    horizontalSpacing = value;
            }
        }
        public int VerticalSpacing
        {
            get { return verticalSpacing; }
            set
            {
                if (value > 0)
                    verticalSpacing = value;
            }
        }

        public Pen LinePen
        {
            get { return linePen; }
            set
            {
                if (value != null)
                    linePen = value;
                else
                    throw new ArgumentNullException("The line pen cannot be null!");
            }
        }

        public void draw(Graphics g, Point p)
        {
            //horizontalSpacing = (int)Math.Floor(g.VisibleClipBounds.Width / Math.Pow(2, height()));
            //verticalSpacing = (int)Math.Floor(g.VisibleClipBounds.Height / height());
            
            //radius = horizontalSpacing / 4;
            if (!isEmpty())
                draw(root, g, p, Point.Empty, horizontalSpacing * (int)Math.Pow(2, height()));
        }

        private void draw(Node n, Graphics g, Point p, Point previous, int horizontalSpacing) 
        {
            //determine color
            Brush b;
            Color c;
            if (n == null)
                c = Color.Blue;
            else if (isRed(n))
                c = Color.Red;
            else
                c = Color.Black;
            b = new SolidBrush(c);


            //draw connecting line
            if(previous != Point.Empty)
            {
                //Point start = new Point(p.X - radius, p.Y - radius);
                //Point end = new Point(p.X - horizontalSpacing + radius,
                //    p.Y - verticalSpacing + radius);
                Point start = new Point(p.X + radius, p.Y + radius);
                Point end = new Point(previous.X + radius, previous.Y + radius);
                g.DrawLine(linePen, start, end);
            }
            //draw circle
            g.FillEllipse(b, p.X, p.Y, radius * 2, radius * 2);
            //draw circle outline
            g.DrawEllipse(linePen, p.X, p.Y, radius * 2, radius * 2);
   
            //increment for the next one
            Point left = new Point(p.X - horizontalSpacing, p.Y + verticalSpacing);

            Point right = new Point(p.X + horizontalSpacing, p.Y + verticalSpacing);

            if(n != null)
            {
                //draw the next nodes
                draw(n.Left, g, left, p, horizontalSpacing / 2);
                draw(n.Right, g, right, p, horizontalSpacing / 2);
            }
            
            
        }
    }
}
