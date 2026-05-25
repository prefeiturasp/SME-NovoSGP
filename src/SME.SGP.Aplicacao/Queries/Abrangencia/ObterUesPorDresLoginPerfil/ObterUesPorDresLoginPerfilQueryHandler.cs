using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesPorDresLoginPerfilQueryHandler : IRequestHandler<ObterUesPorDresLoginPerfilQuery, IEnumerable<AbrangenciaUeComDreRetorno>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterUesPorDresLoginPerfilQueryHandler(IMediator mediator, IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaUeComDreRetorno>> Handle(ObterUesPorDresLoginPerfilQuery request, CancellationToken cancellationToken)
        {
            var anoNovosTiposUE = request.AnoLetivo > 0 ? request.AnoLetivo : DateTime.Today.Year;
            var parametroNovosTiposUE = await mediator.Send(new ObterNovosTiposUEPorAnoQuery(anoNovosTiposUE));
            var novosTiposUE = parametroNovosTiposUE?.Split(',').Select(a => int.Parse(a)).ToArray();

            return await repositorioAbrangencia.ObterUesPorListaDres(
                request.CodigosDres, request.Login, request.Perfil,
                request.Modalidade, request.Periodo, request.ConsideraHistorico,
                request.AnoLetivo, novosTiposUE);
        }
    }
}
