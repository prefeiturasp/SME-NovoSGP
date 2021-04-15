using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirTurmasComplementaresCommandHandler : IRequestHandler<InserirTurmasComplementaresCommand, bool>
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioConselhoClasseAlunoTurmaComplementar repositorioCCAlunoTurmaComplementar;
        private readonly IMediator mediator;

        public InserirTurmasComplementaresCommandHandler(IRepositorioTurma repositorioTurma, IMediator mediator,
            IRepositorioConselhoClasseAlunoTurmaComplementar repositorioCCAlunoTurmaComplementar)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCCAlunoTurmaComplementar = repositorioCCAlunoTurmaComplementar ?? throw new ArgumentNullException(nameof(repositorioCCAlunoTurmaComplementar));
        }

        public async Task<bool> Handle(InserirTurmasComplementaresCommand request, CancellationToken cancellationToken)
        {
            var turmaRegular = await repositorioTurma.ObterPorId(request.TurmaId);

            if (turmaRegular.DeveVerificarRegraRegulares())
            {
                string[] turmasCodigos;

                var turmasCodigosParaConsulta = new List<TipoTurma>() { turmaRegular.TipoTurma };
                turmasCodigosParaConsulta.AddRange(turmaRegular.ObterTiposRegularesDiferentes());
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turmaRegular.AnoLetivo, request.AlunoCodigo, turmasCodigosParaConsulta));

                if (turmasCodigos != null && turmasCodigos.Any())
                {
                    var turmasNaoRegulares = (await repositorioTurma.ObterPorCodigosAsync(turmasCodigos))?.Where(t => !t.EhTurmaRegular());

                    if (turmasNaoRegulares != null && turmasNaoRegulares.Any())
                    {
                        foreach (var turma in turmasNaoRegulares)
                        {
                            if (!await repositorioCCAlunoTurmaComplementar.VerificarSeExisteRegistro(request.ConselhoClasseAlunoId, turma.Id))
                            {
                                await repositorioCCAlunoTurmaComplementar.Inserir(request.ConselhoClasseAlunoId, turma.Id);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
