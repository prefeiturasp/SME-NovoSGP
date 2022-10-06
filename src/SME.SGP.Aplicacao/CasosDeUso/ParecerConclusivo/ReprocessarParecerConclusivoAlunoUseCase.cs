using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Commands;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarParecerConclusivoAlunoUseCase : AbstractUseCase, IReprocessarParecerConclusivoAlunoUseCase
    {
        public ReprocessarParecerConclusivoAlunoUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registro = param.ObterObjetoMensagem<ConselhoClasseFechamentoAlunoDto>();

            await mediator.Send(new GerarParecerConclusivoPorConselhoFechamentoAlunoCommand(registro.ConselhoClasseId, registro.FechamentoTurmaId, registro.AlunoCodigo));

            return true;
        }
    }
}
