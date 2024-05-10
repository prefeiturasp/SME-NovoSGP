using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

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
