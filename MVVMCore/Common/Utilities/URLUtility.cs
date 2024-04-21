using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMCore.Common.Utilities
{
    public class URLUtility
    {
        /// <summary>
        /// URLを既定のブラウザで開く
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Process</returns>
        public static Process OpenUrl(string url)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true,
            };

            return Process.Start(pi)!;
        }
    }
}
