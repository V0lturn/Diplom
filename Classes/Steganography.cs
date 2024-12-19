using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using WinFormsApp1.Interfaces;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;

public class Steganography : ISteganography
{
    public void HideMessage(string sourcePath, string destinationPath, string message, int[] password)                      // Method to hide a message within a Word document
    {
        File.Copy(sourcePath, destinationPath, true);
        using var wordDoc = WordprocessingDocument.Open(destinationPath, true);
        var content = Encoding.UTF8.GetBytes(message);                                                                      // Convert the message to byte array

        var currentByte = 0; 
        var body = wordDoc.MainDocumentPart!.Document.Body!;
        var newElements = new List<OpenXmlElement>();

        foreach (var element in body.Elements())                                                                            // Iterate over each element in the document's body
        {
            if (currentByte >= content.Length)
            {
                newElements.Add(element.CloneNode(true));
                continue;
            }

            if (element is Table table)
            {
                var newTable = ProcessTable(table, ref currentByte, content, password);                                     // Process tables
                newElements.Add(newTable);
            }
            else if (element is Paragraph paragraph)
            {
                var newParagraph = ProcessParagraph(paragraph, ref currentByte, content, password);                         // Process paragraphs
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

    private Table ProcessTable(Table table, ref int currentByte, byte[] content, int[] password)                            // Method to process tables within the Word document
    {
        var newTable = table.CloneNode(true) as Table;

        foreach (var row in newTable!.Elements<TableRow>())                                                                 // Iterate over each row and cell within the table
        {
            foreach (var cell in row.Elements<TableCell>())
            {
                ProcessTableCell(cell, ref currentByte, content, password);                                                 // Process each cell
            }
        }

        return newTable;
    }

    private void ProcessTableCell(TableCell cell, ref int currentByte, byte[] content, int[] password)                      // Method to process individual cells within the table
    {
        var newCellContent = new List<OpenXmlElement>();

        foreach (var cellElement in cell.Elements())
        {
            if (currentByte >= content.Length)
            {
                newCellContent.Add(cellElement.CloneNode(true));
                continue;
            }

            if (cellElement is Paragraph cellParagraph)
            {
                var newParagraph = ProcessParagraph(cellParagraph, ref currentByte, content, password);                     // Process paragraph inside cell
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

    private Paragraph ProcessParagraph(Paragraph paragraph, ref int currentByte, byte[] content, int[] password)            // Method to process paragraphs in the Word document
    {
        var newParagraph = new Paragraph();

        foreach (var pEl in paragraph.Elements())                                                                           // Iterate over each element within the paragraph
        {
            if (currentByte >= content.Length)
            {
                newParagraph.AppendChild(pEl.CloneNode(true));                                                              
                continue;
            }

            if (pEl is not Run run || pEl is Drawing || run.Descendants<Drawing>().Any())                                   // Skip runs containing drawings or other unsupported elements
            {
                newParagraph.AppendChild(pEl.CloneNode(true)); 
                continue;
            }

            if (run.RunProperties?.Color?.Val != null && run.RunProperties.Color.Val != "000000")                           // Skip if the text has non-black text color
            {
                newParagraph.AppendChild(pEl.CloneNode(true));
                continue;
            }
            
            if (pEl is Hyperlink innerHyperlink ||                                                                          // Skip hyperlinks and inner elements
                pEl.Ancestors<Hyperlink>().Any() ||
                run.Ancestors<Hyperlink>().Any())
            {
                newParagraph.AppendChild(pEl.CloneNode(true));
                continue;
            }

            if (run.InnerText.Contains("HYPERLINK", StringComparison.OrdinalIgnoreCase))                                    // Ignore text containing "HYPERLINK"
            {
                continue; 
            }

            for (var index = 0; index < run.InnerText.Length; index++)                                                      // Process each character in the run
            {
                var b = currentByte < content.Length ? content[currentByte++] : -1;
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

                newRun.RunProperties.Color = new Color { Val = $"{colorRed:X2}{colorGreen:X2}{colorBlue:X2}" };
                newParagraph.AppendChild(newRun);
            }
        }

        return newParagraph;
    }

    public string ExtractMessage(string filePath, int[] password)                                                           // Method to extract a hidden message from the Word document
    {
        using var wordDoc = WordprocessingDocument.Open(filePath, false);
        var extractedBytes = new List<byte>();
        var body = wordDoc.MainDocumentPart!.Document.Body!;                                                                // Get the body of the Word document

        foreach (var element in body.Elements())                                                                            // Iterate over each element in the document's body
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
                                ExtractFromParagraph(cellParagraph, extractedBytes, password);                              // Extract data from table cells
                            }
                        }
                    }
                }
            }
            else if (element is Paragraph paragraph)
            {
                ExtractFromParagraph(paragraph, extractedBytes, password);                                                  // Extract data from paragraphs
            }
        }

        return Encoding.UTF8.GetString(extractedBytes.ToArray());                                                           // Convert byte array to string
    }

    private void ExtractFromParagraph(Paragraph paragraph, List<byte> extractedBytes, int[] password)                       // Helper method to extract data from a paragraph
    {
        foreach (var run in paragraph.Elements<Run>())
        {
            if (run.RunProperties?.Color?.Val == null) continue;

            var colorHex = run.RunProperties.Color.Val?.Value;
            if (string.IsNullOrEmpty(colorHex) || colorHex.Length != 6) continue;

            var colorRed = Convert.ToInt32(colorHex.Substring(0, 2), 16);
            var colorGreen = Convert.ToInt32(colorHex.Substring(2, 2), 16);
            var colorBlue = Convert.ToInt32(colorHex.Substring(4, 2), 16);

            var byteValue =
                (colorRed << (password[1] + password[2])) |
                (colorGreen << password[2]) |
                colorBlue;

            extractedBytes.Add((byte)byteValue);                                                                            // Append the extracted byte to the list
        }
    }

    public bool EnoughSpace(string filePath, string message)
    {
        using var wordDoc = WordprocessingDocument.Open(filePath, false);

        var docSpace = wordDoc.MainDocumentPart!.Document.Descendants<Text>()
            .Sum(text => text.Text.Length);
        int messageLength = message.Length;

        return docSpace >= messageLength;
    }

}
