namespace SME.SGP.Infra
{
    public static class RotasRabbit
    {
        public static string ExchangeServidorRelatorios => "sme.sr.workers.relatorios";
        public static string ExchangeSgp => "sme.sgp.workers";

        public static string FilaSgp => "sme.sgp.clients";
        public static string WorkerRelatoriosSgp => "sme.sr.workers.sgp";
        
        public static string RotaRelatoriosSolicitados => "relatorios.solicitados";
        public static string RotaRelatoriosProntos=> "relatorios.prontos";
        public static string ExcluirAulaRecorrente => "aulas.excluir";
    }
}
