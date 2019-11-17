using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


//sendClick посмотри

namespace jacketApp
{
    public partial class Form2 : Form
    {
        //при нажатии кнопок посмотри команды которые я тебе посылаю
        SerialPort port;
        int[] startData = { 1, 0, 2, 3 };
        int[] usersChannels = new int[5];
        int channelIndex;
        int sendedInd;
        int gotInd;
        bool sended = false;
        Button[] buttons=new Button[10] ;
        Color[] colors = new Color[10];

        bool send = false;


        //тут нужные команды для мигания
        private void sendClick(int index)
        {
            string[] commands = new string[5] { "front_blinck", "left_blinck_", "right_blinck", "back_blinck_", "clever_blinck" };
            if (!send)
            {
                buttons[index].BackColor = colors[index];
                send = true;
                channelIndex = index;

                label2.Text = "sended" + commands[index];

                try
                {
                    port.Write(commands[index]);
                }
                catch (Exception) { }
                //для тестировки
               // label2.Text = "port not connected!!!";
            }
        }
        private void getClick(int index)
        {
            if (send)
            {
                
                index += 5;
                buttons[index].BackColor = colors[index];
                send = false;
               // buttons[index + 5].BackColor = colors[index];

                label2.Text = "got" + Convert.ToString(index);
                
                usersChannels[channelIndex] = index-5;
                gotInd = index;
                //Thread.Sleep(350);
                //buttons[index - 5].BackColor = Color.GhostWhite;
                //buttons[index].BackColor = Color.GhostWhite;

            }
        }
        

        public Form2()
        {
            InitializeComponent();

            buttons[0] = button6;
            buttons[1] = button7;
            buttons[2] = button8;
            buttons[3] = button9;
            buttons[4] = button12;
            buttons[5] = button2;
            buttons[6] = button3;
            buttons[7] = button4;
            buttons[8] = button5;
            buttons[9] = button13;



            colors[4] = Color.Violet;
            colors[3] = Color.DarkBlue;
            colors[2] = Color.DarkGray;
            colors[1] = Color.DarkKhaki;
            colors[0] = Color.Gray;

            for(int i=0;i<5;i++)
            {
                colors[i + 5] = colors[i];
            }
            
            
             // предпоказ цветов
            for(int i = 0; i<5;i++)
            {
                buttons[i].BackColor = colors[i];
            }

            for (int i = 0; i < 10; i++)
            {
                buttons[i].BackColor = Color.GhostWhite;
            }

            string[] ports = SerialPort.GetPortNames();
            bool connected = false;
            foreach(string com in ports)
            {
                try
                {
                    port = new SerialPort(com, 9600);
                    connected = true;
                    break;
                }
                catch (Exception) { }

            }
            if (!connected) label2.Text = "not connected";
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getClick(1);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            getClick(0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sendClick(0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sendClick(1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sendClick(2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            sendClick(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            getClick(2);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            usersChannels[channelIndex] = 3;
            sended = false;
            sendedInd = 3;

            getClick(3);
        }


        void changeIndexses(int sended,int got)
        {
            if(sended!=got)
            {
                int old = startData[sended];
                startData[sended] = got;
                startData[got] = old;

            }

        }
        

        string[] stats = { "not sended", "sended" };
        void changeStatus()
        {
            if(label2.Text==stats[0])
            {
                label2.Text = stats[1];
            }
            else
            {
                label2.Text = stats[0];
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            usersChannels = new int[4];
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Program.channels = usersChannels;
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            sendClick(4);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            getClick(4);
        }
    }
}
