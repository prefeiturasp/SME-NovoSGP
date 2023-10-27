using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosUesPorAbrangenciaQueryHandler : IRequestHandler<ObterFiltroRelatoriosUesPorAbrangenciaQuery, List<AbrangenciaUeRetorno>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterFiltroRelatoriosUesPorAbrangenciaQueryHandler(IMediator mediator,
                                                                   IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<List<AbrangenciaUeRetorno>> Handle(ObterFiltroRelatoriosUesPorAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            var parametroNovosTiposUE = await mediator
                .Send(new ObterNovosTiposUEPorAnoQuery(request.AnoLetivo));

            var novosTiposUE = parametroNovosTiposUE?.Split(',')
                .Select(a => int.Parse(a)).ToArray();

            var ues = (await repositorioAbrangencia
                .ObterUes(request.CodigoDre, request.UsuarioLogado.Login, request.UsuarioLogado.PerfilAtual, ignorarTiposUE: novosTiposUE, consideraHistorico: request.ConsideraHistorico, anoLetivo: request.AnoLetivo))?
                .ToList();

            var possuiAbrangenciaEmTodasAsUes = await mediator
                .Send(new ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery(request.UsuarioLogado.PerfilAtual));

            if (ues.NaoEhNulo() && ues.Any())
                ues = ues.OrderBy(c => c.Nome).ToList();

            if (possuiAbrangenciaEmTodasAsUes && ues.Count > 1)
                ues?.Insert(0, new AbrangenciaUeRetorno { Codigo = "-99", NomeSimples = "Todas" });

            return ues;
        }
    }
}
