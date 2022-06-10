namespace SME.SGP.Infra
{
    public static class RotasRabbitSgpAula
    {
        public const string RotaNotificacaoExclusaoAulasComFrequencia = "sgp.notificacao.aulas.exclusao.frequencia";

        public const string RotaExcluirAulaRecorrencia = "sgp.aula.excluir.recorrencia";
        public const string RotaInserirAulaRecorrencia = "sgp.aula.cadastrar.recorrencia";
        public const string RotaAlterarAulaRecorrencia = "sgp.aula.alterar.recorrencia";
        public const string RotaAlterarAulaFrequenciaTratar = "sgp.aula.alterar.frequencia.tratar";

        public const string RotaCriarAulasInfatilAutomaticamente = "sgp.aulas.infantil.criar";
        public const string RotaSincronizarAulasInfatil = "sgp.aulas.infantil.sincronizar";

        public const string CarregarDadosUeTurmaRegenciaAutomaticamente = "aulas.automaticas.regencia.ue.turma.carregar";
        public const string SincronizarDadosUeTurmaRegenciaAutomaticamente = "aulas.automaticas.regencia.ue.turma.sync";
        public const string SincronizarAulasRegenciaAutomaticamente = "aulas.automaticas.regencia.sync";

        public const string DiarioBordoDaAulaExcluir = "sgp.diarios.bordo.aula.excluir";

        public const string NotificacoesDaAulaExcluir = "sgp.notificacoes.aula.excluir";
        public const string PlanoAulaDaAulaExcluir = "sgp.plano.aula.excluir";

        public const string RotaNotificacaoAulasPrevistasSync = "sgp.aulas.previstas.notificacao.sync";
        public const string RotaNotificacaoAulasPrevistas = "sgp.aulas.previstas.notificacao";
    }
}
