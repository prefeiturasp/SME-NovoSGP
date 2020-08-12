using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosAnosPorCicloModalidadeQueryHandler : IRequestHandler<ObterFiltroRelatoriosAnosPorCicloModalidadeQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAnoEscolar repositorioAnoEscolar;
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosAnosPorCicloModalidadeQueryHandler(IRepositorioAnoEscolar  repositorioAnoEscolar, IMediator mediator)
        {
            this.repositorioAnoEscolar = repositorioAnoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioAnoEscolar));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosAnosPorCicloModalidadeQuery request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o usuário logado.");

            var listaRetorno = await repositorioAnoEscolar.ObterPorModalidadeCicloIdAbrangencia(request.Modalidade, request.CicloId, usuarioLogado.Id, usuarioLogado.PerfilAtual);
            if (!listaRetorno.Any())
                throw new NegocioException("Não foi possível localizar Anos escolares.");

            return listaRetorno.Select(a => new OpcaoDropdownDto(a.Ano.ToString(), $"{a.Ano}º ano - {a.Modalidade.Name()} ") );
        }

    
    }
}
