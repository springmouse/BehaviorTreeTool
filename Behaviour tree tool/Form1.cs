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

namespace Behaviour_tree_tool
{
    public partial class Form1 : Form
    {
        private List<Keys> m_keyCurrentlyPressed = new List<Keys>();

        public Form1()
        {
            KeyPreview = true;
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewForm();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void NewForm()
        {
            Form2 newMDIChild = new Form2();

            newMDIChild.MdiParent = this;

            newMDIChild.Show();
        }
        
        public void SaveFile()
        {
            Stream myStream;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {

                    myStream.Close();
                }
            }
        }
        
        public void OpenFile()
        {

        }
        
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!m_keyCurrentlyPressed.Contains((Keys)e.KeyValue))
            {
                m_keyCurrentlyPressed.Add((Keys)e.KeyValue);
            }

            if (m_keyCurrentlyPressed.Contains(Keys.ControlKey) 
                && m_keyCurrentlyPressed.Contains(Keys.ShiftKey) 
                && m_keyCurrentlyPressed.Contains(Keys.N))
            {
                NewForm();
            }

            if (m_keyCurrentlyPressed.Contains(Keys.ControlKey)
                && m_keyCurrentlyPressed.Contains(Keys.S))
            {
                SaveFile();
            }

            if (m_keyCurrentlyPressed.Contains(Keys.ControlKey)
                && m_keyCurrentlyPressed.Contains(Keys.O))
            {
                OpenFile();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            m_keyCurrentlyPressed.Remove((Keys)e.KeyValue);
        }
    }
}
