using System;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasDetalhadasPorDataQuery : IRequest<IEnumerable<FrequenciaDetalhadaPorDataDto>>
    {
        public ObterFrequenciasDetalhadasPorDataQuery(string codigoAluno, DateTime dataInicio, DateTime dataFim)
        {
            CodigoAluno = codigoAluno;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string CodigoAluno { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
