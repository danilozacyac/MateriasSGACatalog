using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CatalogoSga.Dto;
using CatalogoSga.Model;
using CatalogoSga.Singletons;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Utilities;

namespace CatalogoSga
{
    /// <summary>
    /// Lógica de interacción para TesisRelacionadas.xaml
    /// </summary>
    public partial class TesisRelacionadas : UserControl
    {
        private List<ClasificacionSga> listaMaterias;

        private List<Tesis> tesisRel;

        private ClasificacionSga selectedClasif;

        public TesisRelacionadas()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            listaMaterias = ClasificacionSingleton.Clasificacion;
            MateriasTree.DataContext = listaMaterias;

            Volumen dummyVol = new Volumen();
            dummyVol.Volumenes = 0;
            dummyVol.VolumenTxt = "Todas";

            List<Volumen> volumenes = Utils.GetVolumenesEpoca(100);
            volumenes.Insert(0, dummyVol);

            CbxFiltroVol.DataContext = volumenes;

        }

        private void MateriasTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxFiltroVol.SelectedIndex > 1)
            {
                CbxFiltroVol.SelectedIndex = 0;
            }

            selectedClasif = MateriasTree.SelectedItem as ClasificacionSga;

            tesisRel = new RelacionesModel().GetTesisRelacionadas(selectedClasif.IdClasificacion);

            GTesisRel.DataContext = tesisRel;

            LblTotal.Content = GTesisRel.Items.Count + " tesis relacionadas"; 
        }

        private void CbxFiltroVol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Volumen selectedVol = CbxFiltroVol.SelectedItem as Volumen;

            if (selectedVol.Volumenes > 0)
            {
                GTesisRel.DataContext = (from n in tesisRel
                                         where n.VolumenInt == selectedVol.Volumenes
                                         select n).ToList();
                LblTotal.Content = GTesisRel.Items.Count + " tesis relacionadas en el " + selectedVol.VolumenTxt;
            }
            else
            {
                GTesisRel.DataContext = tesisRel;

                LblTotal.Content = GTesisRel.Items.Count + " tesis relacionadas";
            }
        }
    }
}
