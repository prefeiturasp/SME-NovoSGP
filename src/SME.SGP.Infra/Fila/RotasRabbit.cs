namespace SME.SGP.Infra
{
    public static class RotasRabbit
    {
        public static string ExchangeServidorRelatorios => "sme.sr.workers.relatorios";
        public static string ExchangeSgp => "sme.sgp.workers";

        public static string FilaSgp => "sme.sgp.clients";
        public static string WorkerRelatoriosSgp => "sme.sr.workers.sgp";
        
        public static string RotaRelatoriosSolicitados => "relatorios.solicitados";
        public static string RotaRelatoriosProntos => "relatorios.prontos";

        public static string RotaExcluirAulaRecorrencia => "aula.excluir.recorrencia";
        public static string RotaInserirAulaRecorrencia => "aula.cadastrar.recorrencia";
        public static string RotaAlterarAulaRecorrencia => "aula.alterar.recorrencia";
        public static string RotaNotificacaoUsuario => "notificacao.usuario";
        public static string RotaRelatorioComErro => "relatorios.erro";
        public static string RotaRelatorioCorrelacaoCopiar => "relatorios.correlacao.copiar";
    }
}
