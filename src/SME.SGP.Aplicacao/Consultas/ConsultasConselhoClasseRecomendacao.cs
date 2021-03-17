using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasseRecomendacao : IConsultasConselhoClasseRecomendacao
    {
        private readonly IConsultasFechamentoAluno consultasFechamentoAluno;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IMediator mediator;

        public ConsultasConselhoClasseRecomendacao(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
            IConsultasFechamentoAluno consultasFechamentoAluno, IConsultasPeriodoFechamento consultasPeriodoFechamento,
            IConsultasConselhoClasse consultasConselhoClasse, IRepositorioTipoCalendario repositorioTipoCalendario, IMediator mediator)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.consultasFechamentoAluno = consultasFechamentoAluno ?? throw new ArgumentNullException(nameof(consultasFechamentoAluno));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int? bimestre)
        {
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo));
            var turma = fechamentoTurma?.Turma;

            long[] conselhosClassesIds;
            string[] turmasCodigos;

            if (turma.DeveVerificarRegraRegulares())
            {
                var tipos = new List<TipoTurma>() { turma.TipoTurma };

                tipos.AddRange(turma.ObterTiposRegularesDiferentes());

                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, tipos));
                conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, fechamentoTurma.PeriodoEscolarId));
            }
            else { 
                conselhosClassesIds = new long[1] { conselhoClasseId };
                turmasCodigos = new string[] { turma.CodigoTurma };
            }

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (tipoCalendario == null) throw new NegocioException("Tipo calendário não encontrado");


            bool emFechamento;

            if (!bimestre.HasValue)
            {
                if (fechamentoTurma.Turma.AnoLetivo != 2020)
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

            var anotacoesDoAluno = await consultasFechamentoAluno.ObterAnotacaoAlunoParaConselhoAsync(alunoCodigo, turmasCodigos);
            if (anotacoesDoAluno == null)
                anotacoesDoAluno = new List<FechamentoAlunoAnotacaoConselhoDto>();

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

            consultasConselhoClasseRecomendacaoConsultaDto.AnotacoesAluno = anotacoesDoAluno;
            consultasConselhoClasseRecomendacaoConsultaDto.AnotacoesPedagogicas = anotacoesPedagogicas.ToString();

            var auditoria = auditoriaListaDto.Any() ? auditoriaListaDto.OrderBy(a => a.AlteradoEm).ThenBy(a => a.CriadoEm).FirstOrDefault() : null;

            consultasConselhoClasseRecomendacaoConsultaDto.Auditoria = auditoria;
            consultasConselhoClasseRecomendacaoConsultaDto.RecomendacaoAluno = recomendacaoAluno.ToString();
            consultasConselhoClasseRecomendacaoConsultaDto.RecomendacaoFamilia = recomendacaoFamilia.ToString();
            consultasConselhoClasseRecomendacaoConsultaDto.SomenteLeitura = !emFechamento;

            return consultasConselhoClasseRecomendacaoConsultaDto;
        }
    }
}