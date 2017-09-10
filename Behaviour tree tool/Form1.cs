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
using System.Xml.Serialization;

namespace Behaviour_tree_tool
{
    /*
    * Class: Form1
    * ------------
    *
    * this is the base form class that the other classes 
    * and extensions of this programme will be parented to
    *
    * Author: Callum Dunstone
    */
    public partial class Form1 : Form
    {
        /*
        * Enum: ExportType
        * ----------------
        *
        *  the diffrent export types this programe can export you 
        *  behaviour tree as
        *
        */
        private enum ExportType
        {
            CPP,
            CS,
            ALL
        };

        /* this holds all the keys you are currently holding for macros */
        private List<Keys> m_keyCurrentlyPressed = new List<Keys>();

        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor sets up some directory paths
        * used for things such as the saving and exporting
        *
        */
        public Form1()
        {
            KeyPreview = true;
            InitializeComponent();
            System.IO.Directory.CreateDirectory(Application.StartupPath + "/Saves");
            System.IO.Directory.CreateDirectory(Application.StartupPath + "/Exports");
        }

        /*
        * Function: newToolStripMenuItem_Click
        * ------------------------------------
        *
        * button function, when pressed it creates a new child Form
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewForm();
        }

        /*
        * Function: saveToolStripMenuItem_Click
        * -------------------------------------
        *
        * button function, when pressed this will start up the save file process
        * to save your behaviour tree
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        /*
        * Function: openToolStripMenuItem_Click
        * -------------------------------------
        *
        * button function, when pressed allows you to load/open up a saved behaviour
        * tree previously worked on
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        /*
        * Function: NewForm
        * -----------------
        *
        * this creates a new form and parents it to this one
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */

        private void NewForm()
        {
            Form2 newMDIChild = new Form2();

            newMDIChild.MdiParent = this;
                        
            newMDIChild.Show();
        }

        /*
        * Function: SaveFile
        * ------------------
        *
        * this saves your current active project, it will open up a file dialog and ask
        * where and what you want to save it as, then it will find this forms currently active 
        * form child and save the tree there into and xml file
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void SaveFile()
        {
            Stream myStream;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //setting up the file dialog filters
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            //get active form child
            Form2 saveForm = (Form2)this.ActiveMdiChild;

            //checks it is the right type of form
            if (saveForm is Form2)
            {
                //slect what you want to save it as
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //set the sream path to the file dialogs open file
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        //serialize the list of nodes making up the tree
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Node>));

                        StreamWriter sw = new StreamWriter(myStream);

                        saveForm.SaveNodeDescriptions();

                        serializer.Serialize(sw, saveForm.m_nodes);

                        sw.Close();
                        myStream.Close();
                    }
                }
            }            
        }

        /*
        * Function: OpenFile
        * ------------------
        *
        * this opens an xml file and reads the conntents and converts it to a list of nodes
        * effectivly loading in our behaviour tree in what ever its current state is
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void OpenFile()
        {
            Stream myStream;

            //file dialog that will let you choose what xml file to open
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 1;

            //makes sure we have picked a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //passes the file path to the stream and makes sure we actually have a valid one
                if ((myStream = openFileDialog.OpenFile()) != null)
                {
                    //creat a new Form2
                    Form2 newForm = new Form2();
                    newForm.MdiParent = this;
                    newForm.Show();

                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<Node>));

                    StreamReader SR = new StreamReader(myStream);

                    //deserialise the previously slected file into a list of nodes and pass them into the new forms nodes
                    newForm.m_nodes = mySerializer.Deserialize(SR) as List<Node>;

                    //tells the new form to finish untangaling and reconnecting the nodes properly
                    newForm.Deserialize();

                    SR.Close();
                    myStream.Close();
                }
            }

        }

        /*
        * Function: Form1_KeyDown
        * -----------------------
        *
        * this stores and checks for any macros that we may be pressing
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //if we dont have the key add it to the list of keys held down
            if (!m_keyCurrentlyPressed.Contains((Keys)e.KeyValue))
            {
                m_keyCurrentlyPressed.Add((Keys)e.KeyValue);
            }

            //if we are holding down control, sift and n we create a new form
            if (m_keyCurrentlyPressed.Contains(Keys.ControlKey) 
                && m_keyCurrentlyPressed.Contains(Keys.ShiftKey) 
                && m_keyCurrentlyPressed.Contains(Keys.N))
            {
                NewForm();
            }

            //if we are holding down control and s we save the file
            if (m_keyCurrentlyPressed.Contains(Keys.ControlKey)
                && m_keyCurrentlyPressed.Contains(Keys.S))
            {
                SaveFile();
            }

            //if we are holding down control and o we open a file
            if (m_keyCurrentlyPressed.Contains(Keys.ControlKey)
                && m_keyCurrentlyPressed.Contains(Keys.O))
            {
                OpenFile();
            }
        }

        /*
        * Function: Form1_KeyUp
        * ---------------------
        *
        * this removes a key from keys being held down when we release the key
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            m_keyCurrentlyPressed.Remove((Keys)e.KeyValue);
        }

        /*
        * Function: csToolStripMenuItem_Click
        * -----------------------------------
        *
        * this tells the programme to export the behaviour tree as a
        * C# file
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void csToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefineExportType(ExportType.CS);
        }

        /*
        * Function: cppToolStripMenuItem1_Click
        * -------------------------------------
        * 
        * this tells the programme to export the behaviour tree as a
        * C++ file
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void cppToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DefineExportType(ExportType.CPP);
        }

        /*
        * Function: allToolStripMenuItem_Click
        * ------------------------------------
        *
        * this tells the programe to export the behaviour tree as a C#, C++ and JV files
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefineExportType(ExportType.ALL);
        }

        /*
        * Function: DefineExportType
        * -------------------------
        *
        * this actually creates the new C++, CS and JV file
        * bassed on the enum type passed in to the function
        * 
        * Parameters: ExportType type(the Enum type used to determin what we are exporting)
        *
        * returns: returns nothing as it is a void function
        */
        private void DefineExportType(ExportType type)
        {
            Form2 activeForm = (Form2)this.ActiveMdiChild;

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            Stream myStream;

            StreamWriter sw = null;

            if (activeForm != null)
            {
                if (activeForm.CheckTreeIsAllConnected())
                {
                    switch (type)
                    {
                        case ExportType.CPP:
                            //setting up the file dialog filters
                            saveFileDialog.Filter = "h files (*.h)|*.h";
                            saveFileDialog.FilterIndex = 1;
                            saveFileDialog.RestoreDirectory = true;

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                //set the sream path to the file dialogs open file
                                if ((myStream = saveFileDialog.OpenFile()) != null)
                                {
                                    sw = new StreamWriter(myStream);

                                    activeForm.CreatHeaderFile(sw);

                                    sw.Close();
                                    myStream.Close();
                                }
                            }
                            return;

                        case ExportType.CS:
                            //setting up the file dialog filters
                            saveFileDialog.Filter = "cs files (*.cs)|*.cs";
                            saveFileDialog.FilterIndex = 1;
                            saveFileDialog.RestoreDirectory = true;

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                //set the sream path to the file dialogs open file
                                if ((myStream = saveFileDialog.OpenFile()) != null)
                                {
                                    sw = new StreamWriter(myStream);

                                    activeForm.CreatcsFile(sw);

                                    sw.Close();
                                    myStream.Close();
                                }
                            }

                            return;

                        case ExportType.ALL:
                            //setting up the file dialog filters
                            saveFileDialog.Filter = "cs files (*.cs)|*.cs";
                            saveFileDialog.FilterIndex = 1;
                            saveFileDialog.RestoreDirectory = true;

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                //set the sream path to the file dialogs open file
                                if ((myStream = saveFileDialog.OpenFile()) != null)
                                {
                                    sw = new StreamWriter(myStream);

                                    activeForm.CreatcsFile(sw);

                                    sw.Close();
                                    myStream.Close();
                                }
                            }
                            
                            saveFileDialog = new SaveFileDialog();

                            //setting up the file dialog filters
                            saveFileDialog.Filter = "h files (*.h)|*.h";
                            saveFileDialog.FilterIndex = 1;
                            saveFileDialog.RestoreDirectory = true;

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                //set the sream path to the file dialogs open file
                                if ((myStream = saveFileDialog.OpenFile()) != null)
                                {
                                    sw = new StreamWriter(myStream);

                                    activeForm.CreatHeaderFile(sw);

                                    sw.Close();
                                    myStream.Close();
                                }
                            }

                            return;

                        default:
                            return;
                    }
                }
                else
                {
                    MessageBox.Show("Not all of your tree is connected up");
                }

            }
        }

    }
}
