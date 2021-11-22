﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.LinearAlgebra.Double;


namespace angle_control
{
    public partial class Form1 : Form
    {
        private StringBuilder sb = new StringBuilder();
        private long receive_count = 0;
        public string record;
        Form2 f2 = new Form2();
        Form3 f3 = new Form3();
        Form4 f4 = new Form4();
        Form5 f5 = new Form5();
        Form6 f6 = new Form6();

        public string send_instruction = "";//to receive the instuction
        double[] Acceleration_x = new double[2];
        double[] Acceleration_y = new double[2];
        double Acceleration_z;
        double[] distance = new double[3];
        double[] velocty = new double[3];
        DenseMatrix A = new DenseMatrix(3, 3);
        DenseMatrix A_2 = new DenseMatrix(3, 3);
        DenseMatrix B = new DenseMatrix(3, 1);
        DenseMatrix C_2 = new DenseMatrix(3, 3);
        DenseMatrix time= new DenseMatrix(3, 2);
        float[] data_64_x = new float[64];
        int time_aceleration=0;//记录加速度次数
        double time_span_x=0;
        double[] time_span_x_64 = new double[64];



        int flag = 0;
        int send_get = 0;
        int time_flag = 0;
        int time_zero_x = 0;
        int time_zero_y = 0;
        //int count=0;


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
            //f3.chart1.ChartAreas[0].AxisY.Maximum = 0.2;
            //f3.chart1.ChartAreas[0].AxisY.Minimum = -0.2;
            f4.chart1.ChartAreas[0].AxisY.Maximum = 1;
            f4.chart1.ChartAreas[0].AxisY.Minimum = -1;
            A[0, 0] = 1;
            A[1, 1] = 1;
            A[2, 2] = 1;

            f2.TopLevel = false;
            f3.TopLevel = false;
            f4.TopLevel = false;
            f5.TopLevel = false;
            f6.TopLevel = false;

            this.panel1.Controls.Add(f2);
            //this.panel1.Controls.Add(f3);
            //this.panel1.Controls.Add(f4);
            //this.panel1.Controls.Add(f5);
            //f3.Visible = false;
            f2.FormBorderStyle = FormBorderStyle.None;
            f2.Dock = DockStyle.Fill;
            f2.Show();
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox2.Text = Convert.ToString(serialPort1.BaudRate);
            comboBox3.Text = Convert.ToString(serialPort1.DataBits);
            comboBox4.Text = Convert.ToString(serialPort1.Parity);
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
                    send_instruction = "";

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
                    string[] input = send_instruction.Split();
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

                    send_instruction = BitConverter.ToString(bytesend);
                    send_instruction = send_instruction.Replace('-', ' ');
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
                sb.Append(b.ToString("X2") + ' ');
            }


            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                Invoke((EventHandler)(delegate
                {
                    f2.textBox1.AppendText(sb.ToString());
                    record += sb.ToString();
                    string[] record_strs = record.Split();
                    byte[] byteget = new byte[record_strs.Length - 1];
                        //if (record_strs.Length == 12)

                        for (int i = 0; i < record_strs.Length - 1; i++)
                    {
                        byteget[i] = Convert.ToByte(record_strs[i], 16);
                    }

                    if (record_strs.Length == 12 && Crc.IsCrcOK(byteget))
                    {
                        if(send_instruction== "50 03 00 31 00 03 59 85")
                        {
                            send_get++;
                                time[0, time_flag ] = byteget[6];//分
                                time[1, time_flag] = byteget[5];//秒
                                time[2, time_flag ] = (short)(((short)byteget[7] << 8) | byteget[8]);




                           // Debug.WriteLine("分：{1}秒：{0}毫秒:{2}", time[0, time_flag], time[1, time_flag], time[2, time_flag]) ;
                        }
                        if (send_instruction == "50 03 00 3D 00 03 99 86")
                        {
                            send_get++;

                           // Debug.WriteLine("angle get:time:{0}",currentCount);
                            Ax = (short)(((short)byteget[3] << 8) | byteget[4]) / (double)32768 * (double)180;
                            Ay = (short)(((short)byteget[5] << 8) | byteget[6]) / (double)32768 * (double)180;
                            Az = (short)(((short)byteget[7] << 8) | byteget[8]) / (double)32768 * (double)180;


                            //A_2[0, 0] = Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
                            //A_2[0, 1] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) - Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
                            //A_2[0, 2] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) + Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
                            //A_2[1, 0] = Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
                            //A_2[1, 1] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) + Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
                            //A_2[1, 2] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) - Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
                            //A_2[2, 0] = -Math.Sin(Ay * (Math.PI / 180));
                            //A_2[2, 1] = Math.Cos(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
                            //A_2[2, 2] = Math.Cos(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
                            A_2 = angle_ola_zyx(Ax, Ay, Az);

                            B[0, 0] = Ax;
                            B[1, 0] = Ay;
                            B[2, 0] = Az;
                            if (flag == 1)
                            {

                                C_2 = (DenseMatrix)(A.Transpose() * A_2);
                                C_2 = (DenseMatrix)(C_2.Transpose());
                                    //C = (DenseMatrix)(A.Transpose() * B);

                                    B[1, 0] = Math.Atan2(-C_2[2, 0], Math.Sqrt(Math.Pow(C_2[0, 0], 2) + Math.Pow(C_2[1, 0], 2))) * (180 / Math.PI);
                                B[2, 0] = Math.Atan2(C_2[1, 0] / Math.Cos(B[1, 0] * (Math.PI / 180)), C_2[0, 0] / Math.Cos(B[1, 0] * (Math.PI / 180))) * (180 / Math.PI);
                                B[0, 0] = Math.Atan2(C_2[2, 1] / Math.Cos(B[1, 0] * (Math.PI / 180)), C_2[2, 2] / Math.Cos(B[1, 0] * (Math.PI / 180))) * (180 / Math.PI);
                            }

                            if (f5.chart1.Series[0].Points.Count == 100)
                            {
                                f5.chart1.ChartAreas[0].AxisX.Maximum = currentCount;
                                f5.chart1.ChartAreas[0].AxisX.Minimum = currentCount - 50;
                                for (int i = 0; i < 99; i++)
                                {
                                    f5.chart1.Series[0].Points[i] = f5.chart1.Series[0].Points[i + 1];
                                    f5.chart1.Series[1].Points[i] = f5.chart1.Series[1].Points[i + 1];
                                    f5.chart1.Series[2].Points[i] = f5.chart1.Series[2].Points[i + 1];
                                }
                                f5.chart1.Series[0].Points.RemoveAt(0);
                                f5.chart1.Series[0].Points.AddXY(currentCount, B[0, 0]);
                                f5.chart1.Series[1].Points.RemoveAt(0);
                                f5.chart1.Series[1].Points.AddXY(currentCount, B[1, 0]);
                                f5.chart1.Series[2].Points.RemoveAt(0);
                                f5.chart1.Series[2].Points.AddXY(currentCount, B[2, 0]);



                            }
                            else
                            {
                                f5.chart1.Series[0].Points.AddXY(currentCount, B[0, 0]);
                                f5.chart1.Series[1].Points.AddXY(currentCount, B[1, 0]);
                                f5.chart1.Series[2].Points.AddXY(currentCount, B[2, 0]);
                            }
                        }
                        if (send_instruction == "50 03 00 34 00 03 49 84")
                        {
                            send_get = 0;
                            time_flag = 0;



                          //  Debug.WriteLine("accelerate get:time:{0}",currentCount);



                            B = delete_grativy(Ax, Ay, Az);


                            //Acceleration_x[1] = Math.Cos(B[2,0]*Math.PI/180)*((short)(((short)byteget[3] << 8) | byteget[4]) / (double)32768 * (double)(16 * 9.8) - A_2[0, 0])- Math.Sin(B[2, 0] * Math.PI / 180)*((short)(((short)byteget[5] << 8) | byteget[6]) / (double)32768 * (double)(16 * 9.8) - A_2[1, 0]);
                            Acceleration_x[1] =  (short)(((short)byteget[3] << 8) | byteget[4]) / (double)32768 * (double)(16 * 9.8) - B[0, 0];
                            data_64_x[time_aceleration] = (float)Acceleration_x[1];
                            //Acceleration_x[1] = Math.Round(Acceleration_x[1], 2);
                            Debug.WriteLine("Acceleration_x:{0}",Acceleration_x[1]);





                            Acceleration_y[1] = (short)(((short)byteget[5] << 8) | byteget[6]) / (double)32768 * (double)(16 * 9.8) - B[1, 0];
                            //Acceleration_y[1] = Math.Round(Acceleration_y[1], 2);

                            //Debug.WriteLine("y is{0}", Acceleration_y[1]);

                            Acceleration_z = (short)(((short)byteget[7] << 8) | byteget[8]) / (double)32768 * (double)(16 * 9.8) - B[2, 0];
                            //Debug.WriteLine("Acceleration_z:{0}", Acceleration_z);
                            //if(Acceleration_x[1]<0.01&& Acceleration_x[1] > -0.01)
                            //{
                            //    Acceleration_x[1] = 0;
                            //}
                            //if (Acceleration_y[1] < 0.01 && Acceleration_y[1] > -0.01)
                            //{
                            //    Acceleration_y[1] = 0;
                            //}

                            //if (Acceleration_x[1] < 0.02 && Acceleration_x[1] > -0.02)
                            //{
                            //    Acceleration_x[1] = 0;

                            //    time_zero_x++;
                            //    if (time_zero_x == 10)
                            //    {
                            //        velocty[0] = 0;
                            //        time_zero_x = 0;
                            //    }

                            //}

                            //if (Acceleration_y[1] < 0.02 && Acceleration_y[1] > -0.02)
                            //{
                            //    Acceleration_y[1] = 0;
                            //    time_zero_y++;
                            //    if (time_zero_y == 10)
                            //    {
                            //        velocty[1] = 0;
                            //        time_zero_y = 0;
                            //    }

                            //}


                            if (time[0, 0]==0&& time[1, 0]==0&& time[2, 0]==0)
                            {
                                time[0, 0] = time[0, 1];
                                time[1, 0] = time[1, 1];
                                time[2, 0] = time[2, 1];
                            }


                            double time_span = (time[0,1]-time[0,0])*60+ (time[1, 1] - time[1, 0])+ (time[2, 1] - time[2, 0])/1000;
                            time_span_x = time_span_x + time_span;
                            time_span_x_64[time_aceleration] = time_span_x;
                            //Debug.WriteLine("分：{0}秒：{1}毫秒:{2}", time[0, 0], time[1, 0], time[2, 0]);
                            //Debug.WriteLine("分：{0}秒：{1}毫秒:{2}", time[0, 1], time[1, 1], time[2, 1]);
                            //Debug.WriteLine("分：{0}秒：{1}毫秒:{2}", (time[0, 1] -time[0, 0]) * 60, time[1, 1] - time[1, 0], (time[2, 1] - time[2, 0]) / 1000);
                            Debug.WriteLine(time_span);



















                            time[0, 0] = time[0, 1];
                            time[1, 0] = time[1, 1];
                            time[2, 0] = time[2, 1];



                            Debug.WriteLine("distance:{0}", distance[0]);

                            //distance[0] = distance[0] + time * velocty[0] +  0.5 * Acceleration_x[0] * Math.Pow(time, 2);
                            //velocty[0] = velocty[0] + Acceleration_x[1]* time ;
     //distance[1] = distance[1] + time * velocty[1] + 1 / (6 * time) * (Acceleration_y[1] - Acceleration_y[0]) * Math.Pow(time, 3) + 0.5 * Acceleration_y[0] * Math.Pow(time, 2);
                            //velocty[1] = velocty[1] + (Acceleration_y[1] + Acceleration_y[0]) * time / 2;
                            //Acceleration_y[0] = Acceleration_y[1];
                            //  Debug.WriteLine("Acceleration_y:{0}", Acceleration_y[1]);

                            if (time_span != 0)
                            {
                                distance[0] = distance[0] + time_span * velocty[0] + 1 / (6 * time_span) * (Acceleration_x[1] - Acceleration_x[0]) * Math.Pow(time_span, 3) + 0.5 * Acceleration_x[0] * Math.Pow(time_span, 2);
                                distance[1] = distance[1] + time_span * velocty[1] + 1 / (6 * time_span) * (Acceleration_y[1] - Acceleration_y[0]) * Math.Pow(time_span, 3) + 0.5 * Acceleration_y[0] * Math.Pow(time_span, 2);
                            }
                            Acceleration_x[0] = Acceleration_x[1];
                            Acceleration_y[0] = Acceleration_y[1];
                            velocty[0] = velocty[0] + (Acceleration_x[1] + Acceleration_x[0]) * time_span / 2;
                            velocty[1] = velocty[1] + (Acceleration_y[1] + Acceleration_y[0]) * time_span / 2;
                            //Debug.WriteLine("Acceleration_x:{0}", Acceleration_x[1]);
                            //Debug.WriteLine("Acceleration_y:{0}", Acceleration_y[1]);
                            //Debug.WriteLine("Acceleration_z:{0}", Acceleration_z);


                            // Debug.WriteLine("路程X:{0}", distance [0]);
                            // Debug.WriteLine("路程Y:{0}", distance[1]);


                            f4.chart1.Series[0].Points.AddXY(currentCount, distance[0]);
                            f4.chart1.Series[1].Points.AddXY(currentCount, distance[1]);
                            f6.chart1.Series[0].Points.AddXY(currentCount, velocty[0]);
                            f6.chart1.Series[1].Points.AddXY(currentCount, velocty[1]);
                            if (time_aceleration==63)
                            {
                                FFT a = new FFT();
                                float[] b = a.ResultArr(data_64_x);
                                for (int i=0;i<64;i++)
                                {
                                    if (time_span_x_64[i] != 0)
                                    {
                                        f3.chart1.Series[3].Points.AddXY(time_span_x_64[i], b[i]);
                                        Debug.WriteLine("currentCount:{0} b:{1}", time_span_x_64[i], b[i]);
                                    }


                                }
                            }
                            if(time_span_x!=0)
                            {
                                f3.chart1.Series[0].Points.AddXY(time_span_x, Acceleration_x[1]);
                                Debug.WriteLine("currentCount:{0} Acceleration_x:{1}", time_span_x, Acceleration_x[1]);
                            }

                                //f3.chart1.Series[1].Points.AddXY(currentCount, Acceleration_y[1]);
                                //f3.chart1.Series[2].Points.AddXY(currentCount, Acceleration_z);


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
                            //    f3.chart1.Series[0].Points.AddXY(currentCount, Acceleration_x[1]);
                            //    f3.chart1.Series[1].Points.RemoveAt(0);
                            //    f3.chart1.Series[1].Points.AddXY(currentCount, Acceleration_y[1]);
                            //    f3.chart1.Series[2].Points.RemoveAt(0);
                            //    f3.chart1.Series[2].Points.AddXY(currentCount, Acceleration_z);



                            //}
                            //else
                            //{
                            //    f3.chart1.Series[0].Points.AddXY(currentCount, Acceleration_x[1]);
                            //    f3.chart1.Series[1].Points.AddXY(currentCount, Acceleration_y[1]);
                            //    f3.chart1.Series[2].Points.AddXY(currentCount, Acceleration_z);
                            //}
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


            if (this.timer1.Enabled == false)
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
        public int currentCount = 0;
        public double Ax;
        public double Ay;
        public double Az;
        public int start;

        public void timer1_Tick(object sender, EventArgs e)
        {

            currentCount += 1;

            if (time_flag==0)
            {
                start = currentCount;
                send_instruction = "50 03 00 31 00 03 59 85";
                Send();
                send_get = send_get - 1;
                time_flag++;

            }
            if (send_get==0)//时间
            {
                send_instruction = "50 03 00 31 00 03 59 85";
                Send();
            }
            if (send_get == 1)//角度
            {
                send_instruction = "50 03 00 3D 00 03 99 86";
                Send();
               // Debug.WriteLine("angel:time:{0}", currentCount);
            }



            if (send_get == 2)//加速度
            {
                if (time_aceleration == 63)
                {
                    time_aceleration = 0;
                }
                send_instruction = "50 03 00 34 00 03 49 84 ";
                time_aceleration++;
                Send();
                //Debug.WriteLine("accelerate:time:{0}", currentCount);


            }

            //Debug.WriteLine("currentCount:{0}", currentCount);




            //    button2.PerformClick();
            //if (send_instruction == "50 03 00 3D 00 03 99 86")
            //{
            //    角度ToolStripMenuItem.PerformClick();

            //}
            //if (send_instruction == "50 03 00 34 00 03 49 84 ")
            //{
            //    加速度ToolStripMenuItem1.PerformClick(); 

            //}

            //(short)record[3];





        }



        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void 角度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            send_instruction = "50 03 00 3D 00 03 99 86";
            f3.chart1.ChartAreas[0].AxisY.Maximum = 180;
            f3.chart1.ChartAreas[0].AxisY.Minimum = -180;

            //button2.PerformClick();
            Send();
        }

        private void 参考系修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            send_instruction = "50 06 00 01 00 08 D4 4D ";
            //button2.PerformClick();
            Send();
        }

        private void button3_Click(object sender, EventArgs e)
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
            //f2.Visible = false;
            //f3.TopLevel = false;
            //f2.Parent = panel1;
            //this.panel1.Controls.Add(f3);
            panel1.Controls.Clear();
            f5.TopLevel = false;
            f5.Parent = panel1;
            f5.FormBorderStyle = FormBorderStyle.None;
            f5.Dock = DockStyle.Fill;
            f5.Show();
        }
        private void 加速度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //panel1.Controls.Clear();
            //f2.Visible = false;
            //f3.TopLevel = false;
            //f2.Parent = panel1;
            //this.panel1.Controls.Add(f3);
            panel1.Controls.Clear();
            f3.Parent = panel1;
            f3.TopLevel = false;
            f3.FormBorderStyle = FormBorderStyle.None;
            f3.Dock = DockStyle.Fill;
            f3.Show();
        }
        private void 轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //panel1.Controls.Clear();
            //f2.Visible = false;
            //f2.Visible = false;
            //f3.Visible = false;
            //f5.Visible = false;
            panel1.Controls.Clear();
            f4.Parent = panel1;
            f4.TopLevel = false;
            //f2.Parent = panel1;
            //this.panel1.Controls.Add(f3);
            f4.FormBorderStyle = FormBorderStyle.None;
            f4.Dock = DockStyle.Fill;
            f4.Show();
        }
        private void 速度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            f6.Parent = panel1;
            f6.TopLevel = false;
            //f2.Parent = panel1;
            //this.panel1.Controls.Add(f3);
            f6.FormBorderStyle = FormBorderStyle.None;
            f6.Dock = DockStyle.Fill;
            f6.Show();
        }

        private void 加速度ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            send_instruction = "50 03 00 34 00 03 49 84";
            Send();
        }



        private void 轨迹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            send_instruction = "50 03 00 3D 00 03 99 86";
            A = angle_ola_zyx(Ax, Ay, Az);
            flag = 1;
            button3.PerformClick();

        }

        private void 参考系修改自制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            send_instruction = "50 03 00 3D 00 03 99 86";


            //A[0, 0] = Math.Cos(Az*(Math.PI/180)) * Math.Cos(Ay * (Math.PI / 180));
            //A[0, 1] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) - Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            //A[0, 2] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) + Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            //A[1, 0] = Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
            //A[1, 1] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) + Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            //A[1, 2] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) - Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            //A[2, 0] = -Math.Sin(Ay * (Math.PI / 180));
            //A[2, 1] = Math.Cos(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            //A[2, 2] = Math.Cos(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            A = angle_ola_zyx(Ax, Ay, Az);
            flag = 1;
            Send();


        }
        static DenseMatrix angle_ola_zyx(double Ax, double Ay, double Az)
        {
            DenseMatrix A = new DenseMatrix(3, 3);
            A[0, 0] = Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
            A[0, 1] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) - Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            A[0, 2] = Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) + Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            A[1, 0] = Math.Sin(Az * (Math.PI / 180)) * Math.Cos(Ay * (Math.PI / 180));
            A[1, 1] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180)) + Math.Cos(Az * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            A[1, 2] = Math.Sin(Az * (Math.PI / 180)) * Math.Sin(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180)) - Math.Cos(Az * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            A[2, 0] = -Math.Sin(Ay * (Math.PI / 180));
            A[2, 1] = Math.Cos(Ay * (Math.PI / 180)) * Math.Sin(Ax * (Math.PI / 180));
            A[2, 2] = Math.Cos(Ay * (Math.PI / 180)) * Math.Cos(Ax * (Math.PI / 180));
            return A;
        }
        static DenseMatrix delete_grativy(double Ax, double Ay, double Az)
        {
            DenseMatrix A = new DenseMatrix(3, 3);
            DenseMatrix B = new DenseMatrix(3, 1);


            double z =Az * Math.PI / 180;
            double x = Ax * Math.PI / 180;
            double y = Ay * Math.PI / 180;
            A[0, 0] = Math.Cos(z) * Math.Cos(y);
            A[0, 1] = Math.Cos(z) * Math.Sin(y) * Math.Sin(x) - Math.Sin(z) * Math.Cos(x);
            A[0, 2] = Math.Cos(z) * Math.Sin(y) * Math.Cos(x) + Math.Sin(z) * Math.Sin(x);
            A[1, 0] = Math.Sin(z) * Math.Cos(y);
            A[1, 1] = Math.Sin(z) * Math.Sin(y) * Math.Sin(x) + Math.Cos(z) * Math.Cos(x);
            A[1, 2] = Math.Sin(z) * Math.Sin(y) * Math.Cos(x) - Math.Cos(z) * Math.Sin(x);
            A[2, 0] = -Math.Sin(y);
            A[2, 1] = Math.Cos(y) * Math.Sin(x);
            A[2, 2] = Math.Cos(y) * Math.Cos(x);
            B[0, 0] = 0;
            B[1, 0] = 0;
            B[2, 0] = 1;
            B = (DenseMatrix)(A.Transpose() * B);

            B = B * 9.8;


            return B;
        }
    private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
        


        public void 清零ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            time_aceleration = 0;//记录加速度次数
            Array.Clear(time_span_x_64, 0, time_span_x_64.Length);
            currentCount = 0;
            flag = 0;
            send_get = 0;
            time_flag = 0;
            start = 0;
            time_span_x = 0;
            for (int i = 0; i < 2; i++)
            {
                distance[i] = 0;

            }
            for (int i = 0; i < 2; i++)
            {
                velocty[i] = 0;

            }
            for (int i = 0; i < 2; i++)
            {
                Acceleration_x[i] = 0;

            }
            for (int i = 0; i < 2; i++)
            {
                Acceleration_y[i] = 0;

            }
            foreach (var series in f3.chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in f4.chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in f5.chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in f6.chart1.Series)
            {
                series.Points.Clear();
            }

            flag = 0;
        }

        private void 重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();

        }

        private void 加速度校正ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            send_instruction = "50 06 00 01 00 01 14 4B ";
            Send();
        }

    }

    public class FFT
    {
        MathNet.Numerics.Complex32[] mathNetComplexArrRe = new MathNet.Numerics.Complex32[64];
        float[] resultArr = new float[64];
        public float[] ResultArr(float[] data_64)
        {
            //float[] filterArr = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            float[] filterArr = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            resultArr = Filter(data_64, filterArr);
            return resultArr;

        }
        /// <summary>
        /// 滤波数组
        /// </summary>
        /// <param name="inData">输入的数据</param>
        /// <param name="filterArr">滤波数组，可以自定义</param>
        /// <returns></returns>
        public float[] Filter(float[] inData, float[] filterArr)
        {
            float[] outArr = new float[64];
            outArr = inData;
            MathNet.Numerics.Complex32[] mathNetComplexArr = new MathNet.Numerics.Complex32[64];
            for (int i = 0; i < mathNetComplexArr.Length; i++)
            {
                mathNetComplexArr[i] = new MathNet.Numerics.Complex32((float)outArr[i], 0);
            }
            Fourier.Forward(mathNetComplexArr);//傅里叶变换
            for (int i = 0; i < mathNetComplexArr.Length; i++)
            {
                mathNetComplexArr[i] = new MathNet.Numerics.Complex32(mathNetComplexArr[i].Real * filterArr[i], mathNetComplexArr[i].Imaginary * filterArr[i]);
            }
            float[] ArrFreq = new float[64];
            for (int i = 0; i < ArrFreq.Length; i++)
            {
                ArrFreq[i] = (float)Math.Sqrt(mathNetComplexArr[i].Imaginary * mathNetComplexArr[i].Imaginary + mathNetComplexArr[i].Real * mathNetComplexArr[i].Real);//利用LineRenderer显示频域结果
            }
            Fourier.Inverse(mathNetComplexArr);//逆傅里叶变换
            for (int i = 0; i < mathNetComplexArr.Length; i++)
            {
                outArr[i] = mathNetComplexArr[i].Real;
            }
            return outArr;
        }
    }
}


