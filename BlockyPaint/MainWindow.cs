using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockyPaint
{
    public partial class MainWindow : Form
    {
        private Bitmap Palette = (Bitmap)Image.FromFile("palette.png");
        private Bitmap Canvas;
        private Graphics CanvasGfx;
        private SolidBrush MainColor;
        private int BrushWidth = 30;
        private int BrushHeight = 30;

        public MainWindow()
        {
            InitializeComponent();
            CreateNewCanvas();
            SetMainColor(Color.Black);
            UpdateBrushSizeLabel();

            PalettePanel.Paint += PalettePanel_Paint;
            PalettePanel.Click += PalettePanel_Click;
            CanvasPanel.Paint += CanvasPanel_Paint;
            CanvasPanel.Click += CanvasPanel_Click;
            CanvasPanel.MouseMove += CanvasPanel_MouseMove;
        }

        private void CreateNewCanvas()
        {
            Canvas = new Bitmap(CanvasPanel.ClientSize.Width, CanvasPanel.ClientSize.Height);
            CanvasGfx = Graphics.FromImage(Canvas);
            CanvasGfx.Clear(Color.White);
            CanvasPanel.Invalidate();
        }

        private void SetMainColor(Color color)
        {
            MainColor = new SolidBrush(color);
            BtnColor.BackColor = color;
        }

        private void PalettePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(Palette, 0, 0);            
        }

        private void PalettePanel_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            try
            {
                SetMainColor(Palette.GetPixel(me.X, me.Y));
            }
            catch
            {
            }
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(Canvas, 0, 0);
        }

        private void CanvasPanel_Click(object sender, EventArgs e)
        {
            CanvasClicked(e);
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            CanvasClicked(e);
        }

        private void CanvasClicked(EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                PutBlock(e);
            else if (me.Button == MouseButtons.Right)
                GrabColor(e);
        }

        private void PutBlock(EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            int x = me.X;
            int y = me.Y;
            Rectangle rect = new Rectangle(x, y, BrushWidth, BrushHeight);
            CanvasGfx.FillRectangle(MainColor, rect);
            CanvasPanel.Invalidate(rect);
        }

        private void GrabColor(EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            try
            {
                SetMainColor(Canvas.GetPixel(me.X, me.Y));
            }
            catch
            {
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            CreateNewCanvas();
        }

        private void BtnFill_Click(object sender, EventArgs e)
        {
            CanvasGfx.Clear(MainColor.Color);
            CanvasPanel.Invalidate();
        }

        private void BtnIncrease_Click(object sender, EventArgs e)
        {
            BrushWidth += 10;
            BrushHeight += 10;
            UpdateBrushSizeLabel();
        }

        private void BtnDecrease_Click(object sender, EventArgs e)
        {
            if (BrushWidth > 10 && BrushHeight > 10)
            {
                BrushWidth -= 10;
                BrushHeight -= 10;
                UpdateBrushSizeLabel();
            }
        }

        private void UpdateBrushSizeLabel()
        {
            if (BrushWidth == BrushHeight)
                StSize.Text = BrushWidth.ToString();
            else
                StSize.Text = BrushWidth + "x" + BrushHeight;
        }
    }
}
