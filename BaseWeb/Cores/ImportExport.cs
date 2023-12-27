using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Data;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Vml.Spreadsheet;

namespace BaseWeb.Cores
{
    public class ImportExport
    {
        public static void ExportExcel(string FilePath, DataSet tableSet, int? skipRow = 0)
        {
            WorkbookPart wBookPart = null;
            using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(FilePath, SpreadsheetDocumentType.Workbook))
            {
                wBookPart = spreadsheetDoc.AddWorkbookPart();
                wBookPart.Workbook = new Workbook();
                uint sheetId = 1;
                spreadsheetDoc.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = spreadsheetDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                foreach (DataTable table in tableSet.Tables)
                {
                    WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    SheetData sheetData = new SheetData();
                    wSheetPart.Worksheet = new Worksheet(sheetData);

                    Row headerRow = new Row();
                    foreach (DataColumn column in table.Columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dr in table.Rows)
                    {
                        Row row = new Row();
                        foreach (DataColumn column in table.Columns)
                        {
                            Cell cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(dr[column].ToString());
                            row.AppendChild(cell);
                        }
                        sheetData.AppendChild(row);
                    }
                    sheetId++;
                }
            }
        }

        public static DataSet ImportExcel(string fileName, string fileType, int? skipRows = 0)
        {
            var ds = new DataSet();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fs, false))
                {
                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    foreach (Sheet sheet in sheets)
                    {
                        var dt = new DataTable();
                        var isColAssign = false;
                        string relationshipId = sheet.Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        string sheetName = sheet.Name;
                        dt.TableName = sheetName;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                        //if (sheetName == "Readme")
                        //{
                        //    dt.Columns.Add("Run");
                        //    DataRow tempRow = dt.NewRow();
                        //    tempRow[0] = GetCellValue(spreadSheetDocument, GetCell(sheetData, "B1"));
                        //    dt.Rows.Add(tempRow);
                        //}
                        //else
                        //{

                        //}
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();
                        if (fileType == "DA")
                        {
                            dt.TableName = dt.TableName + "-" + GetCellValue(spreadSheetDocument, GetCell(sheetData, "A1"));
                        }
                        foreach (Row row in rows)
                        {
                            if (row.RowIndex <= skipRows)
                            {
                                continue;
                            }
                            DataRow tempRow = dt.NewRow();
                            for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                            {
                                if (!isColAssign)
                                {
                                    dt.Columns.Add(GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i)));

                                }
                                else
                                {
                                    if (row.Descendants<Cell>().ElementAt(i).CellValue == null)
                                    {
                                        break;
                                    }
                                    tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));

                                }
                            }
                            if (isColAssign)
                            {
                                dt.Rows.Add(tempRow);
                            }
                            isColAssign = true;
                        }
                        ds.Tables.Add(dt);
                    }
                }
            }

            return ds;
        }
        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        private static Cell GetCell(SheetData sheetData, string cellAddress)
        {
            uint rowIndex = uint.Parse(Regex.Match(cellAddress, @"[0-9]+").Value);
            return sheetData.Descendants<Row>().FirstOrDefault(p => p.RowIndex == rowIndex).Descendants<Cell>().FirstOrDefault(p => p.CellReference == cellAddress);
        }
    }
}
