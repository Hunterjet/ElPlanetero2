namespace ExamenFinal
{
    partial class Form1
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
            this.csgL12Control1 = new CSGL12.CSGL12Control();
            this.SuspendLayout();
            // 
            // csgL12Control1
            // 
            this.csgL12Control1.Location = new System.Drawing.Point(12, 12);
            this.csgL12Control1.Name = "csgL12Control1";
            this.csgL12Control1.Size = new System.Drawing.Size(879, 529);
            this.csgL12Control1.TabIndex = 0;
            this.csgL12Control1.Text = "csgL12Control1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 588);
            this.Controls.Add(this.csgL12Control1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CSGL12.CSGL12Control csgL12Control1;
    }
}

