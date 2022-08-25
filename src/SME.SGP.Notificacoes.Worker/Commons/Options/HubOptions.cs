namespace SME.SGP.Notificacoes.Worker
{
    public class HubOptions
    {
        public const string Secao = "Hub";

        public string Endpoint { get; set; }
        public string HubNotificacoesToken { get; set; }
    }
}
