using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace AgricultureAnalytics
{
    public partial class ChartForm : Form
    {
        private DataTable dataTable;

        public ChartForm(DataTable dataTable)
        {
            InitializeComponent();
            this.dataTable = dataTable;
            LoadChart();
        }

        private void LoadChart()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea
            {
                AxisX = { Title = "Harvest Date" },
                AxisY = { Title = "Quantity" }
            };
            chart1.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "Harvest",
                Color = System.Drawing.Color.Green,
                ChartType = SeriesChartType.Line
            };

            chart1.Series.Add(series);

            foreach (DataRow row in dataTable.Rows)
            {
                series.Points.AddXY(Convert.ToDateTime(row["HarvestDate"]), Convert.ToDouble(row["Quantity"]));
            }

            chart1.Invalidate();
        }

        private void btnSaveChart_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save Report as PDF";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveReportAsPdf(saveFileDialog.FileName);
                }
            }
        }

        private void SaveReportAsPdf(string filePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Add title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var title = new Paragraph("Harvest Distribution Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(title);

                // Add data table
                PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);
                pdfTable.WidthPercentage = 100;

                // Add table header
                foreach (DataColumn column in dataTable.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, FontFactory.GetFont(FontFactory.HELVETICA_BOLD)))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    pdfTable.AddCell(cell);
                }

                // Add table rows
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var cellValue in row.ItemArray)
                    {
                        pdfTable.AddCell(cellValue.ToString());
                    }
                }

                document.Add(pdfTable);

                // Add chart
                iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(ms);
                chartImage.ScaleToFit(document.PageSize.Width - document.LeftMargin - document.RightMargin, document.PageSize.Height - document.TopMargin - document.BottomMargin);
                chartImage.Alignment = Element.ALIGN_CENTER;
                document.Add(chartImage);

                document.Close();
                writer.Close();

                MessageBox.Show("Report saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
