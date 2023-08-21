namespace LomboReaderAPI.Services.File
{
    public interface IFileWriter
    {
        public Task<string> BookFileWriter(string blobString, string _fileName,string ext);
        public Task<string> CoverFileWriter(string blobString, string _fileName,string ext);  

    }
}
