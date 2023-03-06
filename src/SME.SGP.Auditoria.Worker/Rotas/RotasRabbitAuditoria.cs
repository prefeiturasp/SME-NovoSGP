namespace SME.SGP.Auditoria.Worker
{
    public static class RotasRabbitAuditoria
    {
        public static string ExchangeSgp => "sme.sgp.workers";
        public static string ExchangeSgpDeadLetter => "sme.sgp.workers.deadletter";

        public const string PersistirAuditoriaDB = "sgp.auditoria.incluir";
    }
}
