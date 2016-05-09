using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CisLab2
{
    public partial class Form1 : Form
    {

        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private DiskFile root;
        private TreeNode node;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();

            // OK button was pressed.
            if (result == DialogResult.OK)
            {
                root = new DiskFile(folderBrowserDialog.SelectedPath);
                treeView1.Nodes.Clear();
                node = treeView1.Nodes.Add(root.Name);
                node.Tag = root;
                fileLoader(root, node);


            }
            // Cancel button was pressed.
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }

        public void refreshTreeView()
        {
            root = new DiskFile(folderBrowserDialog.SelectedPath);
            treeView1.Nodes.Clear();
            node = treeView1.Nodes.Add(root.Name);
            node.Tag = root;
            fileLoader(root, node);
        }

        void fileLoader(DiskFile diskFile, TreeNode parentNode)
        {
            TreeNode childrenNode;
            foreach (var f in diskFile.Children)
            {
                childrenNode = parentNode.Nodes.Add(f.Name);
                childrenNode.Tag = f;
                if(f.Type == DiskFile.Types.Directory)
                    fileLoader(f, childrenNode);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

 
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //tworzenie
            if (treeView1.SelectedNode != null)
            {
                DiskFile file = (DiskFile)treeView1.SelectedNode.Tag;
                if (file.Type == DiskFile.Types.Directory)
                {
                    Form2 form = new Form2(file.Dir.FullName, this);
                    form.Show(); 
                }
                else
                {
                    Form2 form = new Form2(Directory.GetParent(file.FileInfo.FullName).FullName, this);
                    form.Show();
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //usuwanie
            if (treeView1.SelectedNode != null)
            {
                DiskFile file = (DiskFile)treeView1.SelectedNode.Tag;              
                //file.DeleteFile();
                treeView1.SelectedNode.Remove();
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }
    }

    class DiskFile
    {
        public enum Types
        {
            Directory,
            File
        };


        private SortedDictionary<string, long> elements;

        private long size;
        private DirectoryInfo dir;
        private FileSystemInfo root;
        private FileInfo fileInfo;
        private List<DiskFile> children;
        private string name;
        private string location;
        private Types type;

        public DirectoryInfo Dir
        {
            get { return this.dir; }
        }


        public FileInfo FileInfo
        {
            get { return this.fileInfo;  }
        }
        public FileSystemInfo Root
        {
            get { return this.root;  }
        }
        public string Name
        {
            get { return this.name; }
        }

        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public List<DiskFile> Children
        {
            get { return this.children;  }
        }

        public Types Type
        {
            get { return this.type;  }
        }

        public DiskFile(String location)
        {
            //sprawdzenie czy to folder czy plik
            FileAttributes attr = File.GetAttributes(location);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                this.type = Types.Directory;
                var file = new DirectoryInfo(location);
                this.children = new List<DiskFile>();

                this.root = file;
                this.dir = file;
                this.location = location;
                this.name = file.Name;

                var files = file.GetFileSystemInfos();
                foreach (var f in files)
                {
                    this.children.Add(new DiskFile(f.FullName));
                }
                this.size = this.children.Count;

            }
            else
            {
                this.type = Types.File;
                
                var file = new FileInfo(location);
                this.fileInfo = file;
                this.root = file;
                this.location = location;
                this.name = file.Name;
                this.size = file.Length;
            }


        }

        public void DeleteFile()
        {

            if (Type == DiskFile.Types.File)
            {
                if (FileInfo.IsReadOnly)
                {
                    FileInfo.IsReadOnly = false;
                }
                FileInfo.Delete();
            }
            else
            {
                foreach (var d in children)
                {
                    
                        d.DeleteFile();                        
                   
                }
                dir.Delete();
            }
        }

        public void WriteTree(int level = 1)
        {
            for (int i = 0; i < level; i++)
            {
                Console.Write("\t");
            }

            Console.Write("{0} ", this.name);

            if (this.type == Types.Directory)
                Console.Write(" ({0}) ", this.children.Count);
            else
                Console.Write(" {0}  bajts ", this.size);

          //  Console.Write(" {0} \n", this.root.RAHS());
            // Console.Write("\n");

            level++;
            if (this.type == Types.Directory)
                foreach (DiskFile f in this.children)
                {
                    f.WriteTree(level);
                }

        }

        public void AddToSortedColection()
        {
       //     this.elements = new SortedDictionary<string, long>(new CustomStringComparer(StringComparer.CurrentCulture));
            foreach (DiskFile f in this.children)
            {
                this.elements.Add(f.name, f.size);
            }
        }

        public void Serialization()
        {
            IFormatter formatter = new BinaryFormatter();

            Stream outputStream = File.OpenWrite("dictionary.bin");
            formatter.Serialize(outputStream, this.elements);
            outputStream.Close();

            Stream inputStream = File.OpenRead("dictionary.bin");
            SortedDictionary<string, long> elements2 = (SortedDictionary<string, long>)formatter.Deserialize(inputStream);

            Console.WriteLine();
            foreach (KeyValuePair<string, long> kvp in elements2)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }


    }
}
