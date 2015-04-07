using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KladrTask.WebUI
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> elements, int i, int j)
        {
            if (i < 0 || j < 0 || i >= elements.Count || j >= elements.Count)
            {
                throw new ArgumentOutOfRangeException(@"Incorrect indexes.");
            }
            var tmp = elements[i];
            elements[i] = elements[j];
            elements[j] = tmp;
        }
    }
}