using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERXTest.Shared.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> ConvertToBase64(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return base64;
            //return new MemoryStream(Encoding.UTF8.GetBytes(base64));
        }
    }
}
