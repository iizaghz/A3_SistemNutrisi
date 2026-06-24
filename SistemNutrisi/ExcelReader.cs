using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace SistemNutrisi
{
    public static class ExcelReader
    {
        public static DataTable ReadExcel(string filePath)
        {
            DataTable dt = new DataTable();
            List<string> sharedStrings = new List<string>();

            using (ZipArchive archive = ZipFile.OpenRead(filePath))
            {
                // 1. Read shared strings
                ZipArchiveEntry sharedStringsEntry = archive.GetEntry("xl/sharedStrings.xml");
                if (sharedStringsEntry != null)
                {
                    using (Stream stream = sharedStringsEntry.Open())
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                        nsmgr.AddNamespace("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                        XmlNodeList list = doc.SelectNodes("//x:t", nsmgr);
                        if (list != null)
                        {
                            foreach (XmlNode node in list)
                            {
                                sharedStrings.Add(node.InnerText);
                            }
                        }
                    }
                }

                // 2. Read sheet1
                ZipArchiveEntry sheetEntry = archive.GetEntry("xl/worksheets/sheet1.xml");
                if (sheetEntry != null)
                {
                    using (Stream stream = sheetEntry.Open())
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                        nsmgr.AddNamespace("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");

                        XmlNodeList rows = doc.SelectNodes("//x:row", nsmgr);
                        if (rows != null)
                        {
                            int maxCols = 0;
                            // Pre-calculate number of columns
                            foreach (XmlNode row in rows)
                            {
                                XmlNodeList cells = row.SelectNodes("x:c", nsmgr);
                                if (cells != null && cells.Count > maxCols)
                                {
                                    maxCols = cells.Count;
                                }
                            }

                            // Create columns
                            for (int i = 0; i < maxCols; i++)
                            {
                                dt.Columns.Add("Column" + (i + 1));
                            }

                            // Read rows
                            foreach (XmlNode row in rows)
                            {
                                DataRow dr = dt.NewRow();
                                XmlNodeList cells = row.SelectNodes("x:c", nsmgr);
                                if (cells != null)
                                {
                                    foreach (XmlNode cell in cells)
                                    {
                                         string cellRef = cell.Attributes["r"]?.Value;
                                         string typeAttr = cell.Attributes["t"]?.Value;
                                         string val = "";

                                         if (typeAttr == "inlineStr")
                                         {
                                             XmlNode inlineNode = cell.SelectSingleNode("x:is/x:t", nsmgr);
                                             if (inlineNode != null)
                                             {
                                                 val = inlineNode.InnerText;
                                             }
                                         }
                                         else
                                         {
                                             XmlNode valNode = cell.SelectSingleNode("x:v", nsmgr);
                                             val = valNode != null ? valNode.InnerText : "";

                                             // Map the value
                                             if (typeAttr == "s" && int.TryParse(val, out int idx) && idx >= 0 && idx < sharedStrings.Count)
                                             {
                                                 val = sharedStrings[idx];
                                             }
                                         }

                                        // Handle cell column alignment in case of empty cells
                                        int colIdx = GetColumnIndexFromRef(cellRef);
                                        if (colIdx >= 0 && colIdx < maxCols)
                                        {
                                            dr[colIdx] = val;
                                        }
                                    }
                                }
                                dt.Rows.Add(dr);
                            }

                            // Use the first row as headers if appropriate
                            if (dt.Rows.Count > 0)
                            {
                                DataRow firstRow = dt.Rows[0];
                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    string colName = firstRow[i]?.ToString()?.Trim();
                                    if (!string.IsNullOrEmpty(colName) && !dt.Columns.Contains(colName))
                                    {
                                        dt.Columns[i].ColumnName = colName;
                                    }
                                }
                                dt.Rows.RemoveAt(0);
                            }
                        }
                    }
                }
            }

            return dt;
        }

        private static int GetColumnIndexFromRef(string cellRef)
        {
            if (string.IsNullOrEmpty(cellRef)) return -1;
            string letters = "";
            foreach (char c in cellRef)
            {
                if (char.IsLetter(c)) letters += c;
                else break;
            }

            int index = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                index *= 26;
                index += (letters[i] - 'A' + 1);
            }
            return index - 1;
        }
    }
}
