using System.Collections.Generic;

namespace PlantillaAutoCAD
{
    class ManejoDeArticulos
    {

        public ManejoDeArticulos()
        {
        }


        public Articulo getArticulo(string codigoArticulo)
        {
            //En una aplicación real, los valores se leerian desde una base de datos o similar
            //pero para esta prueba los pongo literales aqui mismo...
            return new Articulo(codigoArticulo: codigoArticulo, 
                nombreArticulo_ES_: "Un nombre de pruebas para el articulo " + codigoArticulo,
                nombreArticulo_EN_: "Dummy name for " + codigoArticulo + " item");
        }


    }
}
