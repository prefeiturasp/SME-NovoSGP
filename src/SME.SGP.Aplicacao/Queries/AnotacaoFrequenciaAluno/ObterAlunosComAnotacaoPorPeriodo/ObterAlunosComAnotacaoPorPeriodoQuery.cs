using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComAnotacaoPorPeriodoQuery : IRequest<IEnumerable<AnotacaoAlunoAulaDto>>
    {
        public ObterAlunosComAnotacaoPorPeriodoQuery(string turmaCodigo, DateTime dataInicio, DateTime dataFim)
        {
            TurmaCodigo = turmaCodigo;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string TurmaCodigo { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
    }
}
