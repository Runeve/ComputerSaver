using Microsoft.WindowsAPICodePack.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerSaver
{
    public partial class main : Form
    {
        public string IpNumber = null;
        Martin mr;
        DateTime sleepTime;
        TCPServer tcp = null;

        public main()
        {
            InitializeComponent();
            PowerManager.IsMonitorOnChanged += new EventHandler(wake);
            mr = new Martin(this);
        }
  
       
        private void wake(object sender, EventArgs e)//모니터가 켜질 시 wakeup()함수중 프로그램이 실행되는 부분말고 call한다.
        {
            if (PowerManager.IsMonitorOn == true)
            {
                String resultStr;
                String URL = "http://210.94.194.100:20151/log.asp?";
                String message = "id=" + ip.Text + "&cmd=write&action=wakeup";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.ContentLength = sendData.Length;
                request.Method = "POST";

                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(message);
                sw.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                resultStr = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();
                MessageBox.Show(resultStr);

                String message2 = "id=" + ip.Text + "&cmd=read";

                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(URL);
                byte[] sendData2 = UTF8Encoding.UTF8.GetBytes(message2);
                request2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request2.ContentLength = sendData2.Length;
                request2.Method = "POST";

                StreamWriter sw2 = new StreamWriter(request2.GetRequestStream());
                sw2.Write(message2);
                sw2.Close();

                HttpWebResponse httpWebResponse2 = (HttpWebResponse)request2.GetResponse();
                StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                resultStr = streamReader2.ReadToEnd();
                streamReader2.Close();
                httpWebResponse2.Close();
                textBox2.Text = resultStr;

                LTwakeup.Text = DateTime.Now.ToString("MM-dd HH:mm:ss");

                DateTime now = DateTime.Now;
                TimeSpan dateDifferent = now - sleepTime;//TIMESOAN을 이용해 시간 차이를 계산해낸다
                MessageBox.Show("마지막 종료 로부터 " + dateDifferent.Days + "일" + dateDifferent.Hours + "시간" + dateDifferent.Minutes + "분" + dateDifferent.Seconds + "초");                

            }
        }
        

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)//단축키를 사용자정의대로 지정한다
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control);//현재 키보드에 눌러진 keyData를 받는다.

            string st = key.ToString();//string으로 변환

            //각 Text박스에 지정되어있는 string과 비교해 동일하면 각 기능을 수행한다.
            if (st == setsl.Text)
            {
                sleep();
                return true;
            }
            else if (st == setsh.Text)
            {
                shutdown();
                return true;
            }
            else if (st == setsu.Text)
            {
                suspend();
                return true;
            }
            else if (st == sethi.Text)
            {
                hibernate();
                return true;
            }
            else
                return false;
        }

        private void main_Load(object sender, EventArgs e)
        {
            mr.Show();

            //트레이 아이콘을 표시하고 메뉴를 삽입한다.
            notifyIcon1.Icon = new Icon("ico.ico");
            notifyIcon1.ContextMenuStrip = Menu;

            //각각 단축키가 저장되어있는 텍스트 파일을 열어 TextBox에 쓴다.
            StreamReader dr1 = new StreamReader(@"sleepkeys.txt");
            string st = dr1.ReadLine();
            setsl.Text = st;
            dr1.Close();

            StreamReader dr2 = new StreamReader(@"shutkeys.txt");
            st = dr2.ReadLine();
            setsh.Text = st;
            dr2.Close();

            StreamReader dr3 = new StreamReader(@"suspendkeys.txt");
            st = dr3.ReadLine();
            setsu.Text = st;
            dr3.Close();

            StreamReader dr4 = new StreamReader(@"hibernatekeys.txt");
            st = dr4.ReadLine();
            sethi.Text = st;
            dr4.Close();

            PowerStatus status = SystemInformation.PowerStatus;
            int nTemp;
            nTemp = (int)(status.BatteryLifePercent * 100.0); // 배터리 용량 %표시
            label1.Text = nTemp.ToString() + "%\nBattery";

            Qmonitoroff.Image = Image.FromFile("monitoroffpic.GIF");
            Qsuspend.Image = Image.FromFile("suspendpic.GIF");
            Qhibernate.Image = Image.FromFile("hibernatepic.GIF");
            Qshutdown.Image = Image.FromFile("shutdownpic.GIF");
            martin.Image = Image.FromFile("walkingM.GIF");

            timer1.Start();

            login.Text = "Log out";
            login.BackColor = Color.Red;




            //Reservation에 관한 설정

            timer2.Interval = 1000;//1초 간격으로 한다.

            day.Items.AddRange(new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            hour.Items.AddRange(new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                               11,12,13,14,15,16,17,18,19,20,21,22,23 });
            minute.Items.AddRange(new object[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ,
                                                 11,12,13,14,15,16,17,18,19,20,
                                                 21,22,23,24,25,26,27,28,29,30,
                                                 31,32,33,34,35,36,37,38,39,40,
                                                 41,42,43,44,45,46,47,48,49,50,
                                                 51,52,53,54,55,56,57,58,59,60});
            function.Items.AddRange(new object[] { "Sleep", "Shut donw", "Suspend", "Hibernate" });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PowerStatus status = SystemInformation.PowerStatus;
            int nTemp;
            nTemp = (int)(status.BatteryLifePercent * 100.0); // 배터리 용량 %표시
            label1.Text = nTemp.ToString() + "% Battery";

            label_battery_charge_status.Text = "State : " + status.BatteryChargeStatus.ToString(); // 배터리 상태 표시 (High등)

            label_battery_powerline.Text = "Charging : " + status.PowerLineStatus.ToString(); // 배터리 AC전원 on,off 표시

            if(nTemp <= 10)
                martin.Image = Image.FromFile("warningM.GIF");
            else if(nTemp <= 25)
                martin.Image = Image.FromFile("downM.GIF");
            else if(nTemp <= 50)
                martin.Image = Image.FromFile("sitdownM.GIF");
        }



        //각 기능을 수행하는 기능함수
        public void shutdown()
        {
            LTshutdown.Text = "Last action :\n" + DateTime.Now.ToString("MM-dd HH:mm:ss");

            String resultStr;
            String URL = "http://210.94.194.100:20151/log.asp?";
            String message = "id=" + ip.Text + "&cmd=write&action=shutdown";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            MessageBox.Show(resultStr);

            //Process.Start(fileName: "nircmd.exe", arguments: "exitwin poweroff");

            String message2 = "id=" + ip.Text + "&cmd=read";

            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData2 = UTF8Encoding.UTF8.GetBytes(message2);
            request2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request2.ContentLength = sendData2.Length;
            request2.Method = "POST";

            StreamWriter sw2 = new StreamWriter(request2.GetRequestStream());
            sw2.Write(message2);
            sw2.Close();

            HttpWebResponse httpWebResponse2 = (HttpWebResponse)request2.GetResponse();
            StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader2.ReadToEnd();
            streamReader2.Close();
            httpWebResponse2.Close();
            textBox2.Text = resultStr;

        }
        public void sleep()
        {
            LTsleep.Text = "Last action :\n" + DateTime.Now.ToString("MM-dd HH:mm:ss");
            sleepTime = DateTime.Now;

            String resultStr;
            String URL = "http://210.94.194.100:20151/log.asp?";
            String message = "id=" + ip.Text + "&cmd=write&action=sleep";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            MessageBox.Show(resultStr);

            Process.Start(fileName: "nircmd.exe", arguments: "monitor off");//외부 프로그램을 실행하여 기능을 수행한다.

            String message2 = "id=" + ip.Text + "&cmd=read";

            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData2 = UTF8Encoding.UTF8.GetBytes(message2);
            request2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request2.ContentLength = sendData2.Length;
            request2.Method = "POST";

            StreamWriter sw2 = new StreamWriter(request2.GetRequestStream());
            sw2.Write(message2);
            sw2.Close();

            HttpWebResponse httpWebResponse2 = (HttpWebResponse)request2.GetResponse();
            StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader2.ReadToEnd();
            streamReader2.Close();
            httpWebResponse2.Close();
            textBox2.Text = resultStr;//텍스트 박스2에 반환된 결과, 즉 로그를 출력한다.
        }
        public void suspend()
        {
            LTsuspends.Text = "Last action :\n" + DateTime.Now.ToString("MM-dd HH:mm:ss");
            sleepTime = DateTime.Now;

            String resultStr;
            String URL = "http://210.94.194.100:20151/log.asp?";
            String message = "id=" + ip.Text + "&cmd=write&action=suspend";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            MessageBox.Show(resultStr);

            Process.Start(fileName: "nircmd.exe", arguments: "standby");//외부 프로그램을 실행하여 기능을 수행한다.

            String message2 = "id=" + ip.Text + "&cmd=read";

            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData2 = UTF8Encoding.UTF8.GetBytes(message2);
            request2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request2.ContentLength = sendData2.Length;
            request2.Method = "POST";

            StreamWriter sw2 = new StreamWriter(request2.GetRequestStream());
            sw2.Write(message2);
            sw2.Close();

            HttpWebResponse httpWebResponse2 = (HttpWebResponse)request2.GetResponse();
            StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader2.ReadToEnd();
            streamReader2.Close();
            httpWebResponse2.Close();
            textBox2.Text = resultStr;//텍스트 박스2에 반환된 결과, 즉 로그를 출력한다.
        }
        public void hibernate()
        {
            LThibernate.Text = "Last action :\n" + DateTime.Now.ToString("MM-dd HH:mm:ss");
            sleepTime = DateTime.Now;

            String resultStr;
            String URL = "http://210.94.194.100:20151/log.asp?";
            String message = "id=" + ip.Text + "&cmd=write&action=hibernate";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            MessageBox.Show(resultStr);

            Application.SetSuspendState(PowerState.Hibernate, false, false);

            String message2 = "id=" + ip.Text + "&cmd=read";

            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData2 = UTF8Encoding.UTF8.GetBytes(message2);
            request2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request2.ContentLength = sendData2.Length;
            request2.Method = "POST";

            StreamWriter sw2 = new StreamWriter(request2.GetRequestStream());
            sw2.Write(message2);
            sw2.Close();

            HttpWebResponse httpWebResponse2 = (HttpWebResponse)request2.GetResponse();
            StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader2.ReadToEnd();
            streamReader2.Close();
            httpWebResponse2.Close();
            textBox2.Text = resultStr;//텍스트 박스2에 반환된 결과, 즉 로그를 출력한다.
        }
        public void wakeup()
        {
            String resultStr;
            String URL = "http://210.94.194.100:20151/log.asp?";
            String message = "id=" + ip.Text + "&cmd=write&action=wakeup";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            MessageBox.Show(resultStr);

            Process.Start(fileName: "nircmd.exe", arguments: "changesysvolume 2000");

            String message2 = "id=" + ip.Text + "&cmd=read";

            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData2 = UTF8Encoding.UTF8.GetBytes(message2);
            request2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request2.ContentLength = sendData2.Length;
            request2.Method = "POST";

            StreamWriter sw2 = new StreamWriter(request2.GetRequestStream());
            sw2.Write(message2);
            sw2.Close();

            HttpWebResponse httpWebResponse2 = (HttpWebResponse)request2.GetResponse();
            StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader2.ReadToEnd();
            streamReader2.Close();
            httpWebResponse2.Close();
            textBox2.Text = resultStr;

            //마지막 SHUTDOWN시간과 마지막 WAKEUP시간까지의 차이를 계산해 메세지 박스로 출력한다.
            string sub1 = resultStr;
            string sub2 = resultStr;
            int length;
            string slp = "SHUTDOWN";//비교할 문자열
            string wake = "WAKEUP";

            sub1 = sub1.Substring(sub1.LastIndexOf(slp) + 9);//마지막 SHUTDOWN까지 문자열을 잘라낸다
            length = sub1.IndexOf("<BR>");//그 다음 들여쓰기를 잘라 자른다
            sub1 = sub1.Substring(0, length);//그 길이 만큼 문자열을 자른다

            sub2 = sub2.Substring(sub2.LastIndexOf(wake) + 7);
            length = sub2.IndexOf("<BR>");
            sub2 = sub2.Substring(0, length);

            sub1 = sub1.Replace("<BR>", "");
            sub2 = sub2.Replace("<BR>", "");

            DateTime slpDate = new DateTime();//DATATIME객체를 만든다.
            DateTime.TryParse(sub1, null, System.Globalization.DateTimeStyles.AssumeLocal, out slpDate);//서버에 기록되 로그 문자열로 DATATIME을 설정한다
            DateTime wakeDate = new DateTime();
            DateTime.TryParse(sub2, null, System.Globalization.DateTimeStyles.AssumeLocal, out wakeDate);

            TimeSpan dateDifferent = wakeDate - slpDate;//TIMESOAN을 이용해 시간 차이를 계산해낸다
            MessageBox.Show("마지막 종료 로부터 " + dateDifferent.Days + "일" + dateDifferent.Hours + "시간" + dateDifferent.Minutes + "분" + dateDifferent.Seconds + "초");

        }



        //원격 조종을 수행하는 기능함수
        public void romoteWakeup()
        {
            String resultStr;
            String URL = "http://210.94.194.100:20151/Command.asp";
            String message = "ip=" + IpNumber + "&cmd=wakeup";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
        }
        public void romoteShutdown()
        {
            String resultStr;
            String URL = "http://210.94.194.100:20151/Command.asp";
            String message = "ip=" + IpNumber + "&cmd=shutdown";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
        }
        public void romoteSleep()
        {
            String resultStr;
            MessageBox.Show(IpNumber);
            String URL = "http://210.94.194.100:20151/Command.asp";
            String message = "ip=" + IpNumber + "&cmd=SLEEP";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            MessageBox.Show(resultStr);
        }


    
        //QuickMenu 동작
        private void Qmonitoroff_Click(object sender, EventArgs e)
        {
            sleep();
        }
        private void Qsuspend_Click(object sender, EventArgs e)
        {
            suspend();
        }
        private void Qhibernate_Click(object sender, EventArgs e)
        {
            hibernate();
        }
        private void Qshutdown_Click(object sender, EventArgs e)
        {
            Progress pr = new Progress(this);
            pr.Show();
        }

        //각 버튼을 누르면 동작
        private void MonitoroffbuttonX1_Click(object sender, EventArgs e)
        {
            if (switchButton1.Value == true)
            {
                romoteSleep();
            }
            else
            {
                sleep();
            }
        }
        private void suspendbuttonX2_Click(object sender, EventArgs e)
        {
            suspend();
        }
        private void hibernatebuttonX3_Click(object sender, EventArgs e)
        {
            hibernate();
        }
        private void shutdownbuttonX4_Click(object sender, EventArgs e)
        {
            if (switchButton1.Value == true)
            {
                romoteShutdown();
            }
            else
            {
                Progress pr = new Progress(this);
                pr.Show();
            }
        }

        //Menu에 관한 동작
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;//폼을 나타나게 하면
            this.Activate();//활성화시킨다
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }
        private void sleepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sleep();
        }
        private void suspendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            suspend();
        }
        private void hibernateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hibernate();
        }
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        private void diconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tcp.Disconnect();
            MessageBox.Show("Disconnect");
            tcp = null;//tcp를 초기화시켜준다.
        }
        private void shutDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Progress pr = new Progress(this);
            pr.Show();
        }
        public void exit()
        {
            //폼을 종료시키고 트레이 아이콘을 숨긴다
            this.FormClosing -= main_FormClosing;
            notifyIcon1.Visible = false;
            Application.Exit();

            int count;

            //count.txt의 실행횟수를 증가시킨다.
            StreamReader dr = new StreamReader(@"count.txt");
            string st = dr.ReadLine();
            count = int.Parse(st) + 1;
            dr.Close();

            //StreamWriter wr = new StreamWriter(@"count.txt");
            //wr.WriteLine(count);
            //wr.Close();
            //tcp.Disconnect();
            //tcp = null;
        }



        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;//폼이 나타나도록 한다
            this.Activate();
        }

        private void Enroll_Click(object sender, EventArgs e)
        {
            String resultStr;
            String URL = "http://210.94.194.100:20151/registerUser.asp?";
            String message = "id=" + ip.Text;//id에 관한 파라미터로 만들어 서버에 전달한다.

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);//반환 객체 생성
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(message);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = sendData.Length;
            request.Method = "POST";//메소드 생성

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(message);//서버로 부터 받은 메세지를 출력한다.
            sw.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            resultStr = streamReader.ReadToEnd();//resultStr에 반환 값을 저장한다.
            streamReader.Close();
            httpWebResponse.Close();

            MessageBox.Show("Enrolled!");
            login.Text = "Log in";
            login.BackColor = Color.Green;
        }


        //Form에 관한 동작
        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;//종료되지 않게 하고
            this.Hide();//폼만 숨기게 한다.
        }
        private void main_ResizeBegin(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)//폼이 사라지게 한다.
            {
                this.Hide();
            }
        }
        private void main_Shown(object sender, EventArgs e)
        {
            //count.txt의 실행횟수를 읽어 2번 이상 실행때는 폼을 감추게한다.
            StreamReader dr = new StreamReader(@"count.txt");
            string st = dr.ReadLine();
            if (int.Parse(st) >= 1)
            {
                this.Hide();
            }
            dr.Close();
        }


        //set키로 단축키 설정
        private void setmonitoroff_Click(object sender, EventArgs e)
        {
            StreamWriter wr = new StreamWriter(@"sleepkeys.txt");
            wr.WriteLine(setsl.Text);
            wr.Close();
            MessageBox.Show("Setting Complete!");
        }
        private void sethibernate_Click(object sender, EventArgs e)
        {
            StreamWriter wr = new StreamWriter(@"hibernatekeys.txt");
            wr.WriteLine(setsh.Text);
            wr.Close();
            MessageBox.Show("Setting Complete!");
        }
        private void setsuspend_Click(object sender, EventArgs e)
        {
            StreamWriter wr = new StreamWriter(@"suspendkeys.txt");
            wr.WriteLine(setsh.Text);
            wr.Close();
            MessageBox.Show("Setting Complete!");
        }
        private void setshutdown_Click(object sender, EventArgs e)
        {
            StreamWriter wr = new StreamWriter(@"shutkeys.txt");
            wr.WriteLine(setsh.Text);
            wr.Close();
            MessageBox.Show("Setting Complete!");
        }

        private void setcolor_Click(object sender, EventArgs e)//컬러 설정
        {
            Color c = new Color();
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                c = cd.Color;
            }
            this.BackColor = c;
        }

        //Reservation에 관한 동작

        int reservationNumber = 0;
        int[] days = new int[3];
        int[] hours = new int[3];
        int[] minutes = new int[3];
        int[] func = new int[3];
        int[] time = new int[3];
        int[] count = new int[3];

        private void res_Click(object sender, EventArgs e)
        {
            if (reservationNumber == 0)
            {
                days[0] = day.SelectedIndex;
                hours[0] = hour.SelectedIndex;
                minutes[0] = minute.SelectedIndex;

                MessageBox.Show("The function is executed after " + days[0] + "days "
                                + hours[0] + "hours" + minutes[0] + "minutes");

                //지정된 시간을 계산한다.
                time[0] = days[0] * 86400 + hours[0] * 3600 + minutes[0] * 60;
                timer2.Start();
            }
            else if (reservationNumber == 1)
            {
                days[1] = day.SelectedIndex;
                hours[1] = hour.SelectedIndex;
                minutes[1] = minute.SelectedIndex;

                MessageBox.Show("The function is executed after " + days[1] + "days "
                                + hours[1] + "hours" + minutes[1] + "minutes");

                //지정된 시간을 계산한다.
                time[1] = days[1] * 86400 + hours[1] * 3600 + minutes[1] * 60;
                timer3.Start();
            }
            else if (reservationNumber == 2)
            {
                days[2] = day.SelectedIndex;
                hours[2] = hour.SelectedIndex;
                minutes[2] = minute.SelectedIndex;

                MessageBox.Show("The function is executed after " + days[2] + "days "
                                + hours[2] + "hours" + minutes[2] + "minutes");

                //지정된 시간을 계산한다.
                time[2] = days[2] * 86400 + hours[2] * 3600 + minutes[2] * 60;
                timer4.Start();
            }
            else
            {
                MessageBox.Show("Up to three settings can be made.");
            }
            reservationNumber++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            reservationlog1.Text = "After " + days[0] + " days " + hours[0] + " hours " +
                                minutes[0] + " minutes Do ";

            switch (func[0])
            {
                case 0:
                    reservationlog1.Text += " Monitor off";
                    break;
                case 1:
                    reservationlog1.Text += "shutdown";
                    break;
                case 2:
                    reservationlog1.Text += "suspend";
                    break;
                case 3:
                    reservationlog1.Text += "hibernate";
                    break;
            }

                    count[0]++;
            if (count[0] >= time[0])//지정된 시간이 될 시
            {
                timer2.Enabled = false;//타이머를 멈추고
                func[0] = function.SelectedIndex;

                //각 기능을 실행한다.
                switch (func[0])
                {
                    case 0:
                        sleep();
                        break;
                    case 1:
                        shutdown();
                        break;
                    case 2:
                        suspend();
                        break;
                    case 3:
                        hibernate();
                        break;
                }
                return;
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            reservationlog2.Text = "After " + days[1] + " days " + hours[1] + " hours " +
                                minutes[1] + " minutes Do ";

            switch (func[1])
            {
                case 0:
                    reservationlog2.Text += " Monitor off";
                    break;
                case 1:
                    reservationlog2.Text += "shutdown";
                    break;
                case 2:
                    reservationlog2.Text += "suspend";
                    break;
                case 3:
                    reservationlog2.Text += "hibernate";
                    break;
            }

            count[1]++;
            if (count[1] >= time[1])//지정된 시간이 될 시
            {
                timer3.Enabled = false;//타이머를 멈추고
                func[1] = function.SelectedIndex;

                //각 기능을 실행한다.
                switch (func[1])
                {
                    case 0:
                        sleep();
                        break;
                    case 1:
                        shutdown();
                        break;
                    case 2:
                        suspend();
                        break;
                    case 3:
                        hibernate();
                        break;
                }
                return;
            }
        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            reservationlog3.Text = "After " + days[2] + " days " + hours[2] + " hours " +
                                minutes[2] + " minutes Do ";

            switch (func[2])
            {
                case 0:
                    reservationlog3.Text += " Monitor off";
                    break;
                case 1:
                    reservationlog3.Text += "shutdown";
                    break;
                case 2:
                    reservationlog3.Text += "suspend";
                    break;
                case 3:
                    reservationlog3.Text += "hibernate";
                    break;
            }

            count[2]++;
            if (count[2] >= time[2])//지정된 시간이 될 시
            {
                timer4.Enabled = false;//타이머를 멈추고
                func[2] = function.SelectedIndex;

                //각 기능을 실행한다.
                switch (func[2])
                {
                    case 0:
                        sleep();
                        break;
                    case 1:
                        shutdown();
                        break;
                    case 2:
                        suspend();
                        break;
                    case 3:
                        hibernate();
                        break;
                }
                return;
            }
        }

        //파일로 저장
        private void save_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text File |*.txt";//파일 다이어로그를 이용하여 파일을 저장한다
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(saveFileDialog1.FileName, textBox2.Text);//TESTBOX2에 있는 내용을 텍스트 파일로 저장한다.

        }



        //Remote 페이지
        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            if(switchButton1.Value == true)
            {
                MessageBox.Show("Remote On!");
            }
            else if(switchButton1.Value == false)
            {
                MessageBox.Show("RemoteOff!");
            }
        }

        private void remoteenroll_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Start to connect");
            
            tcp = new TCPServer(this);
            if (tcp.Connect("210.94.194.100", 20151) == false)//연결이 제대로 되어있지않으면
            {
                MessageBox.Show("Fail to connect");
                tcp = null;
                return;
            }
            
            MessageBox.Show("Sucess to connect");
            IpNumber = remoteip.Text;
            MessageBox.Show("Connect to " + IpNumber);
            tcp.Start();

            timer5.Start();
            
        }

        int progressCount = 0; 
        private void timer5_Tick(object sender, EventArgs e)
        {
            if (switchButton1.Value == true)
            {
                circularProgress1.IsRunning = true;
                circularProgress2.IsRunning = true;

                if(progressCount == 1)
                {
                    label18.Text = "Connecting.";
                    remote.Text = "Connecting.";
                    progressCount = 1;
                }
                else if (progressCount <= 3 && progressCount >1)
                {
                    label18.Text += ".";
                    remote.Text += ".";
                }
                else
                {
                    progressCount = 0;
                }
                progressCount++;
            }
            else if(switchButton1.Value == false)
            {
                circularProgress1.IsRunning = false;
                circularProgress2.IsRunning = false;
                label18.Text = "Not Working";
                remote.Text = "Not Working";
            }
        }
    }
}
