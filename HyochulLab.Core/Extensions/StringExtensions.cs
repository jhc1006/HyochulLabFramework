using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyochulLab.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? str) =>
            string.IsNullOrEmpty(str);

        public static bool IsNotEmpty(this string? str) =>
            !string.IsNullOrEmpty(str);
    }
}
