using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;


namespace angle_control
{
    public partial class Form1 : Form
    {
        private StringBuilder sb = new StringBuilder();
        private long receive_count = 0;
        public string record;
        Form2 f2 = new Form2();
        Form4 f4 = new Form4();
        public string send_insruction="";//to receive the instuction
        Form3 f3 = new Form3();
        double[] Acceleration_x =new double[2];
        double[] Acceleration_y = new double[2];
        double Acceleration_z;
        double[] distance=new double [3];
        double[] velocty =new double[3];
        DenseMatrix A = new DenseMatrix(3, 3);
        DenseMatrix A_2 = new DenseMatrix(3, 3);
        DenseMatrix B = new DenseMatrix(3, 1);
        DenseMatrix C_2 = new DenseMatrix(3,3);
        DenseMatrix C = new DenseMatrix(3, 1);
        int flag = 0;


        public Form1()
        {
            InitializeComponent();
            
        //serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //for(int i=300;i<=38400;i=i*2)
            //{
            //    comboBox2.Items.Add(i.ToString());
            //}
            //string[] baud = { "43000", "56000", "57600", "115200", "128000", "230400", "256000", "460800" };
            //comboBox2.Items.AddRange(baud);
            A[0, 0] = 1;
            A[1, 1] = 1;
            A[2, 2] = 1;

            f2.TopLevel = false;
            f3.TopLevel = false;
            f4.TopLevel = false;
            //f2.Parent = panel1;
            this.panel1.Controls.Add(f2);
            this.panel1.Controls.Add(f3);
            this.panel1.Controls.Add(f4);
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
            button1.BackColor = Color.SteelBlue;

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
                    button1.BackColor = Color.SteelBlue;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    f2.textBox1.Text = "";
                    send_insruction = "";

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
                    button1.BackColor = Color.Gray;

                }
            }
            catch (Exception ex)
            {
                serialPort1 = new System.IO.Ports.SerialPort();
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                System.Media.SystemSounds.Beep.Play();//警告提示音
                button1.Text = "打开串口";
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
                button1.BackColor = Color.SteelBlue;

            }
            }



        private void Send()
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
                    string[] input = send_insruction.Split();
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
                        byte[] CRC = Crc.GetModbusCrc16(bytesend_6);
                        for (int i = 0; i < 6; i++)
                        {
                            bytesend[i] = Convert.ToByte(input[i], 16);
                        }
                        for (int i = 6; i < 8; i++)
                        {
                            bytesend[i] = CRC[i - 6];
                        }
                    }

                    send_insruction = BitConverter.ToString(bytesend);
                    send_insruction = send_insruction.Replace('-', ' ');
                    //串口处于开启状态，将发送区文本发送
                    serialPort1.Write(bytesend, 0, bytesend.Length);
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
                button1.BackColor = Color.SteelBlue;
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

                        //A[0, 0] = Math.Cos(angle_velocity_z)*Math.Cos(angle_velocity_y);
                        //A[0, 1] = Math.Cos(angle_velocity_z) * Math.Sin(angle_velocity_y) * Math.Sin(angle_velocity_x) - Math.Sin(angle_velocity_z) * Math.Cos(angle_velocity_y);
                        //A[0, 2] = Math.Cos(angle_velocity_z) * Math.Sin(angle_velocity_y) * Math.Cos(angle_velocity_x) + Math.Sin(angle_velocity_z) * Math.Cos(angle_velocity_z);
                        //A[1, 0] = Math.Sin(angle_velocity_z) * Math.Cos(angle_velocity_y);
                        //A[1, 1] = Math.Sin(angle_velocity_z) * Math.Sin(angle_velocity_y) * Math.Sin(angle_velocity_x) + Math.Cos(angle_velocity_z) * Math.Cos(angle_velocity_x);
                        //A[1, 2] = Math.Sin(Az) * Math.Sin(angle_velocity_y) * Math.Cos(angle_velocity_x) - Math.Cos(Az) * Math.Sin(angle_velocity_x);
                        //A[2, 0] = -Math.Sin(angle_velocity_y);
                        //A[2, 1] = Math.Cos(angle_velocity_y) * Math.Sin(angle_velocity_x);
                        //A[2, 2] = Math.Cos(angle_velocity_y) * Math.Cos(angle_velocity_x);
                        //B[0,0] = Ax;
                        //B[1,0] = Ay;
                        //B[2,0] = Az;
                        //C = A * B;


                        //if (f3.chart1.Series[0].Points.Count == 100)
                        //{
                        //    f3.chart1.ChartAreas[0].AxisX.Maximum = currentCount;
                        //    f3.chart1.ChartAreas[0].AxisX.Minimum = currentCount - 50;
                        //    for (int i = 0; i < 99; i++)
                        //    {
                        //        f3.chart1.Series[0].Points[i] = f3.chart1.Series[0].Points[i + 1];
                        //        f3.chart1.Series[1].Points[i] = f3.chart1.Series[1].Points[i + 1];
                        //        f3.chart1.Series[2].Points[i] = f3.chart1.Series[2].Points[i + 1];
                        //    }
                        //    f3.chart1.Series[0].Points.RemoveAt(0);
                        //    f3.chart1.Series[0].Points.AddXY(currentCount, C[0,0]);
                        //    f3.chart1.Series[1].Points.RemoveAt(0);
                        //    f3.chart1.Series[1].Points.AddXY(currentCount, C[1, 0]);
                        //    f3.chart1.Series[2].Points.RemoveAt(0);
                        //    f3.chart1.Series[2].Points.AddXY(currentCount, C[2, 0]);



                        //}
                        //else
                        //{
                        //    f3.chart1.Series[0].Points.AddXY(currentCount, C[0, 0]);
                        //    f3.chart1.Series[1].Points.AddXY(currentCount, C[1, 0]);
                        //    f3.chart1.Series[2].Points.AddXY(currentCount, C[2, 0]);
                        //}


                    if (record_strs.Length == 12&& send_insruction== "50 03 00 3D 00 03 99 86")
                    {

                        Ax = (short)(((short)byteget[3] << 8) | byteget[4]) / (double)32768 * (double)180;
                        Ay = (short)(((short)byteget[5] << 8) | byteget[6]) / (double)32768 * (double)180;
                        Az = (short)(((short)byteget[7] << 8) | byteget[8]) / (double)32768 * (double)180;
                        B[0, 0] = Ax;
                        B[1, 0] = Ay;
                        B[2, 0] = Az;
                        if( flag==1)
                        {
                            A_2[0, 0] = Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
                            A_2[0, 1] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) - Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
                            A_2[0, 2] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) + Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
                            A_2[1, 0] = Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
                            A_2[1, 1] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) + Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
                            A_2[1, 2] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) - Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
                            A_2[2, 0] = -Math.Sin(Ay * (Math.PI / 180));
                            A_2[2, 1] = Math.Cos(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
                            A_2[2, 2] = Math.Cos(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
                            C_2 = (DenseMatrix)(A.Transpose() * A_2);
                            C_2 = (DenseMatrix)(C_2.Transpose());
                            //C = (DenseMatrix)(A.Transpose() * B);

                            B[1, 0] = Math.Atan2(-C_2[2, 0], Math.Sqrt(Math.Pow(C_2[0, 0], 2) + Math.Pow(C_2[1, 0], 2))) * (180 / Math.PI);
                            B[2, 0] = Math.Atan2(C_2[1, 0] / Math.Cos(B[1, 0] * (Math.PI / 180)), C_2[0, 0] / Math.Cos(B[1, 0] * (Math.PI / 180))) * (180 / Math.PI);
                            B[0, 0] = Math.Atan2(C_2[2, 1] / Math.Cos(B[1, 0] * (Math.PI / 180)), C_2[2, 2] / Math.Cos(B[1, 0] * (Math.PI / 180))) * (180 / Math.PI);
                        }

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
                            f3.chart1.Series[0].Points.AddXY(currentCount, B[0, 0]);
                            f3.chart1.Series[1].Points.RemoveAt(0);
                            f3.chart1.Series[1].Points.AddXY(currentCount, B[1, 0]);
                            f3.chart1.Series[2].Points.RemoveAt(0);
                            f3.chart1.Series[2].Points.AddXY(currentCount, B[2, 0]);



                        }
                        else
                        {
                            f3.chart1.Series[0].Points.AddXY(currentCount, B[0, 0]);
                            f3.chart1.Series[1].Points.AddXY(currentCount, B[1, 0]);
                            f3.chart1.Series[2].Points.AddXY(currentCount, B[2, 0]);
                        }
                    }
                    if (record_strs.Length == 12 && send_insruction == "50 03 00 34 00 03 49 84")
                    {
                        f3.chart1.ChartAreas[0].AxisY.Maximum = 30;
                        f3.chart1.ChartAreas[0].AxisY.Minimum = -30;

                        Acceleration_x[1] = (short)(((short)byteget[3] << 8) | byteget[4]) / (double)32768 * (double)(16 * 9.8);
                        if (Acceleration_x[1] < 0.5 && -0.5 < Acceleration_x[1])
                        {
                            Acceleration_x[1] = 0;
                        }


                        Acceleration_y[1] = (short)(((short)byteget[5] << 8) | byteget[6]) / (double)32768 * (double)(16 * 9.8);
                        if (Acceleration_y[1] < 0.5 && -0.5 < Acceleration_y[1])
                        {
                            Acceleration_y[1] = 0;
                        }
                        if (Acceleration_x[0] == 0&& Acceleration_x[1] == 0)
                        {
                            velocty[0] = 0;
                        }
                        if (Acceleration_y[0] == 0&& Acceleration_y[1] == 0)
                        {
                            velocty[1] = 0;
                        }
                        Acceleration_z = (short)(((short)byteget[7] << 8) | byteget[8]) / (double)32768 * (double)(16 * 9.8)-9.87;
                        
                        distance[0] = distance[0] + 0.1 * velocty[0]+5/3* (Acceleration_x[1] - Acceleration_x[0]) * Math.Pow(0.1,3)+0.1* Acceleration_x[0]*0.01;
                        velocty[0] = velocty[0] + (Acceleration_x[1] + Acceleration_x[0]) * 0.1 / 2;
                        Acceleration_x[0] = Acceleration_x[1];
                        Debug.WriteLine("速度{0}", velocty[0]);
                        Debug.WriteLine("加速度{0}", Acceleration_x[0]);
                        Debug.WriteLine("路程{0}", distance[0]);
                        distance[1] = distance[1] + 0.1 * velocty[1] + 5/3 * (Acceleration_y[1] - Acceleration_y[0]) * Math.Pow(0.1, 3) + 0.5 * Acceleration_y[0]*0.01;
                        velocty[1] = velocty[1] + (Acceleration_y[1] + Acceleration_y[0]) * 0.1 / 2;
                        Acceleration_y[0] = Acceleration_y[1];

                       f4.chart1.Series[0].Points.AddXY(currentCount, distance[0]);
                        f4.chart1.Series[1].Points.AddXY(currentCount, distance[1]);

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
                            f3.chart1.Series[0].Points.AddXY(currentCount, Acceleration_x[1]);
                            f3.chart1.Series[1].Points.RemoveAt(0);
                            f3.chart1.Series[1].Points.AddXY(currentCount, Acceleration_y[1]);
                            f3.chart1.Series[2].Points.RemoveAt(0);
                            f3.chart1.Series[2].Points.AddXY(currentCount, Acceleration_z);



                        }
                        else
                        {
                            f3.chart1.Series[0].Points.AddXY(currentCount, Acceleration_x[1]);
                            f3.chart1.Series[1].Points.AddXY(currentCount, Acceleration_y[1]);
                            f3.chart1.Series[2].Points.AddXY(currentCount, Acceleration_z);
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


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            

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

                Send();
            //    button2.PerformClick();
            if (send_insruction == "50 03 00 3D 00 03 99 86")
            {
                角度ToolStripMenuItem.PerformClick();

            }
            if (send_insruction == "50 03 00 34 00 03 49 84 ")
            {
                加速度ToolStripMenuItem1.PerformClick(); 

            }

            //(short)record[3];





        }



        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void 角度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            send_insruction = "50 03 00 3D 00 03 99 86";
            f3.chart1.ChartAreas[0].AxisY.Maximum = 180;
            f3.chart1.ChartAreas[0].AxisY.Minimum = -180;

            //button2.PerformClick();
            Send();
        }

        private void 参考系修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            send_insruction = "50 06 00 01 00 08 D4 4D ";
            //button2.PerformClick();
            Send();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (send_insruction != "")
            {
                if (this.timer1.Enabled == false)
                {
                    this.timer1.Enabled = true;
                    timer1.Start();
                    button3.Text = "停止";
                    button3.BackColor = Color.Gray;
                }
                else
                {
                    this.timer1.Enabled = false;
                    timer1.Stop();
                    button3.Text = "开始";
                    button3.BackColor = Color.SteelBlue;
                }
            }
            else
            {
                MessageBox.Show("请先选择功能");
            }



            
        }

        private void 数据ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void 角度ToolStripMenuItem1_Click(object sender, EventArgs e)
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
        private void 加速度ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void 加速度ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            send_insruction = "50 03 00 34 00 03 49 84";
            Send();
        }

        private void 轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //panel1.Controls.Clear();
            //f2.Visible = false;
            f2.Visible = false;
            f3.Visible = false;
            f4.TopLevel = false;
            //f2.Parent = panel1;
            //this.panel1.Controls.Add(f3);
            f4.FormBorderStyle = FormBorderStyle.None;
            f4.Dock = DockStyle.Fill;
            f4.Show();
        }

        private void 轨迹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            send_insruction = "50 03 00 34 00 03 49 84";
            Acceleration_x[0] = 0;
            Acceleration_x[1] = 0;
            Acceleration_y[0] = 0;
            Acceleration_y[1] = 0;
            velocty[0] = 0;
            velocty[1] = 0;
            distance[0] = 0;
            distance[1] = 0;
            Send();
        }

        private void 参考系修改自制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            send_insruction = "50 03 00 3D 00 03 99 86";
            //A[0, 0] = Math.Cos(Az) * Math.Cos(Ay);
            //A[0, 1] = Math.Cos(Az) * Math.Sin(Ay) * Math.Sin(Ax) - Math.Sin(Az) * Math.Cos(Ay);
            //A[0, 2] = Math.Cos(Az) * Math.Sin(Ay) * Math.Cos(Ax) + Math.Sin(Az) * Math.Cos(Az);
            //A[1, 0] = Math.Sin(Az) * Math.Cos(Ay);
            //A[1, 1] = Math.Sin(Az) * Math.Sin(Ay) * Math.Sin(Ax) + Math.Cos(Az) * Math.Cos(Ax);
            //A[1, 2] = Math.Sin(Az) * Math.Sin(Ay) * Math.Cos(Ax) - Math.Cos(Az) * Math.Sin(Ax);
            //A[2, 0] = -Math.Sin(Ay);
            //A[2, 1] = Math.Cos(Ay) * Math.Sin(Ax);
            //A[2, 2] = Math.Cos(Ay) * Math.Cos(Ax);

            //A[0, 0] = Math.Cos(Ax) * Math.Cos(Ay);
            //A[0, 1] = -Math.Cos(Ay)*Math.Sin(Ax);
            //A[0, 2] = Math.Sin(Ay);
            //A[1, 0] = Math.Sin(Az) * Math.Sin(Ay) * Math.Cos(Ax) +Math.Cos(Az) * Math.Sin(Ax);
            //A[1, 1] = -Math.Sin(Az) * Math.Sin(Ay) * Math.Sin(Ax) + Math.Cos(Az) * Math.Cos(Ax);
            //A[1, 2] = - Math.Cos(Ay) * Math.Sin(Az);
            //A[2, 0] = -Math.Cos(Az)*Math.Sin(Ay)*Math.Cos(Ax)+Math.Sin(Az)*Math.Sin(Ax);
            //A[2, 1] = Math.Cos(Az) * Math.Sin(Ax) * Math.Sin(Ay)+Math.Sin(Az)*Math.Cos(Ax);
            //A[2, 2] = Math.Cos(Ay) * Math.Cos(Az);

            A[0, 0] = Math.Cos(Az*(Math.PI/180)) * Math.Cos(Ay * (Math.PI / 180));
            A[0, 1] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) - Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            A[0, 2] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) + Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            A[1, 0] = Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
            A[1, 1] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) + Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            A[1, 2] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) - Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            A[2, 0] = -Math.Sin(Ay * (Math.PI / 180));
            A[2, 1] = Math.Cos(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            A[2, 2] = Math.Cos(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            flag = 1;
            Send();
            
            
        }

    }
    }


