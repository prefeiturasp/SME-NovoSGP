using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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
            var turmasItinerarioEnsinoMedio = await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance);

            if (turmaRegular.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turmaRegular.TipoTurma))
            {
                var turmasCodigos = await ObterCodigosTurmasAluno(turmaRegular, request.AlunoCodigo, turmasItinerarioEnsinoMedio);
                if (turmasCodigos.PossuiRegistros())
                {
                    var turmasNaoRegulares = (await repositorioTurmaConsulta.ObterPorCodigosAsync(turmasCodigos))?.Where(t => !t.EhTurmaRegular());
                    await InserirTurmasComplementaresConselhoClasse(turmasNaoRegulares, request.ConselhoClasseAlunoId);
                }
            }

            return true;
        }

        private async Task InserirTurmasComplementaresConselhoClasse(IEnumerable<Turma> turmasNaoRegulares, long conselhoClasseAlunoId)
        {
            if (turmasNaoRegulares.PossuiRegistros())
                foreach (var turma in turmasNaoRegulares)
                    await InserirTurmasComplementaresConselhoClasse(conselhoClasseAlunoId, turma.Id);
        }
        private async Task InserirTurmasComplementaresConselhoClasse(long conselhoClasseAlunoId, long turmaId)
        {
            if (!await repositorioCCAlunoTurmaComplementar.VerificarSeExisteRegistro(conselhoClasseAlunoId, turmaId))
                await repositorioCCAlunoTurmaComplementar.Inserir(conselhoClasseAlunoId, turmaId);
        }
        private async Task<string[]> ObterCodigosTurmasAluno(Turma turmaRegular, string codigoAluno, IEnumerable<TurmaItinerarioEnsinoMedioDto> turmasItinerarioEnsinoMedio)
        {
            var tiposParaConsulta = new List<int> { (int)turmaRegular.TipoTurma };
            var tiposRegularesDiferentes = turmaRegular.ObterTiposRegularesDiferentes();
            tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
            tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));
            return await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turmaRegular.AnoLetivo, codigoAluno, tiposParaConsulta, semestre: turmaRegular.Semestre != 0 ? turmaRegular.Semestre : null));
        }
    }
}
