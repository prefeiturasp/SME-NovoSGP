using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
        private readonly IConsultasDisciplina consultasDisciplina;

        public ConsolidarTurmaConselhoClasseAlunoCommandHandler(IMediator mediator, IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado, IConsultasDisciplina consultasDisciplina)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<bool> Handle(ConsolidarTurmaConselhoClasseAlunoCommand request, CancellationToken cancellationToken)
        {
            var statusNovo = SituacaoConselhoClasse.NaoIniciado;

            var consolidadoTurmaAluno = await repositorioConselhoClasseConsolidado
                    .ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(request.TurmaId, request.AlunoCodigo);

            if (consolidadoTurmaAluno == null)
            {
                consolidadoTurmaAluno = new ConselhoClasseConsolidadoTurmaAluno();
                consolidadoTurmaAluno.AlunoCodigo = request.AlunoCodigo;
                consolidadoTurmaAluno.TurmaId = request.TurmaId;
                consolidadoTurmaAluno.Status = statusNovo;
            }

            if (!request.Inativo)
            {
                var componentesDoAluno = await mediator
                    .Send(new ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery(request.AlunoCodigo, request.Bimestre, request.TurmaId));

                var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

                if (turma == null)
                    throw new NegocioException("Turma não encontrada.");

                var turmasCodigos = Array.Empty<string>();
                var turmasItinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery(), cancellationToken);

                if (turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
                {
                    var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                    var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();
                    
                    tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
                    tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));                    
                    
                    turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, request.AlunoCodigo, tiposParaConsulta), cancellationToken);
                }

                if (!turmasCodigos.Any())
                    turmasCodigos = new string[1] { turma.CodigoTurma };

                var notasFechamento = await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, request.AlunoCodigo, request.Bimestre, null, null));

                Usuario usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

                var disciplinasDaTurmaEol = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigos, usuarioAtual.PerfilAtual, usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares, false));

                var disciplinasCodigo = disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray();

                var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(disciplinasCodigo));

                var areasDoConhecimento = await mediator.Send(new ObterAreasConhecimentoQuery(disciplinasDaTurmaEol));

                if (areasDoConhecimento == null || !areasDoConhecimento.Any())
                    return false;

                var ordenacaoGrupoArea = await mediator.Send(new ObterOrdenacaoAreasConhecimentoQuery(disciplinasDaTurma, areasDoConhecimento));

                var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();

                var gruposMatrizes = disciplinasDaTurma.Where(c => c.GrupoMatrizNome != null && c.LancaNota).OrderBy(d => d.GrupoMatrizId).GroupBy(c => c.GrupoMatrizId).ToList();

                foreach (var grupoDisiplinasMatriz in gruposMatrizes)
                {
                    var conselhoClasseAlunoNotas = new ConselhoClasseAlunoNotasConceitosDto();
                    conselhoClasseAlunoNotas.GrupoMatriz = disciplinasDaTurma.FirstOrDefault(dt => dt.GrupoMatrizId == grupoDisiplinasMatriz.Key)?.GrupoMatrizNome;

                    var areasConhecimento = await mediator.Send(new MapearAreasConhecimentoQuery(grupoDisiplinasMatriz, areasDoConhecimento, ordenacaoGrupoArea, Convert.ToInt64(grupoDisiplinasMatriz.Key)));

                    foreach (var areaConhecimento in areasConhecimento)
                    {
                        var componentes = await mediator.Send(new ObterComponentesAreasConhecimentoQuery(grupoDisiplinasMatriz, areaConhecimento));

                        var componentesIds = componentes.Select(c => c.Id.ToString()).ToArray();

                        foreach (var disciplina in componentes.Where(d => d.LancaNota).OrderBy(g => g.Nome))
                        {
                            var disciplinaEol = disciplinasDaTurmaEol.FirstOrDefault(d => d.CodigoComponenteCurricular == disciplina.Id);

                            if (disciplinaEol.Regencia)
                            {
                                conselhoClasseAlunoNotas.ComponenteRegencia = await ObterComponenteRegencia(disciplina.CodigoComponenteCurricular, turma);
                            }
                            else
                            {
                                conselhoClasseAlunoNotas.ComponentesCurriculares.Add(await ObterComponenteCurricular(disciplina.Nome,
                                                                                                                    disciplina.CodigoComponenteCurricular,
                                                                                                                    turma));
                            }
                        }
                    }

                    gruposMatrizesNotas.Add(conselhoClasseAlunoNotas);
                }

                if (componentesDoAluno != null && componentesDoAluno.Any())
                {
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

                    var componentesComNotaFechamentoOuConselho = await mediator
                        .Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma.AnoLetivo, turmasCodigos, request.Bimestre, request.AlunoCodigo));

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
                    var possuiNotasFechamento = notasFechamento.Where(x => x.NotaConceito == null).Any();

                    statusNovo = possuiComponentesSemNotaConceito || possuiNotasFechamento ? SituacaoConselhoClasse.EmAndamento : SituacaoConselhoClasse.Concluido;
                }
                else if (notasFechamento != null && notasFechamento.Any())
                {
                    var listaComponentesTurma = new List<long>();
                    foreach (var componente in gruposMatrizesNotas)
                    {
                        if (componente.ComponenteRegencia != null)
                            listaComponentesTurma = componente.ComponenteRegencia.ComponentesCurriculares.Select(x => x.CodigoComponenteCurricular).ToList();
                        listaComponentesTurma.AddRange(componente.ComponentesCurriculares.Select(x=> x.CodigoComponenteCurricular).ToList());
                        
                    }
                    var listaComponentesFechamento = notasFechamento.Select(x => x.ComponenteCurricularCodigo).ToList();
                    var componentesFaltantes = listaComponentesTurma.Except(listaComponentesFechamento);
                    var possuiNotasFechamento = notasFechamento.Where(x => x.NotaConceito == null).Any();
                    statusNovo = possuiNotasFechamento || componentesFaltantes.Any() ? SituacaoConselhoClasse.EmAndamento : SituacaoConselhoClasse.Concluido;
                    
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

        private async Task<ConselhoClasseComponenteFrequenciaDto> ObterComponenteCurricular(string componenteCurricularNome, long componenteCurricularCodigo, Turma turma)
        {
            var conselhoClasseComponente = new ConselhoClasseComponenteFrequenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo
            };                     
            return conselhoClasseComponente;
        }

        private async Task<ConselhoClasseComponenteRegenciaFrequenciaDto> ObterComponenteRegencia(long componenteCurricularCodigo, Turma turma)
        {
            var componentesRegencia = await consultasDisciplina.ObterComponentesRegencia(turma);

            if (componentesRegencia == null || !componentesRegencia.Any())
                throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

            // Excessão de disciplina ED. Fisica para modalidade EJA
            if (turma.EhEJA())
                componentesRegencia = componentesRegencia.Where(a => a.CodigoComponenteCurricular != 6);

            var conselhoClasseComponente = new ConselhoClasseComponenteRegenciaFrequenciaDto();

            foreach (var componenteRegencia in componentesRegencia)
                conselhoClasseComponente.ComponentesCurriculares.Add(await ObterRegencia(componenteRegencia.Nome, componenteRegencia.CodigoComponenteCurricular));

            return conselhoClasseComponente;
        }
        private async Task<ConselhoClasseNotasComponenteRegenciaDto> ObterRegencia(string componenteCurricularNome, long componenteCurricularCodigo)
        {
            return new ConselhoClasseNotasComponenteRegenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo
            };
        }


    }
}
