namespace SME.SGP.Notificacao.Worker
{
    public static class RotasRabbitNotificacao
    {
        public static string ExchangeSgp => "sme.sgp.workers";
        public static string ExchangeSgpDeadLetter => "sme.sgp.workers.deadletter";

        public const string Criacao = "sgp.notificacao.criada";
        public const string Leitura = "sgp.notificacao.leitura";
        public const string Exclusao = "sgp.notificacao.excluida";
    }
}
