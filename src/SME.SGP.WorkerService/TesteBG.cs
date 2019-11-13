using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SME.SGP.Worker.Service
{
    public class TesteBG
    {
        static int execucoes = 0;
        public void Testando(string ip)
        {
            Console.WriteLine($"Execução de teste {ip} - {Thread.CurrentThread.Name} - {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}");
            SentrySdk.CaptureMessage($"Execução de teste {ip} - {Thread.CurrentThread.Name}");
        }

        public void TestandoPeriodico(string ip)
        {
            execucoes++;
            Console.WriteLine($"Execução de Perioridco {ip} - {Thread.CurrentThread.Name} - {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}");
            SentrySdk.AddBreadcrumb($"Execução de Perioridco {ip} - {Thread.CurrentThread.Name} - {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}");

            if (execucoes >= 3)
                SentrySdk.CaptureMessage($"Execução de teste {ip} - {Thread.CurrentThread.Name}");
        }
    }
}
