using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaSemestresRelatorioPAPQuery : IRequest<List<SemestreAcompanhamentoDto>>
    {
        public ObterListaSemestresRelatorioPAPQuery(int bimestreAtual)
        {
            BimestreAtual = bimestreAtual;
        }

        public int BimestreAtual { get; set; }
    }
}
