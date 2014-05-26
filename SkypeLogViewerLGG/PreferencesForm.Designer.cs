namespace SkypeLogViewerLGG
{
    partial class PreferencesForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1Preview = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1custom = new System.Windows.Forms.TextBox();
            this.radioButtonCustom = new System.Windows.Forms.RadioButton();
            this.radioButtonOtherDefault = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonDefault1 = new System.Windows.Forms.RadioButton();
            this.button1Save = new System.Windows.Forms.Button();
            this.button1Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1Preview);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1custom);
            this.groupBox1.Controls.Add(this.radioButtonCustom);
            this.groupBox1.Controls.Add(this.radioButtonOtherDefault);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButtonDefault1);
            this.groupBox1.Location = new System.Drawing.Point(26, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 318);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log Format";
            // 
            // textBox1Preview
            // 
            this.textBox1Preview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1Preview.Location = new System.Drawing.Point(9, 292);
            this.textBox1Preview.Name = "textBox1Preview";
            this.textBox1Preview.ReadOnly = true;
            this.textBox1Preview.Size = new System.Drawing.Size(347, 20);
            this.textBox1Preview.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Preview:";
            // 
            // textBox1custom
            // 
            this.textBox1custom.Location = new System.Drawing.Point(9, 238);
            this.textBox1custom.Name = "textBox1custom";
            this.textBox1custom.Size = new System.Drawing.Size(347, 20);
            this.textBox1custom.TabIndex = 4;
            this.textBox1custom.Text = "[[Month]/[Day]/[Year] [Time]] [DisplayName]:";
            this.textBox1custom.TextChanged += new System.EventHandler(this.updatePreview);
            // 
            // radioButtonCustom
            // 
            this.radioButtonCustom.AutoSize = true;
            this.radioButtonCustom.Location = new System.Drawing.Point(12, 215);
            this.radioButtonCustom.Name = "radioButtonCustom";
            this.radioButtonCustom.Size = new System.Drawing.Size(60, 17);
            this.radioButtonCustom.TabIndex = 3;
            this.radioButtonCustom.TabStop = true;
            this.radioButtonCustom.Text = "Custom";
            this.radioButtonCustom.UseVisualStyleBackColor = true;
            this.radioButtonCustom.CheckedChanged += new System.EventHandler(this.updatePreview);
            // 
            // radioButtonOtherDefault
            // 
            this.radioButtonOtherDefault.AutoSize = true;
            this.radioButtonOtherDefault.Location = new System.Drawing.Point(9, 176);
            this.radioButtonOtherDefault.Name = "radioButtonOtherDefault";
            this.radioButtonOtherDefault.Size = new System.Drawing.Size(301, 17);
            this.radioButtonOtherDefault.TabIndex = 2;
            this.radioButtonOtherDefault.TabStop = true;
            this.radioButtonOtherDefault.Text = "[[Month]/[Day]/[Year] [Time]] [DisplayName] ([UserName]):";
            this.radioButtonOtherDefault.UseVisualStyleBackColor = true;
            this.radioButtonOtherDefault.CheckedChanged += new System.EventHandler(this.updatePreview);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 91);
            this.label1.TabIndex = 1;
            this.label1.Text = "Keywords:\r\n[DisplayName]\r\n[UserName]\r\n[Month]\r\n[Day]\r\n[Year]\r\n[Time]";
            // 
            // radioButtonDefault1
            // 
            this.radioButtonDefault1.AutoSize = true;
            this.radioButtonDefault1.Location = new System.Drawing.Point(9, 138);
            this.radioButtonDefault1.Name = "radioButtonDefault1";
            this.radioButtonDefault1.Size = new System.Drawing.Size(236, 17);
            this.radioButtonDefault1.TabIndex = 0;
            this.radioButtonDefault1.TabStop = true;
            this.radioButtonDefault1.Text = "[[Month]/[Day]/[Year] [Time]] [DisplayName]:";
            this.radioButtonDefault1.UseVisualStyleBackColor = true;
            this.radioButtonDefault1.CheckedChanged += new System.EventHandler(this.updatePreview);
            // 
            // button1Save
            // 
            this.button1Save.Location = new System.Drawing.Point(315, 349);
            this.button1Save.Name = "button1Save";
            this.button1Save.Size = new System.Drawing.Size(75, 23);
            this.button1Save.TabIndex = 1;
            this.button1Save.Text = "Save";
            this.button1Save.UseVisualStyleBackColor = true;
            this.button1Save.Click += new System.EventHandler(this.button1Save_Click);
            // 
            // button1Cancel
            // 
            this.button1Cancel.Location = new System.Drawing.Point(234, 348);
            this.button1Cancel.Name = "button1Cancel";
            this.button1Cancel.Size = new System.Drawing.Size(75, 23);
            this.button1Cancel.TabIndex = 2;
            this.button1Cancel.Text = "Cancel";
            this.button1Cancel.UseVisualStyleBackColor = true;
            this.button1Cancel.Click += new System.EventHandler(this.button1Cancel_Click);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 383);
            this.Controls.Add(this.button1Cancel);
            this.Controls.Add(this.button1Save);
            this.Controls.Add(this.groupBox1);
            this.Name = "PreferencesForm";
            this.Text = "PreferencesForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonDefault1;
        private System.Windows.Forms.RadioButton radioButtonCustom;
        private System.Windows.Forms.RadioButton radioButtonOtherDefault;
        private System.Windows.Forms.Button button1Save;
        private System.Windows.Forms.Button button1Cancel;
        private System.Windows.Forms.TextBox textBox1custom;
        private System.Windows.Forms.TextBox textBox1Preview;
        private System.Windows.Forms.Label label2;
    }
}