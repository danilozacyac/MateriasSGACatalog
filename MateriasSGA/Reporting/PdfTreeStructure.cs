﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using MateriasSGA.Dto;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MateriasSGA.Reporting
{
    public class PdfTreeStructure
    {
        private iTextSharp.text.Document myDocument;

        public void GenerateTreeStructure(ObservableCollection<Materia> listaMaterias)
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 60);

            string filePath = Path.GetTempFileName() + ".pdf";

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filePath, FileMode.Create));
                //HeaderFooter pdfPage = new HeaderFooter();
                //writer.PageEvent = pdfPage;
                //writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                ////Paragraph para;

                myDocument.Open();

                this.PrintTree(listaMaterias, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                myDocument.Close();
                System.Diagnostics.Process.Start(filePath);
            }

        }

        public void PrintTree(ObservableCollection<Materia> materias, int indent)
        {
            foreach (Materia mat in materias)
            {

                Paragraph para = new Paragraph(mat.Descripcion, Fuentes.ContenidoCelda);
                para.IndentationLeft = indent;

                myDocument.Add(para);

                this.PrintTree(mat.MateriasHijas, indent + 15);

            }


        }

    }
}
