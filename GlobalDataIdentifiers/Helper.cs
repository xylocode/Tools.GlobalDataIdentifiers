using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XyloCode.Tools.GlobalDataIdentifiers
{
    internal static class Helper
    {

        public static string GetFileData(byte[] bytes)
        {
            var preamble = Encoding.UTF8.GetPreamble();
            bool withPreample = true;
            if (bytes.Length > preamble.Length)
            {
                for (int i = 0; i < preamble.Length; i++)
                {
                    withPreample &= bytes[i] == preamble[i];
                }
            }

            if (withPreample)
                return Encoding.UTF8.GetString(bytes, preamble.Length, bytes.Length - preamble.Length);
            else
                return Encoding.UTF8.GetString(bytes);
        }
    }
}
