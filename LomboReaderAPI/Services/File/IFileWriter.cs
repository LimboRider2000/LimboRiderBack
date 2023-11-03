namespace LimboReaderAPI.Services.File
{
    public interface IFileWriter
    {
     
        public Task<string> CoverFileWriter(string blobString, string _fileName,string ext);
        public Task BookFileWriter(IFormFile file, string fileName);
        public Task<string> EditCover(string str, string _fileName, string? titleImgPath,string ext);
        void DeleteBookFile(string filePath);
        string AvatarFileChange(IFormFile file, string newFileName, string? oldFilePath);

    }
}
