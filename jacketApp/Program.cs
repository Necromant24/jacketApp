using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jacketApp
{
    static class Program
    {

        public static Form1 form1;

        public static int[] channels= { 1,0,2,3};

        public static List<List<List<List<int>>>> allVals = null;

        public static List<List<List<string>>> strAllVals = null;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        


    }
}
