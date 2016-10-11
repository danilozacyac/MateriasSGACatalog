using System;
using System.Collections.Generic;
using System.Linq;
using CatalogoSga.Dto;
using CatalogoSga.Model;

namespace CatalogoSga.Singletons
{
    public class ClasificacionSingleton
    {

        private static List<ClasificacionSga> clasificacion;

        private ClasificacionSingleton() { }

        public static List<ClasificacionSga> Clasificacion
        {
            get
            {
                if (clasificacion == null)
                    clasificacion = new ClasificacionSgaModel().GetClasificacion(-1);

                return clasificacion;
            }
        }
    }
}
