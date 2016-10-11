using System;
using System.Linq;
using MantesisVerIusCommonObjects.Dto;

namespace CatalogoSga.Dto
{
    public class Tesis : TesisDto
    {
        private string salaStr;
        private string fuenteStr;
        private string tatjStr;
        private string materia1Str;
        private string materia2Str;
        private string materia3Str;
        private string materia4Str;
        private string materia5Str;
        private Int64 idClasificacionSga;
       
        public string SalaStr
        {
            get
            {
                return this.salaStr;
            }
            set
            {
                this.salaStr = value;
            }
        }

        public string FuenteStr
        {
            get
            {
                return this.fuenteStr;
            }
            set
            {
                this.fuenteStr = value;
            }
        }

        public string TatjStr
        {
            get
            {
                return this.tatjStr;
            }
            set
            {
                this.tatjStr = value;
            }
        }

        public string Materia1Str
        {
            get
            {
                return this.materia1Str;
            }
            set
            {
                this.materia1Str = value;
            }
        }

        public string Materia2Str
        {
            get
            {
                return this.materia2Str;
            }
            set
            {
                this.materia2Str = value;
            }
        }

        public string Materia3Str
        {
            get
            {
                return this.materia3Str;
            }
            set
            {
                this.materia3Str = value;
            }
        }

        public string Materia4Str
        {
            get
            {
                return this.materia4Str;
            }
            set
            {
                this.materia4Str = value;
            }
        }

        public string Materia5Str
        {
            get
            {
                return this.materia5Str;
            }
            set
            {
                this.materia5Str = value;
            }
        }

        public Int64 IdClasificacionSga
        {
            get
            {
                return this.idClasificacionSga;
            }
            set
            {
                this.idClasificacionSga = value;
            }
        }
    }
}
