using static System.Net.Mime.MediaTypeNames;

namespace LomboReaderAPI.Services.File
{
    public class FileWriter : IFileWriter
    {
        public async Task<string> BookFileWriter(string blobString,string _fileName,string ext)
        {
            try
            {
                
                string convert = blobString.Substring(blobString.LastIndexOf(',') + 1);
                byte[] buteArey = Convert.FromBase64String(convert);
                string fileName = _fileName + "." + ext;
                var folderName = Path.Combine("Resources", "Book");
                var pathOnServer = Path.Combine(Directory.GetCurrentDirectory(), folderName,fileName);
                await System.IO.File.WriteAllBytesAsync(pathOnServer, buteArey);
                return Path.Combine(folderName, fileName);
            }
            catch (Exception ex) {
                 throw new Exception(ex.Message);
            }
        }

        public async Task<string> CoverFileWriter(string blobString, string _fileName, string ext)
        {
            try
            {
                string convert = blobString.Substring(blobString.LastIndexOf(',') + 1);


                byte[] buteArey = Convert.FromBase64String(convert);
                string fileName = _fileName + "." + ext;
                var folderName = Path.Combine("Resources", "Cover");
                var pathOnServer = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileName);
                await System.IO.File.WriteAllBytesAsync(pathOnServer, buteArey);
                return Path.Combine(folderName, fileName);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
