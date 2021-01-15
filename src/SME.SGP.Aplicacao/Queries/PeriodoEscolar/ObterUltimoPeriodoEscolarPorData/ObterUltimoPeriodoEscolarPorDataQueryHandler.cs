using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoPeriodoEscolarPorDataQueryHandler : IRequestHandler<ObterUltimoPeriodoEscolarPorDataQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterUltimoPeriodoEscolarPorDataQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<PeriodoEscolar> Handle(ObterUltimoPeriodoEscolarPorDataQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterUltimoPeriodoEscolarPorData(request.AnoLetivo, request.ModalidadeTipoCalendario, request.DataAtual);
        }
    }
}
