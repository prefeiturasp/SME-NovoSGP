using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase : AbstractUseCase, IGerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase
    {
        public GerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<GerarFechamentoTurmaEdFisica2020Command>();
            await mediator.Send(command);

            return true;
        }
    }
}
