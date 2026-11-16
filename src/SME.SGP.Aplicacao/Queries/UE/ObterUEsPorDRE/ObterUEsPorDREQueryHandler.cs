using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.UE.ObterUEsPorDRE
{
    public class
        ObterUEsPorDREQueryHandler : IRequestHandler<ObterUEsPorDREQuery, IEnumerable<AbrangenciaUeRetorno>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;

        public ObterUEsPorDREQueryHandler(IMediator mediator, IRepositorioAbrangencia repositorioAbrangencia, IRepositorioCacheAbrangencia repositorioCacheAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioCacheAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> Handle(ObterUEsPorDREQuery request, CancellationToken cancellationToken)
        {
            var anoNovosTiposUE = ObterAno(request.ConsideraNovasUEs, request.AnoLetivo);
            var parametroNovosTiposUE = await mediator.Send(new ObterNovosTiposUEPorAnoQuery(anoNovosTiposUE));
            var novosTiposUE = parametroNovosTiposUE?.Split(',').Select(a => int.Parse(a)).ToArray();

            var ues = await ObterUes(request, novosTiposUE, cancellationToken);

            if (request.FiltrarTipoEscolaPorAnoLetivo && request.AnoLetivo <= 2020)
                ues = ues.Where(u => !u.EhInfantil);

            return ues.OrderBy(c => c.Nome);
        }

        private async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(ObterUEsPorDREQuery request, int[] novosTiposUE, CancellationToken cancellationToken)
        {
            Task<IEnumerable<AbrangenciaUeRetorno>> Buscar()
                => repositorioAbrangencia.ObterUes(request.CodigoDre, request.Login, request.Perfil,
                    new FiltroModalidade(request.Modalidade ?? 0, request.AnosTurma),
                    new FiltroPeriodoLetivo(request.AnoLetivo, request.ConsideraHistorico, request.Periodo),
                    novosTiposUE,
                    new FiltroTexto(request.Filtro, request.FiltroEhCodigo));

            var podeUsarCache = request.UsarCache
                                && string.IsNullOrWhiteSpace(request.Filtro)
                                && !(request.AnosTurma?.Any() ?? false);

            var configuracaoCache = podeUsarCache
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Chave do cache = todos os argumentos de RepositorioAbrangencia.ObterUes que mudam o resultado:
            //   codigoDre, login, perfil, modalidade, anoLetivo, consideraHistorico, periodo, ignorarTiposUE (ordenados).
            // Sem cache quando: filtro de texto ou anosTurma preenchidos.
            // Ao adicionar argumento em ObterUes, incluir aqui ou na regra de "sem cache".
            // Remoção de infantil (<= 2020) e ordenação, feitas depois (Handle), ficam fora do cache, como hoje.
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_UES,
                request.Login, request.Perfil, request.CodigoDre, (int)(request.Modalidade ?? 0),
                request.AnoLetivo, request.ConsideraHistorico, request.Periodo, novosTiposUE.ParaChaveCache());

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(request.Perfil));
        }

        private int ObterAno(bool consideraNovasUEs, int anoLetivo)
        {
            var ano = anoLetivo > 0 ? anoLetivo : DateTime.Today.Year;
            return consideraNovasUEs ? ano + 1 : ano;
        }
    }
}
