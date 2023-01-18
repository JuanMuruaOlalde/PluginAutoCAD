using System;
using System.Collections.Generic;
using System.IO;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Internal.DatabaseServices;

namespace PlantillaAutoCAD
{
    class AutoCAD_funciones
    {

        public static void ProcesarCarpeta_cargandoSoloEnMemoria(System.IO.DirectoryInfo carpeta)
        {
            ManejoDeArticulos articulos = new ManejoDeArticulos();

            Autodesk.AutoCAD.ApplicationServices.DocumentCollection gestorDeDWGs = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
            foreach (System.IO.FileInfo archivo in carpeta.GetFiles("*.dwg"))
            {
                System.Text.RegularExpressions.Regex esBloqueDeArticulo = new System.Text.RegularExpressions.Regex("^A[0-9]{6}");
                if (archivo.Extension.ToUpper().Equals(".DWG") && esBloqueDeArticulo.IsMatch(archivo.Name))
                {
                    try
                    {
                        // nota: En lugar de abrir el .dwg en pantalla, es mucho más rápido si solo lo cargamos en memoria.
                        // (truco aprendido leyendo https://stackoverflow.com/questions/43530751/why-wont-my-autocad-accoreconsole-open-files-or-execute-scripts)
                        using (Autodesk.AutoCAD.DatabaseServices.Database datosDWG = new Autodesk.AutoCAD.DatabaseServices.Database(buildDefaultDrawing: false, noDocument: true))
                        {
                            datosDWG.ReadDwgFile(fileName: archivo.FullName, mode: FileOpenMode.OpenForReadAndWriteNoShare, allowCPConversion: true, password: String.Empty);
                            datosDWG.CloseInput(closeFile: true);
                            using (Autodesk.AutoCAD.DatabaseServices.Transaction transaccionDWG = datosDWG.TransactionManager.StartTransaction())
                            {
                                //AQUI VA EL PROCESO QUE QUERAMOS HACERLE A CADA .dwg
                                //Como ejemplo, insertarle unos atributos...
                                using (LayerTableRecord capaParaAtributos = obtenerOCrearCapaParaAtributos(datosDWG, transaccionDWG))
                                {
                                    capaParaAtributos.IsFrozen = false;
                                    String codigoDeArticulo = System.IO.Path.GetFileNameWithoutExtension(archivo.Name);
                                    Articulo datosDelArticulo = articulos.getArticulo(codigoDeArticulo);
                                    ponerAtributos(datosDelArticulo, capaParaAtributos, datosDWG, transaccionDWG);
                                    capaParaAtributos.IsFrozen = true;
                                }

                                transaccionDWG.Commit();
                            }
                            datosDWG.SaveAs(archivo.FullName, DwgVersion.Current);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ignorar el archivo que ha causado el error y seguir con el siguiente.
                        //TODO _pendiente: es conveniente dejar registro en un log 
                    }
                }
            }
        }

        public static void ProcesarCarpeta_cargandoEnElEditorEnPantalla(System.IO.DirectoryInfo carpeta)
        {
            ManejoDeArticulos manejoDeBloques = new ManejoDeArticulos();

            //setVariableSDI(true); // Poner AutoCAD en modo SingleDocumentInterface es necesario para que los bloques dinámicos no se paren a preguntar si queremos que se abran en el editor de bloques.

            Autodesk.AutoCAD.ApplicationServices.DocumentCollection gestorDeDWGs = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
            foreach (System.IO.FileInfo archivo in carpeta.GetFiles("*.dwg"))
            {
                System.Text.RegularExpressions.Regex esBloqueDeArticulo = new System.Text.RegularExpressions.Regex("^A[0-9]{6}");
                if (archivo.Extension.ToUpper().Equals(".DWG") && esBloqueDeArticulo.IsMatch(archivo.Name))
                {
                    try
                    {
                        Autodesk.AutoCAD.ApplicationServices.Document documentoDWG;
                        documentoDWG = Autodesk.AutoCAD.ApplicationServices.DocumentCollectionExtension.Open(gestorDeDWGs, archivo.FullName, forReadOnly: false);
                        Autodesk.AutoCAD.ApplicationServices.DocumentLock reservaDelDocumento = documentoDWG.LockDocument();
                        using (Autodesk.AutoCAD.DatabaseServices.Database datosDWG = documentoDWG.Database)
                        {
                            using (Autodesk.AutoCAD.DatabaseServices.Transaction transaccionDWG = datosDWG.TransactionManager.StartTransaction())
                            {
                                //AQUI VA EL PROCESO QUE QUERAMOS HACERLE A CADA .dwg
                                //Como ejemplo, insertarle unos atributos...
                                using (LayerTableRecord capaParaAtributos = obtenerOCrearCapaParaAtributos(datosDWG, transaccionDWG))
                                {
                                    capaParaAtributos.IsFrozen = false;
                                    String codigoDeArticulo = System.IO.Path.GetFileNameWithoutExtension(archivo.Name);
                                    Articulo datosDelArticulo = manejoDeBloques.getArticulo(codigoDeArticulo);
                                    ponerAtributos(datosDelArticulo, capaParaAtributos, datosDWG, transaccionDWG);
                                    capaParaAtributos.IsFrozen = true;
                                }

                                transaccionDWG.Commit();
                            }
                        }
                        documentoDWG.CloseAndSave(archivo.FullName);
                    }
                    catch (Exception ex)
                    {
                        // Ignorar el archivo que ha causado el error y seguir con el siguiente.
                        //TODO _pendiente: es conveniente dejar registro en un log 
                    }
                }
            }
            //(*) setVariableSDI(false);
        }



        private static LayerTableRecord obtenerOCrearCapaParaAtributos(Autodesk.AutoCAD.DatabaseServices.Database datosDWG, Autodesk.AutoCAD.DatabaseServices.Transaction transaccionDWG)
        {
            LayerTableRecord capaParaAtributos;
            using (LayerTable tablaDeCapas = (LayerTable)transaccionDWG.GetObject(datosDWG.LayerTableId, OpenMode.ForRead))
            {
                if (tablaDeCapas.Has("0"))
                {
                    datosDWG.Clayer = tablaDeCapas["0"];
                }
                if (tablaDeCapas.Has("MICAPA_PARA_ATRIBUTOS"))
                {
                    capaParaAtributos = (LayerTableRecord)transaccionDWG.GetObject(tablaDeCapas["MICAPA_PARA_ATRIBUTOS"], OpenMode.ForWrite);
                    capaParaAtributos.IsLocked = false;
                }
                else
                {
                    capaParaAtributos = new LayerTableRecord();
                    capaParaAtributos.Name = "MICAPA_PARA_ATRIBUTOS";
                    capaParaAtributos.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 40);
                    tablaDeCapas.UpgradeOpen();
                    ObjectId idCapaParaAtributos = tablaDeCapas.Add(capaParaAtributos);
                    transaccionDWG.AddNewlyCreatedDBObject(capaParaAtributos, add: true);
                }
            }

            return capaParaAtributos;
        }


        private static void ponerAtributos(Articulo datosDelArticulo, LayerTableRecord capaParaAtributos, Autodesk.AutoCAD.DatabaseServices.Database datosDWG, Autodesk.AutoCAD.DatabaseServices.Transaction transaccionDWG)
        {
            const String ATRIBUTO_CODIGO_ARTICULO = "CODIGO_ARTICULO";
            const String ATRIBUTO_NOMBRE_ES = "NOMBRE_ES";
            const String ATRIBUTO_NOMBRE_EN = "NOMBRE_EN";
            bool estaPendientePonerCODIGO_ARTICULO = true;
            bool estaPendientePonerNOMBRE_ES = true;
            bool estaPendientePonerNOMBRE_EN = true;
            using (BlockTable tabla = (BlockTable)transaccionDWG.GetObject(datosDWG.BlockTableId, OpenMode.ForWrite))
            {
                ObjectId idEspacioModelo = tabla[BlockTableRecord.ModelSpace];
                using (BlockTableRecord espacioModelo = (Autodesk.AutoCAD.DatabaseServices.BlockTableRecord)transaccionDWG.GetObject(idEspacioModelo, OpenMode.ForWrite))
                {
                    foreach (ObjectId idElemento in espacioModelo)
                    {
                        using (DBObject elemento = transaccionDWG.GetObject(idElemento, OpenMode.ForWrite))
                        {
                            if (elemento.GetType().Name == "AttributeDefinition")
                            {
                                using (AttributeDefinition definicionDeAtributo = (AttributeDefinition)elemento)
                                {
                                    switch (definicionDeAtributo.Tag)
                                    {
                                        case ATRIBUTO_CODIGO_ARTICULO:
                                            estaPendientePonerCODIGO_ARTICULO = false;
                                            definicionDeAtributo.TextString = datosDelArticulo.getCodigoArticulo();
                                            definicionDeAtributo.Prompt = ATRIBUTO_CODIGO_ARTICULO;
                                            definicionDeAtributo.Position = new Autodesk.AutoCAD.Geometry.Point3d(0, -10, 0);
                                            ajustarOtrasPropiedadesDelAtributo(definicionDeAtributo);
                                            break;
                                        case ATRIBUTO_NOMBRE_ES:
                                            estaPendientePonerNOMBRE_ES = false;
                                            definicionDeAtributo.TextString = datosDelArticulo.getNombreArticulo_ES_();
                                            definicionDeAtributo.Prompt = ATRIBUTO_NOMBRE_ES;
                                            definicionDeAtributo.Position = new Autodesk.AutoCAD.Geometry.Point3d(0, -20, 0);
                                            ajustarOtrasPropiedadesDelAtributo(definicionDeAtributo);
                                            break;
                                        case ATRIBUTO_NOMBRE_EN:
                                            estaPendientePonerNOMBRE_EN = false;
                                            definicionDeAtributo.TextString = datosDelArticulo.getNombreArticulo_EN_();
                                            definicionDeAtributo.Prompt = ATRIBUTO_NOMBRE_EN;
                                            definicionDeAtributo.Position = new Autodesk.AutoCAD.Geometry.Point3d(0, -20, 0);
                                            ajustarOtrasPropiedadesDelAtributo(definicionDeAtributo);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                
                    if (estaPendientePonerCODIGO_ARTICULO)
                    {
                        using (AttributeDefinition definicionDeAtributo = new AttributeDefinition())
                        {
                            definicionDeAtributo.Tag = ATRIBUTO_CODIGO_ARTICULO;
                            definicionDeAtributo.TextString = datosDelArticulo.getCodigoArticulo();
                            definicionDeAtributo.Prompt = ATRIBUTO_CODIGO_ARTICULO;
                            definicionDeAtributo.Position = new Autodesk.AutoCAD.Geometry.Point3d(0, -10, 0);
                            ajustarOtrasPropiedadesDelAtributo(definicionDeAtributo);
                            ObjectId instanciaDeAtributo = espacioModelo.AppendEntity(definicionDeAtributo);
                            definicionDeAtributo.Layer = capaParaAtributos.Name;
                        }
                    }
                    if (estaPendientePonerNOMBRE_ES)
                    {
                        using (AttributeDefinition definicionDeAtributo = new AttributeDefinition())
                        {
                            definicionDeAtributo.Tag = ATRIBUTO_NOMBRE_ES;
                            definicionDeAtributo.TextString = datosDelArticulo.getNombreArticulo_ES_();
                            definicionDeAtributo.Prompt = ATRIBUTO_NOMBRE_ES;
                            definicionDeAtributo.Position = new Autodesk.AutoCAD.Geometry.Point3d(0, -20, 0);
                            ajustarOtrasPropiedadesDelAtributo(definicionDeAtributo);
                            ObjectId instanciaDeAtributo = espacioModelo.AppendEntity(definicionDeAtributo);
                            definicionDeAtributo.Layer = capaParaAtributos.Name;
                        }
                    }
                    if (estaPendientePonerNOMBRE_EN)
                    {
                        using (AttributeDefinition definicionDeAtributo = new AttributeDefinition())
                        {
                            definicionDeAtributo.Tag = ATRIBUTO_NOMBRE_EN;
                            definicionDeAtributo.TextString = datosDelArticulo.getNombreArticulo_EN_();
                            definicionDeAtributo.Prompt = ATRIBUTO_NOMBRE_EN;
                            definicionDeAtributo.Position = new Autodesk.AutoCAD.Geometry.Point3d(0, -20, 0);
                            ajustarOtrasPropiedadesDelAtributo(definicionDeAtributo);
                            ObjectId instanciaDeAtributo = espacioModelo.AppendEntity(definicionDeAtributo);
                            definicionDeAtributo.Layer = capaParaAtributos.Name;
                        }
                    }

                }
            }
        }

        private static void ajustarOtrasPropiedadesDelAtributo(AttributeDefinition definicionDeAtributo)
        {
            // .Invisible es para que no se vea el atributo en el propio dibujo
            definicionDeAtributo.Invisible = true;
            // .Visible es para que si se muestre el atributo al usuario en las propiedades del bloque
            definicionDeAtributo.Visible = true;
            definicionDeAtributo.LockPositionInBlock = true;
            definicionDeAtributo.Height = 20;
            definicionDeAtributo.Constant = false;
        }



        private static void setVariableSDI(Boolean estaActivadoSingleDocumentInterface)
        {
            if (estaActivadoSingleDocumentInterface)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("SDI", 1);
            }
            else
            {
                Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("SDI", 0);
            }
        }


    }
}
