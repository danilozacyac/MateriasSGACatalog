using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CatalogoSga.Model;

namespace CatalogoSga.Dto
{
    public class ClasificacionSga: INotifyPropertyChanged
    {
        #region Data

        bool? isChecked = false;
        ClasificacionSga parent;

        #endregion // Data

        bool isReadOnly = false;
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly = value;
            }
        }

        #region CreateMateriasTree

        public static List<ClasificacionSga> CreateMateriasTree(bool isReadOnly)
        {
            List<ClasificacionSga> listaMaterias = new ClasificacionSgaModel().GetEstructuraNivel(-1,isReadOnly);

            ClasificacionSga root = new ClasificacionSga("Todas")
            {
                IsInitiallySelected = true,
                SubClasificaciones = listaMaterias
            };

            root.Initialize();
            return new List<ClasificacionSga> { root };
        }

        public ClasificacionSga(string name)
        {
            this.Descripcion = name;

            this.SubClasificaciones = new List<ClasificacionSga>();
        }

        public ClasificacionSga(string name, List<ClasificacionSga> hijos, int idClasificacion)
        {
            this.Descripcion = name;
            this.SubClasificaciones = hijos;
            this.IdClasificacion = idClasificacion;
        }

        void Initialize()
        {
            foreach (ClasificacionSga child in this.SubClasificaciones)
            {
                child.parent = this;

                child.Initialize();
            }
        }

        #endregion

        #region Properties

        public List<ClasificacionSga> SubClasificaciones { get; private set; }

        public bool IsInitiallySelected { get; private set; }

        public string Descripcion { get; set; }

        public int IdClasificacion { get; set; }

        public int Nivel { get; set; }

        public int Padre { get; set; }

        public int SeccionPadre { get; set; }

        public int Historica { get; set; }

        public int Consec { get; set; }

        public int Hoja { get; set; }

        public int NvlImpresion { get; set; }

        #region IsChecked

        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child FooViewModels.  Setting this property to true or false
        /// will set all children to the same check state, and setting it 
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                this.SetIsChecked(value, true, true);
            }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == isChecked)
                return;

            isChecked = value;

            if (updateChildren && isChecked.HasValue)
                this.SubClasificaciones.ForEach(c => c.SetIsChecked(isChecked, true, false));

            if (updateParent && parent != null)
                parent.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.SubClasificaciones.Count; ++i)
            {
                bool? current = this.SubClasificaciones[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        #endregion // IsChecked

        #endregion // Properties

        #region INotifyPropertyChanged Members

        void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

