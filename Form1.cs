using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Program_robot
{
    public partial class Robot1 : Form
    {
        private SerialPort _sp = new SerialPort();

        private string _portCom;
        private string _servoCords;

        private const string PATH_TO_SAVE = @"..\..\\Data\\";
        private const string SEPARATOR = ",";

        public Robot1()
        {
            InitializeComponent();
        }

        private void Operate_Click(object sender, EventArgs e)
        {
            try
            {

                _portCom = textBox5.Text;
                if(_portCom.Equals("COM4"))
                {
                    _sp.PortName = _portCom;
                }
                else
                {
                    MessageBox.Show("Incorrect COM");
                    return;
                }
                //_sp = new SerialPort();
                _sp.BaudRate = 9600;//!!
                _sp.Open();
                timer1.Enabled = true;
                MessageBox.Show("You open the port");

                _servoCords = _sp.ReadLine();
                _servoCords = _servoCords.TrimStart(':');
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Standby_Click(object sender, EventArgs e)
        {
            try
            {
                if (_sp != null)
                {
                    _sp.Close();
                    timer1.Enabled = false;
                    MessageBox.Show("You close the port");
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var d = _sp.ReadLine();
            char[] MyChar = { ':', 'o', 'n', 'e', 't', 'w', 'h', 'r', 'f', 'u' };

            if (d.StartsWith("one:"))
            {
                textBox1.Text = d.TrimStart(MyChar);
            }

            if (d.StartsWith("two:"))
            {
                textBox2.Text = d.TrimStart(MyChar);
            }

            if (d.StartsWith("three:"))
            {
                textBox3.Text = d.TrimStart(MyChar);
            }

            if (d.StartsWith("four:"))
            {
                textBox4.Text = d.TrimStart(MyChar);
            }


        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (IsAnyServoEmpty())
            {
                MessageBox.Show("No data to record");
                return;
            }

            Directory.CreateDirectory(PATH_TO_SAVE);

            
            var text = new StreamWriter(PATH_TO_SAVE + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss" + ".CSV"));

            using(text)
            {
                var strBuilder = new StringBuilder();

                strBuilder.Append("Servo1,Servo2,Servo3,Servo4");
                strBuilder.Append("\r");
                strBuilder.Append(RemoveNewLine(textBox1.Text)).Append(SEPARATOR);
                strBuilder.Append(RemoveNewLine(textBox2.Text)).Append(SEPARATOR);
                strBuilder.Append(RemoveNewLine(textBox3.Text)).Append(SEPARATOR);
                strBuilder.Append(RemoveNewLine(textBox4.Text));

                var file = strBuilder.ToString();

                text.Write(file);

                text.Close();
            }

            MessageBox.Show("SAVED");
        }

        private static string RemoveNewLine(string str) => str.Replace("\r", "");
        private  bool IsAnyServoEmpty()
        {
            var servoes = new List<string> { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text };

            if(servoes.Any(x => x.Equals(string.Empty)))  //sprawdza po kolei kazda wartosc z tablicy czy jest rowna string.empty
            {
                return true;
            }

            return false;
        }

        private void aboutAuthor(object sender, EventArgs e)
        {
            string message = "The author of this program is student Hubert Derbis.\nProject was created in cooperation with\nUniversity of Zielona Gora.";
            string title = "Information about author";
            MessageBox.Show(message, title);
        }
    }
}


