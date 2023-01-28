

using CSVCleaner;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;

string workingDirectory = Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
var path = Path.Combine(projectDirectory, "moviereviews.csv");

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

    int loop = 1;

    while (!parser.EndOfData)
    {
        if ((loop % 10000) == 0) { Console.WriteLine("Running... \n"); }
        loop++;

        //Process row or tuple
        string currentRow = parser.PeekChars(IMDBCharLimit);
        string[] fields = parser.ReadFields();

        badTuple = false;
        tupleError = "";

        if (headerRow == true)
            rowsToKeep.Add(currentRow);
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
                        if (value == "Was this review helpful?  Sign in to vote.")
                        {
                            // Console.WriteLine(value);
                            badTuple = true;
                            tupleError = "Usefulness Vote Error. Value was 'Was this review helpful? Sign in to vote.'";
                        }
                        else {
                            results = validator.checkNum(value, "Usefulness Vote Error");
                            badTuple = results.Item1;
                            tupleError = results.Item2;
                            // if (badTuple) { Console.WriteLine(value); }
                        }
                        break;
                    /*
                    case 6:
                        // Review Title
                        // dont need to validate review title right now
                        break;
                    case 7:
                        // Review
                        // dont need to validate review content right now
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
    File.WriteAllLines(Path.Combine(projectDirectory, "BadData.csv"), rowsToDelete);

    Console.WriteLine("Deleted Rows: \n" + rowsToDelete.Capacity);
    Console.WriteLine("Good data stored in CleanedData.csv. Deleted data stored in BadData.csv");

    /*
    foreach (var row in rowsToDelete)
    {
        Console.WriteLine(row + "\n");
    }
    */
}