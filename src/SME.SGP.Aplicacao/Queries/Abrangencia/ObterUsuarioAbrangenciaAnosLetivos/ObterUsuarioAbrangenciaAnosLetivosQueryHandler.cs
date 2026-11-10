using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioAbrangenciaAnosLetivosQueryHandler : IRequestHandler<ObterUsuarioAbrangenciaAnosLetivosQuery, IEnumerable<int>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;
        private readonly IMediator mediator;

        public ObterUsuarioAbrangenciaAnosLetivosQueryHandler(IRepositorioAbrangencia repositorioAbrangencia,
                                                              IRepositorioCacheAbrangencia repositorioCacheAbrangencia,
                                                              IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new ArgumentNullException(nameof(repositorioCacheAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<int>> Handle(ObterUsuarioAbrangenciaAnosLetivosQuery request, CancellationToken cancellationToken)
        {
            Task<IEnumerable<int>> Buscar()
                => repositorioAbrangencia.ObterAnosLetivos(request.Login, request.Perfil, request.ConsideraHistorico, request.AnoMinimo);

            var configuracaoCache = request.UsarCache
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Chave do cache = todos os argumentos de RepositorioAbrangencia.ObterAnosLetivos que mudam o resultado:
            //   login, perfil, consideraHistorico, anoMinimo.
            // Ao adicionar argumento em ObterAnosLetivos, incluir aqui.
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_ANOS_LETIVOS,
                request.Login, request.Perfil, request.ConsideraHistorico, request.AnoMinimo);

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(request.Perfil));
        }
    }
}
