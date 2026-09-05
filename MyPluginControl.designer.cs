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
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
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
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbComponent = new System.Windows.Forms.RadioButton();
            this.rbSolution = new System.Windows.Forms.RadioButton();
            this.pnSolution = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dgvRelatedSolutions = new System.Windows.Forms.DataGridView();
            this.colComponentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComponentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelatedSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPublisher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManaged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInstalledOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.tsbRefresh});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(960, 25);
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
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(102, 22);
            this.tsbRefresh.Text = "Refresh Solutions";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbComponent);
            this.groupBox1.Controls.Add(this.rbSolution);
            this.groupBox1.Location = new System.Drawing.Point(10, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 70);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analysis Mode";
            // 
            // rbComponent
            // 
            this.rbComponent.AutoSize = true;
            this.rbComponent.Location = new System.Drawing.Point(120, 30);
            this.rbComponent.Name = "rbComponent";
            this.rbComponent.Size = new System.Drawing.Size(96, 17);
            this.rbComponent.TabIndex = 1;
            this.rbComponent.TabStop = true;
            this.rbComponent.Text = "By Component";
            this.rbComponent.UseVisualStyleBackColor = true;
            this.rbComponent.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // rbSolution
            // 
            this.rbSolution.AutoSize = true;
            this.rbSolution.Checked = true;
            this.rbSolution.Location = new System.Drawing.Point(16, 30);
            this.rbSolution.Name = "rbSolution";
            this.rbSolution.Size = new System.Drawing.Size(78, 17);
            this.rbSolution.TabIndex = 0;
            this.rbSolution.TabStop = true;
            this.rbSolution.Text = "By Solution";
            this.rbSolution.UseVisualStyleBackColor = true;
            this.rbSolution.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // pnSolution
            // 
            this.pnSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnSolution.Controls.Add(this.lblStatus);
            this.pnSolution.Controls.Add(this.dgvRelatedSolutions);
            this.pnSolution.Controls.Add(this.label2);
            this.pnSolution.Controls.Add(this.cbSolutionComponents);
            this.pnSolution.Controls.Add(this.label1);
            this.pnSolution.Controls.Add(this.cbSolutions);
            this.pnSolution.Location = new System.Drawing.Point(10, 110);
            this.pnSolution.Name = "pnSolution";
            this.pnSolution.Size = new System.Drawing.Size(940, 545);
            this.pnSolution.TabIndex = 6;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(400, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 5;
            // 
            // dgvRelatedSolutions
            // 
            this.dgvRelatedSolutions.AllowUserToAddRows = false;
            this.dgvRelatedSolutions.AllowUserToDeleteRows = false;
            this.dgvRelatedSolutions.AllowUserToOrderColumns = true;
            this.dgvRelatedSolutions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRelatedSolutions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRelatedSolutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelatedSolutions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colComponentName,
            this.colComponentType,
            this.colRelatedSolution,
            this.colVersion,
            this.colPublisher,
            this.colManaged,
            this.colCreatedOn,
            this.colInstalledOn});
            this.dgvRelatedSolutions.Location = new System.Drawing.Point(5, 75);
            this.dgvRelatedSolutions.Name = "dgvRelatedSolutions";
            this.dgvRelatedSolutions.ReadOnly = true;
            this.dgvRelatedSolutions.RowHeadersVisible = false;
            this.dgvRelatedSolutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRelatedSolutions.Size = new System.Drawing.Size(930, 465);
            this.dgvRelatedSolutions.TabIndex = 4;
            // 
            // colComponentName
            // 
            this.colComponentName.HeaderText = "Component Name";
            this.colComponentName.Name = "colComponentName";
            this.colComponentName.ReadOnly = true;
            // 
            // colComponentType
            // 
            this.colComponentType.HeaderText = "Component Type";
            this.colComponentType.Name = "colComponentType";
            this.colComponentType.ReadOnly = true;
            // 
            // colRelatedSolution
            // 
            this.colRelatedSolution.HeaderText = "Related Solution";
            this.colRelatedSolution.Name = "colRelatedSolution";
            this.colRelatedSolution.ReadOnly = true;
            // 
            // colVersion
            // 
            this.colVersion.HeaderText = "Version";
            this.colVersion.Name = "colVersion";
            this.colVersion.ReadOnly = true;
            // 
            // colPublisher
            // 
            this.colPublisher.HeaderText = "Publisher";
            this.colPublisher.Name = "colPublisher";
            this.colPublisher.ReadOnly = true;
            // 
            // colManaged
            // 
            this.colManaged.HeaderText = "Managed";
            this.colManaged.Name = "colManaged";
            this.colManaged.ReadOnly = true;
            // 
            // colCreatedOn
            // 
            this.colCreatedOn.HeaderText = "Created On";
            this.colCreatedOn.Name = "colCreatedOn";
            this.colCreatedOn.ReadOnly = true;
            // 
            // colInstalledOn
            // 
            this.colInstalledOn.HeaderText = "Installed On";
            this.colInstalledOn.Name = "colInstalledOn";
            this.colInstalledOn.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Filter Component:";
            // 
            // cbSolutionComponents
            // 
            this.cbSolutionComponents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSolutionComponents.FormattingEnabled = true;
            this.cbSolutionComponents.Location = new System.Drawing.Point(125, 42);
            this.cbSolutionComponents.Name = "cbSolutionComponents";
            this.cbSolutionComponents.Size = new System.Drawing.Size(260, 21);
            this.cbSolutionComponents.TabIndex = 2;
            this.cbSolutionComponents.SelectedIndexChanged += new System.EventHandler(this.cbSolutionComponents_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Solution:";
            // 
            // cbSolutions
            // 
            this.cbSolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSolutions.FormattingEnabled = true;
            this.cbSolutions.Location = new System.Drawing.Point(125, 12);
            this.cbSolutions.Name = "cbSolutions";
            this.cbSolutions.Size = new System.Drawing.Size(260, 21);
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
            this.Size = new System.Drawing.Size(960, 670);
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
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbComponent;
        private System.Windows.Forms.RadioButton rbSolution;
        private System.Windows.Forms.Panel pnSolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSolutions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSolutionComponents;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView dgvRelatedSolutions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComponentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComponentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelatedSolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPublisher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManaged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstalledOn;
    }
}
