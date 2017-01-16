using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerSaver
{
    public partial class Martin : Form
    {
        Random random;
        main m;
        int rrand;
        public Martin(main ma)
        {
            m = ma;
            InitializeComponent();
            mar.Image = Image.FromFile(@"walkingM.GIF");
            timer1.Start();
            random = new Random();
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(1700, 850);
            this.Show();
            mar.ContextMenuStrip = menu;
        }

    private void timer1_Tick(object sender, EventArgs e)
        {

            PowerStatus status = SystemInformation.PowerStatus;
            int nTemp;
            nTemp = (int)(status.BatteryLifePercent * 100.0); // 배터리 용량 %표시

            if (nTemp <= 10)
                mar.Image = Image.FromFile("warningM.GIF");
            else if (nTemp <= 25)
                mar.Image = Image.FromFile("downM.GIF");
            else if (nTemp <= 50)
                mar.Image = Image.FromFile("sitdownM.GIF");
            else
            {
                if (random != null)
                    rrand = random.Next(100);

                if (rrand > 0 && rrand < 3)
                {
                    mar.Image = Image.FromFile("1.GIF");
                }
                else if (rrand > 5 && rrand < 8)
                {
                    mar.Image = Image.FromFile("2.GIF");
                }
                else if (rrand > 10 && rrand < 13)
                {
                    mar.Image = Image.FromFile("3.GIF");
                }
                else if (rrand > 15 && rrand < 28)
                {
                    mar.Image = Image.FromFile("4.GIF");
                }
                else if (rrand > 20 && rrand < 23)
                {
                    mar.Image = Image.FromFile("5.GIF");
                }
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m.exit();
        }

        private void sleepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m.sleep();
        }

        private void suspendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m.suspend();
        }

        private void hibernateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m.hibernate();
        }

        private void shutDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m.shutdown();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
