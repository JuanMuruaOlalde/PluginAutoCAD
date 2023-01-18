
namespace PlantillaAutoCAD
{
    partial class PantallaPrincipal
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
            this.btnLanzarProceso = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lblCarpeta = new System.Windows.Forms.Label();
            this.txtPathCarpeta = new System.Windows.Forms.TextBox();
            this.btnElegirCarpeta = new System.Windows.Forms.Button();
            this.txtInformacionDeEstado = new System.Windows.Forms.TextBox();
            this.lblNotaFijaDeAviso = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLanzarProceso
            // 
            this.btnLanzarProceso.AutoSize = true;
            this.btnLanzarProceso.BackColor = System.Drawing.SystemColors.Control;
            this.btnLanzarProceso.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLanzarProceso.Location = new System.Drawing.Point(129, 104);
            this.btnLanzarProceso.Name = "btnLanzarProceso";
            this.btnLanzarProceso.Size = new System.Drawing.Size(110, 34);
            this.btnLanzarProceso.TabIndex = 30;
            this.btnLanzarProceso.Text = "Lanzar proceso";
            this.btnLanzarProceso.UseVisualStyleBackColor = false;
            this.btnLanzarProceso.Click += new System.EventHandler(this.btnLanzarProceso_Click);
            // 
            // lblCarpeta
            // 
            this.lblCarpeta.AutoSize = true;
            this.lblCarpeta.Location = new System.Drawing.Point(23, 49);
            this.lblCarpeta.Name = "lblCarpeta";
            this.lblCarpeta.Size = new System.Drawing.Size(100, 13);
            this.lblCarpeta.TabIndex = 1;
            this.lblCarpeta.Text = "Carpeta a procesar:";
            // 
            // txtPathCarpeta
            // 
            this.txtPathCarpeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPathCarpeta.Location = new System.Drawing.Point(129, 44);
            this.txtPathCarpeta.Multiline = true;
            this.txtPathCarpeta.Name = "txtPathCarpeta";
            this.txtPathCarpeta.Size = new System.Drawing.Size(475, 44);
            this.txtPathCarpeta.TabIndex = 20;
            // 
            // btnElegirCarpeta
            // 
            this.btnElegirCarpeta.AutoSize = true;
            this.btnElegirCarpeta.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnElegirCarpeta.Location = new System.Drawing.Point(23, 12);
            this.btnElegirCarpeta.Name = "btnElegirCarpeta";
            this.btnElegirCarpeta.Size = new System.Drawing.Size(88, 30);
            this.btnElegirCarpeta.TabIndex = 10;
            this.btnElegirCarpeta.Text = "Elegir carpeta";
            this.btnElegirCarpeta.UseVisualStyleBackColor = true;
            this.btnElegirCarpeta.Click += new System.EventHandler(this.btnElegirCarpeta_Click);
            // 
            // txtInformacionDeEstado
            // 
            this.txtInformacionDeEstado.BackColor = System.Drawing.SystemColors.Info;
            this.txtInformacionDeEstado.Location = new System.Drawing.Point(165, 154);
            this.txtInformacionDeEstado.Multiline = true;
            this.txtInformacionDeEstado.Name = "txtInformacionDeEstado";
            this.txtInformacionDeEstado.ReadOnly = true;
            this.txtInformacionDeEstado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInformacionDeEstado.Size = new System.Drawing.Size(439, 117);
            this.txtInformacionDeEstado.TabIndex = 2;
            this.txtInformacionDeEstado.TabStop = false;
            // 
            // lblNotaFijaDeAviso
            // 
            this.lblNotaFijaDeAviso.Location = new System.Drawing.Point(20, 303);
            this.lblNotaFijaDeAviso.Name = "lblNotaFijaDeAviso";
            this.lblNotaFijaDeAviso.Size = new System.Drawing.Size(677, 72);
            this.lblNotaFijaDeAviso.TabIndex = 31;
            this.lblNotaFijaDeAviso.Text = "nota: Se consideran bloques de articulo los archivos con extensión .dwg, cuyo nom" +
    "bre consiste en una letra A y 6 números tras ella.\r\nnota: Solo se procesa un niv" +
    "el (no subcarpetas).";
            // 
            // PantallaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 384);
            this.Controls.Add(this.lblNotaFijaDeAviso);
            this.Controls.Add(this.txtInformacionDeEstado);
            this.Controls.Add(this.btnElegirCarpeta);
            this.Controls.Add(this.txtPathCarpeta);
            this.Controls.Add(this.lblCarpeta);
            this.Controls.Add(this.btnLanzarProceso);
            this.Name = "PantallaPrincipal";
            this.Text = "Poner atributos a bloques";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLanzarProceso;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lblCarpeta;
        private System.Windows.Forms.TextBox txtPathCarpeta;
        private System.Windows.Forms.Button btnElegirCarpeta;
        private System.Windows.Forms.TextBox txtInformacionDeEstado;
        private System.Windows.Forms.Label lblNotaFijaDeAviso;
    }
}