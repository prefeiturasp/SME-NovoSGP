using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaSyncComparativoGoogleClassroomUseCase : AbstractUseCase, IExecutaSyncComparativoGoogleClassroomUseCase
    {
        public ExecutaSyncComparativoGoogleClassroomUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar()
        {
            string mensagem = "Mensagem API Google Classroom - Comparativo de Dados";
            return await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbit.FilaComparativoGoogleSync, mensagem));
        }
    }
}
