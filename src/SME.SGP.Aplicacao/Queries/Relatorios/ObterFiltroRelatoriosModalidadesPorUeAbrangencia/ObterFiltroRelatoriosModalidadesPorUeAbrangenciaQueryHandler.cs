using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler :  IRequestHandler<ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;
        private readonly IMediator mediator;

        public string CodigoUe { get; }
        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                                                            IRepositorioCacheAbrangencia repositorioCacheAbrangencia, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new ArgumentNullException(nameof(repositorioCacheAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigoUe == "-99")
            {
                var todasAsModalidades = EnumExtensao.ListarDto<Modalidade>();

                if (request.ModalidadesQueSeraoIgnoradas.NaoEhNulo() && request.ModalidadesQueSeraoIgnoradas.Any()) {
                    var idsIgnoradas = request.ModalidadesQueSeraoIgnoradas.Select(a => (int)a);
                    var listaTratada = todasAsModalidades.Where(m => !idsIgnoradas.Contains(m.Id));
                    return listaTratada.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
                }
                return todasAsModalidades.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
            }

            var modalidades = await ObterModalidadesPorUeAbrangencia(request, cancellationToken);

            if (request.Perfil == Perfis.PERFIL_SUPERVISOR)
                modalidades = await AcrescentarModalidadesSupervisor(request, modalidades);

            return modalidades?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
        }
        private async Task<IEnumerable<Modalidade>> ObterModalidadesPorUeAbrangencia(ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            Task<IEnumerable<Modalidade>> Buscar()
                => repositorioAbrangencia.ObterModalidadesPorUeAbrangencia(request.CodigoUe, request.Login, request.Perfil, request.ModalidadesQueSeraoIgnoradas, request.ConsideraHistorico, request.AnoLetivo);

            var configuracaoCache = request.UsarCache
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Chave do cache = todos os argumentos de RepositorioAbrangencia.ObterModalidadesPorUeAbrangencia que mudam o resultado:
            //   login, perfil, codigoUe, consideraHistorico, anoLetivo, modalidadesQueSeraoIgnoradas (ordenadas; null ≠ vazio).
            // Ao adicionar argumento em ObterModalidadesPorUeAbrangencia, incluir aqui.
            // O acréscimo de modalidades de supervisor (Handle) fica fora do cache, como hoje.
            var modalidadesIgnoradas = request.ModalidadesQueSeraoIgnoradas?.Select(m => (int)m);
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_MODALIDADES_UE,
                request.Login, request.Perfil, request.CodigoUe, request.ConsideraHistorico, request.AnoLetivo, modalidadesIgnoradas.ParaChaveCache());

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(request.Perfil));
        }

        private async Task<IEnumerable<Modalidade>> AcrescentarModalidadesSupervisor(ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery request, IEnumerable<Modalidade> lista)
        {
            var dadosAbrangenciaSupervisor = await repositorioSupervisorEscolaDre
                .ObterDadosAbrangenciaSupervisor(request.Login, request.ConsideraHistorico, request.AnoLetivo, request.CodigoUe);

            if (dadosAbrangenciaSupervisor.NaoEhNulo() && dadosAbrangenciaSupervisor.Any())
            {
                lista = lista.Select(m => (int)m)
                .Union(dadosAbrangenciaSupervisor
                    .Where(d => !request.ModalidadesQueSeraoIgnoradas.Contains((Modalidade)d.Modalidade))
                    .Select(d => d.Modalidade)
                    .Distinct())
                .Select(m => (Modalidade)m);
            }

            return lista;
        }
    }
  
}
