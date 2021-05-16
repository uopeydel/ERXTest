using ClosedXML.Excel;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ERXTest.Server.Helper
{
    public static class ReadXLS
    {
        public static XLSObject ReadToPhone(Stream stream)
        {
            var response = new XLSObject { Phonenumber = new List<string> { } };
            bool isSkipFirstRow = false;

            try
            {

                using (var excelWorkbook = new XLWorkbook(stream))
                {
                    var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();
                    foreach (var dataRow in nonEmptyDataRows)
                    {
                        if (isSkipFirstRow == false)
                        {
                            isSkipFirstRow = true;
                            continue;
                        }

                        var collumPhone = dataRow.Cell(1)?.Value?.ToString();

                        if (string.IsNullOrEmpty(response.Message))
                        {
                            var message = dataRow.Cell(2)?.Value?.ToString();
                            response.Message = message;

                            var Questionname = dataRow.Cell(3)?.Value?.ToString();
                            response.QuestionName = Questionname;


                            //string iString = "2005-05-05 22:12";
                            //DateTime oDate = DateTime.ParseExact(iString, "yyyy-MM-dd HH:mm", null);

                            var daymonthyearhourmin = dataRow.Cell(4)?.Value?.ToString();
                            var isValidFormat = DateTime.TryParseExact(
                                daymonthyearhourmin, "dd/MM/yyyy HH:mm",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out DateTime date);
                            if (isValidFormat)
                            {
                                response.DateTimeSend = date;
                            }
                        }
                        for (int c = 0; c < collumPhone.Length; c++)
                        {
                            var alphNum = (int)collumPhone[c];
                            if (alphNum >= 48 && alphNum <= 57)
                            {
                                continue;
                            }
                            collumPhone.Replace(collumPhone[c].ToString(), "");
                        }
                        response.Phonenumber.Add(collumPhone);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }

        public static Stream ByteToStream(byte[] byteArray)
        {
            return new MemoryStream(byteArray);
        }

        public static byte[] Base64ToByteArray(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
