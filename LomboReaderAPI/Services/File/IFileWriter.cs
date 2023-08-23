namespace LomboReaderAPI.Services.File
{
    public interface IFileWriter
    {
     
        public Task<string> CoverFileWriter(string blobString, string _fileName,string ext);
        public Task BookFileWriter(IFormFile file, string fileName);

    }
}
