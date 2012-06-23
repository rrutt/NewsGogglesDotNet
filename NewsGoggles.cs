using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using alice.tuprolog;
using alice.tuprolog.@event;

using Com.Live.RRutt.NewsGogglesDotNet.Lib;
using Com.Live.RRutt.TuProlog.Util;

namespace Com.Live.RRutt.NewsGogglesDotNet
{
  public class NewsGoggles : OutputListener
  {

    public Prolog engine;

    private static bool testing = false;

    public NewsGoggles(string[] args)
    {
      System.Console.Out
          .WriteLine("Rick Rutt's News Goggles - Using the tuProlog system "
              + Prolog.getVersion());

      PrologLibrary.traceEnabled = false;

      foreach (var arg in args)
      {
        if ((arg.Length > 1) && (arg[0] == '-'))
        {
          if (arg.Equals("-trace", StringComparison.CurrentCultureIgnoreCase))
          {
            PrologLibrary.traceEnabled = true;
            System.Console.Out.WriteLine("Trace output enabled.");
          }
          else if (arg.Equals("-test", StringComparison.CurrentCultureIgnoreCase))
          {
            testing = true;
            System.Console.Out.WriteLine("Test mode enabled.");
          }
          else
          {
            System.Console.Out.WriteLine("Unknown command argument ignored: "
                + arg);
          }
        }
        else
        {
          System.Console.Out.WriteLine("Unknown command argument ignored: " + arg);
        }
      }
    }

    public void Run()
    {
      engine = new Prolog();
      try
      {
        var library = new PrologLibrary();
        engine.loadLibrary(library);
      }
      catch (Exception e)
      {
        var s = string.Format(
          "*****\n{0}: {1}\n-----\n{2}\n",
          e.GetType().Name, e.Message, e.StackTrace);
        System.Console.Out.Write(s);
      }
      engine.addOutputListener(this);

      try
      {
        TheoryLoader loader = new TheoryLoader();
        String theoryText = loader.load();

        Theory theory = new Theory(theoryText);
        engine.setTheory(theory);

        if (testing)
        {
          SolveInfo testInfo = engine.solve("test.");
          if (testInfo.isSuccess())
          {
            System.Console.Out.WriteLine("Test run succeeded.");
          }
          else
          {
            System.Console.Out.WriteLine("Test run did not succeed.");
          }
        }
        else
        {
          SolveInfo info = engine
              .solve("all_subscriber_feeds(ResultList).");

          if (info.isSuccess())
          {
            System.Console.Out.WriteLine("Success.");
            var bindings = info.getBindingVars();

            if (PrologLibrary.traceEnabled)
            {
              System.Console.Out.WriteLine("Bindings:");
              System.Console.Out.WriteLine(bindings.ToString());
            }

            Var resultList = (Var)bindings.get(0);
            showSubscriberFeedsResultList(resultList);

            System.Console.Out.WriteLine("Done.");
          }
          else
          {
            System.Console.Out.WriteLine("Failure.");
          }
        }
      }
      catch (Exception e)
      {
        var s = string.Format(
          "*****\n{0}: {1}\n-----\n{2}\n",
          e.GetType().Name, e.Message, e.StackTrace);
        System.Console.Out.Write(s);
      }
    }

    private void showSubscriberFeedsResultList(Var resultList)
    {
      System.Console.Out.WriteLine("\nAll Subscriber Feeds:");

      Struct feedList = (Struct)resultList.getTerm();
      while (!feedList.isEmptyList())
      {
        Struct subscriberFeed = (Struct)feedList.getArg(0)
            .getTerm();
        feedList = (Struct)feedList.getArg(1).getTerm();

        // System.Console.Out.WriteLine("  " + subscriberFeed.ToString());

        Struct subscriber = (Struct)subscriberFeed.getArg(0)
            .getTerm();
        Struct articleList = (Struct)subscriberFeed.getArg(1)
            .getTerm();

        // System.Console.Out.WriteLine("  " + subscriber.getName() + " "
        // + articleList.ToString());
        System.Console.Out.WriteLine("\n  Feed for " + subscriber.getName()
            + ":");

        while (!articleList.isEmptyList())
        {
          Struct article = (Struct)articleList.getArg(0)
              .getTerm();
          articleList = (Struct)articleList.getArg(1)
              .getTerm();

          String articleId = PrologLibrary
              .stringValueFromTerm(article.getArg(0));
          String provider = PrologLibrary
              .stringValueFromTerm(article.getArg(1));
          String contents = PrologLibrary
              .stringValueFromTerm(article.getArg(2));

          System.Console.Out.WriteLine("    #" + articleId.ToString()
              + " from " + provider + ": " + contents);
        }
      }

      System.Console.Out.WriteLine("\n(End of Feeds.)");
    }

    public void onOutput(OutputEvent ev)
    {
      String s = Utilities.stripQuotes(ev.getMsg());
      System.Console.Out.Write(s);
    }
  }
}
