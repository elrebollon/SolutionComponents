namespace SolutionComponents
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSample = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbComponent = new System.Windows.Forms.RadioButton();
            this.rbSolution = new System.Windows.Forms.RadioButton();
            this.pnSolution = new System.Windows.Forms.Panel();
            this.dgvRelatedSolutions = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInstalledOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManaged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSolutionComponents = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSolutions = new System.Windows.Forms.ComboBox();
            this.toolStripMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnSolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatedSolutions)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.tsbSample});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(943, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(86, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSample
            // 
            this.tsbSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSample.Name = "tsbSample";
            this.tsbSample.Size = new System.Drawing.Size(46, 22);
            this.tsbSample.Text = "Try me";
            this.tsbSample.Click += new System.EventHandler(this.tsbSample_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbComponent);
            this.groupBox1.Controls.Add(this.rbSolution);
            this.groupBox1.Location = new System.Drawing.Point(3, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 81);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // rbComponent
            // 
            this.rbComponent.AutoSize = true;
            this.rbComponent.Location = new System.Drawing.Point(6, 42);
            this.rbComponent.Name = "rbComponent";
            this.rbComponent.Size = new System.Drawing.Size(79, 17);
            this.rbComponent.TabIndex = 1;
            this.rbComponent.TabStop = true;
            this.rbComponent.Text = "Component";
            this.rbComponent.UseVisualStyleBackColor = true;
            // 
            // rbSolution
            // 
            this.rbSolution.AutoSize = true;
            this.rbSolution.Checked = true;
            this.rbSolution.Location = new System.Drawing.Point(6, 19);
            this.rbSolution.Name = "rbSolution";
            this.rbSolution.Size = new System.Drawing.Size(63, 17);
            this.rbSolution.TabIndex = 0;
            this.rbSolution.TabStop = true;
            this.rbSolution.Text = "Solution";
            this.rbSolution.UseVisualStyleBackColor = true;
            // 
            // pnSolution
            // 
            this.pnSolution.Controls.Add(this.dgvRelatedSolutions);
            this.pnSolution.Controls.Add(this.label2);
            this.pnSolution.Controls.Add(this.cbSolutionComponents);
            this.pnSolution.Controls.Add(this.label1);
            this.pnSolution.Controls.Add(this.cbSolutions);
            this.pnSolution.Location = new System.Drawing.Point(7, 115);
            this.pnSolution.Name = "pnSolution";
            this.pnSolution.Size = new System.Drawing.Size(933, 550);
            this.pnSolution.TabIndex = 6;
            // 
            // dgvRelatedSolutions
            // 
            this.dgvRelatedSolutions.AllowUserToAddRows = false;
            this.dgvRelatedSolutions.AllowUserToDeleteRows = false;
            this.dgvRelatedSolutions.AllowUserToOrderColumns = true;
            this.dgvRelatedSolutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelatedSolutions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colCreatedOn,
            this.colInstalledOn,
            this.colManaged});
            this.dgvRelatedSolutions.Location = new System.Drawing.Point(6, 57);
            this.dgvRelatedSolutions.Name = "dgvRelatedSolutions";
            this.dgvRelatedSolutions.ReadOnly = true;
            this.dgvRelatedSolutions.Size = new System.Drawing.Size(924, 490);
            this.dgvRelatedSolutions.TabIndex = 4;
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colCreatedOn
            // 
            this.colCreatedOn.HeaderText = "Created on";
            this.colCreatedOn.Name = "colCreatedOn";
            this.colCreatedOn.ReadOnly = true;
            // 
            // colInstalledOn
            // 
            this.colInstalledOn.HeaderText = "Installed on";
            this.colInstalledOn.Name = "colInstalledOn";
            this.colInstalledOn.ReadOnly = true;
            // 
            // colManaged
            // 
            this.colManaged.HeaderText = "Managed";
            this.colManaged.Name = "colManaged";
            this.colManaged.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Solution component";
            // 
            // cbSolutionComponents
            // 
            this.cbSolutionComponents.FormattingEnabled = true;
            this.cbSolutionComponents.Location = new System.Drawing.Point(114, 30);
            this.cbSolutionComponents.Name = "cbSolutionComponents";
            this.cbSolutionComponents.Size = new System.Drawing.Size(224, 21);
            this.cbSolutionComponents.TabIndex = 2;
            this.cbSolutionComponents.SelectedIndexChanged += new System.EventHandler(this.cbSolutionComponents_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Solution";
            // 
            // cbSolutions
            // 
            this.cbSolutions.FormattingEnabled = true;
            this.cbSolutions.Location = new System.Drawing.Point(114, 3);
            this.cbSolutions.Name = "cbSolutions";
            this.cbSolutions.Size = new System.Drawing.Size(224, 21);
            this.cbSolutions.TabIndex = 0;
            this.cbSolutions.SelectedIndexChanged += new System.EventHandler(this.cbSolutions_SelectedIndexChanged);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnSolution);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(943, 668);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnSolution.ResumeLayout(false);
            this.pnSolution.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatedSolutions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbSample;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbComponent;
        private System.Windows.Forms.RadioButton rbSolution;
        private System.Windows.Forms.Panel pnSolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSolutions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSolutionComponents;
        private System.Windows.Forms.DataGridView dgvRelatedSolutions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstalledOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManaged;
    }
}
