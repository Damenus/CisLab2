using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CisLab2
{
    public partial class Form2 : Form
    {
        private readonly Form1 parnetForm;
        private string parentPath;
        private string pattern = "^[a-zA-Z0-9~_-]{1,8}.(txt|php|htm)$";


        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string path, Form1 form)
        {
            InitializeComponent();
            this.parentPath = path;
            this.parnetForm = form;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string name = textBox1.Text;
                string path = Path.Combine(parentPath, name);

                if(radioButton1.Checked) {
                    if(!File.Exists(path) && Regex.IsMatch(name,pattern)) 
                           {
                               File.Create(path).Dispose();
                           }                
                } 
                else
                {
                    Directory.CreateDirectory(path);
                }     
                FileAttributes attr = File.GetAttributes(path);
                if (checkBox1.Checked)
                {
                    File.SetAttributes(path, attr | FileAttributes.ReadOnly);
                    attr = File.GetAttributes(path);
                }
                if (checkBox1.Checked)
                {
                    File.SetAttributes(path, attr | FileAttributes.Archive);
                    attr = File.GetAttributes(path);
                }
                if (checkBox1.Checked)
                {
                    File.SetAttributes(path, attr | FileAttributes.Hidden);
                    attr = File.GetAttributes(path);
                }
                if (checkBox1.Checked)
                {
                    File.SetAttributes(path, attr | FileAttributes.System);
                    attr = File.GetAttributes(path);
                }

                parnetForm.refreshTreeView();
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
