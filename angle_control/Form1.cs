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

namespace angle_control
{
    public partial class Form1 : Form
    {
        private StringBuilder sb = new StringBuilder();
        private long receive_count = 0;
        public string record;
        Form2 f2 = new Form2();
        Form3 f3 = new Form3();
        public Form1()
        {
            InitializeComponent();
            
        serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //for(int i=300;i<=38400;i=i*2)
            //{
            //    comboBox2.Items.Add(i.ToString());
            //}
            //string[] baud = { "43000", "56000", "57600", "115200", "128000", "230400", "256000", "460800" };
            //comboBox2.Items.AddRange(baud);
            f2.TopLevel = false;
            f3.TopLevel = false;
            //f2.Parent = panel1;
            this.panel1.Controls.Add(f2);
            this.panel1.Controls.Add(f3);
            f3.Visible = false;
            f2.FormBorderStyle = FormBorderStyle.None;
            f2.Dock = DockStyle.Fill;
            f2.Show();
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox2.Text = Convert.ToString(serialPort1.BaudRate);
            comboBox3.Text = Convert.ToString(serialPort1.DataBits);
            comboBox4.Text= Convert.ToString(serialPort1.Parity);
            comboBox5.Text = Convert.ToString(serialPort1.StopBits);

            //comboBox3.Text = "8";
            //comboBox4.Text = "None";
            //comboBox5.Text = "1";
            button1.BackColor = Color.Green;

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    button1.Text = "打开串口";
                    button1.BackColor = Color.Green;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    f2.textBox1.Text = "";
                    textBox2.Text = "";

                }
                else
                {
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;  
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.DataBits = Convert.ToInt16(comboBox3.Text);


                    serialPort1.Open();

                    button1.Text = "关闭串口";
                    button1.BackColor = Color.Green;

                }
            }
            catch (Exception ex)
            {
                serialPort1 = new System.IO.Ports.SerialPort();
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                System.Media.SystemSounds.Beep.Play();//警告提示音
                button1.Text = "打开串口";
                button1.BackColor = Color.Gray;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
                button1.BackColor = Color.Green;

            }
            }



        private void button2_Click_1(object sender, EventArgs e)
        {
            record = "";
            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    f2.textBox1.AppendText("\r\n");
                    byte[] bytesend = new byte[8];
                    byte[] bytesend_6 = new byte[6];
                    string[] input = textBox2.Text.Split();
                    if (input.Length == 8)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            bytesend[i] = Convert.ToByte(input[i], 16);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            bytesend_6[i] = Convert.ToByte(input[i], 16);
                            //textBox2.AppendText(BitConverter.ToString(Crc.GetModbusCrc16(bytesend_6)[0]));
                        }
                        byte[] CRC =Crc.GetModbusCrc16(bytesend_6);
                        for (int i = 0; i < 6; i++)
                        {
                            bytesend[i] = Convert.ToByte(input[i], 16);
                        }
                        for (int i = 6; i < 8; i++)
                        {
                            bytesend[i] = CRC[i - 6];
                        }
                    }
                    
                    textBox2.Text= BitConverter.ToString(bytesend);
                    textBox2.Text = textBox2.Text.Replace('-', ' ');
                    //串口处于开启状态，将发送区文本发送
                    serialPort1.Write(bytesend,0, bytesend.Length);
                }
            }
            catch (Exception ex)
            {
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.Green;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
            }
        }

        public void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {


            int num = serialPort1.BytesToRead;      //获取接收缓冲区中的字节数
            byte[] received_buf = new byte[num];    //声明一个大小为num的字节数据用于存放读出的byte型数据
            
            receive_count += num;                   //接收字节计数变量增加nun
            serialPort1.Read(received_buf, 0, num);   //读取接收缓冲区中num个字节到byte数组中
                                                      //接第二步中的代码
            sb.Clear();     //防止出错,首先清空字符串构造器
                            //遍历数组进行字符串转化及拼接
            foreach (byte b in received_buf)
            {
                sb.Append(b.ToString("X2") +' ');
            }
            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                Invoke((EventHandler)(delegate
                {
                    f2.textBox1.AppendText(sb.ToString());
                    record+=sb.ToString();
                    string[] record_strs = record.Split();
                    byte[] byteget = new byte[record_strs.Length - 1];
                    for (int i = 0; i < record_strs.Length - 1; i++)
                    {
                        byteget[i] = Convert.ToByte(record_strs[i], 16);
                    }

                    //(short)record[3];
                    //short bs=(short)byteget[3];
                    //int bs_2 = (short)byteget[3]<<8;

                    //byte by = byteget[3];
                    //bs = (short)byteget[4];
                    //bs_2 = ((short)byteget[3]<<8)| byteget[4] ;

                    if (record_strs.Length == 12)
                    {

                        Ax = (short)(((short)byteget[3] << 8) | byteget[4]) / (double)32768 * (double)180;
                        Ay = (short)(((short)byteget[5] << 8) | byteget[6]) / (double)32768 * (double)180;
                        Az = (short)(((short)byteget[7] << 8) | byteget[8]) / (double)32768 * (double)180;
                        if (f3.chart1.Series[0].Points.Count == 100)
                        {
                            f3.chart1.ChartAreas[0].AxisX.Maximum = currentCount;
                            f3.chart1.ChartAreas[0].AxisX.Minimum = currentCount - 50;
                            for (int i = 0; i < 99; i++)
                            {
                                f3.chart1.Series[0].Points[i] = f3.chart1.Series[0].Points[i + 1];
                                f3.chart1.Series[1].Points[i] = f3.chart1.Series[1].Points[i + 1];
                                f3.chart1.Series[2].Points[i] = f3.chart1.Series[2].Points[i + 1];
                            }
                            f3.chart1.Series[0].Points.RemoveAt(0);
                            f3.chart1.Series[0].Points.AddXY(currentCount, Ax);
                            f3.chart1.Series[1].Points.RemoveAt(0);
                            f3.chart1.Series[1].Points.AddXY(currentCount, Ay);
                            f3.chart1.Series[2].Points.RemoveAt(0);
                            f3.chart1.Series[2].Points.AddXY(currentCount, Az);



                        }
                        else
                        {
                            f3.chart1.Series[0].Points.AddXY(currentCount, Ax); 
                            f3.chart1.Series[1].Points.AddXY(currentCount, Ay);
                            f3.chart1.Series[2].Points.AddXY(currentCount, Az);
                        }
                    }
                }
                  )
                );
            }
            catch
            {

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
        public class Crc
        {

            /// <summary>
            /// 判断数据中crc是否正确
            /// </summary>
            /// <param name="datas">传入的数据后两位是crc</param>
            /// <returns></returns>
            public static bool IsCrcOK(byte[] datas)
            {
                int length = datas.Length - 2;

                byte[] bytes = new byte[length];
                Array.Copy(datas, 0, bytes, 0, length);
                byte[] getCrc = GetModbusCrc16(bytes);

                if (getCrc[0] == datas[length] && getCrc[1] == datas[length + 1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// 传入数据添加两位crc
            /// </summary>
            /// <param name="datas"></param>
            /// <returns></returns>
            public static byte[] GetCRCDatas(byte[] datas)
            {
                int length = datas.Length;
                byte[] crc16 = GetModbusCrc16(datas);
                byte[] crcDatas = new byte[length + 2];
                Array.Copy(datas, crcDatas, length);
                Array.Copy(crc16, 0, crcDatas, length, 2);
                return crcDatas;
            }
            public static byte[] GetModbusCrc16(byte[] bytes)
            {
                byte crcRegister_H = 0xFF, crcRegister_L = 0xFF;// 预置一个值为 0xFFFF 的 16 位寄存器

                byte polynomialCode_H = 0xA0, polynomialCode_L = 0x01;// 多项式码 0xA001

                for (int i = 0; i < bytes.Length; i++)
                {
                    crcRegister_L = (byte)(crcRegister_L ^ bytes[i]);

                    for (int j = 0; j < 8; j++)
                    {
                        byte tempCRC_H = crcRegister_H;
                        byte tempCRC_L = crcRegister_L;

                        crcRegister_H = (byte)(crcRegister_H >> 1);
                        crcRegister_L = (byte)(crcRegister_L >> 1);
                        // 高位右移前最后 1 位应该是低位右移后的第 1 位：如果高位最后一位为 1 则低位右移后前面补 1
                        if ((tempCRC_H & 0x01) == 0x01)
                        {
                            crcRegister_L = (byte)(crcRegister_L | 0x80);
                        }

                        if ((tempCRC_L & 0x01) == 0x01)
                        {
                            crcRegister_H = (byte)(crcRegister_H ^ polynomialCode_H);
                            crcRegister_L = (byte)(crcRegister_L ^ polynomialCode_L);
                        }
                    }
                }

                return new byte[] { crcRegister_L, crcRegister_H };
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //panel1.Controls.Clear();
            f3.Visible = false;
            f2.TopLevel = false;
            //f2.Parent = panel1;
            //this.panel1.Controls.Add(f2);
            f2.FormBorderStyle = FormBorderStyle.None;
            f2.Dock = DockStyle.Fill;
            f2.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //panel1.Controls.Clear();
            f2.Visible = false;
            f3.TopLevel = false;
            //f2.Parent = panel1;
            //this.panel1.Controls.Add(f3);
            f3.FormBorderStyle = FormBorderStyle.None;
            f3.Dock = DockStyle.Fill;
            f3.Show();
        }

        //void chart_display()
        //{
        //    Draw();
        //}
        //delegate void my_delegate();//创建一个代理,图表刷新需要在主线程，所以需要加委托
        //Queue<double> Q1 = new Queue<double>();
        //public void Draw()
        //{
        //    if (!f3.chart1.InvokeRequired)
        //    {
             
        //        this.f3.chart1.Series["line1"].Points.Clear();
        //        for (int i = 0; i < FFF_chart.Length; i++)
        //        {
        //            f3.chart1.Series["line1"].Points.AddXY(i, FFF_chart[i]);
        //        }
        //    }
        //    else
        //    {
        //        my_delegate delegate_FFF = new my_delegate(Draw);
        //        Invoke(delegate_FFF, new object[] { });//执行唤醒操作
        //    }
        //}
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Thread chart_display_th = new Thread(new ThreadStart(chart_display));////////数值显示线程
            //chart_display_th.Start();
            //while( toolStripButton4.Click)
            //{ }
            //string[] record_strs = record.Split();
            //byte[] byteget = new byte[record_strs.Length-1];
            //    for (int i = 0; i < record_strs.Length-1; i++)
            //    {
            //        byteget[i] = Convert.ToByte(record_strs[i], 16);
            //    }

            ////(short)record[3];
            ////short bs=(short)byteget[3];
            ////int bs_2 = (short)byteget[3]<<8;

            ////byte by = byteget[3];
            ////bs = (short)byteget[4];
            ////bs_2 = ((short)byteget[3]<<8)| byteget[4] ;

            //if (record_strs.Length==12)
            //{
            //    Ax = (double)(((short)byteget[3] << 8) | byteget[4]) / (double)32768 * (double)180;
            //    double Ay = (((short)byteget[5] << 8) | byteget[6]) / 32768 * 180;
            //    double Az = (((short)byteget[7] << 8) | byteget[8]) / 32768 * 180;
            //    if (f3.chart1.Series[0].Points.Count == 100)
            //    {
            //        f3.chart1.ChartAreas[0].AxisX.Maximum = currentCount;
            //        f3.chart1.ChartAreas[0].AxisX.Minimum = currentCount - 50;
            //        for (int i = 0; i < 99; i++)
            //        {
            //            f3.chart1.Series[0].Points[i] = f3.chart1.Series[0].Points[i + 1];
            //        }
            //        f3.chart1.Series[0].Points.RemoveAt(0);
            //        f3.chart1.Series[0].Points.AddXY(currentCount, Ax);



            //    }
            //    else
            //    {
            //        f3.chart1.Series[0].Points.AddXY(currentCount, Ax);
            //    }
            //}

            if (this.timer1.Enabled==false)
            {
                this.timer1.Enabled = true;
                timer1.Start();
            }

            //f3.chart1.Series[0].Points.AddXY()
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        public int currentCount=0;
        public double Ax;
        public double Ay;
        public double Az;
        public void timer1_Tick(object sender, EventArgs e)
        {

                currentCount += 1;

                button2.PerformClick();
                toolStripButton3.PerformClick();
                //string[] record_strs = record.Split();
                //byte[] byteget = new byte[record_strs.Length - 1];
                //button2.PerformClick();

                //for (int i = 0; i < record_strs.Length - 1; i++)
                //{
                //    byteget[i] = Convert.ToByte(record_strs[i], 16);
                //}
                //Ax = (((short)byteget[3] << 8) | byteget[4]) / 32768 * 180;


            //(short)record[3];


                               

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "50 03 00 3D 00 03 99 86";
        }
    }
    }


