namespace Behaviour_tree_tool
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripCompositeNodes = new System.Windows.Forms.ToolStripDropDownButton();
            this.sequenceNodeToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.slectorNodeToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.randomSlectorNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchSlectorNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownQuestionsAndActions = new System.Windows.Forms.ToolStripDropDownButton();
            this.actionNodeToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionNodeToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.decoratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCompositeNodes,
            this.toolStripDropDownQuestionsAndActions});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(32, 262);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripCompositeNodes
            // 
            this.toolStripCompositeNodes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripCompositeNodes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sequenceNodeToolStrip,
            this.slectorNodeToolStrip,
            this.randomSlectorNodeToolStripMenuItem,
            this.switchSlectorNodeToolStripMenuItem,
            this.decoratorToolStripMenuItem});
            this.toolStripCompositeNodes.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCompositeNodes.Image")));
            this.toolStripCompositeNodes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCompositeNodes.Name = "toolStripCompositeNodes";
            this.toolStripCompositeNodes.Size = new System.Drawing.Size(29, 20);
            this.toolStripCompositeNodes.Text = "Composite Nodes";
            // 
            // sequenceNodeToolStrip
            // 
            this.sequenceNodeToolStrip.Name = "sequenceNodeToolStrip";
            this.sequenceNodeToolStrip.Size = new System.Drawing.Size(190, 22);
            this.sequenceNodeToolStrip.Text = "Sequence Node";
            this.sequenceNodeToolStrip.Click += new System.EventHandler(this.sequenceNodeToolStrip_Click);
            // 
            // slectorNodeToolStrip
            // 
            this.slectorNodeToolStrip.Name = "slectorNodeToolStrip";
            this.slectorNodeToolStrip.Size = new System.Drawing.Size(190, 22);
            this.slectorNodeToolStrip.Text = "Slector Node";
            // 
            // randomSlectorNodeToolStripMenuItem
            // 
            this.randomSlectorNodeToolStripMenuItem.Name = "randomSlectorNodeToolStripMenuItem";
            this.randomSlectorNodeToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.randomSlectorNodeToolStripMenuItem.Text = "Random Slector Node";
            // 
            // switchSlectorNodeToolStripMenuItem
            // 
            this.switchSlectorNodeToolStripMenuItem.Name = "switchSlectorNodeToolStripMenuItem";
            this.switchSlectorNodeToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.switchSlectorNodeToolStripMenuItem.Text = "Switch Slector Node";
            // 
            // toolStripDropDownQuestionsAndActions
            // 
            this.toolStripDropDownQuestionsAndActions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownQuestionsAndActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionNodeToolStrip,
            this.conditionNodeToolStrip});
            this.toolStripDropDownQuestionsAndActions.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownQuestionsAndActions.Image")));
            this.toolStripDropDownQuestionsAndActions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownQuestionsAndActions.Name = "toolStripDropDownQuestionsAndActions";
            this.toolStripDropDownQuestionsAndActions.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownQuestionsAndActions.Text = "Questions And Actions";
            // 
            // actionNodeToolStrip
            // 
            this.actionNodeToolStrip.Name = "actionNodeToolStrip";
            this.actionNodeToolStrip.Size = new System.Drawing.Size(159, 22);
            this.actionNodeToolStrip.Text = "Action Node";
            // 
            // conditionNodeToolStrip
            // 
            this.conditionNodeToolStrip.Name = "conditionNodeToolStrip";
            this.conditionNodeToolStrip.Size = new System.Drawing.Size(159, 22);
            this.conditionNodeToolStrip.Text = "Condition Node";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // decoratorToolStripMenuItem
            // 
            this.decoratorToolStripMenuItem.Name = "decoratorToolStripMenuItem";
            this.decoratorToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.decoratorToolStripMenuItem.Text = "Decorator";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripCompositeNodes;
        private System.Windows.Forms.ToolStripMenuItem sequenceNodeToolStrip;
        private System.Windows.Forms.ToolStripMenuItem slectorNodeToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownQuestionsAndActions;
        private System.Windows.Forms.ToolStripMenuItem actionNodeToolStrip;
        private System.Windows.Forms.ToolStripMenuItem conditionNodeToolStrip;
        private System.Windows.Forms.ToolStripMenuItem randomSlectorNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchSlectorNodeToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem decoratorToolStripMenuItem;
    }
}