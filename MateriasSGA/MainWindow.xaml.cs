using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CatalogoSga;
using CatalogoSga.Dto;
using CatalogoSga.Model;
using CatalogoSga.Reportes;

namespace MateriasSGA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool registroNuevo = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Materias.DataContext = new MateriasModel().GetTodaslasMaterias();

            RelacionaMateriaSga wiun = new RelacionaMateriaSga(2000334, 8783, false);
            wiun.Show();

        }

        

        

        private void GeneraPdfArbol_Click(object sender, RoutedEventArgs e)
        {
            List<ClasificacionSga> materias = new ClasificacionSgaModel().GetClasificacion(-1);

            PdfTreeStructure pdf = new PdfTreeStructure();
            pdf.GenerateTreeStructure(materias);
        }

        

        

        
    }
}
