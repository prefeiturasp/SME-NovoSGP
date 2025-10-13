using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAbandonoPainelEducacionalUeUseCase : IConsultasAbandonoPainelEducacionalUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasAbandonoPainelEducacionalUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PainelEducacionalAbandonoUeDto> Executar(int anoLetivo, string codigoDre, string codigoUe, int modalidade, int numeroPagina, int numeroRegistros)
        {
            if (anoLetivo <= 0)
                throw new NegocioException("O ano letivo deve ser informado.");

            if (string.IsNullOrEmpty(codigoUe))
                throw new NegocioException("O c�digo da UE deve ser informado.");

            if (modalidade <= 0)
                throw new NegocioException("A modalidade deve ser informada.");

            if (numeroPagina <= 0)
                throw new NegocioException("O n�mero da p�gina deve ser informado.");

            if (numeroRegistros <= 0)
                throw new NegocioException("O n�mero de registros por p�gina deve ser informado.");

            return await mediator.Send(new ObterAbandonoUeQuery(anoLetivo, codigoDre, codigoUe, modalidade, numeroPagina, numeroRegistros));
        }
    }
}