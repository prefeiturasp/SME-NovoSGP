using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterImpressaoConselhoClasseAlunoUseCase
    {
        public static async Task<bool> Executar(IMediator mediator, long conselhoClasseAlunoId, string alunoCodigo)
        {
            return default;
        }
    }
}
