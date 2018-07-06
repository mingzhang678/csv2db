using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jdrawing
{
    public partial class Form1 : Form
    {
        private Graphics _pictureBoxGraphics;
        private int x1;
        private int y1;
        private int x2;
        private int y2;
        private Pen pen;
        private Color color;
        private Point point1;
        private Point point2;
        private bool isDrawing;
        
        public Form1()
        {
            InitializeComponent();
            Draw();
            this._pictureBoxGraphics = pictureBox1.CreateGraphics();
            pen = new Pen(Color.OrangeRed, 2);
            Graphics panelGraphics = panel2.CreateGraphics();
            panel2.BackgroundImage = Jdrawing.Properties.Resources.Pencil2d_logo;
            Bitmap bm = Jdrawing.Properties.Resources.Pencil2d_logo;
            
            panel2.AutoSize = true;
            //panel2.AutoSizeMode = AutoSizeMode.GrowOnly;
        }

        void Draw()
        {
            pictureBox1.Text = "YES";
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //WebClient webClient = new WebClient();
            //Stream stream = webClient.OpenRead("https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RW9wPJ?ver=211f&q=90&m=6&h=201&w=358&b=%23FFFFFFFF&l=f&o=t&aim=true");
            //Image image = Image.FromStream(stream);
            //g.DrawImage(image,new PointF(1,1));
            if (_pictureBoxGraphics != null)
            {
                this.isDrawing = true;
                point1 = new Point(e.X, e.Y);
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            //Point position =  System.Windows.Forms.Cursor.Position;
            //textBox1.Text = position.X.ToString();
            //textBox2.Text = position.Y.ToString();
        }
        /**
        8                        8
        8                        8
        8                        8
        8                        8       
        8       8 8 8 8 8            8 8 8 8 8 8
        8       8                        8                   8
        8       8                         8                8
        8       8                            8           8 
        8888888888888888888              8  8
        */

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;
            Point position = pictureBox1.PointToClient(new Point(1920,1080));
            textBox1.Text = position.X.ToString();
            textBox2.Text = position.Y.ToString();
            point2 = new Point(e.X, e.Y);
            _pictureBoxGraphics.DrawLine(pen, point1, point2);
            point1 = point2;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.isDrawing = false;
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"JPEG(*.jpeg)|*.jpg|PNG(*.png)|*.png|Bitmap(*.bmp)|*.bmp|All Files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image image = Image.FromFile(openFileDialog.FileName);
                _pictureBoxGraphics.DrawImage(image, 0, 0);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _pictureBoxGraphics.Clear(Color.White);
        }
    }
}
