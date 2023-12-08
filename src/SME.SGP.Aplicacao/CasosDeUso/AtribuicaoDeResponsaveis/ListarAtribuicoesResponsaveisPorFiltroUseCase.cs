using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAtribuicoesResponsaveisPorFiltroUseCase : AbstractUseCase, IListarAtribuicoesResponsaveisPorFiltroUseCase
    {
        public ListarAtribuicoesResponsaveisPorFiltroUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<IEnumerable<AtribuicaoResponsavelDto>> Executar(AtribuicaoResponsaveisFiltroDto filtroDto)
        {
            return await mediator.Send(new ObterAtribuicaoResponsaveisPorUeTipoQuery(filtroDto.UeCodigo, filtroDto.Tipo));
        }
    }
}
