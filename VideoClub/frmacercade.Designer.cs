namespace MaxVideoClub
{
    partial class frmacercade
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
            this.dato = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dato
            // 
            this.dato.Location = new System.Drawing.Point(35, 67);
            this.dato.Name = "dato";
            this.dato.Size = new System.Drawing.Size(221, 161);
            this.dato.TabIndex = 0;
            this.dato.Text = "Proyecto para la materia Propietaria I \n\nProfesor: Juan Pablo Valdez\n\nEstudiantes" +
    ": Angel Valenzuela 2013-1071\n                     Mariel\n";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Volver";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmacercade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dato);
            this.Name = "frmacercade";
            this.Text = "Propietaria I";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox dato;
        private System.Windows.Forms.Button button1;
    }
}