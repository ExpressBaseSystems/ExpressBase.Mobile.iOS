using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressBase.Mobile.Helpers;
using ExpressBase.Mobile.Helpers.Script;
using ExpressBase.Mobile.iOS.Helpers.Script;
using Foundation;
using Mono.CSharp;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(CSharpScriptHelper))]

namespace ExpressBase.Mobile.iOS.Helpers.Script
{
    public class CSharpScriptHelper : IScriptHelper
    {
        private Evaluator evaluator;

        public CSharpScriptHelper()
        {
            InitializeEvaluator();
        }

        private void InitializeEvaluator()
        {
            var settings = new CompilerSettings()
            {
                StdLib = true
            };

            var reportPrinter = new ConsoleReportPrinter();
            var ctx = new CompilerContext(settings, reportPrinter);

            evaluator = new Evaluator(ctx);
        }

        public T Evaluate<T>(string statement, string expr)
        {
            try
            {
                bool runnable = evaluator.Run(statement);
                if (runnable)
                {
                    evaluator.Evaluate(expr, out object result, out bool _set);
                    return (T)result;
                }
            }
            catch (Exception ex)
            {
                EbLog.Info("Error at CSharpScripting evaluate");
                EbLog.Info(ex.Message);
            }

            return default;
        }
    }
}