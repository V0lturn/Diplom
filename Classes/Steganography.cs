using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using WinFormsApp1.Interfaces;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;

public class Steganography : ISteganography
{
    public void HideFile(string sourcePath, string destinationPath, byte[] fileBytes, int[] password)
    {
        File.Copy(sourcePath, destinationPath, true);
        using var wordDoc = WordprocessingDocument.Open(destinationPath, true);

        var currentByte = 0;
        var body = wordDoc.MainDocumentPart!.Document.Body!;
        var newElements = new List<OpenXmlElement>();

        foreach (var element in body.Elements())
        {
            if (currentByte >= fileBytes.Length)
            {
                newElements.Add(element.CloneNode(true));
                continue;
            }

            if (element is Table table)
            {
                var newTable = ProcessTable(table, ref currentByte, fileBytes, password);
                newElements.Add(newTable);
            }
            else if (element is Paragraph paragraph)
            {
                var newParagraph = ProcessParagraph(paragraph, ref currentByte, fileBytes, password);
                newElements.Add(newParagraph);
            }
            else
            {
                newElements.Add(element.CloneNode(true));
            }
        }

        body.RemoveAllChildren();
        body.Append(newElements);
        wordDoc.MainDocumentPart.Document.Save();
    }

    private Table ProcessTable(Table table, ref int currentByte, byte[] fileBytes, int[] password)
    {
        var newTable = table.CloneNode(true) as Table;

        foreach (var row in newTable!.Elements<TableRow>())
        {
            foreach (var cell in row.Elements<TableCell>())
            {
                ProcessTableCell(cell, ref currentByte, fileBytes, password);
            }
        }

        return newTable;
    }

    private void ProcessTableCell(TableCell cell, ref int currentByte, byte[] fileBytes, int[] password)
    {
        var newCellContent = new List<OpenXmlElement>();

        foreach (var cellElement in cell.Elements())
        {
            if (currentByte >= fileBytes.Length)
            {
                newCellContent.Add(cellElement.CloneNode(true));
                continue;
            }

            if (cellElement is Paragraph cellParagraph)
            {
                var newParagraph = ProcessParagraph(cellParagraph, ref currentByte, fileBytes, password);
                newCellContent.Add(newParagraph);
            }
            else
            {
                newCellContent.Add(cellElement.CloneNode(true));
            }
        }

        cell.RemoveAllChildren();
        cell.Append(newCellContent);
    }

    private Paragraph ProcessParagraph(Paragraph paragraph, ref int currentByte, byte[] fileBytes, int[] password)
    {
        var newParagraph = new Paragraph();

        foreach (var pEl in paragraph.Elements())
        {
            if (currentByte >= fileBytes.Length)
            {
                newParagraph.AppendChild(pEl.CloneNode(true));
                continue;
            }

            if (pEl is Drawing drawing)
            {
                newParagraph.AppendChild(drawing.CloneNode(true));
                continue;
            }

            if (pEl is not Run run || pEl is Drawing || run.Descendants<Drawing>().Any())
            {
                newParagraph.AppendChild(pEl.CloneNode(true));
                continue;
            }

            for (var index = 0; index < run.InnerText.Length; index++)
            {
                var b = currentByte < fileBytes.Length ? fileBytes[currentByte++] : -1;
                if (b == -1)
                {
                    newParagraph.AppendChild(new Run(new Text(run.InnerText[index..]))
                    {
                        RunProperties = run.RunProperties?.CloneNode(true) as RunProperties
                    });
                    break;
                }

                var colorRed = (b >> (password[1] + password[2])) & ((int)Math.Pow(2, password[0]) - 1);
                var colorGreen = (b >> password[2]) & ((int)Math.Pow(2, password[1]) - 1);
                var colorBlue = b & ((int)Math.Pow(2, password[2]) - 1);

                var c = run.InnerText[index];
                var text = c == ' '
                    ? new Text(" ") { Space = SpaceProcessingModeValues.Preserve }
                    : new Text(c.ToString());

                var newRun = new Run(text)
                {
                    RunProperties = (run.RunProperties?.CloneNode(true) as RunProperties) ?? new RunProperties()
                };

                newRun.RunProperties.Color = new Color
                {
                    Val = $"{colorRed:X2}{colorGreen:X2}{colorBlue:X2}"
                };

                newParagraph.AppendChild(newRun);
            }
        }

        return newParagraph;
    }


    public byte[] ExtractFile(string filePath, int[] password)
    {
        using var wordDoc = WordprocessingDocument.Open(filePath, false);
        var extractedBytes = new List<byte>();
        var body = wordDoc.MainDocumentPart!.Document.Body!;

        foreach (var element in body.Elements())
        {
            if (element is Table table)
            {
                foreach (var row in table.Elements<TableRow>())
                {
                    foreach (var cell in row.Elements<TableCell>())
                    {
                        foreach (var cellElement in cell.Elements())
                        {
                            if (cellElement is Paragraph cellParagraph)
                            {
                                ExtractFromParagraph(cellParagraph, extractedBytes, password);
                            }
                        }
                    }
                }
            }
            else if (element is Paragraph paragraph)
            {
                ExtractFromParagraph(paragraph, extractedBytes, password);
            }
        }

        return extractedBytes.ToArray();
    }

    private void ExtractFromParagraph(Paragraph paragraph, List<byte> extractedBytes, int[] password)
    {
        foreach (var run in paragraph.Elements<Run>())
        {
            if (run.RunProperties?.Color?.Val == null || run.Descendants<Drawing>().Any())
                continue;

            var colorHex = run.RunProperties.Color.Val?.Value;
            if (string.IsNullOrEmpty(colorHex) || colorHex.Length != 6) continue;

            try
            {
                var colorRed = Convert.ToInt32(colorHex.Substring(0, 2), 16);
                var colorGreen = Convert.ToInt32(colorHex.Substring(2, 2), 16);
                var colorBlue = Convert.ToInt32(colorHex.Substring(4, 2), 16);

                var byteValue =
                    (colorRed << (password[1] + password[2])) |
                    (colorGreen << password[2]) |
                    colorBlue;

                extractedBytes.Add((byte)byteValue);
            }
            catch
            {
                continue;
            }
        }
    }
}

//public bool EnoughSpace(string filePath, byte[] fileBytes)
//{
//    using var wordDoc = WordprocessingDocument.Open(filePath, false);

//    var docSpace = wordDoc.MainDocumentPart!.Document.Descendants<Text>()
//      .Sum(text => text.Text.Length);
//    int requiredSpace = fileBytes.Length;

//    return docSpace >= requiredSpace;
//} 