namespace WinFormsApp1.Interfaces
{
    public interface ISteganography
    {
        void HideMessage(string sourcePath, string destinationPath, string message, int[] password);
        string ExtractMessage(string filePath, int[] password);
        bool EnoughSpace(string filePath, string message);
    }
}
