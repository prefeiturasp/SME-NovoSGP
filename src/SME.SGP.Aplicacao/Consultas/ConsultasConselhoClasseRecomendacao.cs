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
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;

        public ConsultasConselhoClasseRecomendacao(IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao,
            IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno, IConsultasPeriodoEscolar consultasPeriodoEscolar, IConsultasTurma consultasTurma,
            IConsultasFechamentoAluno consultasFechamentoAluno, IConsultasFechamentoTurma consultasFechamentoTurma, IConsultasPeriodoFechamento consultasPeriodoFechamento,
            IConsultasConselhoClasse consultasConselhoClasse)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasFechamentoAluno = consultasFechamentoAluno ?? throw new ArgumentNullException(nameof(consultasFechamentoAluno));
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
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

        public async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(string turmaCodigo, string alunoCodigo, int bimestre, Modalidade turmaModalidade, bool EhFinal = false)
        {
            if (bimestre == 0 && !EhFinal)
                bimestre = ObterBimestreAtual(turmaModalidade);

            PeriodoFechamentoBimestre periodoFechamentoBimestre = null;
            var turma = await consultasTurma.ObterComUeDrePorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não localizada");

            var emFechamento = true;
            if (EhFinal)
            {
                var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(turma);
                if (!validacaoConselhoFinal.Item2)
                    throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
            }
            else
            {
                periodoFechamentoBimestre = await consultasPeriodoFechamento.ObterPeriodoFechamentoTurmaAsync(turma, bimestre);
                emFechamento = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, DateTime.Today, bimestre);
            }

            var fechamentoTurma = await consultasFechamentoTurma.ObterPorTurmaCodigoBimestreAsync(turmaCodigo, bimestre);
            if (fechamentoTurma == null)
                throw new NegocioException("Fechamento da turma não localizado " + (!EhFinal ? $"para o bimestre {bimestre}" : ""));

            var anotacoesDoAluno = await consultasFechamentoAluno.ObterAnotacaoAlunoParaConselhoAsync(alunoCodigo, turmaCodigo, bimestre, EhFinal);
            if (anotacoesDoAluno == null)
                anotacoesDoAluno = new List<FechamentoAlunoAnotacaoConselhoDto>();

            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorFechamentoAsync(fechamentoTurma.Id, alunoCodigo);
            if (conselhoClasseAluno == null)
                return await ObterRecomendacoesIniciais(anotacoesDoAluno, bimestre, fechamentoTurma.Id, periodoFechamentoBimestre, emFechamento);

            return TransformaEntidadeEmConsultaDto(conselhoClasseAluno, anotacoesDoAluno, bimestre, periodoFechamentoBimestre, fechamentoTurma.Id, emFechamento);
        }

        private int ObterBimestreAtual(Modalidade turmaModalidade)
        {
            return consultasPeriodoEscolar.ObterBimestre(DateTime.Today, turmaModalidade);
        }

        private async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesIniciais(IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesAluno, int bimestre, long fechamentoTurmaId, PeriodoFechamentoBimestre periodoFechamentoBimestre, bool emFechamento)
        {
            var recomendacoes = await repositorioConselhoClasseRecomendacao.ObterTodosAsync();

            if (!recomendacoes.Any())
                throw new NegocioException("Não foi possível localizar as recomendações da família e aluno.");

            return new ConsultasConselhoClasseRecomendacaoConsultaDto()
            {
                FechamentoTurmaId = fechamentoTurmaId,
                RecomendacaoAluno = MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Aluno).Select(b => b.Recomendacao)),
                RecomendacaoFamilia = MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Familia).Select(b => b.Recomendacao)),
                AnotacoesAluno = anotacoesAluno,
                Bimestre = bimestre,
                PeriodoFechamentoInicio = periodoFechamentoBimestre?.InicioDoFechamento,
                PeriodoFechamentoFim = periodoFechamentoBimestre?.FinalDoFechamento,
                SomenteLeitura = !emFechamento
            };
        }

        private ConsultasConselhoClasseRecomendacaoConsultaDto TransformaEntidadeEmConsultaDto(ConselhoClasseAluno conselhoClasseAluno,
            IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesAluno, int bimestre, PeriodoFechamentoBimestre periodoFechamentoBimestre, long fechamentoTurmaId, bool emFechamento)
        {
            return new ConsultasConselhoClasseRecomendacaoConsultaDto()
            {
                FechamentoTurmaId = fechamentoTurmaId,
                ConselhoClasseId = conselhoClasseAluno.ConselhoClasseId,
                RecomendacaoAluno = conselhoClasseAluno.RecomendacoesAluno,
                RecomendacaoFamilia = conselhoClasseAluno.RecomendacoesFamilia,
                AnotacoesAluno = anotacoesAluno,
                AnotacoesPedagogicas = conselhoClasseAluno.AnotacoesPedagogicas,
                Bimestre = bimestre,
                PeriodoFechamentoInicio = periodoFechamentoBimestre?.InicioDoFechamento,
                PeriodoFechamentoFim = periodoFechamentoBimestre?.FinalDoFechamento,
                SomenteLeitura = !emFechamento,
                Auditoria = (AuditoriaDto)conselhoClasseAluno
            };
        }
    }
}