using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Extensions
{
    public static class NamesHelper
    {
        public static string GetDateSignedName()
        {
            var buff = new byte[16];

            Random.Shared.NextBytes(buff);

            return DateTime.Now.ToString("dd_MM_yy") + "_" + Convert.ToHexString(buff);
        }
    }
}
