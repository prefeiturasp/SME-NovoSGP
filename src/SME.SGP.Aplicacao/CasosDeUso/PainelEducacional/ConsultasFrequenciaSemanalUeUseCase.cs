using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanalUe;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasFrequenciaSemanalUeUseCase : IConsultasFrequenciaSemanalUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasFrequenciaSemanalUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>> ObterFrequenciaSemanalUe(string codigoUe, int anoLetivo)
        {
            return await mediator.Send(new ObterFrequenciaSemanalUeQuery(codigoUe, anoLetivo));
        }
    }
}
