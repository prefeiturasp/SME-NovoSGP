using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsultaConselhoClasseRecomendacaoUseCase : IConsultaConselhoClasseRecomendacaoUseCase
    {
        private readonly IMediator mediator;

        public ConsultaConselhoClasseRecomendacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> Executar(ConselhoClasseRecomendacaoDto recomendacaoDto)
        {
            int? bimestre = recomendacaoDto.Bimestre > 0 ? recomendacaoDto.Bimestre : null;
        
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(recomendacaoDto.CodigoTurma));

            var ehTurmaEdFisica = EnumExtension.EhUmDosValores(turma.TipoTurma, TipoTurma.EdFisica);

            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);
        
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(recomendacaoDto.FechamentoTurmaId, recomendacaoDto.AlunoCodigo, turma.EhAnoAnterior()));
        
            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;
        
            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else
            {
                if (bimestre.HasValue)
                {
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre.Value));
                    
                    if (periodoEscolar == null) 
                        throw new NegocioException(MensagemNegocioPeriodo.PERIODO_ESCOLAR_NAO_ENCONTRADO);
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
                
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, recomendacaoDto.AlunoCodigo, tiposParaConsulta, recomendacaoDto.ConsideraHistorico, periodoEscolar?.PeriodoFim));

                if (!turmasCodigos.Any())
                    turmasCodigos = new string[1] { turma.CodigoTurma };
                else if (!turmasCodigos.Contains(recomendacaoDto.CodigoTurma))
                    turmasCodigos = turmasCodigos.Concat(new[] { recomendacaoDto.CodigoTurma }).ToArray();
                else if (ehTurmaEdFisica)
                    turmasCodigos = turmasCodigos.Concat(new[] { turma.CodigoTurma }).ToArray();

                conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));
        
                if (conselhosClassesIds == null || !conselhosClassesIds.Any())
                    conselhosClassesIds = Array.Empty<long>();
                else
                    conselhosClassesIds = new long[1] { recomendacaoDto.ConselhoClasseId };
            }
            else
            {
                conselhosClassesIds = new long[1] { recomendacaoDto.ConselhoClasseId };
                turmasCodigos = new[] { turma.CodigoTurma };
            }
            
            var tipoCalendario =
                await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo,
                    turma.ModalidadeTipoCalendario, turma.Semestre));
            
            if (tipoCalendario == null) 
                throw new NegocioException(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO);
        
            var periodosLetivos = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendario.Id));
        
            if (periodosLetivos == null || !periodosLetivos.Any())
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TIPO_CALENDARIO);
        
            var periodoInicio = periodoEscolar?.PeriodoInicio ?? periodosLetivos.OrderBy(pl => pl.Bimestre).First().PeriodoInicio;
            var periodoFim = periodoEscolar?.PeriodoFim ?? periodosLetivos.OrderBy(pl => pl.Bimestre).Last().PeriodoFim;
        
            var turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasQuery(recomendacaoDto.AlunoCodigo, turmasCodigos, periodoInicio, periodoFim));
        
            if (turmasComMatriculasValidas.Any())
                turmasCodigos = turmasComMatriculasValidas.ToArray();
        
            bool emFechamento;
        
            if (!bimestre.HasValue)
            {
                if (fechamentoTurma.Turma.AnoLetivo != 2020 && !fechamentoTurma.Turma.Historica)
                {
                    var validacaoConselhoFinal = await mediator.Send(new ObterUltimoBimestreTurmaQuery(turma));
                    if (!validacaoConselhoFinal.possuiConselho)
                        throw new NegocioException(string.Format(MensagemNegocioConselhoClasse.PARA_ACESSAR_ESTA_ABA_E_PRECISO_REGISTRAR_O_CONSELHO_DE_CLASSE_DO_X_BIMESTRE,validacaoConselhoFinal.bimestre));
                }
        
                emFechamento = await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, DateTime.Today));
            }
            else
                emFechamento = await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, DateTime.Today, bimestre.Value));
        
            IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesDoAluno = null;
        
            if (periodoEscolar != null && periodoEscolar.Id != 0)
                anotacoesDoAluno = await mediator.Send(new ObterAnotacaoAlunoParaConselhoQuery(recomendacaoDto.AlunoCodigo, turmasCodigos, periodoEscolar.Id));
        
            var consultasConselhoClasseRecomendacaoConsultaDto = new ConsultasConselhoClasseRecomendacaoConsultaDto();
            var recomendacaoAluno = new StringBuilder();
            var recomendacaoFamilia = new StringBuilder();
            var anotacoesPedagogicas = new StringBuilder();
            var auditoriaListaDto = new List<AuditoriaDto>();
        
            foreach (var conselhoClassesIdParaTratar in conselhosClassesIds)
            {
                var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClassesIdParaTratar, recomendacaoDto.AlunoCodigo));
        
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
                        
            var recomendacoesAlunoFamiliaSelecionado = await mediator.Send(new ObterRecomendacoesPorAlunoConselhoQuery(recomendacaoDto.AlunoCodigo, bimestre, recomendacaoDto.FechamentoTurmaId, conselhosClassesIds));

            var situacaoConselhoAluno = await BuscaSituacaoConselhoAluno(recomendacaoDto.AlunoCodigo, turma);

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
        
            var statusConselhoAluno = await mediator.Send(new ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQuery(turma.Id, alunoCodigo));
        
            if (statusConselhoAluno != null)
                statusAluno = statusConselhoAluno.Status;
        
            return statusAluno;
        }
    }
}