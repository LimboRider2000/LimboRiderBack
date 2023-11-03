using System.Drawing;

using static System.Net.Mime.MediaTypeNames;

namespace LimboReaderAPI.Services.File
{
    public class FileWriter : IFileWriter
    {
        public async Task BookFileWriter(IFormFile file, string fileName)
        {
            try
            {

                string currFileName = fileName + "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1];

                var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Book");

                using FileStream stream = new FileStream(Path.Combine(pathToFile, currFileName), FileMode.Create);

                await file.CopyToAsync(stream);

            }
            catch (Exception ex)
            {
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
        public async Task<string> EditCover(string blob, string _fileName, string? oldTitleImgPath, string ext)
        {

            if (System.IO.File.Exists(oldTitleImgPath))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), oldTitleImgPath));
            }
            return await CoverFileWriter(blob, _fileName, ext);
        }
        public void DeleteBookFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }
        public string AvatarFileChange(IFormFile file, string newFileName, string? oldFilePath )
        {
            newFileName += file.FileName.Substring( file.FileName.LastIndexOf( "." ) );

            if (oldFilePath != null && System.IO.File.Exists( oldFilePath ) )
            {
                System.IO.File.Delete( Path.Combine(Directory.GetCurrentDirectory(), oldFilePath) );
            }
            
            String newPath = Path.Combine("Resources", "Images", newFileName);

            using FileStream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), newPath
                ), FileMode.Create);
            file.CopyTo(stream);

            return newPath;

        }

    }
}
