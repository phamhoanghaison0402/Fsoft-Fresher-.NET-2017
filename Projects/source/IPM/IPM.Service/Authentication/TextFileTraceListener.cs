using System;
using System.IO;
using Microsoft.Exchange.WebServices.Data;

namespace Authentication
{
    /// <summary>
    /// TraceListener
    /// </summary>
    /// <seealso cref="Microsoft.Exchange.WebServices.Data.ITraceListener" />
    public class TraceListener : ITraceListener
  {
        /// <summary>
        /// Handles a trace message
        /// </summary>
        /// <param name="traceType">Type of trace message.</param>
        /// <param name="traceMessage">The trace message.</param>
        public void Trace(string traceType, string traceMessage)
    {
      //CreateXMLTextFile(traceType, traceMessage);
    }

        /// <summary>
        /// Creates the XML text file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="traceContent">Content of the trace.</param>
        private void CreateXMLTextFile(string fileName, string traceContent)
    {
      try
      {
        if (!Directory.Exists(@"..\\TraceOutput"))
        {
          Directory.CreateDirectory(@"..\\TraceOutput");
        }

        System.IO.File.WriteAllText(@"..\\TraceOutput\\" + fileName + DateTime.Now.Ticks + ".txt", traceContent);
      }
      catch (IOException ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}
