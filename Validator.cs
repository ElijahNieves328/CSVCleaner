using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSVCleaner
{
    internal class Validator
    {
        public Validator() { }

        public (bool, string) checkDate(string value, string errorMessage = "Date Error") {
            bool badData = false;
            string returnMessages = "";

            try
            {
                var parsedDate = DateTime.Parse(value);
                //Console.WriteLine(parsedDate);
            }
            catch (Exception e)
            {
                badData = true;
                returnMessages += errorMessage + "\n";
                returnMessages += e.ToString();
            }
            return (badData, returnMessages);
        }

        public (bool, string) checkNum(string value, string errorMessage = "Number Error", bool lessThan10 = false) {
            bool badData = false;
            string returnMessages = "";
            try
            {
                int result = Int32.Parse(value);
                if (result < 0) { 
                    badData = true;
                    returnMessages += errorMessage + "\n";
                    returnMessages += "Negative Number";
                }
                else if (lessThan10 && result > 10) { 
                    badData = true;
                    returnMessages += errorMessage + "\n";
                    returnMessages += "Number greater than 10";
                }
                //Console.WriteLine(result);
            }
            catch (Exception e)
            {
                badData = true;
                returnMessages += errorMessage + "\n";
                returnMessages += e.ToString();
            }
            return (badData, returnMessages);
        }
    }
}