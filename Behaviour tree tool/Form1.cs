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

        private void NewForm()
        {
            Form2 newMDIChild = new Form2();

            newMDIChild.MdiParent = this;

            newMDIChild.Show();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                try
                {
                    RichTextBox theBox = (RichTextBox)activeChild.ActiveControl;
                    if (theBox != null)
                    {
                        IDataObject data = Clipboard.GetDataObject();

                        if (data.GetDataPresent(DataFormats.Text))
                        {
                            theBox.SelectedText = data.GetData(DataFormats.Text).ToString();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("You need to slelect the RichTextBox.");
                }
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                try
                {
                    RichTextBox theBox = (RichTextBox)activeChild.ActiveControl;
                    if (theBox != null)
                    {
                        Clipboard.SetDataObject(theBox.SelectedText);
                    }
                }
                catch
                {
                    MessageBox.Show("You need to slelect the RichTextBox.");
                }
            }
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
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            m_keyCurrentlyPressed.Remove((Keys)e.KeyValue);
        }
    }
}
