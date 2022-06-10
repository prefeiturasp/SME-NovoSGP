namespace SME.SGP.Infra
{
    public class RotasRabbitSgpFechamentoConselho
    {
        public const string RotaNotificacaoAndamentoFechamento = "sgp.fechamento.andamento.notificar";
        public const string RotaNotificacaoAndamentoFechamentoPorUe = "sgp.fechamento.andamento.notificar.ue";
        public const string RotaNotificacaoInicioFimPeriodoFechamento = "sgp.fechamento.abertura.iniciofim.periodo.notificar";
        public const string RotaNotificacaoInicioPeriodoFechamentoUE = "sgp.fechamento.abertura.inicio.periodo.notificar.ue";
        public const string RotaNotificacaoFimPeriodoFechamentoUE = "sgp.fechamento.abertura.fim.periodo.notificar.ue";
        public const string RotaNotificacaoUeFechamentosInsuficientes = "sgp.fechamento.insuficiente.notificar";
        public const string RotaNotificacaoPeriodoFechamento = "sgp.periodo.fechamento.notificar";
        public const string ConsolidarTurmaFechamentoComponenteTratar = "sgp.consolidacao.turma.fechamento.componente.tratar";
        public const string ConsolidarTurmaConselhoClasseAlunoTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.tratar";
        public const string ConsolidarTurmaFechamentoSync = "sgp.consolidacao.turma.fechamento.sync";
        public const string ConsolidarTurmaConselhoClasseSync = "sgp.consolidacao.turma.conselhoclasse.sync";
        public const string NotificacaoPeriodoFechamentoReaberturaIniciando = "sgp.periodo.fechamento.reabertura.iniciando";
        public const string NotificacaoPeriodoFechamentoReaberturaEncerrando = "sgp.periodo.fechamento.reabertura.encerrando";
        public const string NotificacaoPeriodoFechamentoReaberturaUE = "sgp.periodo.fechamento.reabertura.ue";
        public const string VerificaPendenciasFechamentoTurma = "sgp.fechamento.turma.pendencia.verificar";
    }
}
