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
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;
        private readonly IMediator mediator;

        public ConsultasConselhoClasseRecomendacao(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
            IConsultasFechamentoAluno consultasFechamentoAluno, IConsultasPeriodoFechamento consultasPeriodoFechamento,
            IConsultasConselhoClasse consultasConselhoClasse, IRepositorioTipoCalendario repositorioTipoCalendario, 
            IMediator mediator, IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.consultasFechamentoAluno = consultasFechamentoAluno ?? throw new ArgumentNullException(nameof(consultasFechamentoAluno));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int? bimestre, bool consideraHistorico = false)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, turma.EhAnoAnterior()));

            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else
            {
                if (bimestre > 0)
                {
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre.Value));
                    if (periodoEscolar == null) throw new NegocioException("Período escolar não encontrado");
                }
            }

            long[] conselhosClassesIds;
            string[] turmasCodigos;

            if (turma.DeveVerificarRegraRegulares())
            {
                var tipos = new List<TipoTurma>() { turma.TipoTurma };

                tipos.AddRange(turma.ObterTiposRegularesDiferentes());

                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, tipos, consideraHistorico));
                conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));
            }
            else
            {
                conselhosClassesIds = new long[1] { conselhoClasseId };
                turmasCodigos = new string[] { turma.CodigoTurma };
            }


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
            {
                emFechamento = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, DateTime.Today, bimestre.Value);
            }

            IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesDoAluno = null;
            if (periodoEscolar != null && periodoEscolar.Id != 0)
                anotacoesDoAluno = await consultasFechamentoAluno.ObterAnotacaoAlunoParaConselhoAsync(alunoCodigo, turmasCodigos, periodoEscolar.Id);
   
            var consultasConselhoClasseRecomendacaoConsultaDto = new ConsultasConselhoClasseRecomendacaoConsultaDto();
            var recomendacaoAluno = new StringBuilder();
            var recomendacaoFamilia = new StringBuilder();
            var anotacoesPedagogicas = new StringBuilder();
            var auditoriaListaDto = new List<AuditoriaDto>();

            if (conselhosClassesIds != null)
            {
                foreach (var conselhoClassesIdParaTratar in conselhosClassesIds)
                {
                    var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClassesIdParaTratar, alunoCodigo);

                    if (conselhoClasseAluno != null && (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno) || !string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia) || !string.IsNullOrEmpty(conselhoClasseAluno.AnotacoesPedagogicas)))
                    {
                        if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno))
                            recomendacaoAluno.AppendLine(conselhoClasseAluno.RecomendacoesAluno);


                        if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia))
                            recomendacaoFamilia.AppendLine(conselhoClasseAluno.RecomendacoesFamilia);

                        if (!string.IsNullOrEmpty(conselhoClasseAluno.AnotacoesPedagogicas))
                            anotacoesPedagogicas.AppendLine(conselhoClasseAluno.AnotacoesPedagogicas);

                        auditoriaListaDto.Add((AuditoriaDto)conselhoClasseAluno); //No final, buscar a mais recente
                    }
                }
            }


            if (recomendacaoAluno.Length == 0 || recomendacaoFamilia.Length == 0)
            {
                var recomendacoes = await mediator.Send(new ObterTextoRecomendacoesAlunoFamiliaQuery());

                if (recomendacaoAluno.Length == 0)
                    recomendacaoAluno.AppendLine(recomendacoes.recomendacoesAluno);

                if (recomendacaoFamilia.Length == 0)
                    recomendacaoFamilia.AppendLine(recomendacoes.recomendacoesFamilia);
            }

            var situacaoConselhoAluno = await BuscaSituacaoConselhoAluno(alunoCodigo, bimestre, turma);

            consultasConselhoClasseRecomendacaoConsultaDto.SituacaoConselho = situacaoConselhoAluno.GetAttribute<DisplayAttribute>().Name;
            consultasConselhoClasseRecomendacaoConsultaDto.AnotacoesAluno = anotacoesDoAluno;
            consultasConselhoClasseRecomendacaoConsultaDto.AnotacoesPedagogicas = anotacoesPedagogicas.ToString();

            var auditoria = auditoriaListaDto.Any() ? auditoriaListaDto.OrderBy(a => a.AlteradoEm).ThenBy(a => a.CriadoEm).FirstOrDefault() : null;

            consultasConselhoClasseRecomendacaoConsultaDto.Auditoria = auditoria;
            consultasConselhoClasseRecomendacaoConsultaDto.RecomendacaoAluno = recomendacaoAluno.ToString();
            consultasConselhoClasseRecomendacaoConsultaDto.RecomendacaoFamilia = recomendacaoFamilia.ToString();
            consultasConselhoClasseRecomendacaoConsultaDto.SomenteLeitura = !emFechamento;

            return consultasConselhoClasseRecomendacaoConsultaDto;
        }

        private async Task<SituacaoConselhoClasse> BuscaSituacaoConselhoAluno(string alunoCodigo, int? bimestre, Turma turma)
        {
            SituacaoConselhoClasse statusAluno = SituacaoConselhoClasse.NaoIniciado;

            var statusConselhoAluno = await repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(turma.Id, bimestre.Value, alunoCodigo);

            if (statusConselhoAluno != null)
                statusAluno = statusConselhoAluno.Status;
            return statusAluno;
        }
    }
}