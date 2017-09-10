using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace Behaviour_tree_tool
{
    /*
    * Class: Node
    * -----------
    *
    * this is the base class for all other node types
    * that make up the behaviour tree
    *
    * Author: Callum Dunstone
    */
    [Serializable]
    [XmlInclude(typeof(ActionNode))]
    [XmlInclude(typeof(ConditionNode))]
    [XmlInclude(typeof(SequenceComposite))]
    [XmlInclude(typeof(SlectorComposite))]
    [XmlInclude(typeof(DecoratorComposite))]
    [XmlInclude(typeof(RandomSlector))]
    [XmlInclude(typeof(SwitchSlector))]
    public class Node
    {
        [XmlIgnore]/* List of all the nodes parents */
        public List<Node> m_parent = new List<Node>();

        [XmlArray("List_Of_Node_Parents_IDs"), XmlArrayItem(typeof(int), ElementName = "Parent_Node_ID")]
        public List<int> m_parentIDs = new List<int>();/* this is a list of all of its parents ID used for deserialization and reconnecting up its parents */

        [XmlIgnore]/* a list of all its children */
        public List<Node> m_children = new List<Node>();

        [XmlArray("List_Of_Node_childrens_IDs"), XmlArrayItem(typeof(int), ElementName = "Child_Node_ID")]
        public List<int> m_childIDs = new List<int>();/* this is a list of all of its childrens ID used for deserialization and reconnecting up its children */

        [XmlElement("Node_Num")]
        public int m_nodeNum = 0;/* the Nodes number */

        [XmlElement("Node_Description")]/* this is the discription of this node and what the user wants it to do */
        public string m_description = "Default Description";

        [XmlElement("Node_Type")]/* the node type */
        public string m_nodeType = "Null";
        
        [XmlElement("Node_Rect")]/* the rect of the node(pos width height) */
        public Rectangle m_rect;

        [XmlIgnore]/* the form it belongs to */
        protected Form2 m_formParent = null;

        [XmlIgnore]/* text box that the useer puts the diescription in */
        public TextBox m_textBox = null;

        [XmlIgnore]/* the nodes parent and child connection radius */
        protected float m_connectorRadius = 30;

        [XmlIgnore]/* the nodes ID */
        protected int m_ID = 0;

        [XmlElement("Node_ID")]
        public int ID/* public get set acces of the nodes ID */
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /*
        * operator: Equals
        * ----------------
        *
        * overide the nodes Equals operator so it uses the nodes ID
        *
        */
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Node n = (Node)obj;

            if (n is Node)
            {
                if (this.ID == n.ID)
                {
                    return true;
                }
            }

            return false;
        }

        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public Node()
        {
            m_rect.X = 0;
            m_rect.Y = 0;
            m_rect.Width = 100;
            m_rect.Height = 100;
        }

        /*
        * Function: SetID
        * ---------------
        *
        * this sets the nodes ID to some random 5 digit number
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void SetID()
        {
            Random r = new Random();
            
            m_ID = r.Next(0, 10) + r.Next(10, 100) +r.Next(100,1000) + r.Next(1000, 10000) + r.Next(10000, 100000);
        }

        /*
        * Function: SetFormParent
        * -----------------------
        *
        * sets the Nodes Form parent and sets up some of the text box info
        * 
        * Parameters: Form2 p
        *
        * returns: returns nothing as it is a void function
        */
        public void SetFormParent(Form2 p)
        {
            m_formParent = p;

            m_textBox = new TextBox();
            m_textBox.Text = m_description;

            m_formParent.Controls.Add(m_textBox);
            
            m_textBox.SetBounds(m_rect.X , (m_rect.Y + (int)(m_rect.Width * 0.5f) - 10), m_rect.Width, 20);

            TabStop();
        }

        /*
        * Function: TabStop
        * -----------------
        *
        * this deslects the textbox belonging to this node
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void TabStop()
        {
            m_textBox.TabStop = false;
            m_textBox.Enabled = false;
        }

        /*
        * Function: DeleteTextBox
        * -----------------------
        *
        * this deletes the text box when we delet the node
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void DeleteTextBox()
        {
            m_formParent.Controls.Remove(m_textBox);
            m_textBox = null;
        }

        /*
        * Function: SetParent
        * -------------------
        *
        * this sets the nodes parent node/s
        * 
        * Parameters: Node n the nodes new parent
        *
        * returns: returns nothing as it is a void function
        */
        public virtual void SetParent(Node n)
        {
            //check it is not allready a child node 
            foreach (Node c in m_children)
            {
                if (c == n)
                {
                    return;
                }
            }

            //check that it is not already a parent node
            foreach (Node p in m_parent)
            {
                if (p == n)
                {
                    return;
                }
            }

            //check that it is not us
            if (this == n)
            {
                return;
            }

            //finally add it to our parent list and its ID to our parent ID
            m_parent.Add(n);
            m_parentIDs.Add(n.ID);
        }

        /*
        * Function: SetChild
        * ------------------
        *
        * this sets the nodes child/children
        *
        * Parameters: Node n the nodes new child
        *
        * returns: returns nothing as it is a void function
        */
        public virtual void SetChild(Node n)
        {
            //check we dont already have it as a child
            foreach (Node c in m_children)
            {
                if (c == n)
                {
                    return;
                }
            }

            //check we dont have it already as a parent
            foreach (Node p in m_parent)
            {
                if (p == n)
                {
                    return;
                }
            }

            //check it is not us
            if (this == n)
            {
                return;
            }

            //set it as our new child node
            m_children.Add(n);
            m_childIDs.Add(n.ID);
        }

        /*
        * Function: CheckIfClickedIn
        * --------------------------
        *
        * this checks to see if we clicked on the node or not
        * 
        * Parameters: int x(mouseX), int y(mouseY), int offsetX(offset created from horizontal scroll), int offsetY(offset created by vertical scroll)
        *
        * returns: returns nothing as it is a void function
        */
        public bool CheckIfClickedIn(int x, int y, int offsetX, int offsetY)
        {

            //check to see if our mouse is with in bounds if so we clicked it
            if ((m_rect.X - offsetX) < x && ((m_rect.X + m_rect.Width) - offsetX) > x &&
                (m_rect.Y - offsetY) < y && ((m_rect.Y + m_rect.Height) - offsetY) > y)
            {
                m_textBox.Enabled = true;
                return true;
            }

            return false;
        }

        /*
        * Function: ChecIfClickedOnParentConnector
        * ----------------------------------------
        *
        * checks if we clicked on the parent connector (the one at the top of the node)
        * 
        * Parameters: int x(mouseX), int y(mouseY), int offsetX(offset created from horizontal scroll), int offsetY(offset created by vertical scroll)
        *
        * returns: returns nothing as it is a void function
        */
        public virtual bool ChecIfClickedOnParentConnector(float x, float y, int offsetX, int offsetY)
        {
            if ((((m_rect.X + (m_rect.Width * 0.36f)) - offsetX) - (m_connectorRadius * 0.05f)) < x &&
                (((m_rect.X + (m_rect.Width * 0.36f)) - offsetX) + (m_connectorRadius)) > x &&
                (((m_rect.Y + (m_rect.Height * 0.02f)) - offsetY) - (m_connectorRadius * 0.05f)) < y &&
                (((m_rect.Y + (m_rect.Height * 0.02f)) - offsetY) + (m_connectorRadius)) > y)
            {
                return true;
            }

            return false;
        }

        /*
        * Function: CheckIfClickedOnCildConector
        * --------------------------------------
        *
        * check if we clicked on the child connector (the one at the bottom)
        * 
        * Parameters: int x(mouseX), int y(mouseY), int offsetX(offset created from horizontal scroll), int offsetY(offset created by vertical scroll)
        *
        * returns: returns nothing as it is a void function
        */
        public virtual bool CheckIfClickedOnCildConector(float x, float y, int offsetX, int offsetY)
        {
            if ((((m_rect.X + (m_rect.Width * 0.36f)) - offsetX) - (m_connectorRadius * 0.05f)) < x &&
               (((m_rect.X + (m_rect.Width * 0.36f)) - offsetX) + (m_connectorRadius)) > x &&
               (((m_rect.Y + (m_rect.Height * 0.7f)) - offsetY) - (m_connectorRadius * 0.05f)) < y &&
               (((m_rect.Y + (m_rect.Height * 0.7f)) - offsetY) + (m_connectorRadius)) > y)
            {
                return true;
            }

            return false;
        }

        /*
       * Function: OnDraw
       * ----------------
       *
       * this draws out the node to screen
       *
       * Parameters: PaintEventArgs e(used to draw the item out on screen), int offsetX (the * offset created by the horizontal scroll bar), int offsetY(the offset created by the * vertical scroll bar)
       *
       * returns: returns nothing as it is a void function
       */
        public virtual void OnDraw(PaintEventArgs e, int offsetX, int offsetY) { }

        /*
        * Function: OnDrawConnections
        * ---------------------------
        *
        * this draws out the nodes connections to its children
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public void OnDrawConnections(PaintEventArgs e, int offsetX, int offsetY)
        {
            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            foreach (Node c in m_children)
            {
                g.DrawLine(pen,
                    m_rect.X + (int)(m_rect.Width * 0.5f) - offsetX,
                    m_rect.Y + (int)(m_rect.Height * 0.5f) - offsetY,
                    c.m_rect.X + (int)(c.m_rect.Width * 0.5f) - offsetX,
                    c.m_rect.Y + (int)(c.m_rect.Height * 0.5f) - offsetY);
            }

            if (m_textBox != null)
            {
                m_textBox.SetBounds(m_rect.X - offsetX, ((m_rect.Y + (int)(m_rect.Width * 0.5f) - 10) - offsetY), m_rect.Width, 20);
            }
        }

        /*
        * Function: NodeConstructorCS
        * ---------------------------
        *
        * writes to the file a CS code description for creating this node in the behaviour tree
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public void NodeConstructorCS(StreamWriter sw)
        {
            sw.WriteLine("          TreeNode n" + m_nodeNum.ToString() + " = new TreeNode();");
            sw.WriteLine("          m_nodes.Add(n" + m_nodeNum.ToString() + ");");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public virtual void ConstructRunFunctionCS(StreamWriter sw) { }

        /*
        * Function: ConstructChildConnectionsCS
        * -------------------------------------
        *
        * writes to the file a CS code description for connecting up the nodes children
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public void ConstructChildConnectionsCS(StreamWriter sw)
        {
            foreach (Node n in m_children)
            {
                sw.WriteLine("          n" + m_nodeNum.ToString() + ".m_children.Add(n" + n.m_nodeNum.ToString() + ");");
            }

            if (m_children.Count() > 0)
            {
                sw.WriteLine("          ");
            }
        }

        /*
        * Function: ConstructHeaderNodes
        * ------------------------------
        *
        * writes to the file a Header file the code description for creating this node in the behaviour tree
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public void ConstructHeaderNodes(StreamWriter sw)
        {
            sw.WriteLine("          TreeNode * n" + m_nodeNum.ToString() + " = new TreeNode();");
            sw.WriteLine("          m_nodes.push_back(n" + m_nodeNum.ToString() + ");");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructHeaderRunFunctions
        * -------------------------------------
        *
        * writes to the file a Header File the code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public virtual void ConstructHeaderRunFunctions(StreamWriter sw) { }

        /*
        * Function: ConstructHeaderChildren
        * ---------------------------------
        *
        * writes to the file a Header file the code description for connecting up the nodes children
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public void ConstructHeaderChildren(StreamWriter sw)
        {
            foreach (Node n in m_children)
            {
                sw.WriteLine("          n" + m_nodeNum.ToString() + "->m_children.push_back(n" + n.m_nodeNum.ToString() + ");");
            }

            if (m_children.Count() > 0)
            {
                sw.WriteLine("          ");
            }

        }
    }

    /*
    * Class: ActionNode
    * -----------------
    *
    * this is an action node, it will preform a action when exacuted in a behaviour tree
    *
    * Author: Callum Dunstone
    */
    [Serializable]
    public class ActionNode : Node
    {
        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public ActionNode() { m_nodeType = "Action Node"; }

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out the node and its connectors
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public override void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            Graphics g = e.Graphics;
            
            Brush brush = new SolidBrush(Color.Yellow);
            g.FillPie(brush, m_rect.X - offsetX, m_rect.Y - offsetY, m_rect.Width, m_rect.Height, 0.0f, 360.0f);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (m_rect.Height * 0.02f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        /*
        * Function: CheckIfClickedOnCildConector
        * --------------------------------------
        *
        * this effectivly desiables its child connector as this node can not have children
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public override bool CheckIfClickedOnCildConector(float x, float y, int offsetX, int offsetY)
        {
            return false;
        }

        /*
        * Function: SetChild
        * ------------------
        *
        * disables its set child function as it can not have children
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public override void SetChild(Node n)
        {
            return;
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructRunFunctionCS(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n"+m_nodeNum.ToString() + ".run = () =>");
            sw.WriteLine("          {");
            sw.WriteLine("          ");
            sw.WriteLine("             //code for desiered node action here");
            sw.WriteLine("             ");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructHeaderRunFunctions
        * -------------------------------------
        *
        * writes to the file a header file the code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructHeaderRunFunctions(StreamWriter sw)
        {
            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + "->run = [n" + m_nodeNum.ToString() + "]()->bool");
            sw.WriteLine("          {");
            sw.WriteLine("          ");
            sw.WriteLine("             //code for desiered node action here");
            sw.WriteLine("             ");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }
    }

    /*
    * Class: ConditionNode
    * --------------------
    *
    * ConditionNode this is when exacuted in the behaviour tree
    * will check if a condition is true or not
    *
    * Author: Callum Dunstone
    */
    [Serializable]
    public class ConditionNode : Node
    {
        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public ConditionNode() { m_nodeType = "Condition Node"; }

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out the node and its connectors
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public override void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Green);
            g.FillPie(brush, m_rect.X - offsetX, m_rect.Y - offsetY, m_rect.Width, m_rect.Height, 0.0f, 360.0f);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.02f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        /*
        * Function: CheckIfClickedOnCildConector
        * --------------------------------------
        *
        * this effectivly desiables its child connector as this node can not have children
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public override bool CheckIfClickedOnCildConector(float x, float y, int offsetX, int offsetY)
        {
            return false;
        }

        /*
        * Function: SetChild
        * ------------------
        *
        * disables its set child function as it can not have children
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public override void SetChild(Node n)
        {
            return;
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructRunFunctionCS(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + ".run = () =>");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //code for desiered node check here");
            sw.WriteLine("             if(true)");
            sw.WriteLine("             {");
            sw.WriteLine("                 return true;");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return false;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructHeaderRunFunctions
        * -------------------------------------
        *
        * writes to the file a Header file the code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructHeaderRunFunctions(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + "->run = [n" + m_nodeNum.ToString() + "]()->bool");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //code for desiered node check here");
            sw.WriteLine("             if(true)");
            sw.WriteLine("             {");
            sw.WriteLine("                 return true;");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return false;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }
    }

    /*
   * Class: SequenceComposite
   * ------------------------
   *
   * SequenceComposite preforms and logic on the behaviour tree
   * so it will only keep exacuting its children assuming the last child
   * it exacute returned true
   *
   * Author: Callum Dunstone
   */
    [Serializable]
    public class SequenceComposite : Node
    {
        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public SequenceComposite() { m_nodeType = "Sequence Composite Node"; }

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out the node and its connectors
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public override void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Blue);
            g.FillRectangle(brush, m_rect.X - offsetX, m_rect.Y - offsetY, m_rect.Width, m_rect.Height);
            
            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.7f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.02f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructRunFunctionCS(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + ".run = () =>");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates");
            sw.WriteLine("             foreach(TreeNode n in n" + m_nodeNum.ToString() + ".m_children)");
            sw.WriteLine("             {");
            sw.WriteLine("                 if(n.run() == false)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     return false;");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return false;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructHeaderRunFunctions
        * -------------------------------------
        *
        * writes to the file a Header file the code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructHeaderRunFunctions(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + "->run = [n" + m_nodeNum.ToString() + "]()->bool");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates");
            sw.WriteLine("             for each(TreeNode * n in n" + m_nodeNum.ToString() + "->m_children)");
            sw.WriteLine("             {");
            sw.WriteLine("                 if(n->run() == false)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     return false;");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return false;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }
    }

    /*
   * Class: SlectorComposite
   * -----------------------
   *
   * SlectorComposite preforms or logic on the behaviour tree
   * so it will stop exacuting when one if its children return true
   *
   * Author: Callum Dunstone
   */
    [Serializable]
    public class SlectorComposite : Node
    {
        /*the points we will use to draw the node out to screen*/
        private PointF[] m_pointF = new PointF[4];

        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public SlectorComposite() { m_nodeType = "Slector Composite Node"; }

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out the node and its connectors
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public override void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            SetPoints(offsetX, offsetY);

            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Blue);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (float)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (float)(m_rect.Height * 0.7f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (float)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (float)(m_rect.Height * 0.02f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        /*
        * Function: SetPoints
        * -------------------
        *
        * sets up then nodes points with the appropriate off sets
        * 
        * Parameters: int offsetX, int offsetY (the offset created by the vertical and horizontal scroll bars)
        *
        * returns: returns nothing as it is a void function
        */
        public void SetPoints(int offsetX, int offsetY)
        {
            //creates a diamond
            m_pointF[0].X = m_rect.X - offsetX;
            m_pointF[0].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;

            m_pointF[1].X = (m_rect.X + (m_rect.Width * 0.5f)) - offsetX;
            m_pointF[1].Y = m_rect.Y - offsetY;

            m_pointF[2].X = (m_rect.X + (m_rect.Width)) - offsetX;
            m_pointF[2].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;


            m_pointF[3].X = (m_rect.X + (m_rect.Width * 0.5f)) - offsetX;
            m_pointF[3].Y = (m_rect.Y + (m_rect.Height)) - offsetY;
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructRunFunctionCS(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + ".run = () =>");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through its children Nodes and returns true for the first child that returns true, used to reform or logic gates");
            sw.WriteLine("             foreach(TreeNode n in n" + m_nodeNum.ToString() + ".m_children)");
            sw.WriteLine("             {");
            sw.WriteLine("                 if(n.run() == true)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     return true;");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructHeaderRunFunctions
        * -------------------------------------
        *
        * writes to the file a Header Files code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructHeaderRunFunctions(StreamWriter sw)
        {
            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + "->run = [n" + m_nodeNum.ToString() + "]()->bool");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through its children Nodes and returns true for the first child that returns true, used to reform or logic gates");
            sw.WriteLine("             for each(TreeNode * n in n" + m_nodeNum.ToString() + "->m_children)");
            sw.WriteLine("             {");
            sw.WriteLine("                 if(n->run() == true)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     return true;");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }
    }

    /*
   * Class: DecoratorComposite
   * -------------------------
   *
   * DecoratorComposite allows special logic to be implamented for how it gets run
   * in the behaviour desicion tree
   *
   * Author: Callum Dunstone
   */
    [Serializable]
    public class DecoratorComposite : Node
    {
        /*the points we will use to draw the node out to screen*/
        private PointF[] m_pointF = new PointF[6];

        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public DecoratorComposite() { m_nodeType = "Decorator Node"; }

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out the node and its connectors
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public override void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            SetPoints(offsetX, offsetY);

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            Brush brush = new SolidBrush(Color.RosyBrown);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.7f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.02f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        /*
        * Function: SetPoints
        * -------------------
        *
        * sets up then nodes points with the appropriate off sets
        * 
        * Parameters: int offsetX, int offsetY (the offset created by the vertical and horizontal scroll bars)
        *
        * returns: returns nothing as it is a void function
        */
        public void SetPoints(int offsetX, int offsetY)
        {
            //creates a squashed hexagon
            m_pointF[0].X = m_rect.X - offsetX;
            m_pointF[0].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;

            m_pointF[1].X = (m_rect.X + (m_rect.Width * 0.2f)) - offsetX;
            m_pointF[1].Y = m_rect.Y - offsetY;

            m_pointF[2].X = (m_rect.X + (m_rect.Width * 0.8f)) - offsetX;
            m_pointF[2].Y = m_rect.Y - offsetY;
            
            m_pointF[3].X = (m_rect.X + (m_rect.Width)) - offsetX;
            m_pointF[3].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;

            m_pointF[4].X = (m_rect.X + (m_rect.Width * 0.8f)) - offsetX;
            m_pointF[4].Y = (m_rect.Y + (m_rect.Height)) - offsetY;

            m_pointF[5].X = (m_rect.X + (m_rect.Width * 0.2f)) - offsetX;
            m_pointF[5].Y = (m_rect.Y + (m_rect.Height)) - offsetY;
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructRunFunctionCS(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + ".run = () =>");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through its children Nodes assuming the delegator value check is true");
            sw.WriteLine("             if(true)");
            sw.WriteLine("             {");
            sw.WriteLine("                 foreach(TreeNode n in n" + m_nodeNum.ToString() + ".m_children)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     n.run();");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             //Implament your own true false logic here");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a Header File code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructHeaderRunFunctions(StreamWriter sw)
        {

            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + "->run = [n" + m_nodeNum.ToString() + "]()->bool");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through its children Nodes assuming the delegator value check is true");
            sw.WriteLine("             if(true)");
            sw.WriteLine("             {");
            sw.WriteLine("                 for each(TreeNode * n in n" + m_nodeNum.ToString() + "->m_children)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     n->run();");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             //Implament your own true false logic, this is just a holder");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }
    }

    /*
   * Class: RandomSlector
   * --------------------
   *
   * this will randomly select one of its childrent to exacute
   *
   * Author: Callum Dunstone
   */
    [Serializable]
    public class RandomSlector : Node
    {
        /*the points we will use to draw the node out to screen*/
        private PointF[] m_pointF = new PointF[4];

        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public RandomSlector() { m_nodeType = "Random Slector Node"; }

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out the node and its connectors
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public override void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            SetPoints(offsetX, offsetY);

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            Brush brush = new SolidBrush(Color.LightBlue);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.7f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.02f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        /*
        * Function: SetPoints
        * -------------------
        *
        * sets up then nodes points with the appropriate off sets
        * 
        * Parameters: int offsetX, int offsetY (the offset created by the vertical and horizontal scroll bars)
        *
        * returns: returns nothing as it is a void function
        */
        public void SetPoints(int offsetX, int offsetY)
        {
            m_pointF[0].X = m_rect.X - offsetX;
            m_pointF[0].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;

            m_pointF[1].X = (m_rect.X + (m_rect.Width * 0.5f)) - offsetX;
            m_pointF[1].Y = m_rect.Y - offsetY;

            m_pointF[2].X = (m_rect.X + (m_rect.Width)) - offsetX;
            m_pointF[2].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;


            m_pointF[3].X = (m_rect.X + (m_rect.Width * 0.5f)) - offsetX;
            m_pointF[3].Y = (m_rect.Y + (m_rect.Height)) - offsetY;
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructRunFunctionCS(StreamWriter sw)
        {
            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + ".run = () =>");
            sw.WriteLine("          {");
            sw.WriteLine("             Random r = new Random();");
            sw.WriteLine("             int i = r.Next(0, n" + m_nodeNum.ToString() + ".m_children.Count + 1);");
            sw.WriteLine("             int count = 0;");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through its children Nodes assuming the delegator value check is true");
            sw.WriteLine("             foreach(TreeNode n in n" + m_nodeNum.ToString() + ".m_children)");
            sw.WriteLine("             {");
            sw.WriteLine("                 if(count == i)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     return n.run();");
            sw.WriteLine("                 }");
            sw.WriteLine("                 count++;");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("              return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }

        /*
       * Function: ConstructRunFunctionCS
       * --------------------------------
       *
       * writes to the file a Header File the code description for the delegate function we want to exacute
       * 
       * Parameters: StreamWriter sw, the file we are writing this to
       *
       * returns: returns nothing as it is a void function
       */
        public override void ConstructHeaderRunFunctions(StreamWriter sw)
        {
            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + "->run = [n" + m_nodeNum.ToString()+"]()->bool");
            sw.WriteLine("          {");
            sw.WriteLine("             int i = rand() % n5->m_children.size();");
            sw.WriteLine("             int count = 0;");
            sw.WriteLine("             ");
            sw.WriteLine("             //runs through its children Nodes assuming the delegator value check is true");
            sw.WriteLine("             for each(TreeNode * n in n" + m_nodeNum.ToString() + "->m_children)");
            sw.WriteLine("             {");
            sw.WriteLine("                 if(count == i)");
            sw.WriteLine("                 {");
            sw.WriteLine("                     return n->run();");
            sw.WriteLine("                 }");
            sw.WriteLine("                 count++;");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("              return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }
    }

    /*
   * Class: SwitchSlector
   * --------------------
   *
   * this will exacute one of its children based on a switch statment
   *
   * Author: Callum Dunstone
   */
    [Serializable]
    public class SwitchSlector : Node
    {
        /*the points we will use to draw the node out to screen*/
        private PointF[] m_pointF = new PointF[4];

        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public SwitchSlector() { m_nodeType = "Switch Slector Node"; }

        /*
        * Function: OnDraw
        * ----------------
        *
        * this draws out the node and its connectors
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen), 
        * int offsetX (the offset created by the horizontal scroll bar), 
        * int offsetY(the offset created by the vertical scroll bar)
        *
        * returns: returns nothing as it is a void function
        */
        public override void OnDraw(PaintEventArgs e, int offsetX, int offsetY)
        {
            SetPoints(offsetX, offsetY);

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            Brush brush = new SolidBrush(Color.Cyan);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.37f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.7f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.37f)) - offsetX, (m_rect.Y + (int)(m_rect.Height * 0.02f)) - offsetY, m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        /*
        * Function: SetPoints
        * -------------------
        *
        * sets up then nodes points with the appropriate off sets
        * 
        * Parameters: int offsetX, int offsetY (the offset created by the vertical and horizontal scroll bars)
        *
        * returns: returns nothing as it is a void function
        */
        public void SetPoints(int offsetX, int offsetY)
        {
            m_pointF[0].X = m_rect.X - offsetX;
            m_pointF[0].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;

            m_pointF[1].X = (m_rect.X + (m_rect.Width * 0.5f)) - offsetX;
            m_pointF[1].Y = m_rect.Y - offsetY;

            m_pointF[2].X = (m_rect.X + (m_rect.Width)) - offsetX;
            m_pointF[2].Y = (m_rect.Y + (m_rect.Height * 0.5f)) - offsetY;


            m_pointF[3].X = (m_rect.X + (m_rect.Width * 0.5f)) - offsetX;
            m_pointF[3].Y = (m_rect.Y + (m_rect.Height)) - offsetY;
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a CS code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructRunFunctionCS(StreamWriter sw)
        {
            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + ".run = () =>");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             switch ()");
            sw.WriteLine("             {");
            sw.WriteLine("                 default:");
            sw.WriteLine("                     return true;");
            sw.WriteLine("                     break;");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }

        /*
        * Function: ConstructRunFunctionCS
        * --------------------------------
        *
        * writes to the file a Header File the code description for the delegate function we want to exacute
        * 
        * Parameters: StreamWriter sw, the file we are writing this to
        *
        * returns: returns nothing as it is a void function
        */
        public override void ConstructHeaderRunFunctions(StreamWriter sw)
        {
            sw.WriteLine("          //" + m_description);
            sw.WriteLine("          n" + m_nodeNum.ToString() + "->run = [n" + m_nodeNum.ToString() + "]()->bool");
            sw.WriteLine("          {");
            sw.WriteLine("             ");
            sw.WriteLine("             switch ()");
            sw.WriteLine("             {");
            sw.WriteLine("                 default:");
            sw.WriteLine("                     return true;");
            sw.WriteLine("                     break;");
            sw.WriteLine("             }");
            sw.WriteLine("             ");
            sw.WriteLine("             return true;");
            sw.WriteLine("          };");
            sw.WriteLine("          ");
        }
    }
}
