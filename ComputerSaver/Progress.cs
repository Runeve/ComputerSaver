using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerSaver
{
    public partial class Progress : Form
    {
        main cp;

        public Progress()
        {
            InitializeComponent();
        }

        public Progress(main compterSaver)
        {
            cp = compterSaver;
            InitializeComponent();
            timer1.Interval = 1000;//1초 간격으로 한다.


        }
        private void CancelExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExitImmediately_Click_1(object sender, EventArgs e)
        {
            this.Close();
            cp.shutdown();
        }

        private void Progress_Load_1(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 15;
            progressBar1.Value = 0;
            progressBar1.Step = 1;//step은 하나로
            timer1.Start();  //timer Start를 해주어야 Event가 발생한다.
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (progressBar1.Value == 15)//15초가될시
            {
                timer1.Enabled = false;//타이머를 멈추고
                cp.shutdown();//기능을 실행
                this.Close();//폼을 닫는다
                return;
            }
            progressBar1.PerformStep();//지정한 step만큼 실행한다
            label1.Text = "The computer shuts down after " + (16 - progressBar1.Value).ToString() + " seconds";

        }
    }
}
