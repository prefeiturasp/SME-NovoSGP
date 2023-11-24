using FluentValidation;
using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler :  IRequestHandler<ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public string CodigoUe { get; }
        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
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

            var modalidades = await repositorioAbrangencia
                .ObterModalidadesPorUeAbrangencia(request.CodigoUe, request.Login, request.Perfil, request.ModalidadesQueSeraoIgnoradas, request.ConsideraHistorico, request.AnoLetivo);

            if (request.Perfil == Perfis.PERFIL_SUPERVISOR)
                modalidades = await AcrescentarModalidadesSupervisor(request, modalidades);

            return modalidades?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
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
