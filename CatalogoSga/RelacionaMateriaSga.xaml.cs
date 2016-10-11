using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CatalogoSga.Dto;
using CatalogoSga.Model;

namespace CatalogoSga
{
    /// <summary>
    /// Lógica de interacción para RelacionaMateriaSga.xaml
    /// </summary>
    public partial class RelacionaMateriaSga : Window
    {

        private List<int> idMaterias;
        List<ClasificacionSga> listaMaterias;

        private readonly int ius;
        private readonly int volumen;
        private readonly bool isUpdatable;

        public static  string textoDeLasMaterias;
        private string procede = String.Empty;

        /// <summary>
        /// Permite obtener el árbol de las materia seleccionadas en forma de texto
        /// </summary>
        /// <param name="textoDeLasMaterias"></param>
        /// <param name="procede">Indica el programa que hace el llamado a esta ventana</param>
        public RelacionaMateriaSga(string procede)
        {
            InitializeComponent();
            this.procede = procede;
            this.isUpdatable = true;
        }

        /// <summary>
        /// Permite relacionar una o más materias del Catálogo de SGA con un tesis
        /// </summary>
        /// <param name="ius">Registro digital de la tesis a relacionar</param>
        /// <param name="volumen">Volumen del Semanario al que pertenece la tesis</param>
        /// <param name="isUpdatable">Indica si la vista es o no de solo lectura</param>
        public RelacionaMateriaSga(int ius, int volumen, bool isUpdatable)
        {
            InitializeComponent();
            this.ius = ius;
            this.volumen = volumen;
            this.isUpdatable = isUpdatable;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.idMaterias = new List<int>();

            listaMaterias = ClasificacionSga.CreateMateriasTree(isUpdatable);
            MateriasTree.DataContext = listaMaterias;
            SetSeleccionados(listaMaterias, new ClasificacionSgaModel().GetMateriasRelacionadas(ius, volumen));

            List<int> permisos = (List<int>)this.Tag;

            if (!isUpdatable || !permisos.Contains(4))
            {
                BtnQuitar.Visibility = Visibility.Hidden;
                BtnSalvar.Visibility = Visibility.Hidden;
                BtnSalir.Content = "Cerrar";
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (isUpdatable)
            {
                if (procede.Equals("Listado"))
                {
                    GetSeleccionadosItems(((ClasificacionSga)MateriasTree.Items[0]).SubClasificaciones);

                    if (elementosSeleccionados.Count > 0)
                    {
                        elementosSeleccionados.Reverse();
                        foreach (ClasificacionSga item in elementosSeleccionados)
                        {
                            this.GetMateriasTreeString(item);
                            textoDeLasMaterias += "\n\n";
                        }
                    }

                    textoDeLasMaterias = textoDeLasMaterias.Replace("Todas\n", "");
                    this.Close();
                }
                else
                {
                    GetSeleccionados(((ClasificacionSga)MateriasTree.Items[0]).SubClasificaciones);

                    if (idMaterias.Count == 0)
                    {
                        MessageBox.Show("Seleccione al menos un tema con el cual relacionar la tesis, de los contrario oprima cancelar");
                        return;
                    }
                    else
                    {
                        new ClasificacionSgaModel().SetRelacionMateriasIus(ius, idMaterias, volumen);
                        MessageBox.Show("Esta tesis fue relacionada con " + idMaterias.Count.ToString() + ((idMaterias.Count == 1) ? " tema" : " temas"));
                        idMaterias.Clear();

                        DialogResult = true;
                        this.Close();
                    }
                }
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void GetSeleccionados(List<ClasificacionSga> items)
        {
            foreach (ClasificacionSga item in items)
            {
                if (item.IsChecked == true)
                    idMaterias.Add(item.IdClasificacion);

                GetSeleccionados(item.SubClasificaciones);
            }
        }

        List<ClasificacionSga> elementosSeleccionados = new List<ClasificacionSga>();
        private void GetSeleccionadosItems(List<ClasificacionSga> items)
        {
            foreach (ClasificacionSga item in items)
            {
                if (item.IsChecked == true)
                    elementosSeleccionados.Add(item);

                GetSeleccionadosItems(item.SubClasificaciones);
            }
        }

        private void GetMateriasTreeString(ClasificacionSga item)
        {
            textoDeLasMaterias = this.GetTabNumber(item.Nivel) + item.Descripcion + "\n" + textoDeLasMaterias;

            if (!item.Descripcion.Contains("Todas"))
                GetMateriasTreeString(item.GetParent());
        }

        private void SetSeleccionados(List<ClasificacionSga> items, List<int> matSeleccionadas)
        {
            foreach (ClasificacionSga item in items)
            {
                if (matSeleccionadas.Contains(item.IdClasificacion))
                    item.IsChecked = true;

                SetSeleccionados(item.SubClasificaciones, matSeleccionadas);
            }
        }

        private string GetTabNumber(int nivel)
        {
            string tabs = String.Empty;
            for (int x = 1; x <= nivel; x++)
            {
                tabs += "\t";
            }
            return tabs;
        }
    }
}
