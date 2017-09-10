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
using System.IO;

namespace Behaviour_tree_tool
{
    /*
    * Class: Form2
    * ------------
    *
    * the form we will be creating the behaviour tree on
    *
    * Author: Callum Dunstone
    */
    public partial class Form2 : Form
    {
        /*List of all the nodes*/
        [XmlArray("List_Of_All_Nodes"), XmlArrayItem(typeof(Node), ElementName = "Node")]
        public List<Node> m_nodes = new List<Node>();

        [XmlIgnore]/*the start node of the tree(node with no parent)*/
        private Node m_startNode = null;

        [XmlIgnore]/*our custom mouse info*/
        private Mouse_Controler m_mouse = new Mouse_Controler();

        [XmlIgnore]/*all the used IDs*/
        private List<int> m_usedIDs = new List<int>();

        [XmlIgnore]/*start x, start y, last x, last y (used for box select logic)*/
        private int sx, sy, lx, ly;

        [XmlIgnore]/*previous x, previous y (used to find the mouses last position)*/
        private int px, py;

        [XmlIgnore]/*checks if the mouse is being held down (used for draging nodes around)*/
        private bool m_isHealdDown = false;

        [XmlIgnore]/*check if we can place a node*/
        private bool m_placeNode = false;

        [XmlIgnore]/*checks if we are holding shift (used to place multiple nodes down)*/
        private bool m_shiftPressed = false;

        [XmlIgnore]
        private int m_currNodeNum = 0;

        /*
        * Function: Constructor
        * ---------------------
        *
        * Default constructor
        *
        */
        public Form2()
        {
            InitializeComponent();
        }

        /*
        * Function: timer1_Tick
        * ---------------------
        *
        * thtis is used to update the horizontal and vertical scroll sizes
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        private void timer1_Tick(object sender, EventArgs e)
        {
            int x, y;
            x = y = 0;

            foreach (Node n in m_nodes)
            {
                if (n.m_rect.X > x)
                {
                    x = n.m_rect.X;
                }

                if (n.m_rect.Y > y)
                {
                    y = n.m_rect.Y;
                }
            }

            x += 50;
            y += 50;

            this.SetAutoScrollMargin(x, y);

        }

        /*
        * Function: OnPaint
        * -----------------
        *
        * this draws out all the the nodes and screen info to the screen
        *
        * Parameters: PaintEventArgs e(used to draw the item out on screen)
        *
        * returns: returns nothing as it is a void function
        */
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush2 = new SolidBrush(Color.Brown);

            //if (lx < sx)
            //{
            //    int h = sx;
            //    sx = lx;
            //    lx = h;
            //}

            //if (ly < sy)
            //{
            //    int h = sy;
            //    sy = ly;
            //    ly = h;
            //}

            //Pen pen = new Pen(Color.Black);

            //if (m_mouse.m_connectAsChild || m_mouse.m_connectAsParent)
            //{
            //    g.DrawLine(pen,
            //        px,
            //        py,
            //        m_mouse.m_nodeToConnect.m_rect.X + (int)(m_mouse.m_nodeToConnect.m_rect.Width * 0.5f),
            //        m_mouse.m_nodeToConnect.m_rect.Y + (int)(m_mouse.m_nodeToConnect.m_rect.Height * 0.5f));
            //}

            //draws the box slection 
            g.FillRectangle(brush2, sx, sy, lx - sx, ly - sy);

            //draws out mouse info
            m_mouse.OnDraw(e, this.HorizontalScroll.Value, this.VerticalScroll.Value);

            //draws out all the node connections
            foreach (Node node in m_nodes)
            {
                node.OnDrawConnections(e, this.HorizontalScroll.Value, this.VerticalScroll.Value);
            }

            //draws out all the nodes
            foreach (Node node in m_nodes)
            {
                node.OnDraw(e, this.HorizontalScroll.Value, this.VerticalScroll.Value);
            }
        }

        /*
        * Function: Form2_MouseClick
        * --------------------------
        *
        * preforms mouse click logic
        * 
        * Parameters: object sender, MouseEventArgs e. holds the mouse info for winforms
        *
        * returns: returns nothing as it is a void function
        */
        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ClearMouseVars();
            }

            DeslectSingle(e);

            DeslectMultiple(e);

            SlectNode(e);

            PlaceNode(e);

        }

        /*
        * Function: ClearMouseVars
        * ------------------------
        *
        * clears out all the mouse variables and resets it
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void ClearMouseVars()
        {
            m_mouse.m_nodeToPlace = NodeTypes.NULL;
            m_mouse.m_slectedNodes.Clear();

            m_mouse.m_nodeToConnect = null;

            m_mouse.m_connectAsChild = false;
            m_mouse.m_connectAsParent = false;

            foreach (Node n in m_nodes)
            {
                n.TabStop();
            }

            this.Refresh();
        }

        /*
        * Function: 
        * ---------
        *
        * if a node is slected and we click off it deslect it
        * 
        * Parameters: MouseEventArgs e. holds the mouse info for winforms
        *
        * returns: returns nothing as it is a void function
        */
        public void DeslectSingle(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && m_mouse.m_slectedNodes.Count() == 1)
            {
                bool wasClicked = true;
                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    if (node.CheckIfClickedIn(e.X, e.Y, this.HorizontalScroll.Value, this.VerticalScroll.Value) == false)
                    {
                        wasClicked = false;
                    }
                }

                if (wasClicked == false)
                {
                    ClearMouseVars();
                    this.Refresh();
                }
            }
        }

        /*
        * Function: DeslectMultiple
        * -------------------------
        *
        * creates an invisible box around all slected nodes and if we click out of it
        * we clear the mouse values
        * 
        * Parameters: MouseEventArgs e. holds the mouse info for winforms
        *
        * returns: returns nothing as it is a void function
        */
        public void DeslectMultiple(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && m_mouse.m_slectedNodes.Count() > 1)
            {
                int bx, by, tx, ty;
                bx = by = 1000000000;
                tx = ty = 0;

                //find the top left and bottom right values
                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    if (node.m_rect.X < bx)
                    {
                        bx = node.m_rect.X;
                    }
                    if (node.m_rect.Y < by)
                    {
                        by = node.m_rect.Y;
                    }
                    if ((node.m_rect.X + node.m_rect.Width) > tx)
                    {
                        tx = (node.m_rect.X + node.m_rect.Width);
                    }
                    if ((node.m_rect.Y + node.m_rect.Height) > ty)
                    {
                        ty = (node.m_rect.Y + node.m_rect.Height);
                    }
                }

                //add a bit of meat to the box
                bx -= 50;
                by -= 50;
                tx += 50;
                by += 50;

                //check if we clicked in it
                if ((bx < e.X && tx > e.X &&
                    by < e.Y && ty > e.Y) == false)
                {
                    ClearMouseVars();
                    this.Refresh();
                }
            }
        }

        /*
        * Function: SlectNode
        * -------------------
        *
        * this checks if we have clicked on a node and the if so checks if we clicked
        * on one of its child/parent connectors. then either slects the node and set it up as a node to connect
        * unless we are looking to set up a connection then we connect up our node to the one we just clicked
        * 
        * Parameters: MouseEventArgs e. holds the mouse info for winforms
        *
        * returns: returns nothing as it is a void function
        */
        public void SlectNode(MouseEventArgs e)
        {
            //make sure we are not trying to start up a connection
            if (m_mouse.m_isDragging == false && m_mouse.m_connectAsChild == false && m_mouse.m_connectAsParent == false)
            {
                //go through all the nodes
                foreach (Node node in m_nodes)
                {
                    //returns true if we clicked on this node
                    if (node.CheckIfClickedIn(e.X, e.Y, this.HorizontalScroll.Value, this.VerticalScroll.Value))
                    {
                        //returns true if we clicked on its parent connection
                        if (node.ChecIfClickedOnParentConnector(e.X, e.Y, this.HorizontalScroll.Value, this.VerticalScroll.Value))
                        {
                            m_mouse.m_nodeToConnect = node;
                            m_mouse.m_connectAsChild = true;
                            return;
                        }
                        //returns true if we clicked on its child connector
                        if (node.CheckIfClickedOnCildConector(e.X, e.Y, this.HorizontalScroll.Value, this.VerticalScroll.Value))
                        {
                            m_mouse.m_nodeToConnect = node;
                            m_mouse.m_connectAsParent = true;
                            return;
                        }

                        //clear slected nodes then add it
                        m_mouse.m_slectedNodes.Clear();
                        m_mouse.m_slectedNodes.Add(node);

                        this.Refresh();

                        return;
                    }
                }
            }
            //if we are trying to connect a node up then we do this
            else if (m_mouse.m_connectAsParent == true || m_mouse.m_connectAsChild == true)
            {
                //make sure we are not clicking on our selves
                if (m_mouse.m_nodeToConnect.CheckIfClickedIn(e.X, e.Y, this.HorizontalScroll.Value, this.VerticalScroll.Value))
                {
                    return;
                }

                //go through all the nodes and if we have clicked on one depending on what we are tying to connect it up as (child or parent)
                //do so
                foreach (Node node in m_nodes)
                {
                    if (node.CheckIfClickedIn(e.X, e.Y, this.HorizontalScroll.Value, this.VerticalScroll.Value))
                    {
                        if (m_mouse.m_connectAsChild)
                        {
                            node.SetChild(m_mouse.m_nodeToConnect);
                            m_mouse.m_nodeToConnect.SetParent(node);
                            ClearMouseVars();
                            return;
                        }
                        else
                        {
                            node.SetParent(m_mouse.m_nodeToConnect);
                            m_mouse.m_nodeToConnect.SetChild(node);
                            ClearMouseVars();
                            return;
                        }
                    }
                }
            }
        }

        /*
        * Function: PlaceNode
        * -------------------
        *
        * this places a node down in the world
        * 
        * Parameters: MouseEventArgs e. holds the mouse info for winforms
        *
        * returns: returns nothing as it is a void function
        */
        public void PlaceNode(MouseEventArgs e)
        {
            //check we have a node tpye selected
            if (m_mouse.m_nodeToPlace != NodeTypes.NULL && m_mouse.m_slectedNodes.Count <= 0 && m_placeNode == true)
            {
                m_mouse.m_slectedNodes.Clear();

                Node newNode = NodeToCreat();

                //if we are holding shift we get to place more else no
                if (m_shiftPressed == false)
                {
                    m_placeNode = false;
                    ClearMouseVars();
                }

                //make sure we actually have a node then set it up
                if (newNode != null)
                {
                    newNode.m_rect.X = e.X + this.HorizontalScroll.Value;
                    newNode.m_rect.Y = e.Y + this.VerticalScroll.Value;

                    newNode.SetFormParent(this);

                    newNode.m_nodeNum = m_currNodeNum;
                    m_currNodeNum++;

                    //make a new unique ID
                    while (newNode.ID == 0 || m_usedIDs.Contains(newNode.ID))
                    {
                        newNode.SetID();
                    }

                    m_usedIDs.Add(newNode.ID);

                    m_nodes.Add(newNode);
                    this.Refresh();
                }
            }
        }

        /*
        * Function: NodeToCreat
        * ---------------------
        *
        * this figures out what node wa want to make then creates it
        * 
        * Parameters: none
        *
        * returns: a new node
        */
        public Node NodeToCreat()
        {
            switch (m_mouse.m_nodeToPlace)
            {
                case NodeTypes.ACTIONNODE:
                    return new ActionNode();

                case NodeTypes.CONDITIONNODE:
                    return new ConditionNode();

                case NodeTypes.SEQUENCECOMPOSITE:
                    return new SequenceComposite();

                case NodeTypes.SLECTORCOMPOSITE:
                    return new SlectorComposite();

                case NodeTypes.DECORATOR:
                    return new DecoratorComposite();

                case NodeTypes.RANDOMSLECTOR:
                    return new RandomSlector();

                case NodeTypes.SWITCHSLECTOR:
                    return new SwitchSlector();

                case NodeTypes.NULL:
                    return null;

                default:
                    return null;
            }
        }

        /*
        * Function: sequenceNodeToolStrip_Click
        * -------------------------------------
        *
        * button function, sets the mouse to place a Sequeance node
        * 
        * Parameters:  object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void sequenceNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SEQUENCECOMPOSITE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        /*
        * Function: slectorNodeToolStrip_Click
        * ------------------------------------
        *
        * button function, sets the mouse to place a Slector node
        * 
        * Parameters:  object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void slectorNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SLECTORCOMPOSITE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        /*
        * Function: decoratorToolStripMenuItem_Click
        * ------------------------------------------
        *
        * button function, sets the mouse to place a Decorator node
        * 
        * Parameters:  object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void decoratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.DECORATOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        /*
        * Function: randomSlectorNodeToolStripMenuItem_Click
        * --------------------------------------------------
        *
        * button function, sets the mouse to place a Random Slector node
        * 
        * Parameters:  object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void randomSlectorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.RANDOMSLECTOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        /*
        * Function: switchSlectorNodeToolStripMenuItem_Click
        * --------------------------------------------------
        *
        * button function, sets the mouse to place a Switch Slector node
        * 
        * Parameters:  object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void switchSlectorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.SWITCHSLECTOR;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        /*
        * Function: actionNodeToolStrip_Click
        * -----------------------------------
        *
        * button function, sets the mouse to place a Action node
        * 
        * Parameters:  object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void actionNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.ACTIONNODE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        /*
        * Function: conditionNodeToolStrip_Click
        * --------------------------------------
        *
        * button function, sets the mouse to place a Condition node
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void conditionNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_mouse.m_nodeToPlace = NodeTypes.CONDITIONNODE;
            m_mouse.m_slectedNodes.Clear();
            m_mouse.m_isDragging = false;
            m_placeNode = true;
        }

        /*
        * Function: Form2_KeyUp
        * ---------------------
        *
        * this simply detects when we stop holding down Shift
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                m_shiftPressed = false;
                ClearMouseVars();
            }
        }

        /*
        * Function: Form2_KeyDown
        * -----------------------
        *
        * this detects when we press down a key and preforms appropriate logic
        * 
        * Parameters: object sender, EventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            //deletes slected nodes
            if (m_mouse.m_slectedNodes.Count() > 0 && e.KeyCode == Keys.Delete)
            {
                DeleteNodes();
            }

            //sets holding shift to true
            if (e.KeyCode == Keys.ShiftKey)
            {
                m_shiftPressed = true;
            }
        }

        /*
        * Function: DeleteNodes
        * ---------------------
        *
        * this deletes the slected nodes
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void DeleteNodes()
        {
            //remove all slected nodes from the main list and there IDs from the used IDs
            foreach (Node node in m_mouse.m_slectedNodes)
            {
                m_nodes.Remove(node);
                m_usedIDs.Remove(node.ID);
            }

            //go through all the slected nodes and remove refrences to them in there children and parents
            foreach (Node node in m_mouse.m_slectedNodes)
            {
                foreach (Node p in node.m_parent)
                {
                    p.m_children.Remove(node);
                    p.m_childIDs.Remove(node.ID);
                }

                foreach (Node c in node.m_children)
                {
                    c.m_parent.Remove(node);
                    c.m_parentIDs.Remove(node.ID);
                }

                //delete there text box
                node.DeleteTextBox();
            }

            //clear the slected node list removing all instances of them
            m_mouse.m_slectedNodes.Clear();

            this.Refresh();
        }

        /*
        * Function: Form2_MouseMove
        * -------------------------
        *
        * detects when the mouse moves and preforms appropriate logic
        * 
        * Parameters: object sender, MouseEventArgs e. default winform parameters
        *
        * returns: returns nothing as it is a void function
        */
        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            //deslects a for a single item
            DeslectSingle(e);

            //deslects all slected items
            DeslectMultiple(e);

            //drags a box around an area
            BoxDrag(e);

            //slects nodes in the box draged area
            DragSlectedNodes(e);

            px = e.X;
            py = e.Y;
        }

        /*
        * Function: BoxDrag
        * -----------------
        *
        * logic for the box drag slect
        * 
        * Parameters: MouseEventArgs 
        *
        * returns: returns nothing as it is a void function
        */
        public void BoxDrag(MouseEventArgs e)
        {
            //if conditions met we start drag
            if (e.Button == MouseButtons.Left && m_mouse.m_slectedNodes.Count() <= 0 && m_mouse.m_nodeToPlace == NodeTypes.NULL && m_mouse.m_nodeToConnect == null)
            {
                //if we just started the drag make sure we are not actuall slecting a node and if not
                //set up drag logic variables
                if (m_mouse.m_isDragging == false && m_mouse.m_slectedNodes.Count() <= 0)
                {
                    SlectNode(e);
                    if (m_mouse.m_slectedNodes.Count() <= 0)
                    {
                        if (m_mouse.m_isDragging == false)
                        {
                            sx = e.X;
                            sy = e.Y;
                            m_mouse.m_isDragging = true;
                        }
                    }
                    else
                    {
                        sx = sy = lx = ly = 0;
                    }
                }

                //update drag box position
                if (m_mouse.m_isDragging == true)
                {
                    lx = e.X;
                    ly = e.Y;
                    this.Refresh();
                }
            }
            else
            {
                //if we stoped draging do the box select
                if (m_mouse.m_isDragging == true)
                {
                    BoxSlect(this.HorizontalScroll.Value, this.VerticalScroll.Value);
                    m_mouse.m_isDragging = false;
                    this.Refresh();
                }
            }
        }

        /*
        * Function: DragSlectedNodes
        * --------------------------
        *
        * this drags around the nodeswe have selected
        * 
        * Parameters: MouseEventArgs e
        *
        * returns: returns nothing as it is a void function
        */
        public void DragSlectedNodes(MouseEventArgs e)
        {
            if (m_mouse.m_slectedNodes.Count > 0 && e.Button == MouseButtons.Left)
            {
                //check we are not actually clicking on a node
                if (m_isHealdDown == false)
                {
                    SlectNode(e);

                    if (m_mouse.m_slectedNodes.Count <= 0)
                    {
                        return;
                    }
                }

                //go through all the nodes and update there position by the offset between the mouse in its previous position
                // and the mouse in its current position
                foreach (Node node in m_mouse.m_slectedNodes)
                {
                    node.m_rect.X += (int)((e.X - px));
                    node.m_rect.Y += (int)((e.Y - py));
                }
                this.Refresh();

                //update previous position to current position
                px = e.X;
                py = e.Y;

                m_isHealdDown = true;
            }
            else
            {
                m_isHealdDown = false;
            }
        }

        /*
        * Function: BoxSlect
        * ------------------
        *
        * slects all nodes in the box created from the mouse drag
        * 
        * Parameters: int offsetX, int offsetY
        *
        * returns: returns nothing as it is a void function
        */
        public void BoxSlect(int offsetX, int offsetY)
        {
            if (lx < sx)
            {
                int h = sx;
                sx = lx;
                lx = h;
            }

            if (ly < sy)
            {
                int h = sy;
                sy = ly;
                ly = h;
            }

            foreach (Node node in m_nodes)
            {
                if ((sx) < (node.m_rect.X - offsetX) && (lx) > (node.m_rect.X - offsetX) &&
                (sy) < (node.m_rect.Y - offsetY) && (ly) > (node.m_rect.Y - offsetY))
                {
                    m_mouse.m_slectedNodes.Add(node);
                }
            }

            this.Refresh();

            sx = sy = lx = ly = 0;
        }

        /*
        * Function: Deserialize
        * ---------------------
        *
        * this resets up m_nodes when they are passed into the form from Deserialization
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void Deserialize()
        {
            //reset up used IDs
            m_usedIDs.Clear();

            foreach (Node n in m_nodes)
            {
                m_usedIDs.Add(n.ID);
                if (n.m_nodeNum > m_currNodeNum)
                {
                    m_currNodeNum = n.m_nodeNum;
                }
            }

            m_currNodeNum++;

            //set up the nodes actual parents
            SetActualPareents();

            //reset up the nodes actual children
            SetActualChildren();

            foreach (Node n in m_nodes)
            {
                n.SetFormParent(this);
                n.TabStop();
            }

            this.Refresh();
        }

        /*
        * Function: SetActualPareents
        * ---------------------------
        *
        * this resets the nodes actual parents to nodes in m_nodes using the nodes IDs
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        private void SetActualPareents()
        {
            foreach (Node n in m_nodes)
            {
                n.m_parent.Clear();

                foreach (int id in n.m_parentIDs)
                {
                    foreach (Node p in m_nodes)
                    {
                        if (id == p.ID)
                        {
                            n.m_parent.Add(p);
                        }
                    }
                }
            }
        }

        /*
        * Function: SetActualChildren
        * ---------------------------
        *
        * this resets the nodes actual children to nodes in m_nodes using the nodes IDs
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        private void SetActualChildren()
        {
            foreach (Node n in m_nodes)
            {
                n.m_children.Clear();

                foreach (int id in n.m_childIDs)
                {
                    foreach (Node c in m_nodes)
                    {
                        if (id == c.ID)
                        {
                            n.m_children.Add(c);
                        }
                    }
                }
            }
        }

        /*
        * Function: CheckTreeIsAllConnected
        * ---------------------------------
        *
        * this starts at what the programe has deemed the start node
        * and checks if it can reach all other nodes in the list, if not it returns false
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public bool CheckTreeIsAllConnected()
        {
            if (m_nodes.Count() <= 0)
            {
                return false;
            }

            m_startNode = GetTreeStartNode();

            List<Node> closedNodes = new List<Node>();

            List<Node> gatherNodes = new List<Node>();
            List<Node> openNodes = new List<Node>();

            closedNodes.Clear();
            gatherNodes.Clear();
            openNodes.Clear();

            if (m_startNode != null)
            {
                closedNodes.Add(m_startNode);

                foreach (Node node in m_startNode.m_children)
                {
                    openNodes.Add(node);
                }

                while (openNodes.Count() > 0)
                {
                    foreach (Node o in openNodes)
                    {
                        closedNodes.Add(o);
                        gatherNodes.Add(o);
                    }

                    openNodes.Clear();

                    foreach (Node g in gatherNodes)
                    {
                        foreach (Node c in g.m_children)
                        {
                            openNodes.Add(c);
                        };
                    }

                    gatherNodes.Clear();
                }

                if (closedNodes.Count() >= m_nodes.Count())
                {
                    return true;
                }
            }

            return false;

        }

        /*
        * Function: GetTreeStartNode
        * --------------------------
        *
        * this gets the start node for the behaviour tree, wich is what ever node it comes acrose
        * in m_nodes with no parent
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        private Node GetTreeStartNode()
        {

            foreach (Node n in m_nodes)
            {
                if (n.m_parent.Count() == 0)
                {
                    return n;
                }
            }

            return null;

        }

        /*
         * 
        * Function: SaveNodeDescriptions
        * ------------------------------
        *
        * this stores the text in the node textbox in the node description
        * 
        * Parameters: none
        *
        * returns: returns nothing as it is a void function
        */
        public void SaveNodeDescriptions()
        {
            foreach (Node n in m_nodes)
            {
                n.m_description = n.m_textBox.Text;
            }
        }

        /*
         * 
        * Function: CreatcsFile
        * ---------------------
        *
        * this writes out a CS behaviour tree based off the tree we made
        * graphically
        * 
        * Parameters: StreamWriter sw, the file we need to write in
        *
        * returns: returns nothing as it is a void function
        */
        public void CreatcsFile(StreamWriter sw)
        {
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("");

            sw.WriteLine("/*");
            sw.WriteLine("* class: BehaviourTree");
            sw.WriteLine("* --------------------");
            sw.WriteLine("*");
            sw.WriteLine("* ____________________");
            sw.WriteLine("*");
            sw.WriteLine("* Author: ____________");
            sw.WriteLine("*/");
            sw.WriteLine("public class BehaviourTree");
            sw.WriteLine("{");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * class: TreeNode");
            sw.WriteLine("      * ---------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * _______________");
            sw.WriteLine("      *");
            sw.WriteLine("      * Author: _______");
            sw.WriteLine("      */");
            sw.WriteLine("      public class TreeNode");
            sw.WriteLine("      {");
            sw.WriteLine("          //the list of the nodes children");
            sw.WriteLine("          public List<TreeNode> m_children = new List<TreeNode>();");
            sw.WriteLine("          ");
            sw.WriteLine("          //the delegate function we will assign to exacute");
            sw.WriteLine("          public delegate bool Run();");
            sw.WriteLine("          public Run run;");
            sw.WriteLine("      }");
            sw.WriteLine("");

            sw.WriteLine("      //List of all the nodes making up this tree");
            sw.WriteLine("      private List<TreeNode> m_nodes = new List<TreeNode>();");
            sw.WriteLine("      ");
            sw.WriteLine("      //the node at the begining of the tree");
            sw.WriteLine("      private TreeNode m_startNode = null;");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * Function: Constructor");
            sw.WriteLine("      * ---------------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * Default constructor");
            sw.WriteLine("      *");
            sw.WriteLine("      */");
            sw.WriteLine("      public BehaviourTree()");
            sw.WriteLine("      {");
            sw.WriteLine("          SetUpNodes();");
            sw.WriteLine("      }");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * Function: SetUpNodes");
            sw.WriteLine("      * ---------------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * this function sets up all the nodes in the tree");
            sw.WriteLine("      * assiging them there functions and linking them up");
            sw.WriteLine("      *");
            sw.WriteLine("      * Parameters: none");
            sw.WriteLine("      *");
            sw.WriteLine("      *returns: returns nothing as it is a void function");
            sw.WriteLine("      */");
            sw.WriteLine("      public void SetUpNodes()");
            sw.WriteLine("      {");
            CSConstructNodes(sw);
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("      ");
            CSConstructChildConnections(sw);
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("      ");
            CSConstructRunFunction(sw);
            sw.WriteLine("      }");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * Function: Update");
            sw.WriteLine("      * ----------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * this function runs the behaviour tree each time it is called");
            sw.WriteLine("      *");
            sw.WriteLine("      * Parameters: none");
            sw.WriteLine("      *");
            sw.WriteLine("      *returns: returns nothing as it is a void function");
            sw.WriteLine("      */");
            sw.WriteLine("      public void Update()");
            sw.WriteLine("      {");
            sw.WriteLine("          ");
            sw.WriteLine("          m_startNode.run();");
            sw.WriteLine("          ");
            sw.WriteLine("      }");
            sw.WriteLine("}");
        }

        /*
         * 
        * Function: CSConstructNodes
        * --------------------------
        *
        * this writes out a all the tree nodes constructing them selves
        * 
        * Parameters: StreamWriter sw, the file we need to write in
        *
        * returns: returns nothing as it is a void function
        */
        private void CSConstructNodes(StreamWriter sw)
        {
            int start = 0;
            foreach (Node n in m_nodes)
            {
                n.NodeConstructorCS(sw);

                if (start == 0)
                {
                    sw.WriteLine("          m_startNode = n" + n.m_nodeNum.ToString() + ";");
                    sw.WriteLine("          ");
                    start++;
                }
            }
        }

        /*
         * 
        * Function: CSConstructRunFunction
        * --------------------------------
        *
        * this writes out the default functions for eachNode
        * 
        * Parameters: StreamWriter sw, the file we need to write in
        *
        * returns: returns nothing as it is a void function
        */
        private void CSConstructRunFunction(StreamWriter sw)
        {
            foreach (Node n in m_nodes)
            {
                n.ConstructRunFunctionCS(sw);
            }
        }

        /*
         * 
        * Function: CSConstructChildConnections
        * -------------------------------------
        *
        * this writes out the child connections for each node
        * 
        * Parameters: StreamWriter sw, the file we need to write in
        *
        * returns: returns nothing as it is a void function
        */
        private void CSConstructChildConnections(StreamWriter sw)
        {
            foreach (Node n in m_nodes)
            {
                n.ConstructChildConnectionsCS(sw);
            }
        }

        public void CreatHeaderFile(StreamWriter sw)
        {
            sw.WriteLine("#pragma once");
            sw.WriteLine("#include <list>");
            sw.WriteLine("#include <functional>");
            sw.WriteLine("");

            sw.WriteLine("/*");
            sw.WriteLine("* class: BehaviourTree");
            sw.WriteLine("* --------------------");
            sw.WriteLine("*");
            sw.WriteLine("* ____________________");
            sw.WriteLine("*");
            sw.WriteLine("* Author: ____________");
            sw.WriteLine("*/");
            sw.WriteLine("class BehaviourTree");
            sw.WriteLine("{");
            sw.WriteLine("public:");

            sw.WriteLine("      /*");
            sw.WriteLine("      * class: TreeNode");
            sw.WriteLine("      * ---------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * _______________");
            sw.WriteLine("      *");
            sw.WriteLine("      * Author: _______");
            sw.WriteLine("      */");
            sw.WriteLine("      class TreeNode");
            sw.WriteLine("      {");
            sw.WriteLine("      public:");
            sw.WriteLine("          //the list of the nodes children");
            sw.WriteLine("          std::list<TreeNode *> m_children;");
            sw.WriteLine("          ");
            sw.WriteLine("          //the delegate function we will assign to exacute");
            sw.WriteLine("          std::function<bool()> run;");
            sw.WriteLine("      };");
            sw.WriteLine("");

            sw.WriteLine("      //List of all the nodes making up this tree");
            sw.WriteLine("      std::list<TreeNode *> m_nodes;");
            sw.WriteLine("      ");
            sw.WriteLine("      //the node at the begining of the tree");
            sw.WriteLine("      TreeNode * m_startNode = nullptr;");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * Function: Constructor");
            sw.WriteLine("      * ---------------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * Default constructor");
            sw.WriteLine("      *");
            sw.WriteLine("      */");
            sw.WriteLine("      BehaviourTree()");
            sw.WriteLine("      {");
            sw.WriteLine("          SetUpNodes();");
            sw.WriteLine("      };");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * Function: DeConstructor");
            sw.WriteLine("      * ---------------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * Default DeConstructor");
            sw.WriteLine("      *");
            sw.WriteLine("      */");
            sw.WriteLine("      ~BehaviourTree()");
            sw.WriteLine("      {");
            sw.WriteLine("          m_nodes.clear();");
            sw.WriteLine("      };");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * Function: SetUpNodes");
            sw.WriteLine("      * ---------------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * this function sets up all the nodes in the tree");
            sw.WriteLine("      * assiging them there functions and linking them up");
            sw.WriteLine("      *");
            sw.WriteLine("      * Parameters: none");
            sw.WriteLine("      *");
            sw.WriteLine("      *returns: returns nothing as it is a void function");
            sw.WriteLine("      */");
            sw.WriteLine("      void SetUpNodes()");
            sw.WriteLine("      {");
            HeaderConstructNodes(sw);
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("      ");
            HeaderConstructRunFunctions(sw);
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("          //////////////////////////////////////////////////////");
            sw.WriteLine("      ");
            HeaderConstructChildConnections(sw);
            sw.WriteLine("      };");
            sw.WriteLine("");

            sw.WriteLine("      /*");
            sw.WriteLine("      * Function: Update");
            sw.WriteLine("      * ----------------");
            sw.WriteLine("      *");
            sw.WriteLine("      * this function runs the behaviour tree each time it is called");
            sw.WriteLine("      *");
            sw.WriteLine("      * Parameters: none");
            sw.WriteLine("      *");
            sw.WriteLine("      *returns: returns nothing as it is a void function");
            sw.WriteLine("      */");
            sw.WriteLine("      void Update()");
            sw.WriteLine("      {");
            sw.WriteLine("          ");
            sw.WriteLine("          m_startNode->run();");
            sw.WriteLine("          ");
            sw.WriteLine("      };");
            sw.WriteLine("};");
        }

        /*
        * 
        * Function: HeaderConstructNodes
        * ------------------------------
        *
        * this writes out a all the tree nodes constructing them selves
        * 
        * Parameters: StreamWriter sw, the file we need to write in
        *
        * returns: returns nothing as it is a void function
        */
        private void HeaderConstructNodes(StreamWriter sw)
        {
            int start = 0;
            foreach (Node n in m_nodes)
            {
                n.ConstructHeaderNodes(sw);

                if (start == 0)
                {
                    sw.WriteLine("          m_startNode = n" + n.m_nodeNum.ToString() + ";");
                    sw.WriteLine("          ");
                    start++;
                }
            }
        }

        /*
        * 
        * Function: CSConstructRunFunction
        * --------------------------------
        *
        * this writes out the default functions for eachNode
        * 
        * Parameters: StreamWriter sw, the file we need to write in
        *
        * returns: returns nothing as it is a void function
        */
        private void HeaderConstructRunFunctions(StreamWriter sw)
        {
            foreach (Node n in m_nodes)
            {
                n.ConstructHeaderRunFunctions(sw);
            }
        }

        /*
        * 
        * Function: CSConstructChildConnections
        * -------------------------------------
        *
        * this writes out the child connections for each node
        * 
        * Parameters: StreamWriter sw, the file we need to write in
        *
        * returns: returns nothing as it is a void function
        */
        private void HeaderConstructChildConnections(StreamWriter sw)
        {
            foreach (Node n in m_nodes)
            {
                n.ConstructHeaderChildren(sw);
            }
        }
    }

    
}
