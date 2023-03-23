using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaFrequenciaAulasQuery : IRequest<RegistroFrequenciaPorDataPeriodoDto>
    {
        public ObterListaFrequenciaAulasQuery(Turma turma,
                                              IEnumerable<AlunoPorTurmaResposta> alunosDaTurma,
                                              IEnumerable<Aula> aulas,
                                              IEnumerable<FrequenciaAluno> frequenciaAlunos,
                                              IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos,
                                              IEnumerable<AnotacaoAlunoAulaDto> anotacoesTurma,
                                              IEnumerable<FrequenciaPreDefinidaDto> frequenciasPreDefinidas,
                                              PeriodoEscolar periodoEscolar,
                                              bool registraFrequencia,
                                              bool turmaPossuiFrequenciaRegistrada,
                                              DateTime dataInicio,
                                              DateTime dataFim,
                                              int percentualAlerta,
                                              int percentualCritico)
        {
            Turma = turma;
            AlunosDaTurma = alunosDaTurma;
            Aulas = aulas;
            FrequenciaAlunos = frequenciaAlunos;
            RegistrosFrequenciaAlunos = registrosFrequenciaAlunos;
            AnotacoesTurma = anotacoesTurma;
            FrequenciasPreDefinidas = frequenciasPreDefinidas;
            PeriodoEscolar = periodoEscolar;
            RegistraFrequencia = registraFrequencia;
            TurmaPossuiFrequenciaRegistrada = turmaPossuiFrequenciaRegistrada;
            DataInicio = dataInicio;
            DataFim = dataFim;
            PercentualAlerta = percentualAlerta;
            PercentualCritico = percentualCritico;
        }

        public Turma Turma { get; }
        public IEnumerable<AlunoPorTurmaResposta> AlunosDaTurma { get; }
        public IEnumerable<Aula> Aulas { get; }
        public IEnumerable<FrequenciaAluno> FrequenciaAlunos { get; }
        public IEnumerable<RegistroFrequenciaAlunoPorAulaDto> RegistrosFrequenciaAlunos { get; }
        public IEnumerable<AnotacaoAlunoAulaDto> AnotacoesTurma { get; }
        public IEnumerable<FrequenciaPreDefinidaDto> FrequenciasPreDefinidas { get; }
        public PeriodoEscolar PeriodoEscolar { get; }
        public bool RegistraFrequencia { get; }
        public bool TurmaPossuiFrequenciaRegistrada { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
        public int PercentualAlerta { get; }
        public int PercentualCritico { get; }
    }
}
