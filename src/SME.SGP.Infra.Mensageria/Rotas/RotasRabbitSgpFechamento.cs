namespace SME.SGP.Infra
{
    public static class RotasRabbitSgpFechamento
    {
        public const string ConsolidarTurmaFechamentoSync = "sgp.consolidacao.turma.fechamento.sync";
        public const string ConsolidarTurmaFechamentoComponenteTratar = "sgp.consolidacao.turma.fechamento.componente.tratar";
        public const string ConsolidarDreConselhoClasseSync = "sgp.consolidacao.dre.conselhoclasse.sync";
        public const string ConsolidarUeConselhoClasseSync = "sgp.consolidacao.ue.conselhoclasse.sync";
        public const string ConsolidarTurmaConselhoClasseSync = "sgp.consolidacao.turma.conselhoclasse.sync";
        public const string ConsolidarTurmaConselhoClasseAlunoTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.tratar";
        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.tratar"; 
        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.ue.tratar";        
        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.turma.tratar";        
        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.aluno.tratar";
        public const string RotaAtualizarParecerConclusivoAlunoPorDre = "sgp.conselho.classe.aluno.parecer.atualizar.dre";
        public const string RotaAtualizarParecerConclusivoAlunoPorUe = "sgp.conselho.classe.aluno.parecer.atualizar.ue";
        public const string RotaAtualizarParecerConclusivoAlunoPorTurma = "sgp.conselho.classe.aluno.parecer.atualizar.turma";
        public const string RotaAtualizarParecerConclusivoAluno = "sgp.conselho.classe.aluno.parecer.atualizar";
        public const string RotaExecutaAtualizacaoSituacaoConselhoClasse = "sgp.conselho.classe.situacao.atualizar";
        public const string NotificacaoPeriodoFechamentoReaberturaIniciando = "sgp.periodo.fechamento.reabertura.iniciando";
        public const string NotificacaoPeriodoFechamentoReaberturaEncerrando = "sgp.periodo.fechamento.reabertura.encerrando";
        public const string NotificacaoPeriodoFechamentoReaberturaUE = "sgp.periodo.fechamento.reabertura.ue"; // TODO [Fernando Groeler] Não localizei referência dessa fila
        public const string VerificaPendenciasFechamentoTurma = "sgp.fechamento.turma.pendencia.verificar";
        public const string RotaExecutaVerificacaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.verificacao";
        public const string RotaExecutaExclusaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.excluir";
        public const string RotaNotificacaoAndamentoFechamento = "sgp.fechamento.andamento.notificar";
        public const string RotaNotificacaoAndamentoFechamentoPorUe = "sgp.fechamento.andamento.notificar.ue";
        public const string RotaNotificacaoInicioFimPeriodoFechamento = "sgp.fechamento.abertura.iniciofim.periodo.notificar";
        public const string RotaNotificacaoInicioPeriodoFechamentoUE = "sgp.fechamento.abertura.inicio.periodo.notificar.ue";
        public const string RotaNotificacaoFimPeriodoFechamentoUE = "sgp.fechamento.abertura.fim.periodo.notificar.ue";
        public const string RotaGeracaoPendenciasFechamento = "sgp.fechamento.pendencias.gerar";
        public const string RotaGeracaoFechamentoEdFisica2020 = "sgp.fechamento.turmas.edfisica.2020";
        public const string RotaGeracaoFechamentoEdFisica2020AlunosTurma = "sgp.fechamento.turmas.edfisica.2020.alunos.turma";
        public const string RotaNotificacaoUeFechamentosInsuficientes = "sgp.fechamento.insuficiente.notificar";
        public const string RotaNotificacaoPeriodoFechamento = "sgp.periodo.fechamento.notificar"; // TODO [Fernando Groeler] Não localizei referência dessa fila
        public const string ConsolidarTurmaSync = "sgp.consolidacao.turma.sync";
        public const string ConsolidarTurmaTratar = "sgp.consolidacao.turma.tratar";
        public const string RotaNotificacaoFechamentoReabertura = "sgp.fechamento.reabertura.notificacao";
        public const string RotaNotificacaoFechamentoReaberturaDRE = "sgp.fechamento.reabertura.notificacao.dre";
        public const string RotaNotificacaoFechamentoReaberturaUE = "sgp.fechamento.reabertura.notificacao.ue";
        public const string VarreduraFechamentosTurmaDisciplinaEmProcessamentoPendentes = "sgp.fechamento.turma.disciplina.processamento.varredura";
        public const string GerarNotificacaoAlteracaoLimiteDias = "sgp.gerar.notificacao.alteracao.limite.dias";
        public const string VerificarPendenciasFechamentoTurmaDisciplina = "sgp.verificar.pendencias.fechamento.turma.disciplina";
        public const string AlterarPeriodosComHierarquiaInferiorFechamento = "sgp.alterar.periodo.hierarquia.inferior.fechamento";
        public const string RotaNotificacaoResultadoInsatisfatorio = "sgp.notificacao.nova.resultado.insatisfatorio";
        public const string RotaNotificacaoAprovacaoFechamento = "sgp.fechamento.nota.aprovacao.notificar";
        public const string RotaNotificacaoAprovacaoFechamentoPorTurma = "sgp.fechamento.nota.aprovacao.notificar.turma";
        public const string RotaNotificacaoAprovacaoNotaPosConselho = "sgp.conselho.classe.nota.pos.conselho.aprovacao.notificar";
        public const string RotaNotificacaoAprovacaoParecerConclusivoConselhoClasse = "sgp.conselho.classe.parecer.conclusivo.aprovacao.notificar";
    }
}
