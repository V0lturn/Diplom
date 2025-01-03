namespace WinFormsApp1.Interfaces
{
    public interface ISteganography
    {
        void HideFile(string sourcePath, string destinationPath, byte[] fileBytes, int[] password);
        byte[] ExtractFile(string filePath, int[] password);
        // bool EnoughSpace(string filePath, byte[] fileBytes);
    }
}
