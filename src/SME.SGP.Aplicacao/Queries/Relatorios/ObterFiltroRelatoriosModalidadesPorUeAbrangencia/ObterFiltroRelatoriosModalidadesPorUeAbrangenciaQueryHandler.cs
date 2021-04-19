using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
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
        private readonly IMediator mediator;

        public string CodigoUe { get; }
        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigoUe == "-99")
            {
                var todasAsModalidades = EnumExtensao.ListarDto<Modalidade>();
                if (request.ModalidadesQueSeraoIgnoradas != null && request.ModalidadesQueSeraoIgnoradas.Any()) {
                    var idsIgnoradas = request.ModalidadesQueSeraoIgnoradas.Select(a => (int)a);
                    var listaTratada = todasAsModalidades.Where(m => !idsIgnoradas.Contains(m.Id));
                    return listaTratada.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
                }
                return todasAsModalidades.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
            }

            var modalidades = await repositorioAbrangencia.ObterModalidadesPorUeAbrangencia(request.CodigoUe, request.Login, request.Perfil, request.ModalidadesQueSeraoIgnoradas);
            return modalidades?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
        }
    }
  
}
