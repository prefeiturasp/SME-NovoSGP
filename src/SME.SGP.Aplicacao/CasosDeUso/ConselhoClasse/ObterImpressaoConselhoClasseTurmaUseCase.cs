using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterImpressaoConselhoClasseTurmaUseCase
    {
        public static Task<bool> Executar(IMediator mediator, long conselhoClasseId, long fechamentoTurmaId)
        {
            return default;
        }
    }
}
