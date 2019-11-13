using Microsoft.Extensions.Hosting;
using Sentry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Worker.Service
{
    public class Servico : IHostedService
    {
        string ipLocal;

        protected string IPLocal
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ipLocal))
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipLocal = ip.ToString();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(ipLocal))
                        ipLocal = "127.0.0.1";
                }

                return ipLocal;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            SentrySdk.CaptureMessage($"[SME SGP] Serviço Background iniciado no ip: {IPLocal}");
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            SentrySdk.CaptureMessage($"[SME SGP] Serviço Background finalizado no ip: {IPLocal}");
            return Task.CompletedTask;
        }
    }
}
