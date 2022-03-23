using MediatR;
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
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;

        public ConsolidacaoNotaAlunoCommandHandler(IMediator mediator, IRepositorioConselhoClasseNota repositorioConselhoClasseNota)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        public async Task<bool> Handle(ConsolidacaoNotaAlunoCommand request, CancellationToken cancellationToken)
        {
            var aluno = (await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(new long[] { long.Parse(request.AlunoCodigo) }, request.AnoLetivo))).FirstOrDefault();

            var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(request.AlunoCodigo, 
                                                                                                         request.TurmaId, 
                                                                                                         request.Bimestre, 
                                                                                                         aluno.Inativo,
                                                                                                         request.Nota,
                                                                                                         request.ConceitoId,
                                                                                                         request.ComponenteCurricularId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemConsolidacaoConselhoClasseAluno, Guid.NewGuid(), null));

            return true;
        }
    }
}
