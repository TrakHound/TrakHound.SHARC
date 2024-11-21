// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;

public static class ExcelSpreadsheet
{
    public static SpreadsheetDocument Create(string path)
    {
        try
        {
            return SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
        }
        catch (Exception ex)
        {

        }

        return null;
    }

    public static SpreadsheetDocument Create(Stream stream)
    {
        try
        {
            return SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
        }
        catch (Exception ex)
        {

        }

        return null;
    }

    public static void CreateExcelSpreadsheet<T>(this IEnumerable<T> items, SpreadsheetDocument document, string sheetName, string sheetId)
    {
        if (document != null)
        {
            try
            {
                var workbookPart = CreateWorkbookPart(document, sheetName, sheetId);
                CreateWorksheetPart(items, workbookPart, sheetId);
            }
            catch (Exception ex)
            {

            }
        }
    }

    private static WorkbookPart CreateWorkbookPart(SpreadsheetDocument document, string sheetName, string sheetId)
    {
        if (document.WorkbookPart == null)
        {
            var workbookPart = document.AddWorkbookPart();
            var workbook = new Workbook();
            workbook.Append(new Sheets());
            workbookPart.Workbook = workbook;

            //CreateWorkbookStyles(workbookPart);
        }

        var sheetIndex = UInt32Value.FromUInt32((UInt32)(document.WorkbookPart.Workbook.Sheets.Count() + 1));
        var sheet = new Sheet() { Name = sheetName, SheetId = sheetIndex, Id = sheetId };
        document.WorkbookPart.Workbook.Sheets.Append(sheet);

        return document.WorkbookPart;
    }

    private static void CreateWorkbookStyles(WorkbookPart workbookPart)
    {
        var workbookStylesPart1 = workbookPart.AddNewPart<WorkbookStylesPart>("rId3");

        Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
        stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

        Fonts fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

        Font font1 = new Font();
        FontSize fontSize1 = new FontSize() { Val = 11D };
        Color color1 = new Color() { Theme = (UInt32Value)1U };
        FontName fontName1 = new FontName() { Val = "Calibri" };
        FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
        FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

        font1.Append(fontSize1);
        font1.Append(color1);
        font1.Append(fontName1);
        font1.Append(fontFamilyNumbering1);
        font1.Append(fontScheme1);

        Font font2 = new Font();
        Bold bold1 = new Bold();
        FontSize fontSize2 = new FontSize() { Val = 11D };
        Color color2 = new Color() { Theme = (UInt32Value)1U };
        FontName fontName2 = new FontName() { Val = "Calibri" };
        FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
        FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };

        font2.Append(bold1);
        font2.Append(fontSize2);
        font2.Append(color2);
        font2.Append(fontName2);
        font2.Append(fontFamilyNumbering2);
        font2.Append(fontScheme2);

        fonts1.Append(font1);
        fonts1.Append(font2);

        Fills fills1 = new Fills() { Count = (UInt32Value)2U };

        Fill fill1 = new Fill();
        PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

        fill1.Append(patternFill1);

        Fill fill2 = new Fill();
        PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

        fill2.Append(patternFill2);

        fills1.Append(fill1);
        fills1.Append(fill2);

        Borders borders1 = new Borders() { Count = (UInt32Value)2U };

        Border border1 = new Border();
        LeftBorder leftBorder1 = new LeftBorder();
        RightBorder rightBorder1 = new RightBorder();
        TopBorder topBorder1 = new TopBorder();
        BottomBorder bottomBorder1 = new BottomBorder();
        DiagonalBorder diagonalBorder1 = new DiagonalBorder();

        border1.Append(leftBorder1);
        border1.Append(rightBorder1);
        border1.Append(topBorder1);
        border1.Append(bottomBorder1);
        border1.Append(diagonalBorder1);

        Border border2 = new Border();

        LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
        Color color3 = new Color() { Indexed = (UInt32Value)64U };

        leftBorder2.Append(color3);

        RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
        Color color4 = new Color() { Indexed = (UInt32Value)64U };

        rightBorder2.Append(color4);

        TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
        Color color5 = new Color() { Indexed = (UInt32Value)64U };

        topBorder2.Append(color5);

        BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
        Color color6 = new Color() { Indexed = (UInt32Value)64U };

        bottomBorder2.Append(color6);
        DiagonalBorder diagonalBorder2 = new DiagonalBorder();

        border2.Append(leftBorder2);
        border2.Append(rightBorder2);
        border2.Append(topBorder2);
        border2.Append(bottomBorder2);
        border2.Append(diagonalBorder2);

        borders1.Append(border1);
        borders1.Append(border2);

        CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
        CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

        cellStyleFormats1.Append(cellFormat1);

        CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)3U };
        CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
        CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
        CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };

        cellFormats1.Append(cellFormat2);
        cellFormats1.Append(cellFormat3);
        cellFormats1.Append(cellFormat4);

        CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
        CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

        cellStyles1.Append(cellStyle1);
        DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
        TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

        StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

        StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
        stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
        X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

        stylesheetExtension1.Append(slicerStyles1);

        StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
        stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
        X15.TimelineStyles timelineStyles1 = new X15.TimelineStyles() { DefaultTimelineStyle = "TimeSlicerStyleLight1" };

        stylesheetExtension2.Append(timelineStyles1);

        stylesheetExtensionList1.Append(stylesheetExtension1);
        stylesheetExtensionList1.Append(stylesheetExtension2);

        stylesheet1.Append(fonts1);
        stylesheet1.Append(fills1);
        stylesheet1.Append(borders1);
        stylesheet1.Append(cellStyleFormats1);
        stylesheet1.Append(cellFormats1);
        stylesheet1.Append(cellStyles1);
        stylesheet1.Append(differentialFormats1);
        stylesheet1.Append(tableStyles1);
        stylesheet1.Append(stylesheetExtensionList1);

        workbookStylesPart1.Stylesheet = stylesheet1;
    }

    private static void CreateWorksheetPart<T>(IEnumerable<T> items, WorkbookPart workbookPart, string sheetId)
    {
        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>(sheetId);
        var worksheet = new Worksheet();
        var sheetData = new SheetData();
        sheetData.Append(CreateHeaderRow<T>());

        if (items != null && items.Any())
        {
            foreach (var item in items)
            {
                sheetData.Append(CreateRow(item));
            }
        }

        worksheet.Append(sheetData);
        worksheetPart.Worksheet = worksheet;
    }


    private static Row CreateHeaderRow<T>()
    {
        var row = new Row();

        try
        {
            var properties = typeof(T).GetProperties().ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                row.Append(CreateCell(property.Name));
                //row.Append(CreateCell(property.Name, 2U));
            }
        }
        catch { }

        return row;
    }

    private static Row CreateRow<T>(T item)
    {
        var row = new Row();

        if (item != null)
        {
            try
            {
                var properties = typeof(T).GetProperties().ToList();
                for (int i = 0; i < properties.Count; i++)
                {
                    var property = properties[i];
                    string value = "";

                    var objValue = property.GetValue(item);
                    if (objValue != null)
                    {
                        value = objValue.ToString();
                    }

                    row.Append(CreateCell(value));
                }
            }
            catch { }
        }

        return row;
    }


    private static Cell CreateCell(string text)
    {
        Cell cell = new Cell();
        cell.DataType = ResolveCellDataTypeOnValue(text);
        cell.CellValue = new CellValue(text);
        return cell;
    }

    private static Cell CreateCell(string text, uint styleIndex)
    {
        Cell cell = new Cell();
        cell.StyleIndex = styleIndex;
        cell.DataType = ResolveCellDataTypeOnValue(text);
        cell.CellValue = new CellValue(text);
        return cell;
    }

    private static EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
    {
        int intVal;
        double doubleVal;
        if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
        {
            return CellValues.Number;
        }
        else
        {
            return CellValues.String;
        }
    }
}