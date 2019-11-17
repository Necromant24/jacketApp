using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace jacketApp
{
    
    
    

        

        
    public partial class Form3 : Form
    {
        int prevActionIndex = 0;

        int _prevStateIndex = 0;
        public int prevStateIndex
        {
            get { return _prevStateIndex; }
            set
            {
                _prevStateIndex = value;
            }
        }
        int oldAction = 0;
        bool inProces = false;


        List<List<List<int>>> numsValsTest = new List<List<List<int>>>();
        List<List<List<List<int>>>> allVals = new List<List<List<List<int>>>>();
        int[] allValsSiz = new int[10] {1,1,1,1,1,1,1,1,1,1 };
        List<int> frstMass = new List<int>() { 0, 0, 0, 0, 0 };
        List<List<int>> scndMass;

        List<List<List<string>>> strAllVals = new List<List<List<string>>>();

        //заполняем массив для того чтоб если юзер не заглянет в какойто из стэйтов то чтоб были значения
        private void initiallVals()
        {
            for(int q=0;q<10;q++)
            {
                List<List<List<int>>> ader1 = new List<List<List<int>>>();
                for (int w = 0; w < 1; w++)
                {
                    List<List<int>> ader2 = new List<List<int>>();
                    for (int e = 0; e < 1; e++)
                    {
                        List<int> ader3 = new List<int>();
                        for (int r = 0; r < 5; r++)
                        {
                            ader3.Add(0);

                        }
                        ader2.Add(ader3);

                    }
                    ader1.Add(ader2);
                }
                allVals.Add(  ader1);
            }

            if (Program.strAllVals == null)
            {
                for (int w = 0; w < 10; w++)
                {
                    List<List<string>> ader1 = new List<List<string>>();
                    for (int e = 0; e < 1; e++)
                    {
                        List<string> ader2 = new List<string>();
                        for (int r = 0; r < 1; r++)
                        {
                            ader2.Add("0");

                        }
                        ader1.Add(ader2);

                    }
                    strAllVals.Add(ader1);
                }
            }


        }
        
        bool firstChanging = false;
        public Form3()
        {
            scndMass = new List<List<int>>() { frstMass };

            initiallVals();
            

            InitializeComponent();
            tableLayoutPanel3.RowCount += 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, tableLayoutPanel3.RowStyles[0].Height));
            //tableLayoutPanel3.RowCount += 1;    

            tableLayoutPanel3.Controls.Add(button3, 2, tableLayoutPanel3.RowCount-1);
            tableLayoutPanel3.Controls.Add(button4, 3, tableLayoutPanel3.RowCount-1);
            //Controls.Add - можно использовать для заполнения последующих новых ячеек
           // this.Height += 50;
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    tableLayoutPanel3.Controls.Add(new CheckBox { Width = 18, Height = 18, Anchor = AnchorStyles.None }, 0, tableLayoutPanel3.RowCount - 2);

                }
                else
                {
                    tableLayoutPanel3.Controls.Add(new NumericUpDown { Anchor = AnchorStyles.None, Size = new Size(90, 20), Maximum = 9999 }, i, tableLayoutPanel3.RowCount - 2);

                }
            }
            tableLayoutPanel3.Controls.Add(new TextBox {Text="0", Anchor = AnchorStyles.None, Width = 130 }, 5, tableLayoutPanel3.RowCount - 2);

            if (Program.strAllVals != null) {
                strAllVals = Program.strAllVals;
                string s = "";
                for(int q=0;q<10;q++)
                {
                    for(int w=0;w<strAllVals[q].Count;w++)
                    {
                        for(int e = 0; e < strAllVals[q][w].Count; e++)
                        {
                            s = "";
                            for(int r = 0; r < strAllVals[q][w][e].Length; r++)
                            {
                                if(strAllVals[q][w][e][r]=='p')
                                {
                                    s += " ";
                                }
                                else
                                {
                                    s += strAllVals[q][w][e][r];
                                }

                            }
                            strAllVals[q][w][e] = s;

                        }
                    }
                }
                
            }
            if (Program.allVals != null)
            {
                allVals = null;
                allVals = Program.allVals;
                loadState(true);
            }
            

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;



            comboBox1.SelectionChangeCommitted += 
                delegate{
                    
                        saveState(true); loadState(true); prevActionIndex = comboBox1.SelectedIndex;
                    
                };

            comboBox2.SelectionChangeCommitted += 
                delegate {
                    if (!inProces)
                    {

                        saveState(); loadState(false);
                        prevStateIndex = comboBox2.SelectedIndex;

                        label7.Text = "not action";
                    }
                    inProces = false;
                };
        }

        bool actionChanging = false;

        private void saveState(bool isAction=false)
        {
            int actionIndex = prevActionIndex, stateIndex = prevStateIndex;

            if (isAction)
            {

                stateIndex = comboBox2.SelectedIndex;
                label7.Text = stateIndex.ToString();

            }

            List<string> strAder = new List<string>();
            List<List<int>> adder = new List<List<int>>();
            for (int row = 1; row < tableLayoutPanel3.RowCount-1; row++)
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
                adder.Add(nv);
                strAder.Add(tableLayoutPanel3.GetControlFromPosition(5, row).Text);
            }

            try
            {
                strAllVals[actionIndex][stateIndex] = strAder;
            }
            catch (Exception)
            {
                strAllVals[actionIndex].Add(strAder);
            }

            try
            {
                allVals[actionIndex][stateIndex] = adder;
            }
            catch (Exception)
            {
                allVals[actionIndex].Add(adder);
            }
        }
        
        private void loadBox()
        {

            
        }
        
        
        //load tables in case you change state or action
        private void loadState(bool isAction=false)
        {
            int actionIndex = comboBox1.SelectedIndex, stateIndex = comboBox2.SelectedIndex;
            
            if(isAction) {
               

                inProces = true;
                comboBox2.Items.Clear();
                int co = allVals.Count;
                int pr = actionIndex;
                if (actionIndex < 0) actionIndex = 0;


                for (int x = 0; x < allVals[actionIndex].Count ; x++)
                {
                    comboBox2.Items.Add(Convert.ToString(x + 1));
                }
                // TODO: обнулять состояние
               
                prevStateIndex = comboBox2.SelectedIndex = 0;
                inProces = false;

            }
            
            
            if (isAction) stateIndex = 0;



            //int razn = numsValsTest[getStateIndex()].Count - tableLayoutPanel3.RowCount + 1;
            int razn = allVals[actionIndex][stateIndex].Count - tableLayoutPanel3.RowCount+2;

            if (razn < 0)
            {
                for (int i = 0; i < -razn; i++)
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
            for (int row = 1; row < tableLayoutPanel3.RowCount-1; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (col == 0)
                    {
                        CheckBox ch = (CheckBox)tableLayoutPanel3.GetControlFromPosition(col, row);
                        ch.Checked = !(allVals[actionIndex][stateIndex][row - 1][col] == 0);
                    }
                    else
                    {
                        NumericUpDown numeric = (NumericUpDown)tableLayoutPanel3.GetControlFromPosition(col, row);
                        numeric.Value = (decimal)allVals[actionIndex][stateIndex][row - 1][col];
                    }
                }

                var a = strAllVals;
                int ind = row - 1;

                //string sa= strAllVals[actionIndex][stateIndex][row - 1];

                try
                {
                    tableLayoutPanel3.GetControlFromPosition(5, row).Text = strAllVals[actionIndex][stateIndex][row - 1];
                }
                catch (Exception) { }
            }
            
        }

        
        private void load()
        {
            int razn = numsValsTest[getStateIndex()].Count - tableLayoutPanel3.RowCount + 1;

            if (razn < 0)
            {
                for (int i = 0; i < -razn; i++)
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





        private void button6_Click(object sender, EventArgs e)
        {
            add();
        }
        

        private void add()
        {
                label7.Text = "es";
            //крч это чрезвычайно важная строчка иначе баг
             this.tableLayoutPanel3.RowCount += 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute,(float) tableLayoutPanel3.RowStyles[0].Height));
            //tableLayoutPanel3.RowCount += 1;    

                tableLayoutPanel3.Controls.Add(button3, 1, tableLayoutPanel3.RowCount-1);
                tableLayoutPanel3.Controls.Add(button4, 2, tableLayoutPanel3.RowCount-1);
                //Controls.Add - можно использовать для заполнения последующих новых ячеек
               
                for (int i = 0; i < 5; i++)
                {
                    if (i == 0)
                    {
                        tableLayoutPanel3.Controls.Add(new CheckBox { Width = 18, Height = 18, Anchor = AnchorStyles.None }, 0, tableLayoutPanel3.RowCount - 2);

                    }
                    else
                    {
                        tableLayoutPanel3.Controls.Add(new NumericUpDown { Anchor = AnchorStyles.None, Size = new Size(90, 20), Maximum = 9999 }, i, tableLayoutPanel3.RowCount - 2);

                    }
                }
            tableLayoutPanel3.Controls.Add(new TextBox { Text = "0", Anchor = AnchorStyles.None, Width = 130 }, 5, tableLayoutPanel3.RowCount - 2);

        }
        
        private int getStateIndex()
        {
           return comboBox1.SelectedIndex;
        }
        


        private void delete()
        {
            if (tableLayoutPanel3.RowCount > 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    tableLayoutPanel3.Controls.Remove(tableLayoutPanel3.GetControlFromPosition(i, tableLayoutPanel3.RowCount - 2));
                }
                tableLayoutPanel3.Controls.Remove(tableLayoutPanel3.GetControlFromPosition(5, tableLayoutPanel3.RowCount - 2));

                tableLayoutPanel3.Controls.Remove(button3);
                tableLayoutPanel3.Controls.Remove(button4);

                tableLayoutPanel3.Controls.Add(button3, 1, tableLayoutPanel3.RowCount - 2);
                tableLayoutPanel3.Controls.Add(button4, 2, tableLayoutPanel3.RowCount - 2);
                tableLayoutPanel3.RowCount -= 1;

                tableLayoutPanel3.AutoScrollMinSize = new Size(400, 200);
                //tableLayoutPanel3.RowStyles[tableLayoutPanel3.RowCount - 1].Height = 10;

                //int h =(int) tableLayoutPanel3.RowStyles[0].Height * tableLayoutPanel3.RowCount;

              //  tableLayoutPanel3.AutoScrollMargin = new Size(tableLayoutPanel3.AutoScrollMargin.Width, h);
                   
             }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }


        List<string> strAdd = new List<string> { "0" };
        //return and see this code and fix
        private void button2_Click(object sender, EventArgs e)
        {
            strAllVals[comboBox1.SelectedIndex].Add(strAdd);
            comboBox2.Items.Add(Convert.ToString(comboBox2.Items.Count+1));

            //крч разобраться нужен ли этот allVallsSizes вообще
            //allValsSizes[comboBox1.SelectedIndex] = comboBox2.Items.Count;

            //расширяю границы 2 слоя массива allVals
            allVals[prevActionIndex].Add(scndMass) ;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //comboBox2.Items.Remove(1);
            List<string> list = new List<string>();
            int count = comboBox2.Items.Count;
            for(int i=0;i<count;i++)
            {
                list.Add(comboBox2.Items[i].ToString());
            }
            comboBox2.Items.Clear();
            for(int i=0;i<count-1;i++)
            {
                comboBox2.Items.Add(list[i]);
            }
            inProces = true;
            comboBox2.SelectedIndex = 0;

            allVals[prevActionIndex].RemoveAt(allVals[prevActionIndex].Count - 1);
            inProces = false;
            strAllVals[prevActionIndex].RemoveAt(strAllVals[prevActionIndex].Count - 1);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            add();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.allVals = allVals;

            for (int w = 0; w < 10; w++)
            {
                
                for (int i = 0; i < allVals[w].Count; i++)
                {
                    
                    for (int r = 0; r < allVals[w][i].Count; r++)
                    {
                        string s= strAllVals[w][i][r].Trim();
                        string ends = "";
                        for(int q=0;q<s.Length;q++)
                        {
                            if (s[q] == ' ')
                            {
                                ends += "p";
                            }
                            else
                            {
                                ends += s[q];
                            }
                        }
                        strAllVals[w][i][r] = ends;

                    }
                }
            }
            
            Program.strAllVals = strAllVals;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            delete();
            
        }

        
        
    }
}
