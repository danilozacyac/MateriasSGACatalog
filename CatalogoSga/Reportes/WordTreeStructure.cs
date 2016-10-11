using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using CatalogoSga.Dto;
using CatalogoSga.Singletons;
using ScjnUtilities;

namespace CatalogoSga.Reportes
{
    public class WordTreeStructure
    {

        Microsoft.Office.Interop.Word.Application winword;
        Microsoft.Office.Interop.Word.Document document;

        //Create a missing variable for missing value
        object missing = System.Reflection.Missing.Value;

        public void CreateDocument()
        {
            string filePath = Path.GetTempFileName() + ".docx";
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

                this.PrintTree(ClasificacionSingleton.Clasificacion, 0);

                foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
                {
                    Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdGray50;
                    footerRange.Font.Size = 12;
                    footerRange.Text = DateTimeUtilities.ToLongDateFormat(DateTime.Now);
                }


                //Save the document
                object filename = filePath;
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Estructura generada satisfactoriamente!");

                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfTreeStructure", "MateriasSGA");
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
