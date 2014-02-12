using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MateriasSGA.Dto
{
    public class Materia
    {
        private int materiaInt;
        private int nivel;
        private int padre;
        private String descripcion;
        private int seccionPadre;
        private int historica;
        private int consec;
        private int hoja;
        private int nvlImpresion;

        public int MateriaInt
        {
            get
            {
                return this.materiaInt;
            }
            set
            {
                this.materiaInt = value;
            }
        }

        public int Nivel
        {
            get
            {
                return this.nivel;
            }
            set
            {
                this.nivel = value;
            }
        }

        public int Padre
        {
            get
            {
                return this.padre;
            }
            set
            {
                this.padre = value;
            }
        }

        public String Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
            }
        }

        public int SeccionPadre
        {
            get
            {
                return this.seccionPadre;
            }
            set
            {
                this.seccionPadre = value;
            }
        }

        public int Historica
        {
            get
            {
                return this.historica;
            }
            set
            {
                this.historica = value;
            }
        }

        public int Consec
        {
            get
            {
                return this.consec;
            }
            set
            {
                this.consec = value;
            }
        }

        public int Hoja
        {
            get
            {
                return this.hoja;
            }
            set
            {
                this.hoja = value;
            }
        }

        public int NvlImpresion
        {
            get
            {
                return this.nvlImpresion;
            }
            set
            {
                this.nvlImpresion = value;
            }
        }
    }
}
