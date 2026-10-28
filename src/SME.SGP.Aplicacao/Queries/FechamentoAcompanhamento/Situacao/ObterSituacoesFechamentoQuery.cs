using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{


    public class ObterSituacoesFechamentoQuery : IRequest<List<SituacaoDto>>
    {
        public ObterSituacoesFechamentoQuery(bool unificarNaoIniciado)
        {
            UnificarNaoIniciado = unificarNaoIniciado;
        }
        public bool UnificarNaoIniciado { get; set; }
    }


}
