using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAtribuicaoEsporadicaPorFiltrosQueryHandler : IRequestHandler<ListarAtribuicaoEsporadicaPorFiltrosQuery, PaginacaoResultadoDto<AtribuicaoEsporadica>>
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;

        public ListarAtribuicaoEsporadicaPorFiltrosQueryHandler(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadica>> Handle(ListarAtribuicaoEsporadicaPorFiltrosQuery request, CancellationToken cancellationToken)
        {
            var retorno = await repositorioAtribuicaoEsporadica.ListarPaginada(request.Paginacao, request.AnoLetivo, request.DreId, request.UeId, request.ProfessorRF);

            return retorno;
        }
    }
}
