using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace angle_control
{
    public partial class Form1 : Form
    {
        private StringBuilder sb = new StringBuilder();
        private long receive_count = 0;
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
                    textBox1.Text = "";
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
            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    byte[] bytesend = new byte[8];
                    string[] input = textBox2.Text.Split();
                    for (int i=0;i<8;i++)
                    {
                        bytesend[i] = Convert.ToByte(input[i],16);
                    }

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
                    textBox1.AppendText(sb.ToString());
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
    }
    }


