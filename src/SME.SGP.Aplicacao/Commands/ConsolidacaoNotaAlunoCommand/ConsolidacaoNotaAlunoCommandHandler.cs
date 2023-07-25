using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoNotaAlunoCommandHandler : IRequestHandler<ConsolidacaoNotaAlunoCommand, bool>
    {
        private readonly IMediator mediator;

        public ConsolidacaoNotaAlunoCommandHandler(IMediator mediator, IRepositorioConselhoClasseNota repositorioConselhoClasseNota)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ConsolidacaoNotaAlunoCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommand(request.AlunoCodigo, request.Bimestre ?? 0, request.TurmaId, request.ComponenteCurricularId,
                                                                                             request.Nota, request.ConceitoId,
                                                                                             request.ConselhoClasse ? TipoAlteracao.ConselhoClasseNota : TipoAlteracao.FechamentoNota));

            var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(request.AlunoCodigo, 
                                                                                                         request.TurmaId, 
                                                                                                         request.Bimestre, 
                                                                                                         request.Inativo,
                                                                                                         request.ComponenteCurricularId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemConsolidacaoConselhoClasseAluno, Guid.NewGuid(), null));

            return true;
        }
    }
}
