﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CatalogoSga.Dto;
using CatalogoSga.Singletons;
using ScjnUtilities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CatalogoSga.Reportes
{
    public class PdfTreeStructure
    {
        private iTextSharp.text.Document myDocument;

        public void GenerateTreeStructure()
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 60);

            string filePath = Path.GetTempFileName() + ".pdf";

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filePath, FileMode.Create));
                HeaderFooter pdfPage = new HeaderFooter();
                writer.PageEvent = pdfPage;
                //writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                ////Paragraph para;

                myDocument.Open();

                this.PrintTree(ClasificacionSingleton.Clasificacion, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfTreeStructure", "MateriasSGA");
            }
            finally
            {
                myDocument.Close();
                System.Diagnostics.Process.Start(filePath);
            }

        }

        public void PrintTree(List<ClasificacionSga> materias, int indent)
        {
            foreach (ClasificacionSga mat in materias)
            {

                Paragraph para = new Paragraph(mat.Descripcion, Fuentes.ContenidoCelda);
                para.IndentationLeft = indent;

                myDocument.Add(para);

                this.PrintTree(mat.SubClasificaciones, indent + 15);

            }


        }

    }
}
