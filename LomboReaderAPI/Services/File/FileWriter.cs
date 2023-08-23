using System.Drawing;

using static System.Net.Mime.MediaTypeNames;

namespace LomboReaderAPI.Services.File
{
    public class FileWriter : IFileWriter
    {
        public async Task BookFileWriter(IFormFile file, string fileName)
        {
            try
            {

                string currFileName = fileName + "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1];

                var pathToFile = Path.Combine(Directory.GetCurrentDirectory(),"Resources", "Book");

                using FileStream stream = new FileStream(Path.Combine(pathToFile, currFileName), FileMode.Create);

                await file.CopyToAsync(stream);

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
