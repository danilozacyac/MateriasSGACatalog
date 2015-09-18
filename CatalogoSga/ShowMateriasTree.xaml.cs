using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CatalogoSga.Dto;
using CatalogoSga.Model;

namespace CatalogoSga
{
    /// <summary>
    /// Lógica de interacción para ShowMateriasTree.xaml
    /// </summary>
    public partial class ShowMateriasTree : UserControl
    {
        private List<ClasificacionSga> listaMaterias;

        public ShowMateriasTree()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            listaMaterias = new ClasificacionSgaModel().GetClasificacion(-1);
            MateriasTree.DataContext = listaMaterias;
           
        }
    }
}
