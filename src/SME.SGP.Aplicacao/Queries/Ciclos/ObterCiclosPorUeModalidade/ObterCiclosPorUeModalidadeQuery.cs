using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Queries.Ciclos.ObterCiclosPorUeModalidade
{
    public class ObterCiclosPorUeModalidadeQuery : IRequest<IEnumerable<CicloDto>>
    {
        public string UeCodigo { get; set; }

        public Modalidade Modalidade { get; set; }
    }
}
