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
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IRepositorioConselhoClasseAlunoTurmaComplementar repositorioCCAlunoTurmaComplementar;
        private readonly IMediator mediator;

        public InserirTurmasComplementaresCommandHandler(
            IRepositorioTurmaConsulta repositorioTurmaConsulta,
            IRepositorioConselhoClasseAlunoTurmaComplementar repositorioCCAlunoTurmaComplementar,
            IMediator mediator)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCCAlunoTurmaComplementar = repositorioCCAlunoTurmaComplementar ?? throw new ArgumentNullException(nameof(repositorioCCAlunoTurmaComplementar));
        }

        public async Task<bool> Handle(InserirTurmasComplementaresCommand request, CancellationToken cancellationToken)
        {
            var turmaRegular = await repositorioTurmaConsulta.ObterPorId(request.TurmaId);
            var turmasitinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

            if (turmaRegular.DeveVerificarRegraRegulares() || turmasitinerarioEnsinoMedio.Any(a => a.Id == (int)turmaRegular.TipoTurma))
            {
                string[] turmasCodigos;

                var turmasCodigosParaConsulta = new List<int>() { (int)turmaRegular.TipoTurma };
                turmasCodigosParaConsulta.AddRange(turmaRegular.ObterTiposRegularesDiferentes());
                turmasCodigosParaConsulta.AddRange(turmasitinerarioEnsinoMedio.Select(s => s.Id));
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turmaRegular.AnoLetivo, request.AlunoCodigo, turmasCodigosParaConsulta));

                if (turmasCodigos != null && turmasCodigos.Any())
                {
                    var turmasNaoRegulares = (await repositorioTurmaConsulta.ObterPorCodigosAsync(turmasCodigos))?.Where(t => !t.EhTurmaRegular());

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
