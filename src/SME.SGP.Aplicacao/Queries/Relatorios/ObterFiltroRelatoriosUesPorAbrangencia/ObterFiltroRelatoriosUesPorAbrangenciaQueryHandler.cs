using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
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
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;

        public ObterFiltroRelatoriosUesPorAbrangenciaQueryHandler(IMediator mediator,
                                                                   IRepositorioAbrangencia repositorioAbrangencia,
                                                                   IRepositorioCacheAbrangencia repositorioCacheAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioCacheAbrangencia));
        }

        public async Task<List<AbrangenciaUeRetorno>> Handle(ObterFiltroRelatoriosUesPorAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            var parametroNovosTiposUE = await mediator
                .Send(new ObterNovosTiposUEPorAnoQuery(request.AnoLetivo));

            var novosTiposUE = parametroNovosTiposUE?.Split(',')
                .Select(a => int.Parse(a)).ToArray();

            var ues = (await ObterUes(request, novosTiposUE, cancellationToken))?
                .ToList();

            var possuiAbrangenciaEmTodasAsUes = await mediator
                .Send(new ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery(request.UsuarioLogado.PerfilAtual));

            if (ues != null && ues.Any())
                ues = ues.OrderBy(c => c.Nome).ToList();

            if (possuiAbrangenciaEmTodasAsUes && ues?.Count > 1)
                ues?.Insert(0, new AbrangenciaUeRetorno { Codigo = "-99", NomeSimples = "Todas" });

            return ues;
        }

        private async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(ObterFiltroRelatoriosUesPorAbrangenciaQuery request, int[] novosTiposUE, CancellationToken cancellationToken)
        {
            var login = request.UsuarioLogado.Login;
            var perfil = request.UsuarioLogado.PerfilAtual;

            Task<IEnumerable<AbrangenciaUeRetorno>> Buscar()
                => repositorioAbrangencia.ObterUes(request.CodigoDre, login, perfil,
                    ignorarTiposUE: novosTiposUE,
                    filtroPeriodoLetivo: new FiltroPeriodoLetivo(request.AnoLetivo, request.ConsideraHistorico));

            var configuracaoCache = request.UsarCache
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Mesma chave do ObterUEsPorDREQueryHandler (mesmos argumentos de RepositorioAbrangencia.ObterUes):
            //   login, perfil, codigoDre, modalidade (0: não informada), anoLetivo, consideraHistorico, periodo (0),
            //   ignorarTiposUE (ordenados). Sem filtro de texto e sem anosTurma neste caminho.
            // Ordenação, "Todas" e ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery (Handle) ficam fora do cache, como hoje.
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_UES,
                login, perfil, request.CodigoDre, 0, request.AnoLetivo, request.ConsideraHistorico, 0, novosTiposUE.ParaChaveCache());

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(perfil));
        }
    }
}
