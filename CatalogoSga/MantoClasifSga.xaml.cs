using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CatalogoSga.Dto;
using CatalogoSga.Model;
using CatalogoSga.Reportes;
using Infragistics.Windows.DataPresenter;

namespace CatalogoSga
{
    /// <summary>
    /// Lógica de interacción para MantoClasifSga.xaml
    /// </summary>
    public partial class MantoClasifSga : UserControl
    {
        private bool registroNuevo = false;

        public MantoClasifSga()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Materias.DataContext = new ClasificacionSgaModel().GetClasificacion();
        }

        private void Materias_RecordUpdated(object sender, Infragistics.Windows.DataPresenter.Events.RecordUpdatedEventArgs e)
        {
            try
            {
                DataRecord myRecord = (DataRecord)e.Record;
                // Display the selected Records values in the appropriate 
                // editor

                ClasificacionSga materia = new ClasificacionSga(myRecord.Cells[3].Value.ToString());

                materia.IdClasificacion = Convert.ToInt32(myRecord.Cells[0].Value);
                materia.Nivel = Convert.ToInt32(myRecord.Cells[1].Value);
                materia.Padre = Convert.ToInt32(myRecord.Cells[2].Value);
                materia.Descripcion = myRecord.Cells[3].Value.ToString();
                materia.SeccionPadre = Convert.ToInt32(myRecord.Cells[4].Value);
                materia.Historica = Convert.ToInt32(myRecord.Cells[5].Value);
                materia.Consec = Convert.ToInt32(myRecord.Cells[6].Value);
                materia.Hoja = Convert.ToInt32(myRecord.Cells[7].Value);
                materia.NvlImpresion = Convert.ToInt32(myRecord.Cells[8].Value);


                if (registroNuevo)
                    new ClasificacionSgaModel().InsertarRegistro(materia);
                else
                    new ClasificacionSgaModel().ActualizaRegistroMaterias(materia);

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

        public void ReasignarConsecutivo()
        {
            ClasificacionSgaModel materiasMod = new ClasificacionSgaModel();
            materiasMod.UpdateOrderNumber();

            UserControl_Loaded(null, null);
        }

        public void ImprimeEstructuraPdf()
        {
            List<ClasificacionSga> materias = new ClasificacionSgaModel().GetClasificacion(-1);

            PdfTreeStructure pdf = new PdfTreeStructure();
            pdf.GenerateTreeStructure(materias);
        }

        public void ImprimeEstructuraWord()
        {
            List<ClasificacionSga> materias = new ClasificacionSgaModel().GetClasificacion(-1);

            WordTreeStructure pdf = new WordTreeStructure();
            pdf.CreateDocument(materias);
        }
    }
}
