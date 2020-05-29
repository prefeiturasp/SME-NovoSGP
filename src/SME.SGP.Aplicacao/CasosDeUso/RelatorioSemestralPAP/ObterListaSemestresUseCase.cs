using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaSemestresUseCase
    {
        public static async Task<List<SemestreAcompanhamentoDto>> Executar(IMediator mediator, string turmaCodigo)
        {
            var bimestreAtual = await mediator.Send(new ObterBimestreAtualQuery(turmaCodigo, DateTime.Today));

            return await mediator.Send(new ObterListaSemestresRelatorioPAPQuery(bimestreAtual));
        }
    }
}
