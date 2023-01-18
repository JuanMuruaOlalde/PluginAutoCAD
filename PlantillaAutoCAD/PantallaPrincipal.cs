using System;
using System.Windows.Forms;

namespace PlantillaAutoCAD
{
    public partial class PantallaPrincipal : Form
    {
        public PantallaPrincipal()
        {
            InitializeComponent();
            this.Text = this.Text + " (" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString()
            + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString()
            + ".p" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString()
            + ".c" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString()
            + ") ";

        }

        private void btnElegirCarpeta_Click(object sender, EventArgs e)
       {
            txtInformacionDeEstado.Text = String.Empty;
            folderBrowserDialog1.ShowNewFolderButton = false;
            DialogResult respuesta = folderBrowserDialog1.ShowDialog();
            if (respuesta == DialogResult.OK)
            {
                txtPathCarpeta.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnLanzarProceso_Click(object sender, EventArgs e)
        {
            try
            {
                txtInformacionDeEstado.Text = DateTime.Now.ToString() + Environment.NewLine + "Se está procesando la carpeta...";

                AutoCAD_funciones.ProcesarCarpeta_cargandoSoloEnMemoria(new System.IO.DirectoryInfo(txtPathCarpeta.Text));

                txtInformacionDeEstado.Text = DateTime.Now.ToString() + Environment.NewLine + "Proceso TERMINADO.";
            }
            catch (Exception ex)
            {
                txtInformacionDeEstado.Text = "Se ha producido un error al intentar explorar esa carpeta." 
                    + Environment.NewLine + Environment.NewLine + ex.StackTrace;
            }
        }

     }
}
