namespace SME.SGP.Infra
{
    //TODO: VARIAVEIS PARA CONFIGURACOES!
    public static class RotasRabbit
    {
        public static string ExchangeListenerWorkerRelatorios => "sme.sr.workers.relatorios";
        public static string FilaListenerSgp => "sme.sr.clients.sgp";
        
        public static string WorkerRelatoriosSgp => "sme.sr.workers.sgp";
        public static string RotaRelatoriosSolicitados => "relatorios.solicitados";
        public static string RotaRelatoriosProntos => "relatorios.prontos";

        public static string RotaExcluirAulaRecorrencia => "aula.excluir.recorrencia";
        public static string RotaInserirAulaRecorrencia => "aula.cadastrar.recorrencia";
        public static string RotaAlterarAulaRecorrencia => "aula.excluir.recorrencia";
    }
}
