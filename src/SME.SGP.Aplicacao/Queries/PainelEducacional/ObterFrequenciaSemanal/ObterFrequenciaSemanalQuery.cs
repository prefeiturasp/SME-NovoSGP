using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanal
{
    public class ObterFrequenciaSemanalQuery : IRequest<IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto>>
    {
        public ObterFrequenciaSemanalQuery(List<DateTime> dataAulas)
        {
            DataAulas = dataAulas;
        }

        public List<DateTime> DataAulas { get; set; }
    }
}
