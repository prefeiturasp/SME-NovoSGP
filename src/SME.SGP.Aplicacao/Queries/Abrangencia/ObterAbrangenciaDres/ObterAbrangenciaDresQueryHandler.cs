using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresQueryHandler : IRequestHandler<ObterAbrangenciaDresQuery, IEnumerable<AbrangenciaDreRetornoDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;
        private readonly IMediator mediator;

        public ObterAbrangenciaDresQueryHandler(IRepositorioAbrangencia repositorioAbrangencia,
                                                IRepositorioCacheAbrangencia repositorioCacheAbrangencia,
                                                IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new ArgumentNullException(nameof(repositorioCacheAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> Handle(ObterAbrangenciaDresQuery request, CancellationToken cancellationToken)
        {
            Task<IEnumerable<AbrangenciaDreRetornoDto>> Buscar()
                => repositorioAbrangencia.ObterDres(request.Login, request.Perfil,
                    request.Modalidade, request.Periodo,
                    request.ConsideraHistorico,
                    request.AnoLetivo, request.Filtro, request.FiltroEhCodigo);

            var configuracaoCache = request.UsarCache && string.IsNullOrWhiteSpace(request.Filtro)
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Chave do cache = todos os argumentos de RepositorioAbrangencia.ObterDres que mudam o resultado:
            //   login, perfil, modalidade (null = 0, como no repositório), periodo, consideraHistorico, anoLetivo.
            // Sem cache quando: filtro de texto preenchido (filtroEhCodigo só vale com filtro).
            // Ao adicionar argumento em ObterDres, incluir aqui ou na regra de "sem cache".
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_DRES,
                request.Login, request.Perfil, (int)(request.Modalidade ?? 0), request.Periodo,
                request.ConsideraHistorico, request.AnoLetivo);

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(request.Perfil));
        }
    }
}
