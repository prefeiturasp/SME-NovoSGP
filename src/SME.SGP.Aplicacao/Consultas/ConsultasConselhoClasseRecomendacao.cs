using SME.SGP.Dominio;
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
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;

        public ConsultasConselhoClasseRecomendacao(IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao,
            IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno, IConsultasPeriodoEscolar consultasPeriodoEscolar, IConsultasTurma consultasTurma,
            IConsultasFechamentoAluno consultasFechamentoAluno, IConsultasFechamentoTurma consultasFechamentoTurma, IConsultasPeriodoFechamento consultasPeriodoFechamento,
            IConsultasConselhoClasse consultasConselhoClasse, IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasFechamentoAluno = consultasFechamentoAluno ?? throw new ArgumentNullException(nameof(consultasFechamentoAluno));
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public string MontaTextUlLis(IEnumerable<string> textos)
        {
            var str = new StringBuilder("<ul>");

            foreach (var item in textos)
            {
                str.AppendFormat("<li>{0}</li>", item);
            }
            str.AppendLine("</ul>");

            return str.ToString().Trim();
        }
        
        public async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int? bimestre)
        {
            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
            var turma = fechamentoTurma?.Turma;
            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            var emFechamento = true;

            if(fechamentoTurma == null)
            {
                turma = await consultasTurma.ObterPorCodigo(codigoTurma);
                if (turma == null) throw new NegocioException("Turma não encontrada");

                var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
                if (tipoCalendario == null) throw new NegocioException("Tipo calendário não encontrado");

                periodoEscolar = await consultasPeriodoEscolar.ObterPeriodoEscolarPorTipoCalendarioBimestre(tipoCalendario.Id, bimestre.Value);
            }

            if (!bimestre.HasValue)
            {
                var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(turma);
                if (!validacaoConselhoFinal.Item2)
                    throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                
                emFechamento = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, DateTime.Today);
            }
            else
            {
                emFechamento = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, DateTime.Today, bimestre.Value);
            }

            var anotacoesDoAluno = await consultasFechamentoAluno.ObterAnotacaoAlunoParaConselhoAsync(alunoCodigo, fechamentoTurmaId);
            if (anotacoesDoAluno == null)
                anotacoesDoAluno = new List<FechamentoAlunoAnotacaoConselhoDto>();
            
            var conselhoClasseAluno = fechamentoTurma != null ? await repositorioConselhoClasseAluno.ObterPorFechamentoAsync(fechamentoTurma.Id, alunoCodigo): null;
            if (conselhoClasseAluno == null || string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno) || string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia))
                return await ObterRecomendacoesIniciais(conselhoClasseAluno, anotacoesDoAluno, emFechamento);

            return TransformaEntidadeEmConsultaDto(conselhoClasseAluno, anotacoesDoAluno, emFechamento);
        }

        private async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesIniciais(ConselhoClasseAluno conselhoClasseAluno, IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesAluno, bool emFechamento)
        {
            var recomendacoes = await repositorioConselhoClasseRecomendacao.ObterTodosAsync();
            if (!recomendacoes.Any())
                throw new NegocioException("Não foi possível localizar as recomendações da família e aluno.");

            return new ConsultasConselhoClasseRecomendacaoConsultaDto()
            {
                RecomendacaoAluno = conselhoClasseAluno?.RecomendacoesAluno ?? MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Aluno).Select(b => b.Recomendacao)),
                RecomendacaoFamilia = conselhoClasseAluno?.RecomendacoesFamilia ?? MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Familia).Select(b => b.Recomendacao)),
                AnotacoesAluno = anotacoesAluno,
                SomenteLeitura = !emFechamento,
                Auditoria = conselhoClasseAluno != null ? (AuditoriaDto)conselhoClasseAluno : null
            };
        }

        private ConsultasConselhoClasseRecomendacaoConsultaDto TransformaEntidadeEmConsultaDto(ConselhoClasseAluno conselhoClasseAluno,
            IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesAluno, bool emFechamento)
        {
            return new ConsultasConselhoClasseRecomendacaoConsultaDto()
            {
                RecomendacaoAluno = conselhoClasseAluno.RecomendacoesAluno,
                RecomendacaoFamilia = conselhoClasseAluno.RecomendacoesFamilia,
                AnotacoesAluno = anotacoesAluno,
                AnotacoesPedagogicas = conselhoClasseAluno.AnotacoesPedagogicas,
                SomenteLeitura = !emFechamento,
                Auditoria = (AuditoriaDto)conselhoClasseAluno
            };
        }
    }
}