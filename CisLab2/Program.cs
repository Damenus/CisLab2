using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CisLab2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    static class Extensions
    {

        //wypisz atrybuty RAHS (read archival hidden systemfile)
        public static string getRahs(this DiskFile fsi)
        {
            //ustawianie flag
            bool readOnly = fsi.Root.Attributes.HasFlag(FileAttributes.ReadOnly);
            bool archiv = fsi.Root.Attributes.HasFlag(FileAttributes.Archive);
            bool hidden = fsi.Root.Attributes.HasFlag(FileAttributes.Hidden);
            bool sysfile = fsi.Root.Attributes.HasFlag(FileAttributes.System);


            string attributes = String.Empty;

            //wypisz
            if (readOnly)
                attributes += "r";
            else
                attributes += "-";
            if (archiv)
                attributes += "a";
            else
                attributes += "-";
            if (hidden)
                attributes += "h";
            else
                attributes += "-";
            if (sysfile)
                attributes += "s";
            else
                attributes += "-";


            return attributes;

        }
    }
}
