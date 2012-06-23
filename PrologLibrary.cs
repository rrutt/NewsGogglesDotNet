using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using alice.tuprolog;

namespace Com.Live.RRutt.NewsGogglesDotNet.Lib
{
  public class PrologLibrary : Library
  {
    public static bool traceEnabled = true;

    protected TextWriter outputStream = System.Console.Out;

    public static String stringValueFromTerm(Term t)
    {
      var result = string.Empty;

      Term tt = t.getTerm();
      if (tt is Struct)
      {
        result = ((Struct)tt).getName();
        if (result.Equals("."))
        {
          result = tt.ToString();
        }
      }
      else if (tt is Number)
      {
        Number n = (Number)tt;
        if (n is Int)
        {
          result = n.intValue().ToString();
        }
        else
        {
          result = n.ToString();
        }
      }

      return result;
    }

    public bool trace_enabled_0()
    {
      return traceEnabled;
    }

    public bool enable_trace_0()
    {
      traceEnabled = true;
      System.Console.Out.WriteLine("+++ enable_trace.");
      return true;
    }

    public bool disable_trace_0()
    {
      traceEnabled = false;
      System.Console.Out.WriteLine("--- disable_trace.");
      return true;
    }

    public bool trace_1(Term arg0)
    {
      if (traceEnabled)
      {
        String text = stringValueFromTerm(arg0);
        System.Console.Out.Write(text);
      }
      return true;
    }

    public bool trace_nl_0()
    {
      if (traceEnabled)
      {
        System.Console.Out.WriteLine();
      }
      return true;
    }
  }
}
