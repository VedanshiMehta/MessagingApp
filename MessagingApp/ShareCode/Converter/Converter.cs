using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public static class Converter
    {
        public static ImageSource ConvertBase64ToImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            MemoryStream stream = new MemoryStream(bytes);
            return ImageSource.FromStream(() => stream);
        }
    }
}
