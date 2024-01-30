using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroFrequenciaAulasDto
    {
        public Turma Turma { get; set; }
        public IEnumerable<AlunoPorTurmaResposta> AlunosDaTurma { get; set; }
        public IEnumerable<Aula> Aulas { get; set; }
        public IEnumerable<FrequenciaAluno> FrequenciaAlunos { get; set; }
        public IEnumerable<RegistroFrequenciaAlunoPorAulaDto> RegistrosFrequenciaAlunos { get; set; }
        public IEnumerable<AnotacaoAlunoAulaDto> AnotacoesTurma { get; set; }
        public IEnumerable<FrequenciaPreDefinidaDto> FrequenciasPreDefinidas { get; set; }
        public IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto> CompensacaoAusenciaAlunoAulas { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool TurmaPossuiFrequenciaRegistrada { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int PercentualAlerta { get; set; }
        public int PercentualCritico { get; set; }
    }
}
