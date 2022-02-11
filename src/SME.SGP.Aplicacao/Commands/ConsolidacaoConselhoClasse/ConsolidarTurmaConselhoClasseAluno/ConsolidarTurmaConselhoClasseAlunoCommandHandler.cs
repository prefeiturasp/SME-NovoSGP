using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarTurmaConselhoClasseAlunoCommandHandler : IRequestHandler<ConsolidarTurmaConselhoClasseAlunoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;

        public ConsolidarTurmaConselhoClasseAlunoCommandHandler(IMediator mediator, IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public async Task<bool> Handle(ConsolidarTurmaConselhoClasseAlunoCommand request, CancellationToken cancellationToken)
        {
            var statusNovo = SituacaoConselhoClasse.NaoIniciado;

            var consolidadoTurmaAluno = await repositorioConselhoClasseConsolidado
                    .ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(request.TurmaId, request.Bimestre, request.AlunoCodigo);

            if (consolidadoTurmaAluno == null)
            {
                consolidadoTurmaAluno = new ConselhoClasseConsolidadoTurmaAluno();
                consolidadoTurmaAluno.AlunoCodigo = request.AlunoCodigo;
                consolidadoTurmaAluno.Bimestre = request.Bimestre;
                consolidadoTurmaAluno.TurmaId = request.TurmaId;
                consolidadoTurmaAluno.Status = statusNovo;
            }

            if (!request.Inativo)
            {
                var componentesDoAluno = await mediator
                    .Send(new ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery(request.AlunoCodigo, request.Bimestre, request.TurmaId));

                if (componentesDoAluno != null && componentesDoAluno.Any())
                {
                    var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

                    if (turma == null)
                        throw new NegocioException("Turma não encontrada.");

                    if (request.Bimestre == 0)
                    {
                        var fechamento = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery() { TurmaId = request.TurmaId });
                        if (fechamento == null)
                            throw new NegocioException($"Não foi localizado fechamento para a turma : {request.TurmaId}");
                        var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamento.Id));
                        if (conselhoClasse == null)
                            throw new NegocioException($"Não foi localizado conselho de classe para a turma : {request.TurmaId}");
                        var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClasse.Id, request.AlunoCodigo));
                        consolidadoTurmaAluno.ParecerConclusivoId = conselhoClasseAluno?.ConselhoClasseParecerId;
                    }

                    var turmasCodigos = new string[] { };
                    var turmasitinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

                    if (turma.DeveVerificarRegraRegulares() || turmasitinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
                    {
                        var turmasCodigosParaConsulta = new List<int>() { (int)turma.TipoTurma };
                        turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                        turmasCodigosParaConsulta.AddRange(turmasitinerarioEnsinoMedio.Select(s => s.Id));
                        turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, request.AlunoCodigo, turmasCodigosParaConsulta));
                    }

                    if (turmasCodigos.Any())
                        turmasCodigos = new string[1] { turma.CodigoTurma };


                    var componentesComNotaFechamentoOuConselho = await mediator
                        .Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma.AnoLetivo, request.TurmaId, request.Bimestre, request.AlunoCodigo));

                    if (componentesComNotaFechamentoOuConselho == null || !componentesComNotaFechamentoOuConselho.Any())
                        throw new NegocioException("Não foi encontrado componentes curriculares com nota fechamento");

                    var componentesDaTurmaEol = await mediator
                        .Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigos));

                    if (componentesDaTurmaEol == null || !componentesDaTurmaEol.Any())
                        throw new NegocioException("Não foi encontrado componentes curriculares no eol");

                    var possuiComponentesSemNotaConceito = componentesDaTurmaEol
                        .Where(ct => ct.LancaNota && !ct.TerritorioSaber)
                        .Select(ct => ct.Codigo)
                        .Except(componentesComNotaFechamentoOuConselho.Select(cn => cn.Codigo))
                        .Any();

                    statusNovo = possuiComponentesSemNotaConceito ? SituacaoConselhoClasse.EmAndamento : SituacaoConselhoClasse.Concluido;
                }

                if (consolidadoTurmaAluno.ParecerConclusivoId != null)
                    statusNovo = SituacaoConselhoClasse.Concluido;
            }

            consolidadoTurmaAluno.Status = statusNovo;

            consolidadoTurmaAluno.DataAtualizacao = DateTime.Now;

            await repositorioConselhoClasseConsolidado
                .SalvarAsync(consolidadoTurmaAluno);

            return true;
        }
    }
}
