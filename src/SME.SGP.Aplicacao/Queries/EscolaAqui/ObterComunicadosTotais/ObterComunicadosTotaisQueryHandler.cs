using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisQueryHandler : IRequestHandler<ObterComunicadosTotaisQuery, ComunicadosTotaisResultado>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterComunicadosTotaisQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<ComunicadosTotaisResultado> Handle(ObterComunicadosTotaisQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComunicado.ObterComunicadosTotaisSme(request.AnoLetivo, request.CodigoDre, request.CodigoUe);
        }
    }
}