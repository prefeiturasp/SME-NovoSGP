using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasQuery : IRequest<PaginacaoResultadoDto<OcorrenciaListagemDto>>
    {
        public ListarOcorrenciasQuery(FiltroOcorrenciaListagemDto filtro)
        {
            Filtro = filtro;
        }

        public FiltroOcorrenciaListagemDto Filtro { get; set; }
    }
}
