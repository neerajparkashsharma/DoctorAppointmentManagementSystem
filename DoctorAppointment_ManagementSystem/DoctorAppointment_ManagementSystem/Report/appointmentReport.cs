using DoctorAppointment_ManagementSystem.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DoctorAppointment_ManagementSystem.Report
{
    public class appointmentReport
    {
        #region Declaration
        int total_col = 2;
        Document document;
        Font _fontStyle;
        PdfPTable _pdftable = new PdfPTable(7);

        PdfPCell _pdfCell;
        MemoryStream memorystream = new MemoryStream();
        List<appointment> _app = new List<appointment>();
        #endregion
        public byte[] prepareReport(List<appointment> app)
        {
            _app = app;

            #region 
            document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.A4);
            document.SetMargins(20f, 20f, 20f, 20f);
            _pdftable.WidthPercentage = 100;
            _pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("tahoma", 8f, 1);
            PdfWriter.GetInstance(document, memorystream);
            document.Open();
            _pdftable.SetWidths(new float[] { 20f, 150f, 100f });

            #endregion

            this.ReportHeader();
            this.ReportBody();
            _pdftable.HeaderRows = 2;
            document.Add(_pdftable);
            document.Close();
            return memorystream.ToArray();
        }

        private void ReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfCell = new PdfPCell(new Phrase("DOCTORS INN", _fontStyle));
            _pdfCell.Colspan = total_col;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdftable.AddCell(_pdfCell);
            _pdftable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Appointments List", _fontStyle));
            _pdfCell.Colspan = total_col;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdftable.AddCell(_pdfCell);
            _pdftable.CompleteRow();

        }

        private void ReportBody()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Appointments List", _fontStyle));
            _pdfCell.Colspan = total_col;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdftable.AddCell(_pdfCell);
            _pdftable.CompleteRow();


        }

    }
}
