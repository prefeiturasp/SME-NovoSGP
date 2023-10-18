using MediatR;
using SME.SGP.Dominio;
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

        public ObterModalidadesPorAnoQueryHandler(IRepositorioAbrangencia repositorioAbrangencia,
                                                  IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<EnumeradoRetornoDto>> Handle(ObterModalidadesPorAnoQuery request, CancellationToken cancellationToken)
        {
            var lista = await repositorioAbrangencia
                .ObterModalidades(request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico, request.ModalidadesQueSeraoIgnoradas);

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