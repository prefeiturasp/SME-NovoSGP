using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Relatorios.ObterFiltroRelatoriosDresPorAbrangencia
{
    public class ObterFiltroRelatoriosDresPorAbrangenciaQueryHandler : IRequestHandler<ObterFiltroRelatoriosDresPorAbrangenciaQuery, IEnumerable<AbrangenciaDreRetorno>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterFiltroRelatoriosDresPorAbrangenciaQueryHandler(IMediator mediator,
                                                                   IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaDreRetorno>> Handle(ObterFiltroRelatoriosDresPorAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            var dres = (await repositorioAbrangencia.ObterDres(request.UsuarioLogado.Login, request.UsuarioLogado.PerfilAtual))?.ToList();
            var possuiAbrangenciaEmTodasAsDres = await mediator.Send(new ObterUsuarioPossuiAbrangenciaEmTodasAsDresQuery(request.UsuarioLogado.PerfilAtual));
            if (possuiAbrangenciaEmTodasAsDres)
            {
                dres?.Insert(0, new AbrangenciaDreRetorno { Abreviacao = "Todas", Codigo = "-99", Nome = "Todas" });
            }
            return dres;
        }
    }
}
