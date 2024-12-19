using System.Text;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using WinFormsApp1.Interfaces;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    private readonly ISteganography _steganography;
    private int[] password = new[] { 3, 3, 2 };

    public Form1(ISteganography steganography)
    {
        _steganography = steganography;
        InitializeComponent();
    }

    private void SourceFileButton_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog() { Filter = "(*.docx)|*.docx", };
        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        SourceFileTextBox.Text = ofd.FileName;
    }

    private void FinalFileButton_Click(object sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog()
        {
            Filter = "(*.docx)|*.docx",
        };

        if (sfd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        FinalFileTextBox.Text = sfd.FileName;

    }

    private void HideButton_Click(object sender, EventArgs e)
    {
        if (_steganography.EnoughSpace(SourceFileTextBox.Text, MessageToHideTextBox.Text))
        {
            _steganography.HideMessage(SourceFileTextBox.Text, FinalFileTextBox.Text, MessageToHideTextBox.Text, password);
            MessageBox.Show("Message was hidden successfully", "Succes", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("Cover file does not have enough space to cover the message", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        SourceFileTextBox.Clear();
        FinalFileTextBox.Clear();
        MessageToHideTextBox.Clear();
    }

    private void ExtractButton_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog() { Filter = "(*.docx)|*.docx", };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            string filePath = ofd.FileName;

            string extractedMessage = _steganography.ExtractMessage(filePath, password);
            ExtractedMessageLabel.Text = extractedMessage;
        }

        ExtractFileTextBox.Clear();
    }
}
