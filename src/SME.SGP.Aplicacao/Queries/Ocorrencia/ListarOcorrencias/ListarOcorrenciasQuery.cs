using MediatR;
using SME.SGP.Infra;

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
