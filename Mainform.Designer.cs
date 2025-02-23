namespace Forzza
{
    partial class Mainform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSetting = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStake = new System.Windows.Forms.TextBox();
            this.txtDomain = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageMonitor = new System.Windows.Forms.TabPage();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelBalance = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridViewHistory = new System.Windows.Forms.DataGridView();
            this.homeTeam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.awayTeam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outcome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oddValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stake = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingHistory = new System.Windows.Forms.BindingSource(this.components);
            this.txtLogs = new System.Windows.Forms.RichTextBox();
            this.dataGridViewScan = new System.Windows.Forms.DataGridView();
            this.Naldo_home = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Naldo_away = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Naldo_outcome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Naldo_odd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Naldo_stake = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingTiplist = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageSetting.SuspendLayout();
            this.tabPageMonitor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTiplist)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSetting);
            this.tabControl.Controls.Add(this.tabPageMonitor);
            this.tabControl.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(1, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(816, 607);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.Controls.Add(this.label8);
            this.tabPageSetting.Controls.Add(this.txtStake);
            this.tabPageSetting.Controls.Add(this.txtDomain);
            this.tabPageSetting.Controls.Add(this.btnSave);
            this.tabPageSetting.Controls.Add(this.txtPassword);
            this.tabPageSetting.Controls.Add(this.txtUsername);
            this.tabPageSetting.Controls.Add(this.label4);
            this.tabPageSetting.Controls.Add(this.label2);
            this.tabPageSetting.Controls.Add(this.label1);
            this.tabPageSetting.Location = new System.Drawing.Point(4, 30);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSetting.Size = new System.Drawing.Size(808, 573);
            this.tabPageSetting.TabIndex = 0;
            this.tabPageSetting.Text = "Setting";
            this.tabPageSetting.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(467, 193);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 21);
            this.label8.TabIndex = 13;
            this.label8.Text = "stake:";
            // 
            // txtStake
            // 
            this.txtStake.Font = new System.Drawing.Font("Arial", 9.75F);
            this.txtStake.Location = new System.Drawing.Point(538, 196);
            this.txtStake.Name = "txtStake";
            this.txtStake.Size = new System.Drawing.Size(155, 22);
            this.txtStake.TabIndex = 12;
            // 
            // txtDomain
            // 
            this.txtDomain.Font = new System.Drawing.Font("Arial", 9.75F);
            this.txtDomain.FormattingEnabled = true;
            this.txtDomain.Items.AddRange(new object[] {
            "www.forzza.com"});
            this.txtDomain.Location = new System.Drawing.Point(538, 151);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(155, 24);
            this.txtDomain.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(361, 261);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(116, 35);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Arial", 9.75F);
            this.txtPassword.Location = new System.Drawing.Point(170, 196);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(227, 22);
            this.txtPassword.TabIndex = 5;
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(170, 151);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(227, 22);
            this.txtUsername.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(458, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Domain:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "UserName:";
            // 
            // tabPageMonitor
            // 
            this.tabPageMonitor.Controls.Add(this.btnStart);
            this.tabPageMonitor.Controls.Add(this.btnStop);
            this.tabPageMonitor.Controls.Add(this.labelUser);
            this.tabPageMonitor.Controls.Add(this.labelBalance);
            this.tabPageMonitor.Controls.Add(this.label7);
            this.tabPageMonitor.Controls.Add(this.label6);
            this.tabPageMonitor.Controls.Add(this.label5);
            this.tabPageMonitor.Controls.Add(this.dataGridViewHistory);
            this.tabPageMonitor.Controls.Add(this.txtLogs);
            this.tabPageMonitor.Controls.Add(this.dataGridViewScan);
            this.tabPageMonitor.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageMonitor.Location = new System.Drawing.Point(4, 30);
            this.tabPageMonitor.Name = "tabPageMonitor";
            this.tabPageMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonitor.Size = new System.Drawing.Size(808, 573);
            this.tabPageMonitor.TabIndex = 1;
            this.tabPageMonitor.Text = "Monitor";
            this.tabPageMonitor.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(7, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(142, 21);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click_1);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(372, 3);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(75, 16);
            this.labelUser.TabIndex = 7;
            this.labelUser.Text = "Username:";
            // 
            // labelBalance
            // 
            this.labelBalance.AutoSize = true;
            this.labelBalance.Location = new System.Drawing.Point(600, 3);
            this.labelBalance.Name = "labelBalance";
            this.labelBalance.Size = new System.Drawing.Size(63, 16);
            this.labelBalance.TabIndex = 6;
            this.labelBalance.Text = "Balance:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(447, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Logs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "History";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Scan";
            // 
            // dataGridViewHistory
            // 
            this.dataGridViewHistory.AutoGenerateColumns = false;
            this.dataGridViewHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.homeTeam,
            this.awayTeam,
            this.outcome,
            this.oddValue,
            this.stake});
            this.dataGridViewHistory.DataSource = this.bindingHistory;
            this.dataGridViewHistory.Location = new System.Drawing.Point(0, 369);
            this.dataGridViewHistory.Name = "dataGridViewHistory";
            this.dataGridViewHistory.Size = new System.Drawing.Size(808, 204);
            this.dataGridViewHistory.TabIndex = 2;
            // 
            // homeTeam
            // 
            this.homeTeam.DataPropertyName = "homeTeam";
            this.homeTeam.HeaderText = "HomeTeam";
            this.homeTeam.Name = "homeTeam";
            this.homeTeam.Width = 160;
            // 
            // awayTeam
            // 
            this.awayTeam.DataPropertyName = "awayTeam";
            this.awayTeam.HeaderText = "AwayTeam";
            this.awayTeam.Name = "awayTeam";
            this.awayTeam.Width = 160;
            // 
            // outcome
            // 
            this.outcome.DataPropertyName = "outcome";
            this.outcome.HeaderText = "OutCome";
            this.outcome.Name = "outcome";
            this.outcome.Width = 265;
            // 
            // oddValue
            // 
            this.oddValue.DataPropertyName = "oddValue";
            this.oddValue.HeaderText = "Odd";
            this.oddValue.Name = "oddValue";
            this.oddValue.Width = 90;
            // 
            // stake
            // 
            this.stake.DataPropertyName = "stake";
            this.stake.HeaderText = "Stake";
            this.stake.Name = "stake";
            this.stake.Width = 90;
            // 
            // txtLogs
            // 
            this.txtLogs.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogs.Location = new System.Drawing.Point(450, 82);
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.Size = new System.Drawing.Size(355, 265);
            this.txtLogs.TabIndex = 1;
            this.txtLogs.Text = "";
            // 
            // dataGridViewScan
            // 
            this.dataGridViewScan.AutoGenerateColumns = false;
            this.dataGridViewScan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Naldo_home,
            this.Naldo_away,
            this.Naldo_outcome,
            this.Naldo_odd,
            this.Naldo_stake});
            this.dataGridViewScan.DataSource = this.bindingTiplist;
            this.dataGridViewScan.Location = new System.Drawing.Point(3, 82);
            this.dataGridViewScan.Name = "dataGridViewScan";
            this.dataGridViewScan.Size = new System.Drawing.Size(438, 265);
            this.dataGridViewScan.TabIndex = 0;
            // 
            // Naldo_home
            // 
            this.Naldo_home.DataPropertyName = "Naldo_home";
            this.Naldo_home.HeaderText = "HomeTeam";
            this.Naldo_home.Name = "Naldo_home";
            this.Naldo_home.Width = 90;
            // 
            // Naldo_away
            // 
            this.Naldo_away.DataPropertyName = "Naldo_away";
            this.Naldo_away.HeaderText = "AwayTeam";
            this.Naldo_away.Name = "Naldo_away";
            this.Naldo_away.Width = 90;
            // 
            // Naldo_outcome
            // 
            this.Naldo_outcome.DataPropertyName = "Naldo_outcome";
            this.Naldo_outcome.HeaderText = "Outcome";
            this.Naldo_outcome.Name = "Naldo_outcome";
            // 
            // Naldo_odd
            // 
            this.Naldo_odd.DataPropertyName = "Naldo_odd";
            this.Naldo_odd.HeaderText = "Odd";
            this.Naldo_odd.Name = "Naldo_odd";
            this.Naldo_odd.Width = 55;
            // 
            // Naldo_stake
            // 
            this.Naldo_stake.DataPropertyName = "Naldo_stake";
            this.Naldo_stake.HeaderText = "Stake";
            this.Naldo_stake.Name = "Naldo_stake";
            this.Naldo_stake.Width = 60;
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 608);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Mainform";
            this.Text = "Betting Bot";
            this.tabControl.ResumeLayout(false);
            this.tabPageSetting.ResumeLayout(false);
            this.tabPageSetting.PerformLayout();
            this.tabPageMonitor.ResumeLayout(false);
            this.tabPageMonitor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTiplist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSetting;
        private System.Windows.Forms.TabPage tabPageMonitor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.DataGridView dataGridViewHistory;
        private System.Windows.Forms.RichTextBox txtLogs;
        private System.Windows.Forms.DataGridView dataGridViewScan;
        private System.Windows.Forms.Label labelBalance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox txtDomain;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStake;
        private System.Windows.Forms.DataGridViewTextBoxColumn Naldo_home;
        private System.Windows.Forms.DataGridViewTextBoxColumn Naldo_away;
        private System.Windows.Forms.DataGridViewTextBoxColumn Naldo_outcome;
        private System.Windows.Forms.DataGridViewTextBoxColumn Naldo_odd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Naldo_stake;
        private System.Windows.Forms.BindingSource bindingTiplist;
        private System.Windows.Forms.BindingSource bindingHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn homeTeam;
        private System.Windows.Forms.DataGridViewTextBoxColumn awayTeam;
        private System.Windows.Forms.DataGridViewTextBoxColumn outcome;
        private System.Windows.Forms.DataGridViewTextBoxColumn oddValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn stake;
    }
}

