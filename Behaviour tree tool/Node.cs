using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Behaviour_tree_tool
{
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
        [XmlIgnore]
        public List<Node> m_parent = new List<Node>();

        [XmlArray("List_Of_Node_Parents_IDs"), XmlArrayItem(typeof(int), ElementName = "Parent_Node_ID")]
        public List<int> m_parentIDs = new List<int>();

        [XmlIgnore]
        public List<Node> m_children = new List<Node>();

        [XmlArray("List_Of_Node_childrens_IDs"), XmlArrayItem(typeof(int), ElementName = "Child_Node_ID")]
        public List<int> m_childIDs = new List<int>();

        [XmlElement("Node_Num")]
        public int m_node = 0;

        [XmlElement("Node_Description")]
        public string m_description = "Default Description";

        [XmlElement("Node_Type")]
        public string m_nodeType = "Null";
        
        [XmlElement("Node_Rect")]
        public Rectangle m_rect;

        protected float m_connectorRadius = 30;

        protected int m_ID = 0;

        [XmlElement("Node_ID")]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

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

        public Node()
        {
            m_rect.X = 0;
            m_rect.Y = 0;
            m_rect.Width = 100;
            m_rect.Height = 100;
        }

        public void SetID()
        {
            Random r = new Random();
            
            m_ID = r.Next(0, 10) + r.Next(10, 100) +r.Next(100,1000) + r.Next(1000, 10000) + r.Next(10000, 100000);
        }

        public virtual void SetParent(Node n)
        {
            foreach (Node c in m_children)
            {
                if (c == n)
                {
                    return;
                }
            }
            foreach (Node p in m_parent)
            {
                if (p == n)
                {
                    return;
                }
            }

            if (this == n)
            {
                return;
            }

            m_parent.Add(n);
            m_parentIDs.Add(n.ID);
        }

        public virtual void SetChild(Node n)
        {
            foreach (Node c in m_children)
            {
                if (c == n)
                {
                    return;
                }
            }
            foreach (Node p in m_parent)
            {
                if (p == n)
                {
                    return;
                }
            }

            if (this == n)
            {
                return;
            }

            m_children.Add(n);
            m_childIDs.Add(n.ID);
        }

        public bool CheckIfClickedIn(int x, int y)
        {

            if (m_rect.X < x && (m_rect.X + m_rect.Width) > x &&
                m_rect.Y < y && (m_rect.Y + m_rect.Height) > y)
            {
                return true;
            }

            return false;
        }

        public virtual bool ChecIfClickedOnParentConnector(int x, int y)
        {
            if (SqrMagnatude((m_rect.X + (int)(m_rect.Width * 0.36f)) - x, (m_rect.Y + (int)(m_rect.Height * 0.02f)) - y) < (m_connectorRadius * m_connectorRadius))
            {
                return true;
            }

            return false;
        }

        public virtual bool CheckIfClickedOnCildConector(int x, int y)
        {
            if (SqrMagnatude((m_rect.X + (int)(m_rect.Width * 0.36f)) - x, (m_rect.Y + (int)(m_rect.Height * 0.7)) - y) < (m_connectorRadius * m_connectorRadius))
            {
                return true;
            }

            return false;
        }

        public virtual void OnDraw(PaintEventArgs e) { }

        public void OnDrawConnections(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            foreach (Node c in m_children)
            {
                g.DrawLine(pen,
                    m_rect.X + (int)(m_rect.Width * 0.5f),
                    m_rect.Y + (int)(m_rect.Height * 0.5f),
                    c.m_rect.X + (int)(c.m_rect.Width * 0.5f),
                    c.m_rect.Y + (int)(c.m_rect.Height * 0.5f));
            }
        }
        
        public float SqrMagnatude(int x, int y)
        {
            return ((x * x) + (y*y));
        }

        public virtual void WriteToCPP() { }
        public virtual void WriteToCS() { }
        public virtual void WriteToJava() { }
        public virtual void WriteToPython() { }
    }

    [Serializable]
    public class ActionNode : Node
    {
        public ActionNode() { m_nodeType = "Action Node"; }
        
        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            Brush brush = new SolidBrush(Color.Yellow);
            g.FillPie(brush, m_rect, 0.0f, 360.0f);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.02f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        public override bool CheckIfClickedOnCildConector(int x, int y)
        {
            return false;
        }

        public override void SetChild(Node n)
        {
            return;
        }
    }

    [Serializable]
    public class ConditionNode : Node
    {
        public ConditionNode() { m_nodeType = "Condition Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Green);
            g.FillPie(brush, m_rect, 0.0f, 360.0f);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.02f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        public override bool CheckIfClickedOnCildConector(int x, int y)
        {
            return false;
        }

        public override void SetChild(Node n)
        {
            return;
        }
    }

    [Serializable]
    public class SequenceComposite : Node
    {
        public SequenceComposite() { m_nodeType = "Sequence Composite Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Blue);
            g.FillRectangle(brush, m_rect);
            
            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.7f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.02f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }
               
               
    }

    [Serializable]
    public class SlectorComposite : Node
    {
        private PointF[] m_pointF = new PointF[4];

        public SlectorComposite() { m_nodeType = "Slector Composite Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Brush brush = new SolidBrush(Color.Blue);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.7f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.02f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width);
            m_pointF[2].Y = m_rect.Y + (m_rect.Height * 0.5f);


            m_pointF[3].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height);
        }
    }

    [Serializable]
    public class DecoratorComposite : Node
    {
        private PointF[] m_pointF = new PointF[6];

        public DecoratorComposite() { m_nodeType = "Decorator Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            Brush brush = new SolidBrush(Color.RosyBrown);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.7f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.02f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.2f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width * 0.8f);
            m_pointF[2].Y = m_rect.Y;
            
            m_pointF[3].X = m_rect.X + (m_rect.Width);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[4].X = m_rect.X + (m_rect.Width * 0.8f);
            m_pointF[4].Y = m_rect.Y + (m_rect.Height);

            m_pointF[5].X = m_rect.X + (m_rect.Width * 0.2f);
            m_pointF[5].Y = m_rect.Y + (m_rect.Height);
        }
    }

    [Serializable]
    public class RandomSlector : Node
    {
        private PointF[] m_pointF = new PointF[4];

        public RandomSlector() { m_nodeType = "Random Slector Node"; }

        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            Brush brush = new SolidBrush(Color.LightBlue);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.7f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.36f)), (m_rect.Y + (int)(m_rect.Height * 0.02f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width);
            m_pointF[2].Y = m_rect.Y + (m_rect.Height * 0.5f);


            m_pointF[3].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height);
        }
    }

    [Serializable]
    public class SwitchSlector : Node
    {
        private PointF[] m_pointF = new PointF[4];

        public SwitchSlector() { m_nodeType = "Switch Slector Node"; }
        
        public override void OnDraw(PaintEventArgs e)
        {
            SetPoints();

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black);

            Brush brush = new SolidBrush(Color.Cyan);
            g.FillPolygon(brush, m_pointF);

            Brush connector = new SolidBrush(Color.LightCoral);
            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.37f)), (m_rect.Y + (int)(m_rect.Height * 0.7f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);

            g.FillPie(connector, (m_rect.X + (int)(m_rect.Width * 0.37f)), (m_rect.Y + (int)(m_rect.Height * 0.02f)), m_connectorRadius, m_connectorRadius, 0.0f, 360f);
        }

        public void SetPoints()
        {
            m_pointF[0].X = m_rect.X;
            m_pointF[0].Y = m_rect.Y + (m_rect.Height * 0.5f);

            m_pointF[1].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[1].Y = m_rect.Y;

            m_pointF[2].X = m_rect.X + (m_rect.Width);
            m_pointF[2].Y = m_rect.Y + (m_rect.Height * 0.5f);


            m_pointF[3].X = m_rect.X + (m_rect.Width * 0.5f);
            m_pointF[3].Y = m_rect.Y + (m_rect.Height);
        }
    }
}
