using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentry;
using SME.SGP.IoC;
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
        static SME.Background.Core.Servidor<SME.Background.Hangfire.Worker> WorkerService;

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
            SentrySdk.AddBreadcrumb($"[SME SGP] Serviço Background iniciado no ip: {IPLocal}", "Service Life cycle");
            WorkerService.Registrar();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WorkerService.Dispose();
            SentrySdk.AddBreadcrumb($"[SME SGP] Serviço Background finalizado no ip: {IPLocal}", "Service Life cycle");
            return Task.CompletedTask;
        }

        internal static void Configurar(IConfiguration config)
        {
            WorkerService = new SME.Background.Core.Servidor<SME.Background.Hangfire.Worker>(new SME.Background.Hangfire.Worker(config));
        }

        internal static void ConfigurarDependencias(IServiceCollection services)
        {
            RegistraDependencias.RegistrarWorkerService(services);

        }
    }
}
