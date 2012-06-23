using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Live.RRutt.NewsGogglesDotNet
{
  public class TheoryLoader
  {
    private static readonly string TheoryFolderPath = @".\theories";

    private static readonly string[] TheoryResourceNames = {
		"Articles.pl",
		"Preferences.pl",
		"Rules.pl",
		"Test.pl",
};

    public string load()
    {
      var sb = new StringBuilder();

      foreach (var resourceName in TheoryResourceNames)
      {
        appendResource(resourceName, sb);
      }

      var theoryString = sb.ToString();

      return theoryString;
    }

    private void appendResource(string resourceName, StringBuilder sb)
    {
      System.Console.Out.WriteLine("Loading " + resourceName);

      var theoryFileRelativePath = Path.Combine(TheoryFolderPath, resourceName);
      var theoryFileAbsolutePath = Path.GetFullPath(theoryFileRelativePath);

      try
      {
        var theoryText = File.ReadAllText(theoryFileAbsolutePath);
        sb.Append(theoryText);
      }
      catch (Exception e)
      {
        var s = string.Format(
          "Could not load resource [{0}]\n  {1}: {2}\n",
          theoryFileAbsolutePath, e.GetType().Name, e.Message);
        System.Console.Out.Write(s);
      }
    }
  }
}
