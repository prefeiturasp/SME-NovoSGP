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
                return EnumExtensao.ListarDto<Modalidade>().Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
            }

            var modalidades = await repositorioAbrangencia.ObterModalidadesPorUe(request.CodigoUe);
            
            return modalidades?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
        }
    }
}
