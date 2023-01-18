

using CSVCleaner;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;

string workingDirectory = Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
var path = Path.Combine(projectDirectory, "TestDataSet.csv");

using (TextFieldParser parser = new TextFieldParser(path))
{
    parser.TextFieldType = FieldType.Delimited;
    parser.SetDelimiters(",");

    int IMDBCharLimit = 10000;
    bool headerRow = true;
    bool badTuple = false;
    string tupleError = "";

    Validator validator = new Validator();
    // initialize variable to be used in switch statements later
    var results = validator.checkNum("1");

    var rowsToKeep = new List<string>();
    var rowsToDelete = new List<string>();

    while (!parser.EndOfData)
    {
        //Process row or tuple
        string currentRow = parser.PeekChars(IMDBCharLimit);
        string[] fields = parser.ReadFields();

        badTuple = false;
        tupleError = "";

        if (headerRow == true)
            headerRow = false;
        else
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (badTuple) { break; }

                string value = fields[i];
                //System.Console.WriteLine(value);
                switch (i)
                {
                    case 0:
                        // movieID
                        // check if valid ID?
                        break;
                    case 1:
                        // Date of Review
                        results = validator.checkDate(value);
                        badTuple = results.Item1;
                        tupleError = results.Item2;
                        break;
                    /* case 2:
                        // User
                        // dont need to validate user name right now
                        break;
                    */
                    case 3:
                        // Usefulness Vote
                        results = validator.checkNum(value, "Usefulness Vote Error");
                        badTuple = results.Item1;
                        tupleError = results.Item2;
                        break;
                    case 4:
                        // Total Votes
                        results = validator.checkNum(value, "Total Votes Error");
                        badTuple = results.Item1;
                        tupleError = results.Item2;
                        break;
                    case 5:
                        // User's Rating out of 10
                        results = validator.checkNum(value, "User Rating Error", true);
                        badTuple = results.Item1;
                        tupleError = results.Item2;
                        break;
                    /*
                    case 6:
                        // Review Title
                        // dont need to validate review right now
                        break;
                    case 7:
                        // Review
                        // dont need to validate user name right now
                        break;
                    */
                    default: break;
                }
            }

            if (!badTuple)
            {
                // add this tuple to the "GoodData" csv
                rowsToKeep.Add(currentRow);
            }
            else
            {
                rowsToDelete.Add(tupleError + "\n" + currentRow);
            }
        }
    }
    File.WriteAllLines(Path.Combine(projectDirectory, "CleanedData.csv"), rowsToKeep);

    Console.WriteLine("Deleted Rows: \n");
    foreach (var row in rowsToDelete)
    {
        Console.WriteLine(row + "\n");
    }
}