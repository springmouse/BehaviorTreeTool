using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Behaviour_tree_tool
{
    public partial class Form2 : Form
    {
        private List<Node> m_nodes = new List<Node>();

        private Node m_slectedNodeType = null;

        private Mouse_Controler m_mouseControler = new Mouse_Controler();

        public Form2()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush redPen = new SolidBrush(Color.Red);

            Rectangle drawRect = new Rectangle(40, 40, 100, 200);

            e.Graphics.FillRectangle(redPen, drawRect);

            redPen.Dispose();
        }

        private void sequenceNodeToolStrip_Click(object sender, EventArgs e)
        {
            m_slectedNodeType = new SequenceComposite();
        }
    }
}
