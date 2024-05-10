using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ConsolidacaoNotaAlunoCommandHandlerFake : IRequestHandler<ConsolidacaoNotaAlunoCommand, bool>
    {
        private readonly IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase consolidadoUseCase;

        public ConsolidacaoNotaAlunoCommandHandlerFake(IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase consolidadoUseCase)
        {
            this.consolidadoUseCase = consolidadoUseCase ?? throw new ArgumentNullException(nameof(consolidadoUseCase));
        }

        public async Task<bool> Handle(ConsolidacaoNotaAlunoCommand request, CancellationToken cancellationToken)
        {
            var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(request.AlunoCodigo,
                                                                                             request.TurmaId,
                                                                                             request.Bimestre,
                                                                                             request.Inativo,
                                                                                             request.ComponenteCurricularId);


            var mensagem = JsonConvert.SerializeObject(mensagemConsolidacaoConselhoClasseAluno, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var mensagemRabbit = new MensagemRabbit(mensagem);

            await consolidadoUseCase.Executar(mensagemRabbit);

            return true;
        }
    }
}
