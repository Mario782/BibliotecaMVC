using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? autor, string? genero)
        {
            var query = _context.Libros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(autor))
                query = query.Where(l => l.Autor.Contains(autor));

            if (!string.IsNullOrWhiteSpace(genero))
                query = query.Where(l => l.Genero.Contains(genero));

            ViewBag.AutorFiltro = autor;
            ViewBag.GeneroFiltro = genero;
            ViewBag.GenerosDisponibles = _context.Libros.Select(l => l.Genero).Distinct().ToList();

            return View(query.ToList());
        }

        public IActionResult GenerarPdf(string? autor, string? genero)
        {
            var query = _context.Libros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(autor))
                query = query.Where(l => l.Autor.Contains(autor));

            if (!string.IsNullOrWhiteSpace(genero))
                query = query.Where(l => l.Genero.Contains(genero));

            var libros = query.ToList();
            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Sistema BibliotecaMVC").FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                            col.Item().Text("Reporte Oficial de Inventario de Libros").FontSize(12).FontColor(Colors.Grey.Darken2);
                            if (!string.IsNullOrWhiteSpace(autor) || !string.IsNullOrWhiteSpace(genero))
                            {
                                col.Item().Text($"Filtros aplicados -> Autor: {autor ?? "Todos"} | Género: {genero ?? "Todos"}")
                                    .FontSize(9).Italic().FontColor(Colors.Blue.Medium);
                            }
                        });

                        row.ConstantItem(140).AlignRight().Column(col =>
                        {
                            col.Item().Text("Fecha de generación:").Bold().FontSize(9);
                            col.Item().Text(fechaActual).FontSize(9);
                        });
                    });

                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);   // ID
                                columns.RelativeColumn(3);    // Título
                                columns.RelativeColumn(2);    // Autor
                                columns.RelativeColumn(2);    // Género
                                columns.ConstantColumn(60);   // Año
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5).Text("ID").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5).Text("Título").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5).Text("Autor").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5).Text("Género / Categoría").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(5).AlignCenter().Text("Año").Bold().FontColor(Colors.White);
                            });

                            int index = 0;
                            foreach (var libro in libros)
                            {
                                var bgColor = index % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                                table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(libro.Id.ToString());
                                table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(libro.Titulo);
                                table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(libro.Autor);
                                table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(libro.Genero);
                                table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignCenter().Text(libro.AnioPublicacion.ToString());

                                index++;
                            }
                        });

                        col.Item().PaddingTop(12).AlignRight().Background(Colors.Blue.Lighten5).Padding(8)
                            .Text($"Total de libros incluidos en el reporte: {libros.Count}")
                            .Bold().FontSize(11).FontColor(Colors.Blue.Darken3);
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            });

            byte[] pdfBytes = documento.GeneratePdf();
            return File(pdfBytes, "application/pdf", "ReporteLibros.pdf");
        }

        public IActionResult ExportarExcel(string? autor, string? genero)
        {
            var query = _context.Libros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(autor))
                query = query.Where(l => l.Autor.Contains(autor));

            if (!string.IsNullOrWhiteSpace(genero))
                query = query.Where(l => l.Genero.Contains(genero));

            var libros = query.ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte de Libros");

            worksheet.Cell("A1").Value = "Sistema BibliotecaMVC - Reporte de Libros";
            worksheet.Range("A1:E1").Merge().Style
                .Font.SetBold(true)
                .Font.SetFontSize(14)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.DarkBlue)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            worksheet.Cell("A2").Value = $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}";
            worksheet.Range("A2:E2").Merge().Style
                .Font.SetItalic(true)
                .Font.SetFontSize(10)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            int filaEncabezado = 4;
            worksheet.Cell(filaEncabezado, 1).Value = "ID";
            worksheet.Cell(filaEncabezado, 2).Value = "Nombre del Libro";
            worksheet.Cell(filaEncabezado, 3).Value = "Autor";
            worksheet.Cell(filaEncabezado, 4).Value = "Categoría / Género";
            worksheet.Cell(filaEncabezado, 5).Value = "Año de Publicación";

            var rangoEncabezado = worksheet.Range(filaEncabezado, 1, filaEncabezado, 5);
            rangoEncabezado.Style.Font.SetBold(true);
            rangoEncabezado.Style.Font.SetFontColor(XLColor.White);
            rangoEncabezado.Style.Fill.SetBackgroundColor(XLColor.SteelBlue);
            rangoEncabezado.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            rangoEncabezado.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rangoEncabezado.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            int filaActual = 5;
            foreach (var libro in libros)
            {
                worksheet.Cell(filaActual, 1).Value = libro.Id;
                worksheet.Cell(filaActual, 2).Value = libro.Titulo;
                worksheet.Cell(filaActual, 3).Value = libro.Autor;
                worksheet.Cell(filaActual, 4).Value = libro.Genero;
                worksheet.Cell(filaActual, 5).Value = libro.AnioPublicacion;

                var rangoFila = worksheet.Range(filaActual, 1, filaActual, 5);
                rangoFila.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rangoFila.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(filaActual, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(filaActual, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                filaActual++;
            }

            filaActual++;
            worksheet.Cell(filaActual, 1).Value = "Total de libros incluidos:";
            worksheet.Range(filaActual, 1, filaActual, 4).Merge().Style
                .Font.SetBold(true)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Cell(filaActual, 5).Value = libros.Count;
            worksheet.Cell(filaActual, 5).Style.Font.SetBold(true);
            worksheet.Cell(filaActual, 5).Style.Fill.SetBackgroundColor(XLColor.LightBlue);
            worksheet.Cell(filaActual, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteLibros.xlsx");
        }
    }
}