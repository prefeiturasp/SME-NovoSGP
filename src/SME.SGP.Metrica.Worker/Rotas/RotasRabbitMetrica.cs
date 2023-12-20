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
        public const string DuplicacaoConsolidacaoCCNota = "sgp.metricas.consolidacao.cc.nota.duplicado";
        public const string LimpezaConsolidacaoCCNotaDuplicado = "sgp.metricas.consolidacao.cc.nota.duplicado.limpeza";
        public const string ConselhoClasseNaoConsolidado = "sgp.metricas.consolidacao.cc.faltante";
        public const string ConselhoClasseNaoConsolidadoUE = "sgp.metricas.consolidacao.cc.faltante.ue";
        public const string FrequenciaAlunoInconsistente = "sgp.metricas.frequencia.inconsistente";
        public const string FrequenciaAlunoInconsistenteUE = "sgp.metricas.frequencia.inconsistente.ue";
        public const string FrequenciaAlunoInconsistenteTurma = "sgp.metricas.frequencia.inconsistente.turma";
        public const string DuplicacaoFrequenciaAluno = "sgp.metricas.frequencia.aluno.duplicado";
        public const string DuplicacaoFrequenciaAlunoUE = "sgp.metricas.frequencia.aluno.duplicado.ue";
        public const string LimpezaFrequenciaAlunoDuplicado = "sgp.metricas.frequencia.aluno.duplicado.limpeza";
        public const string DuplicacaoRegistroFrequencia = "sgp.metricas.registro.frequencia.duplicado";
        public const string DuplicacaoRegistroFrequenciaUE = "sgp.metricas.registro.frequencia.duplicado.ue";
        public const string LimpezaRegistroFrequenciaDuplicado = "sgp.metricas.registro.frequencia.duplicado.limpeza";
        public const string DuplicacaoRegistroFrequenciaAluno = "sgp.metricas.registro.frequencia.aluno.duplicado";
        public const string DuplicacaoRegistroFrequenciaAlunoUE = "sgp.metricas.registro.frequencia.aluno.duplicado.ue";
        public const string DuplicacaoRegistroFrequenciaAlunoTurma = "sgp.metricas.registro.frequencia.aluno.duplicado.turma";
        public const string LimpezaRegistroFrequenciaAlunoDuplicado = "sgp.metricas.registro.frequencia.aluno.duplicado.limpeza";
        public const string ConsolidacaoFrequenciaAlunoMensalInconsistente = "sgp.metricas.consolidacao.frequencia.aluno.mensal.inconsistente";
        public const string ConsolidacaoFrequenciaAlunoMensalInconsistenteUE = "sgp.metricas.consolidacao.frequencia.aluno.mensal.inconsistente.ue";
        public const string ConsolidacaoFrequenciaAlunoMensalInconsistenteTurma = "sgp.metricas.consolidacao.frequencia.aluno.mensal.inconsistente.turma";
        public const string DuplicacaoDiarioBordo = "sgp.metricas.diario.bordo.duplicado";
        public const string RegistrosFrequenciaDiarios = "sgp.metricas.registro.frequencia.dia";
        public const string DiariosBordoDiarios = "sgp.metricas.diario.bordo.dia";
    }
}
