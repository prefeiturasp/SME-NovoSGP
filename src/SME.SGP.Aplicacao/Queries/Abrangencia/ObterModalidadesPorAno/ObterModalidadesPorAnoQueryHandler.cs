using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnoQueryHandler : IRequestHandler<ObterModalidadesPorAnoQuery, IEnumerable<EnumeradoRetornoDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;
        private readonly IMediator mediator;

        public ObterModalidadesPorAnoQueryHandler(IRepositorioAbrangencia repositorioAbrangencia,
                                                  IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                                  IRepositorioCacheAbrangencia repositorioCacheAbrangencia,
                                                  IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new ArgumentNullException(nameof(repositorioCacheAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<EnumeradoRetornoDto>> Handle(ObterModalidadesPorAnoQuery request, CancellationToken cancellationToken)
        {
            var lista = await ObterModalidades(request, cancellationToken);

            if (request.Perfil == Perfis.PERFIL_SUPERVISOR)
                lista = await AcrescentarModalidadesSupervisor(request, lista);

            var listaModalidades = from a in lista.Where(x => x != 0).Distinct().ToList()
                                   select new EnumeradoRetornoDto()
                                   {
                                       Id = a,
                                       Descricao = ((Modalidade)a).GetAttribute<DisplayAttribute>().Name
                                   };

            return listaModalidades
                .OrderBy(a => a.Descricao);
        }

        private async Task<IEnumerable<int>> ObterModalidades(ObterModalidadesPorAnoQuery request, CancellationToken cancellationToken)
        {
            Task<IEnumerable<int>> Buscar()
                => repositorioAbrangencia.ObterModalidades(request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico, request.ModalidadesQueSeraoIgnoradas);

            var configuracaoCache = request.UsarCache
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Chave do cache = todos os argumentos de RepositorioAbrangencia.ObterModalidades que mudam o resultado:
            //   login, perfil, anoLetivo, consideraHistorico, modalidadesQueSeraoIgnoradas (ordenadas).
            // Ao adicionar argumento em ObterModalidades, incluir aqui.
            // O acréscimo de modalidades de supervisor feito depois (Handle) fica fora do cache, como hoje.
            var modalidadesIgnoradas = request.ModalidadesQueSeraoIgnoradas?.Select(m => (int)m);
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_MODALIDADES,
                request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico,
                modalidadesIgnoradas.ParaChaveCache());

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(request.Perfil));
        }

        private async Task<IEnumerable<int>> AcrescentarModalidadesSupervisor(ObterModalidadesPorAnoQuery request, IEnumerable<int> lista)
        {
            var dadosAbrangenciaSupervisor = await repositorioSupervisorEscolaDre
                .ObterDadosAbrangenciaSupervisor(request.Login, request.ConsideraHistorico, request.AnoLetivo);

            if (dadosAbrangenciaSupervisor.NaoEhNulo() && dadosAbrangenciaSupervisor.Any())
            {
                lista = lista.Concat(dadosAbrangenciaSupervisor
                    .Where(d => !request.ModalidadesQueSeraoIgnoradas.Contains((Modalidade)d.Modalidade))
                    .Select(d => d.Modalidade).Distinct());
            }

            return lista;
        }
    }
}