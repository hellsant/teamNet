using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;

namespace DataLoader
{
    public class ExcelFileReader
    {
        public ExcelFileReader()
        {
        }

        public IList<IList<string>> ReadExcelFileFormat()
        {
            IList<IList<string>> result = new List<IList<string>>();
            IList<string> categories = new List<string>();
            string category = string.Empty;
            string competency = string.Empty;
            bool isStarting = true;
            foreach (var worksheet in Workbook.Worksheets("Format.xlsx"))
            {
                for (int i = 3; i < worksheet.Rows.Count(); i++)
                {
                    if (isStarting)
                    {
                        isStarting = false;
                        category = worksheet.Rows.ElementAt(i).Cells.ElementAt(1).Text;
                        competency = worksheet.Rows.ElementAt(i).Cells.ElementAt(2).Text;
                    }
                    else
                    {
                        if (worksheet.Rows.ElementAt(i).Cells.ElementAt(1).Text.Equals(string.Empty))
                        {
                            competency = worksheet.Rows.ElementAt(i).Cells.ElementAt(2).Text;
                        }
                        else
                        {
                            category = worksheet.Rows.ElementAt(i).Cells.ElementAt(1).Text;
                            competency = worksheet.Rows.ElementAt(i).Cells.ElementAt(2).Text;
                        }
                    }
                    categories.Add(category);
                    categories.Add(competency);
                    result.Add(categories);
                    categories = new List<string>();
                }
            }
            return result;
        }
        
        public IDictionary<string, IList<float>> ReadExcelFileResults(string filePath)
        {
            IDictionary<string, IList<float>> result = new Dictionary<string, IList<float>>();
            IList<float> scores = new List<float>();
            string evaluated = string.Empty;
            foreach (var worksheet in Workbook.Worksheets(filePath))
            {
                evaluated = worksheet.Rows.ElementAt(0).Cells.ElementAt(1).Text;
                for (int i = 3; i < worksheet.Rows.Count(); i++)
                {
                    if (!worksheet.Rows.ElementAt(i).Cells.ElementAt(6).Text.Equals(string.Empty))
                    {
                        double score = Convert.ToDouble(worksheet.Rows.ElementAt(i).Cells.ElementAt(6).Text);
                        scores.Add(Convert.ToSingle(score));
                    }
                }
            }
            result.Add(evaluated, scores);
            return result;
        }
    }
}
