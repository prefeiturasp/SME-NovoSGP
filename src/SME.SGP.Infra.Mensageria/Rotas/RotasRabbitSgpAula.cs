namespace SME.SGP.Infra
{
    public static class RotasRabbitSgpAula
    {
        public const string RotaExcluirAulaRecorrencia = "sgp.aula.excluir.recorrencia";
        public const string RotaInserirAulaRecorrencia = "sgp.aula.cadastrar.recorrencia";
        public const string RotaAlterarAulaRecorrencia = "sgp.aula.alterar.recorrencia";
        public const string RotaAlterarAulaFrequenciaTratar = "sgp.aula.alterar.frequencia.tratar";
        public const string RotaCriarAulasInfantilERegenciaAutomaticamente = "sgp.aulas.infantil.criar";
        public const string RotaSincronizarAulasInfantil = "sgp.aulas.infantil.sincronizar";
        public const string CarregarDadosUeTurmaRegenciaAutomaticamente = "aulas.automaticas.regencia.ue.turma.carregar";
        public const string SincronizarDadosUeTurmaRegenciaAutomaticamente = "aulas.automaticas.regencia.ue.turma.sync";
        public const string SincronizarAulasRegenciaAutomaticamente = "aulas.automaticas.regencia.sync";
        public const string RotaExecutaExclusaoPendenciaDiarioBordoAula = "sgp.pendencias.diario.bordo.aula.excluir";
        public const string RotaNotificacaoExclusaoAulasComFrequencia = "sgp.notificacao.aulas.exclusao.frequencia";
        public const string RotaExecutaPendenciasAula = "sgp.pendencias.aulas.executa";
        public const string RotaExecutaPendenciasAulaDre = "sgp.pendencias.aulas.dre.executa";
        public const string RotaExecutaPendenciasAulaDreUe = "sgp.pendencias.aulas.dre.ue.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordo = "sgp.pendencias.aulas.diario.bordo.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordoTurma = "sgp.pendencias.aulas.diario.bordo.turma.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordoTurmaAulaComponente = "sgp.pendencias.aulas.diario.bordo.turma.aula.componente.executa";
        public const string PendenciasGeraisAulas = "sgp.pendencias.gerais.aula";
        public const string RotaExecutaPendenciasAulaAvaliacao = "sgp.pendencias.aulas.avaliacao.executa";
        public const string RotaExecutaPendenciasAulaFrequencia = "sgp.pendencias.aulas.frequencia.executa";
        public const string RotaExecutaPendenciasAulaPlanoAula = "sgp.pendencias.aulas.plano.aula.executa";
        public const string RotaExecutaPendenciasTurmasComponenteSemAulaUe = "sgp.pendencias.turmas.componentes.sem.aulas.ue.executar";
        public const string RotaExecutaPendenciasTurmasComponenteSemAula = "sgp.pendencias.turmas.componentes.sem.aulas.executar";
        public const string RotaAvaliarPendenciasAulaDiarioClasseFechamento = "sgp.pendencias.aulas.diario.classe.fechamento.avaliar";
        public const string RotaExecutaExclusaoPendenciasAula = "sgp.pendencias.gerais.pendencias.aula.excluir";
        public const string RotaNotificacaoAlunosFaltososDre = "sgp.aulas.alunos.faltosos.Dre.notificar";
        public const string RotaNotificacaoAlunosFaltososDreUe = "sgp.aulas.alunos.faltosos.Dre.Ue.notificar";
        public const string RotaNotificacaoAlunosFaltosos = "sgp.aulas.alunos.faltosos.notificar";
        public const string RotaNotificacaoAulasPrevistasSync = "sgp.aulas.previstas.notificacao.sync"; 
        public const string RotaNotificacaoAulasPrevistas = "sgp.aulas.previstas.notificacao";
        public const string RotaMuralAvisosSync = "sgp.mural.avisos.sync";
        public const string NotificacoesDaAulaExcluir = "sgp.notificacoes.aula.excluir";
        public const string PlanoAulaDaAulaExcluir = "sgp.plano.aula.excluir";
        public const string RotaExcluirAulasRecorrentesComponentesCurricularesTerritorioSaberDisponibilizado = "sgp.aulas.componentes.curriculares.territorio.saber.disponibilizados.excluir";
        public const string RotaExcluirPendenciasDiarioBordo = "sgp.pendencias.diario.bordo.excluir";
    }
}
