using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SistemNutrisi
{
    /// <summary>
    /// Custom Panel that draws a Pie Chart or Bar Chart using GDI+.
    /// Shows a live preview in the Visual Studio Designer using sample data.
    /// </summary>
    [ToolboxItem(false)]
    public class ChartPanel : Panel
    {
        private double _protein = 0;
        private double _lemak   = 0;
        private double _karbo   = 0;
        private int    _chartType = 0; // 0 = Pie, 1 = Bar

        [Browsable(true), Category("Chart"), DefaultValue(0d)]
        public double Protein  { get { return _protein; } set { _protein  = value; Invalidate(); } }

        [Browsable(true), Category("Chart"), DefaultValue(0d)]
        public double Lemak    { get { return _lemak;   } set { _lemak    = value; Invalidate(); } }

        [Browsable(true), Category("Chart"), DefaultValue(0d)]
        public double Karbo    { get { return _karbo;   } set { _karbo    = value; Invalidate(); } }

        /// <summary>0 = Pie Chart, 1 = Bar Chart</summary>
        [Browsable(true), Category("Chart"), DefaultValue(0)]
        public int ChartType   { get { return _chartType; } set { _chartType = value; Invalidate(); } }

        public ChartPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw   = true;

            // Provide sample data so the designer shows a preview
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                _protein = 45;
                _lemak   = 28;
                _karbo   = 145;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawChart(e.Graphics);
        }

       
        private void DrawChart(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int padding = 20;
            int w = this.Width;
            int h = this.Height;

            double[] values = { _protein, _lemak, _karbo };
            string[] labels = { "Protein", "Lemak", "Karbohidrat" };
            Color[]  colors = {
                Color.FromArgb(231, 76,  60),   // red
                Color.FromArgb(241, 196, 15),   // yellow
                Color.FromArgb(52,  152, 219)   // blue
            };
            double total = _protein + _lemak + _karbo;

            if (total <= 0)
            {
                DrawEmpty(g, w, h, padding);
                return;
            }

            if (_chartType == 0)
                DrawPie(g, w, h, padding, values, labels, colors, total);
            else
                DrawBar(g, w, h, padding, values, labels, colors);
        }

        private void DrawEmpty(Graphics g, int w, int h, int padding)
        {
            int diameter = Math.Min(w - padding * 2, h - 160);
            if (diameter < 80) diameter = 80;
            int cx = (w - diameter) / 2;
            int cy = 80;

            using (SolidBrush br = new SolidBrush(Color.FromArgb(220, 220, 220)))
                g.FillEllipse(br, cx, cy, diameter, diameter);

            using (Font font = new Font("Segoe UI", 10, FontStyle.Italic))
            using (SolidBrush tb = new SolidBrush(Color.Gray))
            {
                string msg = "Belum ada konsumsi hari ini";
                SizeF sz = g.MeasureString(msg, font);
                g.DrawString(msg, font, tb,
                    (w - sz.Width) / 2f,
                    cy + (diameter - sz.Height) / 2f);
            }
        }

        private void DrawPie(Graphics g, int w, int h, int padding,
                             double[] values, string[] labels, Color[] colors, double total)
        {
            int diameter = Math.Min(w - padding * 2, h - 160);
            if (diameter < 80) diameter = 80;
            int cx = (w - diameter) / 2;
            int cy = 80;
            Rectangle rect = new Rectangle(cx, cy, diameter, diameter);

            float startAngle = -90f;
            for (int i = 0; i < 3; i++)
            {
                float sweep = (float)(values[i] / total * 360.0);
                if (sweep <= 0) continue;
                using (SolidBrush br = new SolidBrush(colors[i]))
                    g.FillPie(br, rect, startAngle, sweep);
                startAngle += sweep;
            }

            // Legend
            int legendY  = cy + diameter + 30;
            int itemH    = 25;
            int sqSize   = 14;

            using (Font font = new Font("Segoe UI", 9.5f, FontStyle.Regular))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                for (int i = 0; i < 3; i++)
                {
                    int ry = legendY + i * itemH;
                    int rx = padding + 20;
                    using (SolidBrush br = new SolidBrush(colors[i]))
                        g.FillRectangle(br, rx, ry + 3, sqSize, sqSize);

                    double pct = values[i] / total * 100;
                    string txt = string.Format("{0}: {1:0.0}g ({2:0}%)", labels[i], values[i], pct);
                    g.DrawString(txt, font, tb, rx + sqSize + 8, ry);
                }
            }
        }

        private void DrawBar(Graphics g, int w, int h, int padding,
                             double[] values, string[] labels, Color[] colors)
        {
            int left   = padding + 35;
            int right  = w - padding - 20;
            int top    = 80;
            int bottom = h - 60;
            int chartH = bottom - top;
            int chartW = right  - left;

            // Axes
            using (Pen axPen = new Pen(Color.FromArgb(127, 140, 141), 2))
            {
                g.DrawLine(axPen, left, top - 10, left, bottom);  // Y axis
                g.DrawLine(axPen, left, bottom, right, bottom);   // X axis
            }

            double maxVal = Math.Max(values[0], Math.Max(values[1], values[2]));
            if (maxVal <= 0) maxVal = 1;

            int spacing  = 28;
            int barWidth = (chartW - spacing * 4) / 3;
            if (barWidth < 20) barWidth = 20;

            using (Font labelFont = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (Font valFont   = new Font("Segoe UI", 9f, FontStyle.Regular))
            using (SolidBrush tb  = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                for (int i = 0; i < 3; i++)
                {
                    int bh = (int)(values[i] / maxVal * (chartH - 30));
                    int bx = left + spacing + i * (barWidth + spacing);
                    int by = bottom - bh;

                    using (SolidBrush br = new SolidBrush(colors[i]))
                        g.FillRectangle(br, bx, by, barWidth, bh);

                    // Value above bar
                    string vTxt = string.Format("{0:0.0}g", values[i]);
                    SizeF  vSz  = g.MeasureString(vTxt, valFont);
                    g.DrawString(vTxt, valFont, tb,
                        bx + (barWidth - vSz.Width)  / 2f,
                        by - vSz.Height - 4);

                    // Label below X axis
                    SizeF lSz = g.MeasureString(labels[i], labelFont);
                    g.DrawString(labels[i], labelFont, tb,
                        bx + (barWidth - lSz.Width) / 2f,
                        bottom + 6);
                }
            }
        }
    }
}
