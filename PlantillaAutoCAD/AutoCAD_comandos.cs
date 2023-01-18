using System;
using System.Collections.Generic;

namespace PlantillaAutoCAD
{
    public class AutoCAD_comandos
    {
        [Autodesk.AutoCAD.Runtime.CommandMethod("PONERATRIBUTOSABLOQUES")]
        public void  mostrarLaPantallaPrincipal()
        {
            PantallaPrincipal pantalla = new PantallaPrincipal();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(pantalla);
        }

     }
}
