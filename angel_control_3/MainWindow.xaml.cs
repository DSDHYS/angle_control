using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace angel_control_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            testSerialPortUtils();


        }
        public void testSerialPortUtils()
        {
            Debug.WriteLine("开始调试");
            string[] portNames=SerialPortUtils.GetPortNames();
            

            if(portNames!=null)
                foreach(string name in portNames)
                {
                    Debug.Write(name);
                }
            SerialPortUtils.OpenClosePort("COM3", 9600);
            SerialPortUtils.SendData(System.Text.Encoding.Default.GetBytes("FF AA 01 00 00"));
        }
    }
}
