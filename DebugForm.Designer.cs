namespace Lib
{
    partial class DebugForm
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lvLogFiles = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            this.cbRun = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bClearAll = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.tpPerformance = new System.Windows.Forms.TabPage();
            this.cbAutorefresh = new System.Windows.Forms.CheckBox();
            this.bPerfReset = new System.Windows.Forms.Button();
            this.bPerfRefresh = new System.Windows.Forms.Button();
            this.dgvPerfomance = new System.Windows.Forms.DataGridView();
            this.cbAutoResizeColumns = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tpPerformance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerfomance)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lvLogFiles
            // 
            this.lvLogFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvLogFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.lvLogFiles.FullRowSelect = true;
            this.lvLogFiles.HideSelection = false;
            this.lvLogFiles.Location = new System.Drawing.Point(0, 0);
            this.lvLogFiles.Name = "lvLogFiles";
            this.lvLogFiles.Size = new System.Drawing.Size(189, 414);
            this.lvLogFiles.TabIndex = 1;
            this.lvLogFiles.UseCompatibleStateImageBehavior = false;
            this.lvLogFiles.View = System.Windows.Forms.View.Details;
            this.lvLogFiles.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvLogFiles_ItemSelectionChanged);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 178;
            // 
            // rtbDebug
            // 
            this.rtbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbDebug.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDebug.Location = new System.Drawing.Point(195, 32);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.ReadOnly = true;
            this.rtbDebug.Size = new System.Drawing.Size(633, 376);
            this.rtbDebug.TabIndex = 2;
            this.rtbDebug.Text = "";
            // 
            // cbRun
            // 
            this.cbRun.AutoSize = true;
            this.cbRun.Checked = true;
            this.cbRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRun.Location = new System.Drawing.Point(198, 9);
            this.cbRun.Name = "cbRun";
            this.cbRun.Size = new System.Drawing.Size(46, 17);
            this.cbRun.TabIndex = 3;
            this.cbRun.Text = "Run";
            this.cbRun.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tpPerformance);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(836, 440);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bClearAll);
            this.tabPage1.Controls.Add(this.bClear);
            this.tabPage1.Controls.Add(this.lvLogFiles);
            this.tabPage1.Controls.Add(this.cbRun);
            this.tabPage1.Controls.Add(this.rtbDebug);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(828, 414);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bClearAll
            // 
            this.bClearAll.Location = new System.Drawing.Point(303, 5);
            this.bClearAll.Margin = new System.Windows.Forms.Padding(2);
            this.bClearAll.Name = "bClearAll";
            this.bClearAll.Size = new System.Drawing.Size(50, 23);
            this.bClearAll.TabIndex = 4;
            this.bClearAll.Text = "Clear all";
            this.bClearAll.UseVisualStyleBackColor = true;
            this.bClearAll.Click += new System.EventHandler(this.bClearAll_Click);
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(249, 5);
            this.bClear.Margin = new System.Windows.Forms.Padding(2);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(50, 23);
            this.bClear.TabIndex = 4;
            this.bClear.Text = "Clear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // tpPerformance
            // 
            this.tpPerformance.Controls.Add(this.cbAutoResizeColumns);
            this.tpPerformance.Controls.Add(this.cbAutorefresh);
            this.tpPerformance.Controls.Add(this.bPerfReset);
            this.tpPerformance.Controls.Add(this.bPerfRefresh);
            this.tpPerformance.Controls.Add(this.dgvPerfomance);
            this.tpPerformance.Location = new System.Drawing.Point(4, 22);
            this.tpPerformance.Name = "tpPerformance";
            this.tpPerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tpPerformance.Size = new System.Drawing.Size(828, 414);
            this.tpPerformance.TabIndex = 1;
            this.tpPerformance.Text = "Performance";
            this.tpPerformance.UseVisualStyleBackColor = true;
            // 
            // cbAutorefresh
            // 
            this.cbAutorefresh.AutoSize = true;
            this.cbAutorefresh.Location = new System.Drawing.Point(168, 10);
            this.cbAutorefresh.Name = "cbAutorefresh";
            this.cbAutorefresh.Size = new System.Drawing.Size(80, 17);
            this.cbAutorefresh.TabIndex = 2;
            this.cbAutorefresh.Text = "Autorefresh";
            this.cbAutorefresh.UseVisualStyleBackColor = true;
            // 
            // bPerfReset
            // 
            this.bPerfReset.Location = new System.Drawing.Point(87, 6);
            this.bPerfReset.Name = "bPerfReset";
            this.bPerfReset.Size = new System.Drawing.Size(75, 23);
            this.bPerfReset.TabIndex = 1;
            this.bPerfReset.Text = "Reset";
            this.bPerfReset.UseVisualStyleBackColor = true;
            this.bPerfReset.Click += new System.EventHandler(this.bPerfReset_Click);
            // 
            // bPerfRefresh
            // 
            this.bPerfRefresh.Location = new System.Drawing.Point(6, 6);
            this.bPerfRefresh.Name = "bPerfRefresh";
            this.bPerfRefresh.Size = new System.Drawing.Size(75, 23);
            this.bPerfRefresh.TabIndex = 1;
            this.bPerfRefresh.Text = "Refresh";
            this.bPerfRefresh.UseVisualStyleBackColor = true;
            this.bPerfRefresh.Click += new System.EventHandler(this.bPerfRefresh_Click);
            // 
            // dgvPerfomance
            // 
            this.dgvPerfomance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPerfomance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPerfomance.Location = new System.Drawing.Point(3, 35);
            this.dgvPerfomance.Name = "dgvPerfomance";
            this.dgvPerfomance.Size = new System.Drawing.Size(822, 379);
            this.dgvPerfomance.TabIndex = 0;
            // 
            // cbAutoResizeColumns
            // 
            this.cbAutoResizeColumns.AutoSize = true;
            this.cbAutoResizeColumns.Checked = true;
            this.cbAutoResizeColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoResizeColumns.Location = new System.Drawing.Point(254, 10);
            this.cbAutoResizeColumns.Name = "cbAutoResizeColumns";
            this.cbAutoResizeColumns.Size = new System.Drawing.Size(117, 17);
            this.cbAutoResizeColumns.TabIndex = 3;
            this.cbAutoResizeColumns.Text = "Autoresize columns";
            this.cbAutoResizeColumns.UseVisualStyleBackColor = true;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 440);
            this.Controls.Add(this.tabControl1);
            this.Name = "DebugForm";
            this.Text = "DebugForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugForm_FormClosing);
            this.Load += new System.EventHandler(this.DebugForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tpPerformance.ResumeLayout(false);
            this.tpPerformance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerfomance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ListView lvLogFiles;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.RichTextBox rtbDebug;
        private System.Windows.Forms.CheckBox cbRun;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tpPerformance;
        private System.Windows.Forms.Button bPerfReset;
        private System.Windows.Forms.Button bPerfRefresh;
        private System.Windows.Forms.DataGridView dgvPerfomance;
        private System.Windows.Forms.CheckBox cbAutorefresh;
        private System.Windows.Forms.Button bClearAll;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.CheckBox cbAutoResizeColumns;
    }
}