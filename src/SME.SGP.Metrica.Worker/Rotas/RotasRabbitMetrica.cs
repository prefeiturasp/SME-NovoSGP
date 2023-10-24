namespace SME.SGP.Metrica.Worker.Rotas
{
    public static class RotasRabbitMetrica
    {
        public const string AcessosSGP = "sgp.metricas.acessos";
        public const string DuplicacaoConselhoClasse = "sgp.metricas.conselho.classe.duplicado";
        public const string LimpezaConselhoClasseDuplicado = "sgp.metricas.conselho.classe.duplicado.limpeza";
        public const string DuplicacaoConselhoClasseAluno = "sgp.metricas.conselho.classe.aluno.duplicado";
        public const string DuplicacaoConselhoClasseAlunoUe = "sgp.metricas.conselho.classe.aluno.duplicado.ue";
        public const string LimpezaConselhoClasseAlunoDuplicado = "sgp.metricas.conselho.classe.aluno.duplicado.limpeza";
        public const string DuplicacaoConselhoClasseNota = "sgp.metricas.conselho.classe.nota.duplicado";
        public const string LimpezaConselhoClasseNotaDuplicado = "sgp.metricas.conselho.classe.nota.duplicado.limpeza";
        public const string DuplicacaoFechamentoTurma = "sgp.metricas.fechamento.turma.duplicado";
        public const string LimpezaFechamentoTurmaDuplicado = "sgp.metricas.fechamento.turma.duplicado.limpeza";
        public const string DuplicacaoFechamentoTurmaDisciplina = "sgp.metricas.fechamento.disciplina.duplicado";
        public const string LimpezaFechamentoTurmaDisciplinaDuplicado = "sgp.metricas.fechamento.disciplina.duplicado.limpeza";
        public const string DuplicacaoFechamentoAluno = "sgp.metricas.fechamento.aluno.duplicado";
        public const string DuplicacaoFechamentoAlunoUE = "sgp.metricas.fechamento.aluno.duplicado.ue";
        public const string LimpezaFechamentoAlunoDuplicado = "sgp.metricas.fechamento.aluno.duplicado.limpeza";
        public const string DuplicacaoFechamentoNota = "sgp.metricas.fechamento.nota.duplicado";
        public const string DuplicacaoFechamentoNotaTurma = "sgp.metricas.fechamento.nota.duplicado.turma";
        public const string LimpezaFechamentoNotaDuplicado = "sgp.metricas.fechamento.nota.duplicado.limpeza";
        public const string ConsolidacaoCCNotaNulo = "sgp.metricas.consolidacao.cc.nota.nulo";
        public const string DuplicacaoConsolidacaoCCAlunoTurma = "sgp.metricas.consolidacao.cc.aluno.turma.duplicado";
        public const string DuplicacaoConsolidacaoCCAlunoTurmaUE = "sgp.metricas.consolidacao.cc.aluno.turma.duplicado.ue";
        public const string LimpezaConsolidacaoCCAlunoTurmaDuplicado = "sgp.metricas.consolidacao.cc.aluno.turma.duplicado.limpeza";
    }
}
