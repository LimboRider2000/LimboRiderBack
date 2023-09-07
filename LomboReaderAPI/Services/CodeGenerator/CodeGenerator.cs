namespace LimboReaderAPI.Services.CodeGenerator
{
    public class CodeGenerator : ICodeGenerator
    {
        public string RandomCodeGen()
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < 7; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
