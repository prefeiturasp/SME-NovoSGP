using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasseRecomendacao : IConsultasConselhoClasseRecomendacao
    {
        private readonly IConsultasFechamentoAluno consultasFechamentoAluno;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IConsultasConselhoClasseAluno consultasConselhoClasseAluno;
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;
        private readonly IMediator mediator;

        public ConsultasConselhoClasseRecomendacao(IConsultasFechamentoAluno consultasFechamentoAluno,
            IConsultasPeriodoFechamento consultasPeriodoFechamento,
            IConsultasConselhoClasse consultasConselhoClasse, 
            IMediator mediator, 
            IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado,
            IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
        {
            this.consultasFechamentoAluno = consultasFechamentoAluno ?? throw new ArgumentNullException(nameof(consultasFechamentoAluno));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));            
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasConselhoClasseAluno = consultasConselhoClasseAluno ?? throw new ArgumentNullException(nameof(consultasConselhoClasseAluno));
        }

        public async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestreParam, bool consideraHistorico = false)
        {
            //Tratamento do bimestre que pode vir zero quando é aba final e endpoint não permite passar null
            int? bimestre = bimestreParam > 0 ? bimestreParam : null;

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));

            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, turma.EhAnoAnterior()));

            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else
            {
                if (bimestre.HasValue)
                {
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre.Value));
                    
                    if (periodoEscolar == null) 
                        throw new NegocioException("Período escolar não encontrado");
                }
            }

            long[] conselhosClassesIds;
            string[] turmasCodigos;
            var turmasItinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

            if (turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
            {
                var tiposParaConsulta = new List<int>();
                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();
                    
                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
                tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));
                
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, tiposParaConsulta, consideraHistorico, periodoEscolar?.PeriodoFim));
                
                if (!turmasCodigos.Any())
                    turmasCodigos = new string[1] { turma.CodigoTurma };
                else if (!turmasCodigos.Contains(codigoTurma))
                    turmasCodigos = turmasCodigos.Concat(new[] { codigoTurma }).ToArray();

                conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));

                if (conselhosClassesIds == null || !conselhosClassesIds.Any())
                    conselhosClassesIds = Array.Empty<long>();
                else
                    conselhosClassesIds = new long[1] { conselhoClasseId };
            }
            else
            {
                conselhosClassesIds = new long[1] { conselhoClasseId };
                turmasCodigos = new[] { turma.CodigoTurma };
            }
            
            var tipoCalendario =
                await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo,
                    turma.ModalidadeTipoCalendario, turma.Semestre));
            
            if (tipoCalendario == null) 
                throw new NegocioException("Tipo de calendário não encontrado");

            var periodosLetivos = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendario.Id));

            if (periodosLetivos == null || !periodosLetivos.Any())
                throw new NegocioException("Não foram encontrados períodos escolares do tipo de calendário.");

            var periodoInicio = periodoEscolar?.PeriodoInicio ?? periodosLetivos.OrderBy(pl => pl.Bimestre).First().PeriodoInicio;
            var periodoFim = periodoEscolar?.PeriodoFim ?? periodosLetivos.OrderBy(pl => pl.Bimestre).Last().PeriodoFim;

            var turmasComMatriculasValidas = await consultasConselhoClasseAluno
                    .ObterTurmasComMatriculasValidas(alunoCodigo, turmasCodigos, periodoInicio, periodoFim);

            if (turmasComMatriculasValidas.Any())
                turmasCodigos = turmasComMatriculasValidas.ToArray();

            bool emFechamento;

            if (!bimestre.HasValue)
            {
                if (fechamentoTurma.Turma.AnoLetivo != 2020 && !fechamentoTurma.Turma.Historica)
                {
                    var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(turma);
                    if (!validacaoConselhoFinal.Item2)
                        throw new NegocioException($"Para acessar esta aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                }

                emFechamento = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, DateTime.Today);
            }
            else
                emFechamento = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, DateTime.Today, bimestre.Value);

            IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesDoAluno = null;

            if (periodoEscolar != null && periodoEscolar.Id != 0)
                anotacoesDoAluno = await consultasFechamentoAluno.ObterAnotacaoAlunoParaConselhoAsync(alunoCodigo, turma.CodigoTurma, periodoEscolar.Id);

            var consultasConselhoClasseRecomendacaoConsultaDto = new ConsultasConselhoClasseRecomendacaoConsultaDto();
            var recomendacaoAluno = new StringBuilder();
            var recomendacaoFamilia = new StringBuilder();
            var anotacoesPedagogicas = new StringBuilder();
            var auditoriaListaDto = new List<AuditoriaDto>();

            foreach (var conselhoClassesIdParaTratar in conselhosClassesIds)
            {
                var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClassesIdParaTratar, alunoCodigo));

                if (conselhoClasseAluno == null) 
                    continue;
                
                if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno))
                    recomendacaoAluno.AppendLine(conselhoClasseAluno.RecomendacoesAluno);

                if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia))
                    recomendacaoFamilia.AppendLine(conselhoClasseAluno.RecomendacoesFamilia);

                if (!string.IsNullOrEmpty(conselhoClasseAluno.AnotacoesPedagogicas))
                    anotacoesPedagogicas.AppendLine(conselhoClasseAluno.AnotacoesPedagogicas);

                auditoriaListaDto.Add((AuditoriaDto)conselhoClasseAluno); //No final, buscar a mais recente
            }
                        
            var recomendacoesAlunoFamiliaSelecionado = await mediator.Send(new ObterRecomendacoesPorAlunoConselhoQuery(alunoCodigo, bimestre, fechamentoTurmaId, conselhosClassesIds));

            var alunos = await mediator
                            .Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma, consideraInativos: true));

            if (alunos == null || !alunos.Any())
                throw new NegocioException($"Não foram encontrados alunos para a turma {turma.CodigoTurma} no Eol");

            var alunoFiltrado = alunos.FirstOrDefault(a => a.CodigoAluno == alunoCodigo);
            
            if (bimestre.HasValue && alunoFiltrado != null)
                await mediator.Send(new ConsolidarTurmaConselhoClasseAlunoCommand(alunoCodigo, turma.Id, bimestre.Value, alunoFiltrado.Inativo));

            var situacaoConselhoAluno = await BuscaSituacaoConselhoAluno(alunoCodigo, turma);

            consultasConselhoClasseRecomendacaoConsultaDto.SituacaoConselho = situacaoConselhoAluno.GetAttribute<DisplayAttribute>().Name;
            consultasConselhoClasseRecomendacaoConsultaDto.AnotacoesAluno = anotacoesDoAluno;
            consultasConselhoClasseRecomendacaoConsultaDto.AnotacoesPedagogicas = anotacoesPedagogicas.ToString();

            var auditoria = auditoriaListaDto.Any() ? auditoriaListaDto.OrderByDescending(a => a.AlteradoEm).ThenBy(a => a.CriadoEm).FirstOrDefault() : null;

            consultasConselhoClasseRecomendacaoConsultaDto.Auditoria = auditoria;
            consultasConselhoClasseRecomendacaoConsultaDto.RecomendacoesAlunoFamilia = recomendacoesAlunoFamiliaSelecionado;
            consultasConselhoClasseRecomendacaoConsultaDto.TextoRecomendacaoAluno = recomendacaoAluno.ToString();
            consultasConselhoClasseRecomendacaoConsultaDto.TextoRecomendacaoFamilia = recomendacaoFamilia.ToString();
            consultasConselhoClasseRecomendacaoConsultaDto.SomenteLeitura = !emFechamento;
            consultasConselhoClasseRecomendacaoConsultaDto.MatriculaAtiva = turmasComMatriculasValidas.Any();

            return consultasConselhoClasseRecomendacaoConsultaDto;
        }

        private async Task<SituacaoConselhoClasse> BuscaSituacaoConselhoAluno(string alunoCodigo, Turma turma)
        {
            var statusAluno = SituacaoConselhoClasse.NaoIniciado;

            var statusConselhoAluno = await repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(turma.Id, alunoCodigo);

            if (statusConselhoAluno != null)
                statusAluno = statusConselhoAluno.Status;

            return statusAluno;
        }
    }
}