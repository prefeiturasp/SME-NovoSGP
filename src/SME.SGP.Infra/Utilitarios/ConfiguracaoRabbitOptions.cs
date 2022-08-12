namespace SME.SGP.Infra.Utilitarios
{
    public class ConfiguracaoRabbitOptions
    {
        public const string Secao = "ConfiguracaoRabbit";
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public ushort LimiteDeMensagensPorExecucao { get; set; }
    }
}
