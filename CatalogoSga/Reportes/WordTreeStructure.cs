using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CatalogoSga.Dto;

namespace CatalogoSga.Reportes
{
    public class WordTreeStructure
    {

        Microsoft.Office.Interop.Word.Application winword;
        Microsoft.Office.Interop.Word.Document document;

        //Create a missing variable for missing value
        object missing = System.Reflection.Missing.Value;

        public void CreateDocument(List<ClasificacionSga> listaMaterias)
        {
            try
            {
                //Create an instance for word app
                winword = new Microsoft.Office.Interop.Word.Application();

                //Set animation status for word application
                //winword. ShowAnimation = false;

                //Set status for word application is to be visible or not.
                winword.Visible = false;



                //Create a new document
                document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                this.PrintTree(listaMaterias, 0);

                //Save the document
                object filename = @"C:\ModuloIntercomunicacionBD\temp1.docx";
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Document created successfully !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PrintTree(List<ClasificacionSga> materias, int indent)
        {
            foreach (ClasificacionSga mat in materias)
            {
                Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                //object styleHeading1 = "Heading 1";
                //para1.Range.set_Style(ref styleHeading1);
                para1.Range.Text = mat.Descripcion;
                para1.LeftIndent = indent;

                para1.Range.InsertParagraphAfter();
                this.PrintTree(mat.SubClasificaciones, indent + 15);

            }


        }
    }
}
