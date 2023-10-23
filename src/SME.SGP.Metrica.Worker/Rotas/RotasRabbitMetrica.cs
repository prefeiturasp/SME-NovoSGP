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
    }
}
