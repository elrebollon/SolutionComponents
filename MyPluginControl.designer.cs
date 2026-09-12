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
            this.colModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModifiedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPublisher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManaged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSolutionComponents = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSolutions = new System.Windows.Forms.ComboBox();
            this.pnComponent = new System.Windows.Forms.Panel();
            this.lblComponentStatus = new System.Windows.Forms.Label();
            this.dgvComponentHistory = new System.Windows.Forms.DataGridView();
            this.colHistSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistCreatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistModifiedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistPublisher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistManaged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbComponents = new System.Windows.Forms.ComboBox();
            this.lblComponent = new System.Windows.Forms.Label();
            this.cbEntities = new System.Windows.Forms.ComboBox();
            this.lblEntity = new System.Windows.Forms.Label();
            this.cbComponentTypes = new System.Windows.Forms.ComboBox();
            this.lblComponentType = new System.Windows.Forms.Label();
            this.toolStripMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnSolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatedSolutions)).BeginInit();
            this.pnComponent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentHistory)).BeginInit();
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
            this.tsbRefresh.Size = new System.Drawing.Size(86, 22);
            this.tsbRefresh.Text = "Refresh Data";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbComponent);
            this.groupBox1.Controls.Add(this.rbSolution);
            this.groupBox1.Location = new System.Drawing.Point(10, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 65);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analysis Mode";
            // 
            // rbComponent
            // 
            this.rbComponent.AutoSize = true;
            this.rbComponent.Location = new System.Drawing.Point(120, 28);
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
            this.rbSolution.Location = new System.Drawing.Point(16, 28);
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
            this.pnSolution.Location = new System.Drawing.Point(10, 105);
            this.pnSolution.Name = "pnSolution";
            this.pnSolution.Size = new System.Drawing.Size(940, 555);
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
            this.dgvRelatedSolutions.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvRelatedSolutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelatedSolutions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colComponentName,
            this.colComponentType,
            this.colRelatedSolution,
            this.colVersion,
            this.colModifiedBy,
            this.colCreatedOn,
            this.colModifiedOn,
            this.colPublisher,
            this.colManaged});
            this.dgvRelatedSolutions.Location = new System.Drawing.Point(5, 75);
            this.dgvRelatedSolutions.MultiSelect = true;
            this.dgvRelatedSolutions.Name = "dgvRelatedSolutions";
            this.dgvRelatedSolutions.ReadOnly = true;
            this.dgvRelatedSolutions.RowHeadersVisible = false;
            this.dgvRelatedSolutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvRelatedSolutions.Size = new System.Drawing.Size(930, 475);
            this.dgvRelatedSolutions.TabIndex = 4;
            // 
            // colComponentName
            // 
            this.colComponentName.FillWeight = 180F;
            this.colComponentName.MinimumWidth = 140;
            this.colComponentName.HeaderText = "Component Name";
            this.colComponentName.Name = "colComponentName";
            this.colComponentName.ReadOnly = true;
            // 
            // colComponentType
            // 
            this.colComponentType.FillWeight = 120F;
            this.colComponentType.MinimumWidth = 100;
            this.colComponentType.HeaderText = "Component Type";
            this.colComponentType.Name = "colComponentType";
            this.colComponentType.ReadOnly = true;
            // 
            // colRelatedSolution
            // 
            this.colRelatedSolution.FillWeight = 160F;
            this.colRelatedSolution.MinimumWidth = 130;
            this.colRelatedSolution.HeaderText = "Related Solution";
            this.colRelatedSolution.Name = "colRelatedSolution";
            this.colRelatedSolution.ReadOnly = true;
            // 
            // colVersion
            // 
            this.colVersion.FillWeight = 70F;
            this.colVersion.MinimumWidth = 65;
            this.colVersion.HeaderText = "Version";
            this.colVersion.Name = "colVersion";
            this.colVersion.ReadOnly = true;
            // 
            // colModifiedBy
            // 
            this.colModifiedBy.FillWeight = 130F;
            this.colModifiedBy.MinimumWidth = 100;
            this.colModifiedBy.HeaderText = "Modified By";
            this.colModifiedBy.Name = "colModifiedBy";
            this.colModifiedBy.ReadOnly = true;
            // 
            // colCreatedOn
            // 
            this.colCreatedOn.FillWeight = 90F;
            this.colCreatedOn.MinimumWidth = 85;
            this.colCreatedOn.HeaderText = "Created On";
            this.colCreatedOn.Name = "colCreatedOn";
            this.colCreatedOn.ReadOnly = true;
            // 
            // colModifiedOn
            // 
            this.colModifiedOn.FillWeight = 90F;
            this.colModifiedOn.MinimumWidth = 85;
            this.colModifiedOn.HeaderText = "Modified On";
            this.colModifiedOn.Name = "colModifiedOn";
            this.colModifiedOn.ReadOnly = true;
            // 
            // colPublisher
            // 
            this.colPublisher.FillWeight = 110F;
            this.colPublisher.MinimumWidth = 90;
            this.colPublisher.HeaderText = "Publisher";
            this.colPublisher.Name = "colPublisher";
            this.colPublisher.ReadOnly = true;
            // 
            // colManaged
            // 
            this.colManaged.FillWeight = 50F;
            this.colManaged.MinimumWidth = 50;
            this.colManaged.HeaderText = "Managed";
            this.colManaged.Name = "colManaged";
            this.colManaged.ReadOnly = true;
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
            this.cbSolutionComponents.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSolutionComponents.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSolutionComponents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbSolutionComponents.FormattingEnabled = true;
            this.cbSolutionComponents.Location = new System.Drawing.Point(125, 42);
            this.cbSolutionComponents.Name = "cbSolutionComponents";
            this.cbSolutionComponents.Size = new System.Drawing.Size(260, 21);
            this.cbSolutionComponents.TabIndex = 2;
            this.cbSolutionComponents.SelectedIndexChanged += new System.EventHandler(this.cbSolutionComponents_SelectedIndexChanged);
            this.cbSolutionComponents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbSolutionComponents_KeyDown);
            this.cbSolutionComponents.Leave += new System.EventHandler(this.cbSolutionComponents_Leave);
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
            this.cbSolutions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSolutions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbSolutions.FormattingEnabled = true;
            this.cbSolutions.Location = new System.Drawing.Point(125, 12);
            this.cbSolutions.Name = "cbSolutions";
            this.cbSolutions.Size = new System.Drawing.Size(260, 21);
            this.cbSolutions.TabIndex = 0;
            this.cbSolutions.SelectedIndexChanged += new System.EventHandler(this.cbSolutions_SelectedIndexChanged);
            this.cbSolutions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbSolutions_KeyDown);
            this.cbSolutions.Leave += new System.EventHandler(this.cbSolutions_Leave);
            // 
            // pnComponent
            // 
            this.pnComponent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnComponent.Controls.Add(this.lblComponentStatus);
            this.pnComponent.Controls.Add(this.dgvComponentHistory);
            this.pnComponent.Controls.Add(this.cbComponents);
            this.pnComponent.Controls.Add(this.lblComponent);
            this.pnComponent.Controls.Add(this.cbEntities);
            this.pnComponent.Controls.Add(this.lblEntity);
            this.pnComponent.Controls.Add(this.cbComponentTypes);
            this.pnComponent.Controls.Add(this.lblComponentType);
            this.pnComponent.Location = new System.Drawing.Point(10, 105);
            this.pnComponent.Name = "pnComponent";
            this.pnComponent.Size = new System.Drawing.Size(940, 555);
            this.pnComponent.TabIndex = 7;
            this.pnComponent.Visible = false;
            // 
            // lblComponentStatus
            // 
            this.lblComponentStatus.AutoSize = true;
            this.lblComponentStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblComponentStatus.Location = new System.Drawing.Point(400, 45);
            this.lblComponentStatus.Name = "lblComponentStatus";
            this.lblComponentStatus.Size = new System.Drawing.Size(0, 13);
            this.lblComponentStatus.TabIndex = 7;
            // 
            // dgvComponentHistory
            // 
            this.dgvComponentHistory.AllowUserToAddRows = false;
            this.dgvComponentHistory.AllowUserToDeleteRows = false;
            this.dgvComponentHistory.AllowUserToOrderColumns = true;
            this.dgvComponentHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvComponentHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvComponentHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvComponentHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComponentHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHistSolution,
            this.colHistVersion,
            this.colHistModifiedBy,
            this.colHistCreatedOn,
            this.colHistModifiedOn,
            this.colHistPublisher,
            this.colHistManaged});
            this.dgvComponentHistory.Location = new System.Drawing.Point(5, 75);
            this.dgvComponentHistory.MultiSelect = true;
            this.dgvComponentHistory.Name = "dgvComponentHistory";
            this.dgvComponentHistory.ReadOnly = true;
            this.dgvComponentHistory.RowHeadersVisible = false;
            this.dgvComponentHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvComponentHistory.Size = new System.Drawing.Size(930, 475);
            this.dgvComponentHistory.TabIndex = 6;
            // 
            // colHistSolution
            // 
            this.colHistSolution.FillWeight = 240F;
            this.colHistSolution.MinimumWidth = 160;
            this.colHistSolution.HeaderText = "Solution Name";
            this.colHistSolution.Name = "colHistSolution";
            this.colHistSolution.ReadOnly = true;
            // 
            // colHistVersion
            // 
            this.colHistVersion.FillWeight = 80F;
            this.colHistVersion.MinimumWidth = 65;
            this.colHistVersion.HeaderText = "Version";
            this.colHistVersion.Name = "colHistVersion";
            this.colHistVersion.ReadOnly = true;
            // 
            // colHistModifiedBy
            // 
            this.colHistModifiedBy.FillWeight = 160F;
            this.colHistModifiedBy.MinimumWidth = 110;
            this.colHistModifiedBy.HeaderText = "Modified By";
            this.colHistModifiedBy.Name = "colHistModifiedBy";
            this.colHistModifiedBy.ReadOnly = true;
            // 
            // colHistCreatedOn
            // 
            this.colHistCreatedOn.FillWeight = 110F;
            this.colHistCreatedOn.MinimumWidth = 90;
            this.colHistCreatedOn.HeaderText = "Created On";
            this.colHistCreatedOn.Name = "colHistCreatedOn";
            this.colHistCreatedOn.ReadOnly = true;
            // 
            // colHistModifiedOn
            // 
            this.colHistModifiedOn.FillWeight = 110F;
            this.colHistModifiedOn.MinimumWidth = 90;
            this.colHistModifiedOn.HeaderText = "Modified On";
            this.colHistModifiedOn.Name = "colHistModifiedOn";
            this.colHistModifiedOn.ReadOnly = true;
            // 
            // colHistPublisher
            // 
            this.colHistPublisher.FillWeight = 140F;
            this.colHistPublisher.MinimumWidth = 100;
            this.colHistPublisher.HeaderText = "Publisher";
            this.colHistPublisher.Name = "colHistPublisher";
            this.colHistPublisher.ReadOnly = true;
            // 
            // colHistManaged
            // 
            this.colHistManaged.FillWeight = 60F;
            this.colHistManaged.MinimumWidth = 50;
            this.colHistManaged.HeaderText = "Managed";
            this.colHistManaged.Name = "colHistManaged";
            this.colHistManaged.ReadOnly = true;
            // 
            // cbComponents
            // 
            this.cbComponents.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbComponents.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbComponents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbComponents.FormattingEnabled = true;
            this.cbComponents.Location = new System.Drawing.Point(125, 42);
            this.cbComponents.Name = "cbComponents";
            this.cbComponents.Size = new System.Drawing.Size(260, 21);
            this.cbComponents.TabIndex = 5;
            this.cbComponents.SelectedIndexChanged += new System.EventHandler(this.cbComponents_SelectedIndexChanged);
            this.cbComponents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbComponents_KeyDown);
            this.cbComponents.Leave += new System.EventHandler(this.cbComponents_Leave);
            // 
            // lblComponent
            // 
            this.lblComponent.AutoSize = true;
            this.lblComponent.Location = new System.Drawing.Point(5, 45);
            this.lblComponent.Name = "lblComponent";
            this.lblComponent.Size = new System.Drawing.Size(97, 13);
            this.lblComponent.TabIndex = 4;
            this.lblComponent.Text = "Select Component:";
            // 
            // cbEntities
            // 
            this.cbEntities.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEntities.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEntities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbEntities.FormattingEnabled = true;
            this.cbEntities.Location = new System.Drawing.Point(510, 12);
            this.cbEntities.Name = "cbEntities";
            this.cbEntities.Size = new System.Drawing.Size(260, 21);
            this.cbEntities.TabIndex = 3;
            this.cbEntities.SelectedIndexChanged += new System.EventHandler(this.cbEntities_SelectedIndexChanged);
            this.cbEntities.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbEntities_KeyDown);
            this.cbEntities.Leave += new System.EventHandler(this.cbEntities_Leave);
            // 
            // lblEntity
            // 
            this.lblEntity.AutoSize = true;
            this.lblEntity.Location = new System.Drawing.Point(400, 15);
            this.lblEntity.Name = "lblEntity";
            this.lblEntity.Size = new System.Drawing.Size(104, 13);
            this.lblEntity.TabIndex = 2;
            this.lblEntity.Text = "Select Entity / Table:";
            // 
            // cbComponentTypes
            // 
            this.cbComponentTypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbComponentTypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbComponentTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbComponentTypes.FormattingEnabled = true;
            this.cbComponentTypes.Location = new System.Drawing.Point(125, 12);
            this.cbComponentTypes.Name = "cbComponentTypes";
            this.cbComponentTypes.Size = new System.Drawing.Size(260, 21);
            this.cbComponentTypes.TabIndex = 1;
            this.cbComponentTypes.SelectedIndexChanged += new System.EventHandler(this.cbComponentTypes_SelectedIndexChanged);
            this.cbComponentTypes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbComponentTypes_KeyDown);
            this.cbComponentTypes.Leave += new System.EventHandler(this.cbComponentTypes_Leave);
            // 
            // lblComponentType
            // 
            this.lblComponentType.AutoSize = true;
            this.lblComponentType.Location = new System.Drawing.Point(5, 15);
            this.lblComponentType.Name = "lblComponentType";
            this.lblComponentType.Size = new System.Drawing.Size(91, 13);
            this.lblComponentType.TabIndex = 0;
            this.lblComponentType.Text = "Component Type:";
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnComponent);
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
            this.pnComponent.ResumeLayout(false);
            this.pnComponent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentHistory)).EndInit();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifiedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifiedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPublisher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManaged;
        private System.Windows.Forms.Panel pnComponent;
        private System.Windows.Forms.ComboBox cbComponents;
        private System.Windows.Forms.Label lblComponent;
        private System.Windows.Forms.ComboBox cbEntities;
        private System.Windows.Forms.Label lblEntity;
        private System.Windows.Forms.ComboBox cbComponentTypes;
        private System.Windows.Forms.Label lblComponentType;
        private System.Windows.Forms.Label lblComponentStatus;
        private System.Windows.Forms.DataGridView dgvComponentHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistSolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistModifiedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistCreatedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistModifiedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistPublisher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistManaged;
    }
}
