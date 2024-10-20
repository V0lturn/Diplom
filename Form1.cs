using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog() { Filter = "(*.docx)|*.docx", };
        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        textBox1.Text = ofd.FileName;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog()
        {
            Filter = "(*.docx)|*.docx",
        };

        if (sfd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        textBox2.Text = sfd.FileName;

    }

    private void button3_Click(object sender, EventArgs e)
    {
        File.Copy(textBox1.Text, textBox2.Text);
        using var wordDoc = WordprocessingDocument.Open(textBox2.Text, true);
        var password = new[] { 3, 3, 2 };

        var content = Encoding.UTF8.GetBytes(textBox3.Text);

        var currentByte = 0;
        var body = wordDoc.MainDocumentPart!.Document.Body!;
        var newElements = new List<OpenXmlElement>();

        foreach (var element in body.Elements())
        {
            if (currentByte >= content.Length)
            {
                newElements.Add(element.CloneNode(true));
                continue;
            }

            if (element is not Paragraph paragraph)
            {
                newElements.Add(element.CloneNode(true));
                continue;
            }

            if (element is Drawing || element.Descendants<Drawing>().Any())
            {
                newElements.Add(element.CloneNode(true));
                continue;
            }

            var newParagraph = new Paragraph();
            foreach (var pEl in paragraph.Elements())
            {
                if (currentByte >= content.Length)
                {
                    newParagraph.AppendChild(pEl.CloneNode(true));
                    continue;
                }

                if (pEl is not Run run)
                {
                    newParagraph.AppendChild(pEl.CloneNode(true));
                    continue;
                }

                if (pEl is Drawing || pEl.Descendants<Drawing>().Any())
                {
                    newParagraph.AppendChild(pEl.CloneNode(true));
                    continue;
                }

                if (run.RunProperties?.Color?.Val != null && run.RunProperties.Color.Val != "000000")
                {
                    newParagraph.AppendChild(pEl.CloneNode(true));
                    continue;
                }

                if (pEl is Hyperlink innerHyperlink ||
                    pEl.Ancestors<Hyperlink>().Any() ||
                    run.Ancestors<Hyperlink>().Any())
                {
                    newParagraph.AppendChild(pEl.CloneNode(true));
                    continue;
                }

                if (run.InnerText.Contains("HYPERLINK", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                for (var index = 0; index < run.InnerText.Length; index++)
                {
                    var b = currentByte < content.Length ? content[currentByte++] : -1;
                    if (b == -1)
                    {
                        newParagraph.AppendChild(new Run(new Text(run.InnerText[index..])));
                        break;
                    }

                    var colorRed = (b >> (password[1] + password[2])) & ((int)Math.Pow(2, password[0]) - 1);
                    var colorGreen = (b >> password[2]) & ((int)Math.Pow(2, password[1]) - 1);
                    var colorBlue = b & ((int)Math.Pow(2, password[2]) - 1);

                    var c = run.InnerText[index];
                    var text = c == ' '
                        ? new Text(" ")
                        {
                            Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve)
                        }
                        : new Text(c.ToString());
                    var newRun = new Run(text);
                    newRun.RunProperties = new RunProperties(new Color { Val = $"{colorRed:X2}{colorGreen:X2}{colorBlue:X2}" });
                    newParagraph.AppendChild(newRun);
                }
            }

            newElements.Add(newParagraph);
        }

        // Clear the body and add the new paragraphs
        body.RemoveAllChildren();
        foreach (var element in newElements)
        {
            body.AppendChild(element);
        }

        wordDoc.MainDocumentPart.Document.Save();

    }
}
