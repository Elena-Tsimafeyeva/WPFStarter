using Microsoft.VisualBasic.FileIO;
using WPFStarter.ImportAndExport.Import.Interfaces;

namespace WPFStarter.ImportAndExport.Import
{
    public class CsvParser : ICsvParser
    {
        private readonly TextFieldParser _parser;

        public CsvParser(string filePath)
        {
            _parser = new TextFieldParser(filePath)
            {
                TextFieldType = FieldType.Delimited
            };
            _parser.SetDelimiters(";");
        }

        public bool EndOfData => _parser.EndOfData;

        public string[]? ReadFields() => _parser.ReadFields();
    }
}
