using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MateriasSGA.Model;
using Infragistics.Windows.DataPresenter;
using MateriasSGA.Dto;
using System.Collections.ObjectModel;
using MateriasSGA.Reporting;

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
            Materias.DataContext = new MateriasModel().GetTodaslasMaterias();
        }

        private void Materias_RecordUpdated(object sender, Infragistics.Windows.DataPresenter.Events.RecordUpdatedEventArgs e)
        {
            try
            {
                Materia materia = new Materia();

                DataRecord myRecord = (DataRecord)e.Record;
                // Display the selected Records values in the appropriate 
                // editor

                materia.MateriaInt = Convert.ToInt32(myRecord.Cells[0].Value);
                materia.Nivel = Convert.ToInt32(myRecord.Cells[1].Value);
                materia.Padre = Convert.ToInt32(myRecord.Cells[2].Value);
                materia.Descripcion = myRecord.Cells[3].Value.ToString();
                materia.SeccionPadre = Convert.ToInt32(myRecord.Cells[4].Value);
                materia.Historica = Convert.ToInt32(myRecord.Cells[5].Value);
                materia.Consec = Convert.ToInt32(myRecord.Cells[6].Value);
                materia.Hoja = Convert.ToInt32(myRecord.Cells[7].Value);
                materia.NvlImpresion = Convert.ToInt32(myRecord.Cells[8].Value);


                if (registroNuevo)
                    new MateriasModel().InsertarRegistro(materia);
                else
                    new MateriasModel().ActualizaRegistroMaterias(materia);

                registroNuevo = false;
            }
            catch (InvalidCastException)
            {
                e.Record.Visibility = System.Windows.Visibility.Collapsed;
                e.Record.CancelUpdate();
                MessageBox.Show("Debe completar todos los campos para poder ingresar un elemento", "Atención:", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
        }

        private void Materias_RecordAdded(object sender, Infragistics.Windows.DataPresenter.Events.RecordAddedEventArgs e)
       {
           registroNuevo = true;
        }

        private void DaOrden_Click(object sender, RoutedEventArgs e)
        {
            MateriasModel materiasMod = new MateriasModel();
            materiasMod.UpdateOrderNumber();

            this.Window_Loaded(null, null);
        }

        private void GeneraPdfArbol_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Materia> materias = new MateriasModel().GetMateriasForReport(-1);

            PdfTreeStructure pdf = new PdfTreeStructure();
            pdf.GenerateTreeStructure(materias);
        }

        

        

        
    }
}
