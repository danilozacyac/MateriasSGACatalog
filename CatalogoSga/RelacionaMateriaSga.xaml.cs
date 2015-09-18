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

        private void SetSeleccionados(List<ClasificacionSga> items, List<int> matSeleccionadas)
        {
            foreach (ClasificacionSga item in items)
            {
                if (matSeleccionadas.Contains(item.IdClasificacion))
                    item.IsChecked = true;

                SetSeleccionados(item.SubClasificaciones, matSeleccionadas);
            }
        }
    }
}
