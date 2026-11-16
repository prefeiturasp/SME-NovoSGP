using System;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeQueryHandler : IRequestHandler<ObterFiltroRelatoriosModalidadesPorUeQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IMediator mediator;
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;

        public ObterFiltroRelatoriosModalidadesPorUeQueryHandler(IRepositorioAbrangencia repositorioAbrangencia,IMediator mediator,
                                                                 IRepositorioCacheAbrangencia repositorioCacheAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioCacheAbrangencia));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosModalidadesPorUeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CodigoUe == "-99")
                {
                    var todasAsModalidades = EnumExtensao.ListarDto<Modalidade>();
                    if (request.ModalidadesQueSeraoIgnoradas.NaoEhNulo() && request.ModalidadesQueSeraoIgnoradas.Any())
                    {
                        var idsIgnoradas = request.ModalidadesQueSeraoIgnoradas.Select(a => (int)a);
                        var listaTratada = todasAsModalidades.Where(m => !idsIgnoradas.Contains(m.Id));
                        return listaTratada.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
                    }
                    return todasAsModalidades.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
                }

                var listaAbrangencia = await ObterModalidadesAbrangencia(request, cancellationToken);

                var modalidades = await repositorioAbrangencia.ObterModalidadesPorUe(request.CodigoUe);

                return modalidades?.Where(m => listaAbrangencia.Contains((int)m))?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao executar ObterFiltroRelatoriosModalidadesPorUeQueryHandler", LogNivel.Critico, LogContexto.Geral, ex.Message));
                throw;
            }
        }

        private async Task<IEnumerable<int>> ObterModalidadesAbrangencia(ObterFiltroRelatoriosModalidadesPorUeQuery request, CancellationToken cancellationToken)
        {
            Task<IEnumerable<int>> Buscar()
                => repositorioAbrangencia.ObterModalidades(request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico, request.ModalidadesQueSeraoIgnoradas);

            var configuracaoCache = request.UsarCache
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Mesma chave do ObterModalidadesPorAnoQueryHandler (mesmos argumentos de RepositorioAbrangencia.ObterModalidades):
            //   login, perfil, anoLetivo, consideraHistorico, modalidadesQueSeraoIgnoradas (ordenadas; null ≠ vazio).
            // ObterModalidadesPorUe (query simples em turma) e a interseção (Handle) ficam fora do cache, como hoje.
            var modalidadesIgnoradas = request.ModalidadesQueSeraoIgnoradas?.Select(m => (int)m);
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_MODALIDADES,
                request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico, modalidadesIgnoradas.ParaChaveCache());

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(request.Perfil));
        }
    }
}
