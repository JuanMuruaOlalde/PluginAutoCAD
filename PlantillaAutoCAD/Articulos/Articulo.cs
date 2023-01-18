using System;

namespace PlantillaAutoCAD
{
    class Articulo
    {
        private String codigoArticulo;
        private String nombreArticulo_ES_;
        private String nombreArticulo_EN_;

        public Articulo(String codigoArticulo, String nombreArticulo_ES_, String nombreArticulo_EN_)
        {
            this.codigoArticulo = codigoArticulo;
            this.nombreArticulo_ES_ = nombreArticulo_ES_;
            this.nombreArticulo_EN_ = nombreArticulo_EN_;
        }

        public String getCodigoArticulo()
        {
            return codigoArticulo;
        }

        public String getNombreArticulo_ES_()
        {
            return nombreArticulo_ES_;
        }

        public String getNombreArticulo_EN_()
        {
            return nombreArticulo_EN_;
        }
    }
}
