using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Armazenamento.ObterComprimir
{
    public class ObterComprimirQuery : IRequest<IEnumerable<Arquivo>>
    {
        public ObterComprimirQuery(DateTime dataInicio, DateTime dataFim)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
