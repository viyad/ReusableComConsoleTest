using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;
using CsvHelper;

namespace ReusableComConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvHelperFunction();
            LINQtoCscFunction();
        }

        static void CsvHelperFunction()
        {
            List<Animal> aList = new List<Animal>();

            using (var reader = new StreamReader(@"../../Import/animal-details.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    Animal a = new Animal();
                    a.Id = Convert.ToInt32(csv.GetField<string>(0));
                    a.Type = csv.GetField<string>(1);
                    a.Population = Convert.ToInt32(csv.GetField<string>(2));

                    aList.Add(a);
                }
            }

            using (var writer = new StreamWriter(@"../../Export/animal-details-out-Helper.csv"))
            using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(aList);
            }
        }
        static void LINQtoCscFunction()
        {
            List<Animal> aList = new List<Animal>();

            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<MyDataRow> animals =
                 cc.Read<MyDataRow>(@"../../Import/animal-details.csv", inputFileDescription);

            foreach (MyDataRow dataRow in animals)
            {
                Animal a = new Animal();
                a.Id = dataRow[0].LineNbr;
                a.Type = dataRow[1].Value;
                a.Population = Convert.ToInt32(dataRow[2].Value);

                aList.Add(a);
            }

            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',', // comma delimited
                FirstLineHasColumnNames = true,
            };
            cc = new CsvContext();
            cc.Write(
                aList,
                @"../../Export/animal-details-out-LINQ.csv",
                outputFileDescription);
        }
    }

    internal class MyDataRow : List<DataRowItem>, IDataRow
    {
    }
}
