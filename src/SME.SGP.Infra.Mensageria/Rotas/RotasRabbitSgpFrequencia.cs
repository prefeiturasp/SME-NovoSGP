namespace SME.SGP.Infra
{
    public class RotasRabbitSgpFrequencia
    {
        protected RotasRabbitSgpFrequencia() { }

        public const string RotaCalculoFrequenciaPorTurmaComponente = "sgp.frequencia.turma.componente";
        public const string ConsolidarFrequenciasPorTurma = "sgp.frequencia.turma.consolidar";
        public const string ConsolidarFrequenciasPorTurmaMensal = "sgp.frequencia.turma.mensal.consolidar";
        public const string ConsolidarFrequenciasPorTurmaSemanal = "sgp.frequencia.turma.semanal.consolidar";
        public const string ConsolidarFrequenciasTurmasPorUe = "sgp.frequencia.turma.ue.consolidar";
        public const string ConsolidacaoFrequenciasTurmasCarregar = "sgp.frequencia.turma.carregar";
        public const string ConsolidacaoFrequenciasTurmasMensalCarregar = "sgp.frequencia.turma.mensal.carregar";
        public const string ConsolidacaoFrequenciasTurmasSemanalCarregar = "sgp.frequencia.turma.semanal.carregar";
        public const string ConsolidarFrequenciasTurmasNoAno = "sgp.frequencia.turma.ano.consolidar";
        public const string ConsolidarFrequenciasTurmasPorDre = "sgp.frequencia.turma.dre.consolidar";
        public const string RotaConciliacaoFrequenciaAnoSync = "sgp.frequencia.turma.conciliacao.ano.sync";
        public const string RotaConciliacaoFrequenciaTurmaDreSync = "sgp.frequencia.turma.conciliacao.dre.sync";
        public const string RotaConciliacaoFrequenciaTurmaUeSync = "sgp.frequencia.turma.conciliacao.ue.sync";
        public const string RotaConciliacaoFrequenciaTurmasSync = "sgp.frequencia.turma.conciliacao.sync";
        public const string RotaConciliacaoFrequenciaTurmaPorPeriodo = "sgp.frequencia.turma.conciliacao.periodo";
        public const string RotaConciliacaoCalculoFrequenciaPorTurmaComponente = "sgp.frequencia.turma.conciliacao.componente";
        public const string RotaConciliacaoFrequenciaTurmasAlunosSync = "sgp.frequencia.turma.alunos.conciliacao.sync";
        public const string RotaConciliacaoFrequenciaTurmasAlunosBuscar = "sgp.frequencia.turma.alunos.buscar.sync";
        public const string RotaCalcularFrequenciaGeralSync = "sgp.frequencia.calcular.geral.sync";
        public const string RotaConsolidacaoFrequenciaAlunoPorTurmaMensal = "sgp.frequencia.aluno.turma.mensal.consolidar";
        public const string RotaConsolidacaoFrequenciaTurmaEvasao = "sgp.frequencia.turma.evasao.consolidar";
        public const string RotaConsolidacaoFrequenciaTurmaEvasaoAcumulado = "sgp.frequencia.turma.evasao.acumulado.consolidar";
        public const string RotaConciliacaoFrequenciaTurmaMes = "sgp.frequencia.turma.conciliacao.mes";
        public const string RotaNotificacaoFrequenciaUe = "sgp.frequencia.ue.notificar";
        public const string CarregarDadosAlunosFrequenciaMigracao = "sgp.migracao.frequencia.alunos.carregar";
        public const string SincronizarDadosAlunosFrequenciaMigracao = "sgp.migracao.frequencia.alunos.sync";
        public const string FrequenciaDaAulaExcluir = "sgp.frequencia.aula.excluir";
        public const string AnotacoesFrequenciaDaAulaExcluir = "sgp.anotacoes.frequencia.aula.excluir";
        public const string RotaConsolidacaoDiariaDashBoardFrequencia = "sgp.consolidacao.diaria.frequencia.dashboard";
        public const string RotaConsolidacaoDiariaDashBoardFrequenciaPorUe = "sgp.consolidacao.diaria.frequencia.dashboard.ue";
        public const string RotaConsolidacaoDiariaDashBoardFrequenciaPorTurma = "sgp.consolidacao.diaria.frequencia.dashboard.turma";
        public const string NotificacaoFrequencia = "sgp.notificacoes.frequencia";
        public const string NotifificarRegistroFrequencia = "sgp.registro.frequencia.notificacao";

        public const string RotaTratarCargaRegistroFrequenciaAlunoAno = "sgp.frequencia.tratar.carga.referencia.registro.aluno.ano";
        public const string RotaTratarCargaRegistroFrequenciaAlunoUe = "sgp.frequencia.tratar.carga.referencia.registro.aluno.ue";
        public const string RotaTratarCargaRegistroFrequenciaAlunoTurma = "sgp.frequencia.tratar.carga.referencia.registro.aluno.turma";
        public const string RotaTratarCargaRegistroFrequenciaAlunoAula = "sgp.frequencia.tratar.carga.referencia.registro.aluno.aula";
        public const string RotaTratarCargaRegistroFrequenciaAlunoProcessamento = "sgp.frequencia.tratar.carga.referencia.registro.aluno.processa";
        public const string RotaTratarFrequenciaRegistradaAlunosInativos = "sgp.frequencia.tratar.frequencia.registrada.alunos.inativos";
        
        public const string RotaFrequenciaLancamentoAulaSync = "sgp.frequencia.lancamento.aula.sync";
        public const string IdentificarFrequenciaAlunoPresencasMaiorTotalAulas = "sgp.frequencia.aluno.identificar.presencas.maior.total.aulas";
        public const string IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUe = "sgp.frequencia.aluno.identificar.presencas.maior.total.aulas.ue";
        public const string RegularizarFrequenciaAlunoPresencasMaiorTotalAulas = "sgp.frequencia.aluno.regularizar.presencas.maior.total.aulas";
        public const string RegularizarFrequenciaAlunoPresencasMaiorTotalAulasPorRegistro = "sgp.frequencia.aluno.regularizar.presencas.maior.total.aulas.registro";
    }
}
