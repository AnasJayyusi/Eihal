using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using Eihal.Dto;

namespace Eihal.Tools
{
    public class PdfReportGenerator
    {

        public MemoryStream GenerateReport(string imagePath, MasterDetailsDto masterDetails ,bool includeMasterDetails = true)
        {
            // Prepare Stream
            MemoryStream workStream = new MemoryStream();

            // Prepare Document to write on it
            Document doc = new Document(PageSize.A4);
            PdfPTable tableLayout = new PdfPTable(4);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            // Create Header Logo + Master Details
            AddLogoImage(doc, imagePath);
            AddLineSpace(doc);
            if (includeMasterDetails)
            {
                BuildMasterDetails(doc, masterDetails);
                AddLineSpace(doc);
            }

            // Prepare For Table
            Paragraph tableParagraph = new Paragraph();
            tableParagraph.SpacingAfter = 12;
            doc.Add(tableParagraph);


            // Filling Table 
            doc.Add(BuildTable(tableLayout));
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return workStream;
        }


        private void AddLogoImage(Document doc, string imagePath)
        {
            // Load your image
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
            //Resize image depend upon your need
            jpg.ScaleToFit(140f, 120f);
            //Give space before image
            jpg.SpacingBefore = 10f;
            //Give some space after the image
            jpg.SpacingAfter = 2f;
            jpg.Alignment = Element.ALIGN_CENTER;
            doc.Add(jpg);
        }
        private void BuildMasterDetails(Document doc, MasterDetailsDto masterDetails)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //ToDo : Should Have I Place In Same Project 
            string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Tahoma.ttf";
            BaseFont bf = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new Font(bf, 12);

            // It working with arabic 
            PdfPTable table = new PdfPTable(1); // a table with 1 cell
            table.WidthPercentage = 100;
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL; // can also be set on the cell

            // Clinic Name
            PdfPCell clinicNameCell = new PdfPCell(new Phrase(string.Format($"{masterDetails.ClinicName}:Clinic Name"), font));
            clinicNameCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            clinicNameCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(clinicNameCell);


            // Invoice Number Cell
            PdfPCell invoiceNoCell = new PdfPCell(new Phrase(string.Format($"Invoice No.{masterDetails.OrderNo} "), font));
            invoiceNoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            invoiceNoCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(invoiceNoCell);


            // Date 
            PdfPCell dateCell = new PdfPCell(new Phrase(string.Format($"Date : {masterDetails.OrderDate} "), font));
            dateCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            dateCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(dateCell);


            // Doctor Name 
            PdfPCell doctorCell = new PdfPCell(new Phrase(string.Format($"Doctor Name:{masterDetails.DoctorName}"), font));
            doctorCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            doctorCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(doctorCell);


            // Request From 
            PdfPCell requestFromCell = new PdfPCell(new Phrase(string.Format($"Request From:{masterDetails.RequestFrom}"), font));
            requestFromCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            requestFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(requestFromCell);


            // Patient's Name
            PdfPCell PatientNameCell = new PdfPCell(new Phrase(string.Format($"Patient's Name:{masterDetails.PatientName}"), font));
            PatientNameCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            PatientNameCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(PatientNameCell);



            doc.Add(table);


        }
        protected PdfPTable BuildTable(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            var count = 1;
            //Add header  
            AddCellToHeader(tableLayout, "CustomerName");
            AddCellToHeader(tableLayout, "Address");
            AddCellToHeader(tableLayout, "Email");
            AddCellToHeader(tableLayout, "ZipCode");

            foreach (var cust in customerData())
            {
                if (count >= 1)
                {
                    //Add body  
                    AddCellToBody(tableLayout, cust.CustomerName.ToString(), count);
                    AddCellToBody(tableLayout, cust.Address.ToString(), count);
                    AddCellToBody(tableLayout, cust.Email.ToString(), count);
                    AddCellToBody(tableLayout, cust.ZipCode.ToString(), count);
                    count++;
                }
            }
            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }

        private static void AddCellToBody(PdfPTable tableLayout, string cellText, int count)
        {
            if (count % 2 == 0)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }
            else
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211)
                });
            }
        }

        public List<Customer> customerData()
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer(){ CustomerName="Gnanavel Sekar",Address="Surat",Email="GnanavelSekar@gmail.com",ZipCode="395003" },
                new Customer(){ CustomerName="Subash S",Address="Ahemdabad",Email="SubashS@gmail.com",ZipCode="395006" },
                new Customer(){ CustomerName="Robert A",Address="Surat",Email="RobertA@gmail.com",ZipCode="395005" },
                new Customer(){ CustomerName="Ammaiyappan",Address="Vadodara",Email="Ammaiyappan@gmail.com",ZipCode="395004" },
                new Customer(){ CustomerName="Huijoyan",Address="Surat",Email="Huijoyan@gmail.com",ZipCode="395008" },
            };
            return customers;
        }


        private static void AddLineSpace(Document document)
        {
            document.Add(new Paragraph(" "));
        }


    }
}
