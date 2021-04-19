using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeQueryHandler : IRequestHandler<ObterFiltroRelatoriosModalidadesPorUeQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterFiltroRelatoriosModalidadesPorUeQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosModalidadesPorUeQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigoUe == "-99")
            {
                var todasAsModalidades = EnumExtensao.ListarDto<Modalidade>();
                if (request.ModalidadesQueSeraoIgnoradas != null && request.ModalidadesQueSeraoIgnoradas.Any())
                {
                    var idsIgnoradas = request.ModalidadesQueSeraoIgnoradas.Select(a => (int)a);
                    var listaTratada = todasAsModalidades.Where(m => !idsIgnoradas.Contains(m.Id));
                    return listaTratada.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
                }
                return todasAsModalidades.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
            }

            var listaAbrangencia = await repositorioAbrangencia.ObterModalidades(request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico, request.ModalidadesQueSeraoIgnoradas);

            var modalidades = await repositorioAbrangencia.ObterModalidadesPorUe(request.CodigoUe);

            return modalidades?.Where(m => listaAbrangencia.Contains((int)m))?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
        }
    }
}
