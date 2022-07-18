using MediatR;
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
                                                                                             request.Nota,
                                                                                             request.ConceitoId,
                                                                                             request.ComponenteCurricularId);

            var mensagemRabbit = new MensagemRabbit(mensagemConsolidacaoConselhoClasseAluno);

            await this.consolidadoUseCase.Executar(mensagemRabbit);

            return true;
        }
    }
}
