using jacketApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO.Ports;
using System.IO;
using System.Reflection;
//using Modbus.Device;
using Modbus.Device;

    //!!!!!!!!!
    // void transfer посмотри
    //и в форме 2 команды для мигания диодами
    //будут вопросы пиши


    // вот тут проблем нет , проблема с отрисовкой в форме1 и с данными в форме3 и ее отрисовкой , пытался так же хотя бы как 
    //и в форме1 с отрисовкой но даже так не получилось хотя все вроде одинаково
    
namespace jacketApp
{
    public partial class Form1 : Form
    {
        List<List<List<List<int>>>> intelligentVals=null;

        List<List<List<string>>> strAllVals = null;
         
        bool initialized = false;

        int currentCount = 0, maxCount = 8;
        //20 max

        SerialPort p1 = new SerialPort("COM1",9600);

        //p1.Open();
      
        
        List<bool> booList = new List<bool>();

        List<List<List<int>>> numsValsTest = new List<List<List<int>>>();

        int prevProgStateIndex = 0;
        
        TableLayoutPanel[] tables = new TableLayoutPanel[5];
        

        

        public Form1()
        {
            Program.form1 = this;
            InitializeComponent();

            List<int> ader1 = new List<int>() { 0, 0, 0, 0, 0 };
            List<List<int>> ader2 = new List<List<int>>() { ader1,ader1 };
            List<List<List<int>>> ader3 = new List<List<List<int>>>() { ader2,ader2 };
            List<List<List<List<int>>>> intelligentVals = new List<List<List<List<int>>>>()
            { ader3, ader3, ader3, ader3, ader3, ader3, ader3, ader3, ader3, ader3 };
            List<List<string>> strAder2 = new List<List<string>>();
            List<string> strAder1 = new List<string>() { "0"};

            
            strAllVals = new List<List<List<string>>>();
            for (int w = 0; w < 10; w++)
            {
                List<List<string>> sader2 = new List<List<string>>();
                for (int e = 0; e < 1; e++)
                {
                    List<string> sader3 = new List<string>();
                    for (int r = 0; r < 5; r++)
                    {
                        sader3.Add("0");
                    }
                    sader2.Add(sader3);
                }
                strAllVals.Add(sader2);
            }



            buttons = new List<RadioButton>();

            RegisterRB(radioButton1);
            RegisterRB(radioButton2);
            RegisterRB(radioButton3);
            RegisterRB(radioButton4);

            
            for (int i = 0; i < 16; i++)
            {
                List<List<int>> tempArr = new List<List<int>>();
                numsValsTest.Add(tempArr);
                for (int j = 0; j < tableLayoutPanel3.RowCount - 1; j++)
                {
                    List<int> tempArr2 = new List<int>();
                    numsValsTest[i].Add(tempArr2);
                    for(int k = 0; k < 5; k++)
                    {
                        numsValsTest[i][j].Add(0);
                    }
                }
            }
            
            if (was) this.tableLayoutPanel3.RowCount += 1;
            //tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, (float)tableLayoutPanel3.RowStyles[0].Height));
            
            was = true;

            //перенос кнопки добавления строки на ячейку ниже
            tableLayoutPanel3.Controls.Add(button6, 1, tableLayoutPanel3.RowCount);
            tableLayoutPanel3.Controls.Add(button7, 2, tableLayoutPanel3.RowCount);
            //Controls.Add - можно использовать для заполнения последующих новых ячеек
            this.Height += 50;
            
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    tableLayoutPanel3.Controls.Add(new CheckBox { Width = 18, Height = 18, Anchor = AnchorStyles.None }, i, tableLayoutPanel3.RowCount - 1);

                }
                else
                {
                    tableLayoutPanel3.Controls.Add(new NumericUpDown { Anchor = AnchorStyles.None, Size = new Size(90, 20),Maximum=9999 }, i, tableLayoutPanel3.RowCount - 1);

                }
            }
            if (currentCount == maxCount)
            {
                tableLayoutPanel3.Controls.Remove(button6);
            }
            
            radioButton1.Select();
            comboBox1.SelectedIndex = 0;

            initialized = true;

        }

        private void button1_Click(object sender, EventArgs e){}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private int getStateIndex()
        {
            return IndexOfCheckedRB() * 4 + comboBox1.SelectedIndex;
        }

        private void label5_Click(object sender, EventArgs e){}

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e){}

        private void exitToolStripMenuItem_Click(object sender, EventArgs e){}

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        public void save()
        {
            saveCore(getStateIndex());
        }

        public void save(int index)
        {
            saveCore(index);
        }

        private void saveCore(int index)
        {
            numsValsTest[index] = new List<List<int>>();
            for (int row = 1; row < tableLayoutPanel3.RowCount; row++)
            {
                List<int> nv = new List<int>();
                
                for (int col = 0; col < 5; col++)
                {
                    if (col == 0)
                    {
                        CheckBox ch = (CheckBox)tableLayoutPanel3.GetControlFromPosition(col, row);
                        bool check = ch.Checked;
                        int z = check ? 1 : 0;
                        nv.Add(z);
                    }
                    else
                    {
                        NumericUpDown numeric = (NumericUpDown)tableLayoutPanel3.GetControlFromPosition(col, row);
                        int v = (int)numeric.Value;
                        nv.Add(v);
                    }
                }
                numsValsTest[index].Add(nv);
            }
        }

        private void saveToFile()
        { 
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    save();
                    List<string> progState = new List<string>();
                    for (int i = 0; i < 16; i++) progState.Add("");
                    string bufStr = "";
                    for (int k = 0; k < 16; k++)
                    {
                        for (int i = 0; i < numsValsTest[k].Count; i++)
                        {
                            for (int j = 0; j < numsValsTest[k][i].Count; j++)
                            {
                                bufStr += numsValsTest[k][i][j].ToString() + " ";
                            }
                        }
                        progState[k] = bufStr;
                        bufStr = "";
                    }
                    System.IO.File.Delete(saveFileDialog1.FileName);
                    System.IO.File.WriteAllLines(saveFileDialog1.FileName, progState);

                List<string> cleverVals = new List<string>();
                if (Program.allVals != null) intelligentVals = Program.allVals;
                if (Program.strAllVals != null) strAllVals = Program.strAllVals;
                for(int q=0;q<intelligentVals.Count;q++)
                {
                    string adder = "";
                    for (int w = 0; w < intelligentVals[q].Count; w++)
                    {
                        for (int e = 0; e < intelligentVals[q][w].Count; e++)
                        {
                            for (int r = 0; r < 5; r++)
                            {
                                adder += Convert.ToString(intelligentVals[q][w][e][r]);
                                adder += " ";
                            }
                            string str = strAllVals[q][w][e].Trim();
                            string s2 = "";
                            for (int v = 0; v < str.Length; v++)
                            {
                                if (str[v] == ' ')
                                {
                                    s2 += "p";
                                }
                                else
                                {
                                    s2 += str[v];
                                }

                            }
                            adder += s2 + " ";

                        }
                        adder += "n ";
                    }
                    cleverVals.Add(adder);
                }
                File.AppendAllLines(saveFileDialog1.FileName,cleverVals);
                }
        }
        
        

        private void load()
        {
            int razn = numsValsTest[getStateIndex()].Count - tableLayoutPanel3.RowCount + 1;

            if (razn < 0)
            {
                for (int i = 0; i < -razn ; i++)
                {
                    delete();
                }
            }

            if (razn > 0)
            {
                for (int i = 0; i < razn; i++)
                {
                    add();
                }
            }
            for (int row = 1; row < tableLayoutPanel3.RowCount; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (col == 0)
                    {
                        CheckBox ch = (CheckBox)tableLayoutPanel3.GetControlFromPosition(col, row);
                        ch.Checked = !(numsValsTest[getStateIndex()][row - 1][col] == 0);
                    }
                    else
                    {
                        NumericUpDown numeric = (NumericUpDown)tableLayoutPanel3.GetControlFromPosition(col, row);
                        numeric.Value = (decimal)numsValsTest[getStateIndex()][row - 1][col];
                    }
                }
            }
        }

        private List<int> parse(string s)
        {
            return s.Trim().Split(' ').Select(n => Convert.ToInt32(n)).ToArray().ToList();
        }

        //incorrect work and out of range if string.length> того чты вы изначально задаете при инициализации
        private void loadFromFile()
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> tempProgState = System.IO.File.ReadAllLines(openFileDialog1.FileName).ToList<string>();
                numsValsTest = new List<List<List<int>>>();
                for (int i = 0; i < 16; i++)
                {
                    List<int> newListInt = parse(tempProgState[i]);
                    int x = 0;

                    List<List<int>> myList = new List<List<int>>();
                    for (int j = 0; j < newListInt.Count / 5; j++)
                    {
                        List<int> nv = new List<int>();
                        for (int k = 0; k < 5; k++)
                        {
                            nv.Add(newListInt[x]);
                            x++;
                        }
                        myList.Add(nv);
                        //numsValsTest[i][j] = nv;
                    }
                    numsValsTest.Add(myList);
                }
                load();

                if (tempProgState.Count > 16)
                {
                    

                    //each table on each action
                    List<string> form3Data = new List<string>();
                    for(int i=16;i<26;i++)
                    {
                        form3Data.Add(tempProgState[i]);
                    }


                    List<List<List<List<string>>>> sCleverVals = new List<List<List<List<string>>>>();
                    
                    for (int q = 0; q < 10; q++)
                    {
                        string[] diver1 = form3Data[q].Trim().Split('n');
                        string[] resizer = new string[diver1.Count()-1];
                        for(int i=0;i<diver1.Count()-1;i++)
                        {
                            resizer[i] = diver1[i];
                        }
                        diver1 = resizer;
                        

                        List<List<List<string>>> sAder3 = new List<List<List<string>>>();
                        for (int w = 0; w < diver1.Count<string>(); w++)
                        {
                            string[] diver2 = diver1[w].Trim().Split(' ');

                            List<List<string>> sAder2 = new List<List<string>>();
                            for (int e = 0; e < diver2.Count()/6; e++)
                            {
                                List<string> sAder1 = new List<string>();
                                for (int r = 0; r < 6; r++)
                                {
                                    sAder1.Add(diver2[e * 6 + r]);
                                }
                                sAder2.Add(sAder1);


                            }
                            sAder3.Add(sAder2);

                        }
                        sCleverVals.Add(sAder3);

                    }

                    List<List<List<List<int>>>> cleverVals = new List<List<List<List<int>>>>();
                    int frstSize = form3Data.Count;
                    List<List<List<string>>> strVals = new List<List<List<string>>>();


                    for (int q = 0; q < 10; q++)
                    {
                        List<List<List<int>>> ader3 = new List<List<List<int>>>();
                        List<List<string>> sAder1 = new List<List<string>>();
                        for (int w = 0; w <sCleverVals[q].Count ; w++)
                         {
                            List<List<int>> ader2 = new List<List<int>>();
                            List<string> sAder2 = new List<string>();
                            for (int e = 0; e < sCleverVals[q][w].Count; e++)
                            {
                                List<int> ader1 = new List<int>();
                                for (int r = 0; r < 5; r++)
                                {
                                    ader1.Add(Convert.ToInt32(sCleverVals[q][w][e][r]));
                                    
                                }
                                ader2.Add(ader1);
                                sAder2.Add(sCleverVals[q][w][e][5]);
                            }
                            ader3.Add(ader2);
                            sAder1.Add(sAder2);
                        }
                        cleverVals.Add(ader3);
                        strVals.Add(sAder1);
                    }
                    Program.allVals = cleverVals;

                    Program.strAllVals = strVals;

                    

                }
            }
        }

        private void frontBlink(int glow, int sleep, int times, Button button)
        {
            for (int i = 0; i < times; i++)
            {
                button.BackgroundImage = jacketApp.Properties.Resources.FrontJacketImgActive;
                button.Refresh();
                System.Threading.Thread.Sleep(glow);
                button.BackgroundImage = jacketApp.Properties.Resources.FrontJacketImg1;
                button.Refresh();
                System.Threading.Thread.Sleep(sleep);
            }
        }

        private void leftBlink(int glow, int sleep, int times, Button button)
        {
            for (int i = 0; i < times; i++)
            {
                button.BackgroundImage = jacketApp.Properties.Resources.LeftJacketImgActive;
                button.Refresh();
                System.Threading.Thread.Sleep(glow);
                button.BackgroundImage = jacketApp.Properties.Resources.LeftJacketImg1;
                button.Refresh();
                System.Threading.Thread.Sleep(sleep);
            }
        }

        private void rightBlink(int glow, int sleep, int times, Button button)
        {
            for (int i = 0; i < times; i++)
            {
                button.BackgroundImage = jacketApp.Properties.Resources.RightJacketImgActive;
                button.Refresh();
                System.Threading.Thread.Sleep(glow);
                button.BackgroundImage = jacketApp.Properties.Resources.RightJacketImg1;
                button.Refresh();
                System.Threading.Thread.Sleep(sleep);
            }
        }

        private void backBlink(int glow, int sleep, int times, Button button)
        {
            for (int i = 0; i < times; i++)
            {
                button.BackgroundImage = jacketApp.Properties.Resources.BackJacketImgActive;
                button.Refresh();
                System.Threading.Thread.Sleep(glow);
                button.BackgroundImage = jacketApp.Properties.Resources.BackJacketImg1;
                button.Refresh();
                System.Threading.Thread.Sleep(sleep);
            }
        }


        bool wasTest = false;

        //Эльдар это типо те команды чтоб ты помигал диодом (по индексу)
        private void button3_Click(object sender, EventArgs e)
        {
            int index = IndexOfCheckedRB();
            string[] rbNames = { "front_blinck","left_blinck_", "right_blinck", "back_blinck_" };
            if(currentPort!=null)
            {
                try
                {
                    SerialPort port = new SerialPort(currentPort, 9600);
                    port.Open();
                    port.DataBits = 8;
                    ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
                    port.WriteLine(rbNames[index]);
                    wasTest=true;
                }
                catch (Exception) { }
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e){}

        private void saveConfigToolStripMenuItem_Click(object sender, EventArgs e){save();}

        bool was = false;

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            load();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            delete();
        }

        private void delete()
        {

            if (tableLayoutPanel3.RowCount > 1)
            {
                
                currentCount -= 1;
               
                
                for (int i = 0; i < 5; i++)
                {
                    tableLayoutPanel3.Controls.Remove(tableLayoutPanel3.GetControlFromPosition(i, tableLayoutPanel3.RowCount - 1));
                }
                
                tableLayoutPanel3.Controls.Remove(tableLayoutPanel3.GetControlFromPosition(5, tableLayoutPanel3.RowCount - 1));
                tableLayoutPanel3.Controls.Add(button6, 1, tableLayoutPanel3.RowCount - 1);
                tableLayoutPanel3.Controls.Add(button7, 2, tableLayoutPanel3.RowCount - 1);
                tableLayoutPanel3.RowCount -= 1;

                // tableLayoutPanel3.VerticalScroll.Maximum = 250;
                // tableLayoutPanel3.


                tableLayoutPanel3.RowStyles[tableLayoutPanel3.RowCount].SizeType = SizeType.Absolute;

                //присваиваем стиль последней строке
                //tableLayoutPanel3.RowStyles[tableLayoutPanel3.RowCount].Height = 10;

                //выводим кол-во строк
                label10.Text = tableLayoutPanel3.RowCount.ToString();
                
                //cтиль не перемещается он остается на своей строке и надозаново его перезадавать
                

            }
        }


        private void add()
        {
            if (currentCount < maxCount + 1)
            {
                //крч это чрезвычайно важная строчка иначе баг
                if (was) this.tableLayoutPanel3.RowCount += 1;

                //tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
                tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, (float)tableLayoutPanel3.RowStyles[0].Height));


                was = true;
                //перенос кнопки добавления строки на ячейку ниже
                tableLayoutPanel3.Controls.Add(button6, 1, tableLayoutPanel3.RowCount);
                tableLayoutPanel3.Controls.Add(button7, 2, tableLayoutPanel3.RowCount);
                //Controls.Add - можно использовать для заполнения последующих новых ячеек
               

                for (int i = 0; i < 5; i++)
                {
                    if (i == 0)
                    {
                        tableLayoutPanel3.Controls.Add(new CheckBox { Width = 18, Height = 18, Anchor = AnchorStyles.None }, i, tableLayoutPanel3.RowCount - 1);

                    }
                    else
                    {
                        tableLayoutPanel3.Controls.Add(new NumericUpDown { Anchor = AnchorStyles.None, Size = new Size(90, 20),Maximum=9999 }, i, tableLayoutPanel3.RowCount - 1);

                    }
                }
                

                if (currentCount == maxCount)
                {
                    //tableLayoutPanel3.Controls.Remove(button6);
                    
                }
                ++currentCount;
            }
            label10.Text = tableLayoutPanel3.RowCount.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            add();
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToFile();
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFromFile();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            stateChange();
        }

        public void stateChange()
        {
            if (initialized)
            {
                save(prevProgStateIndex);
                prevProgStateIndex = getStateIndex();
                load();
            }
            
        }

        
        private List<RadioButton> buttons { get; set; }
        private bool auto = false;
        private RadioButton checkedButt;

            
        public int RegisterRB(RadioButton rb)
        {
            if (!buttons.Contains(rb))
            {
                buttons.Add(rb);
                rb.CheckedChanged += rb_CheckedChanged;
               
            }
            return buttons.IndexOf(rb);
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            
            RadioButton rbClicked = sender as RadioButton;
            if (rbClicked == previousRb)
            {
                if (wasTest) wasTest = false;

                return;
            }
            else
            {
                checkedButt = rbClicked;
                int previousState = getStateIndex();
                stateChange();
                int currentState = getStateIndex();
                if(wasTest&&previousState!=currentState)
                {
                    //some code...
                    changeVals(currentState, previousState);


                    wasTest = false;
                }
            }

        }

        


        
        //предыдущая выбранная rb
        RadioButton previousRb;


        private void rbChanged(object sender,EventArgs e)
        {
            RadioButton rbClicked = sender as RadioButton;
            if (rbClicked == previousRb) return;
            else
            {
                checkedButt = rbClicked;
                stateChange();
            }

            
        }



        public void UnregisterRB(RadioButton rb)
        {
            if (buttons.Contains(rb))
            {
                buttons.Remove(rb);
                rb.CheckedChanged -= rb_CheckedChanged;
            }
        }

       // public void Clear() { foreach (RadioButton rb in buttons) UnregisterRB(rb); }

        public int IndexOfRB(RadioButton rb) { return buttons.IndexOf(rb); }
        public int IndexOfCheckedRB() { return buttons.IndexOf(checkedButt); }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        

        //проверка конектинга к ардуине
        private void button5_Click_1(object sender, EventArgs e)
        {
            
        }


        //находим порт подключаемся и передаем значения
        string currentPort=null;
        

        //UInt16
       private void transfer()
        {
            try
            {
                ModbusSerialMaster master;
                //master = new ModbusSerialMaster("COM1",9600);
                //var master = ModbusSerialMaster.
                //change_numsVals();
                change_numsVals();
                string[] sendPack = new string[16];
                SerialPort port = new SerialPort(comboBox2.Text, 9600);

                for (int q = 0; q < 4; q++)
                {

                    string st = Convert.ToString(q);
                    st += "pin" + pinsIndexes[q];

                    for (int w = 0; w < 4; w++)
                    {
                        string strtosend = "start";
                        strtosend += st + Convert.ToString(w);


                        for (int e = 0; e < numsValsTest[q * 4 + w].Count; e++)
                        {
                            strtosend += "frame";
                            if (e < 10)
                            {
                                strtosend += "0" + Convert.ToString(e);
                            }
                            else
                            {
                                strtosend += Convert.ToString(e);
                            }
                            for (int t = 0; t < 5; t++)
                            {
                                for (int r = 0; r < 4 - Convert.ToString(numsValsTest[q * 4 + w][e][t]).Length; r++)
                                {
                                    strtosend += "0";
                                }
                                strtosend += Convert.ToString(numsValsTest[q * 4 + w][e][t]);
                            }
                        }
                        int byteSize = 0;
                        for (int kp = 0; kp < strtosend.Length; kp++)
                        {
                            byteSize += Convert.ToInt32(Convert.ToSByte(strtosend[kp]));
                        }
                        strtosend += "csum:" + Convert.ToString(byteSize);


                        if (q * 4 + w == 15)
                        {
                            strtosend += "END";
                        }
                        else
                        {
                            strtosend += "end";
                        }

                        sendPack[q * 4 + w] = strtosend;

                        //with port
                        //ModbusSerialMaster sender = ModbusSerialMaster.CreateRtu(port);
                        

                        port.WriteLine(strtosend);

                        string answer = port.ReadLine();
                        if (answer == "repeat") --w;



                    }
                }

                if (Program.allVals != null)
                {
                    intelligentVals = Program.allVals;

                    List<string> cleverVals = new List<string>();
                    if (Program.allVals != null) intelligentVals = Program.allVals;
                    for (int q = 0; q < intelligentVals.Count; q++)
                    {
                        string adder = "";
                        for (int w = 0; w < intelligentVals[q].Count; w++)
                        {
                            for (int e = 0; e < intelligentVals[q][w].Count; e++)
                            {
                                for (int r = 0; r < 5; r++)
                                {
                                    adder += Convert.ToString(intelligentVals[q][w][e][r]);
                                    adder += " ";
                                }
                                string str = strAllVals[q][w][e].Trim();
                                string s2 = "";
                                for(int v=0;v<str.Length;v++)
                                {
                                    if(str[v]==' ')
                                    {
                                        s2 += "p";
                                    }
                                    else
                                    {
                                        s2 += str[v];
                                    }

                                }
                                adder+=s2+" ";

                                
                            }
                            adder += "n ";
                        }

                        cleverVals.Add(adder);
                    }
                    for(int i=0;i<10;i++)
                    {
                        port.Write(cleverVals[i]);
                    }


                }
                else { }



                File.Delete("dts.txt");

                File.WriteAllLines("dts.txt", sendPack);
            }
            catch (Exception) { label10.Text = "status of errors:an error occured in transfering data";  }
        }
        
        //меняет блок по 4 в numsVals
        private void changeVals(int currentSt, int previousSt)
        {
            List<List<List<int>>> changer = new List<List<List<int>>>(4);
            for (int i = 0; i < 4; i++)
            {
                changer[i] = numsValsTest[previousSt * 4 + i];
            }
            for (int i = 0; i < 4; i++)
            {
                numsValsTest[previousSt * 4 + i] = numsValsTest[currentSt * 4 + i];
                numsValsTest[currentSt * 4 + i] = changer[i];
            }
        }

        

        /*
         * после теста можно будет удалить
        private void changeNumsVals()
        {
            int[] startChannels = { 0, 1, 2, 3 };
            int[] channels = Program.channels;
            List<List<List<List<int>>>> mainChanger = new List<List<List<List<int>>>>(4);
            for(int i=0;i<4;i++)
            {
                List<List<List<int>>> changer = new List<List<List<int>>>();
                for (int q = 0; q < 4; q++)
                {
                    changer.Add(numsValsTest[i * 4 + q]);

                }
                mainChanger.Add(changer);
            }
            for(int a=0;a<4;a++)
            {
                for (int s = 0; s < 4; s++)
                {
                    numsValsTest[a * 4 + s] = mainChanger[channels[a]][s];
                }
            }
        }
        */
        
        int[] pinsIndexes;
        //меняет значения по блокам в массиве исходя из индекса Ps: надо его впихнуть перед отправкой данных и
        //поговорить с егором все ли верно , хотя скорее всего все верно + протестить передачу данных для stm32 
        //Эльдар это я делал свой формат чтоб сначала отправить то о чем мы с тобой говорили
        //а потом отдельно данные 5 выхода
        private void change_numsVals()
        {
            int[] startChannels = { 0, 1, 2, 3 };
            int[] channels = Program.channels;
            List<int> resizer = new List<int>();

            int index5 = channels[4];
            for (int i = 0; i < 4; i++)
            {
                resizer.Add(channels[i]);
            }
            channels = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (channels[i] > index5) channels[i] -= 1;
            }
            

            pinsIndexes = Program.channels;

            channels = new int[4];
            for(int i=0;i<4;i++)
            {
                channels[i] = resizer[i];
            }


            for(int i=0;i<4;i++)
            {
                //changeVals(i, channels[i]);
                if(channels[i]!=startChannels[i])
                {
                    changeVals(startChannels[i], channels[i]);
                    int changer = startChannels[i];
                    startChannels[i] = channels[i];
                    startChannels[channels[i]] = changer;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            transfer();
        }

        private void openDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();

        }

        private void button8_Click(object sender, EventArgs e)
        {



        }
       
    }
}
