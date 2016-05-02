using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankFileParsers
{
    public class Detail
    {
        public string RecordCode { get; set; }
        public string TypeCode { get; set; }
        public string Amount { get; set; }
        public string FundsType { get; set; }
        public string Immediate { get; set; }
        public string OneDay { get; set; }
        public string TwoOrMoreDays { get; set; }
        public DateTime? AvalibleDate { get; set; }
        public string BankReferenceNumber { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string Text { get; set; }

        public Detail(BaiDetail data, string currencyCode)
        {
            var list = new List<string> { data.TransactionDetail };
            list.AddRange(data.DetailContinuation);

            var lineData = "";
            foreach (var section in list)
            {
                var line = section.Trim();
                // Some / are optional?
                //if (!line.EndsWith("/")) throw new Exception("I got a line without a trailing /");

                if (line.StartsWith("16"))
                {
                    line = line.Replace("/", "");
                }
                else if (line.StartsWith("88"))
                {
                    line = " " + line.Substring(2);//.Replace("/", " ");
                }
                else throw new Exception("I got a bad line: " + line);
                lineData += line;
            }

            // Now try to figure out what's left ;-)
            var stack = new Stack(lineData.Split(',').Reverse().ToArray());

            RecordCode = stack.Pop().ToString();
            TypeCode = stack.Pop().ToString();
            Amount = stack.Pop().ToString();
            FundsType = stack.Pop().ToString();

            switch (FundsType.ToUpper())
            {
                case "S":
                    Immediate = stack.Pop().ToString();
                    OneDay = stack.Pop().ToString();
                    TwoOrMoreDays = stack.Pop().ToString();
                    break;
                case "D":
                    // next field is the number of distripution pairs
                    // number of days, avalible amount
                    // currencyCode would be used here
                    throw new Exception("I don't want to deal with this one yet - " + currencyCode);
                case "V":
                    var date = stack.Pop().ToString();
                    var time = stack.Pop().ToString();
                    AvalibleDate = BaiFileHelpers.DateTimeFromFields(date, time);
                    break;
            }

            BankReferenceNumber = stack.Pop().ToString();
            CustomerReferenceNumber = stack.Pop().ToString();
            // What's left on the stack?
            Text = LeftoverStackToString(stack);
        }

        private string LeftoverStackToString(Stack stack)
        {
            StringBuilder ret = new StringBuilder();
            while (stack.Count > 0)
            {
                ret.Append(stack.Pop().ToString());
                if (stack.Count > 0)
                {
                    ret.Append(",");
                }
            }
            return ret.ToString();
        }
    }
}
