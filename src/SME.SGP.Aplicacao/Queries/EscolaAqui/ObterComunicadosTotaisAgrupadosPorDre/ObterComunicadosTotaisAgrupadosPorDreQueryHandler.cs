using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisAgrupadosPorDreQueryHandler : IRequestHandler<ObterComunicadosTotaisAgrupadosPorDreQuery, IEnumerable<ComunicadosTotaisPorDreResultado>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterComunicadosTotaisAgrupadosPorDreQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<IEnumerable<ComunicadosTotaisPorDreResultado>> Handle(ObterComunicadosTotaisAgrupadosPorDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComunicado.ObterComunicadosTotaisAgrupadosPorDre(request.AnoLetivo);
        }
    }
}