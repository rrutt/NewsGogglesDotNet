using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Live.RRutt.TuProlog.Util
{
  public class Utilities
  {
    public static string stripQuotes(string s)
    {
      var result = s;

      if (string.IsNullOrEmpty(s))
      {
        s = string.Empty;
      }
      else
      {
        var n = s.Length;
        if (n > 1)
        {
          if ((s[0] == '\'') && (s[n - 1] == '\''))
          {
            result = s.Substring(1, n - 2);
          }
        }
      }

      return result;
    }
  }
}
