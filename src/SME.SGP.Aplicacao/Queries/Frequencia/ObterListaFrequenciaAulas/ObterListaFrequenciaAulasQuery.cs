using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaFrequenciaAulasQuery : IRequest<RegistroFrequenciaPorDataPeriodoDto>
    {
        public ObterListaFrequenciaAulasQuery(FiltroFrequenciaAulasDto filtro)
        {
            Turma = filtro.Turma;
            AlunosDaTurma = filtro.AlunosDaTurma;
            Aulas = filtro.Aulas;
            FrequenciaAlunos = filtro.FrequenciaAlunos;
            RegistrosFrequenciaAlunos = filtro.RegistrosFrequenciaAlunos;
            AnotacoesTurma = filtro.AnotacoesTurma;
            FrequenciasPreDefinidas = filtro.FrequenciasPreDefinidas;
            CompensacaoAusenciaAlunoAulas = filtro.CompensacaoAusenciaAlunoAulas;
            PeriodoEscolar = filtro.PeriodoEscolar;
            RegistraFrequencia = filtro.RegistraFrequencia;
            TurmaPossuiFrequenciaRegistrada = filtro.TurmaPossuiFrequenciaRegistrada;
            DataInicio = filtro.DataInicio;
            DataFim = filtro.DataFim;
            PercentualAlerta = filtro.PercentualAlerta;
            PercentualCritico = filtro.PercentualCritico;
        }

        public Turma Turma { get; }
        public IEnumerable<AlunoPorTurmaResposta> AlunosDaTurma { get; }
        public IEnumerable<Aula> Aulas { get; }
        public IEnumerable<FrequenciaAluno> FrequenciaAlunos { get; }
        public IEnumerable<RegistroFrequenciaAlunoPorAulaDto> RegistrosFrequenciaAlunos { get; }
        public IEnumerable<AnotacaoAlunoAulaDto> AnotacoesTurma { get; }
        public IEnumerable<FrequenciaPreDefinidaDto> FrequenciasPreDefinidas { get; }
        public IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto> CompensacaoAusenciaAlunoAulas { get; }
        public PeriodoEscolar PeriodoEscolar { get; }
        public bool RegistraFrequencia { get; }
        public bool TurmaPossuiFrequenciaRegistrada { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
        public int PercentualAlerta { get; }
        public int PercentualCritico { get; }
    }
}
