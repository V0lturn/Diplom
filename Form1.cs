using System.Text;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
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
        using var ofd = new OpenFileDialog()
        {
            Filter = "All Files (*.*)|*.*",
        };

        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        string filePath = ofd.FileName;
        byte[] fileBytes = File.ReadAllBytes(filePath);

        _steganography.HideFile(SourceFileTextBox.Text, FinalFileTextBox.Text, fileBytes, password);

        //if (_steganography.EnoughSpace(SourceFileTextBox.Text, fileBytes))
        //{
        //    _steganography.HideFile(SourceFileTextBox.Text, FinalFileTextBox.Text, fileBytes, password);
        //    MessageBox.Show("File was hidden successfully", "Success", MessageBoxButtons.OK,
        //        MessageBoxIcon.Information);
        //}
        //else
        //{
        //    MessageBox.Show("Cover file does not have enough space to hide the file", "Error", MessageBoxButtons.OK,
        //        MessageBoxIcon.Error);
        //}

        SourceFileTextBox.Clear();
        FinalFileTextBox.Clear();
    }

    private void ExtractButton_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog() { Filter = "(*.docx)|*.docx", };

        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        string filePath = ofd.FileName;

        using var sfd = new SaveFileDialog()
        {
            Filter = "All Files (*.*)|*.*",
        };

        if (sfd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            var extractedFile = _steganography.ExtractFile(filePath, password);
            var extractedText = Encoding.UTF8.GetString(extractedFile);
            File.WriteAllText(sfd.FileName, extractedText, Encoding.UTF8);

            MessageBox.Show("File was extracted successfully", "Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}