namespace Company.G04.PL.Healper
{
    public class DocumentSettings
    {
        //Upload 
        public static string UplodFile(IFormFile file, string folderName)
        {
            //1- Get Folder location 
            //string folderPath = "C:\\Users\\METRO\\source\\repos\\OmarAlshandidy\\CompanyG04\\Company.G04.PL\\wwwroot\\files\\" + folderName;

            //var folderPath = Directory.GetCurrentDirectory() + " \\wwwroot\\\\files\\" + folderName;
            

            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + @"\wwwroot\files\" + folderName);


            //2.File Name And MAke It Unique
            var fileName = $"{Guid.NewGuid()}{file.FileName}";
            //FilePath
            var filePath = Path.Combine(folderPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }

        // 2.Delete  
        public static void DeleteFile(string fileName,string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory() + @"wwwroot\files\" + folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
