using System;

namespace SME.SGP.Infra.Utilitarios
{
    public class ConfiguracaoRabbit
    {
        public string HostName
        {
            get => Environment.GetEnvironmentVariable("ConfiguracaoRabbit__HostName");
        }
        public string UserName
        {
            get => Environment.GetEnvironmentVariable("ConfiguracaoRabbit__UserName");
        }
        public string Password
        {
            get => Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Password");
        }
        public string VirtualHost
        {
            get => Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Virtualhost");
        }
        public string BasicQos
        {
            get => Environment.GetEnvironmentVariable("ConfiguracaoRabbit__BasicQos");
        }

    }
}
