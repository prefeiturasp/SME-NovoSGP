namespace SME.SGP.Infra
{
    public static class RotasRabbitFechamento
    {
        public const string ConsolidarTurmaSync = "sgp.consolidacao.turma.sync";
        public const string ConsolidarTurmaTratar = "sgp.consolidacao.turma.tratar";

        public const string ConsolidarTurmaFechamentoSync = "sgp.consolidacao.turma.fechamento.sync";
        public const string ConsolidarTurmaFechamentoComponenteTratar = "sgp.consolidacao.turma.fechamento.componente.tratar";

        public const string ConsolidarTurmaConselhoClasseSync = "sgp.consolidacao.turma.conselhoclasse.sync";
        public const string ConsolidarTurmaConselhoClasseAlunoTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.tratar";

        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.tratar"; 
        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.ue.tratar";        
        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.turma.tratar";        
        public const string ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoTratar = "sgp.consolidacao.turma.conselhoclasse.aluno.anos.anteriores.aluno.tratar";

        public const string RotaNotificacaoFechamentoReabertura = "sgp.fechamento.reabertura.notificacao";
        public const string RotaNotificacaoFechamentoReaberturaDRE = "sgp.fechamento.reabertura.notificacao.dre";
        public const string RotaNotificacaoFechamentoReaberturaUE = "sgp.fechamento.reabertura.notificacao.ue";
    }
}
