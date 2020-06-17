using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public static class TurmaEmPeriodoAbertoUseCase
    {
        public static async Task<bool> Executar(IMediator mediator, string turmaCodigo, DateTime dataReferencia, int bimestre, bool ehAnoLetivo)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de codigo [{turmaCodigo}] não localizada!");

            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, dataReferencia, bimestre, ehAnoLetivo));
        }
    }
}
