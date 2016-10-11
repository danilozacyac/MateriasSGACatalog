using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CatalogoSga.Dto;
using CatalogoSga.Singletons;
using CatalogoSga.Model;

namespace CatalogoSga
{
    /// <summary>
    /// Lógica de interacción para ShowMateriasTree.xaml
    /// </summary>
    public partial class ShowMateriasTree : UserControl
    {
        private List<ClasificacionSga> listaMaterias;
        ClasificacionSga selectedClasificacion;

        public ShowMateriasTree()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            listaMaterias = ClasificacionSingleton.Clasificacion;
            MateriasTree.DataContext = listaMaterias;
           
        }

        private void MateriasTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClasificacion = MateriasTree.SelectedItem as ClasificacionSga;
        }


        public void DeleteMateria()
        {
            MessageBoxResult result = MessageBox.Show("¿Estas segur@ que quieres eliminar esta materia?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                new ClasificacionSgaModel().EliminaMateria(selectedClasificacion.IdClasificacion);
            }
        }
    }
}
