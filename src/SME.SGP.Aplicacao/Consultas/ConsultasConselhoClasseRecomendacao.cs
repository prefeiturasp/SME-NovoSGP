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
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao;
        private readonly IRepositorioTurma repositorioTurma;

        public ConsultasConselhoClasseRecomendacao(IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao,
            IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno, IConsultasPeriodoEscolar consultasPeriodoEscolar, IRepositorioTurma repositorioTurma,
            IConsultasFechamentoAluno consultasFechamentoAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasFechamentoAluno = consultasFechamentoAluno ?? throw new ArgumentNullException(nameof(consultasFechamentoAluno));
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
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

        public async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(string turmaCodigo, string alunoCodigo, int bimestre)
        {
            if (bimestre == 0)
                bimestre = ObterBimestreAtual(turmaCodigo);

            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorFiltrosAsync(turmaCodigo, alunoCodigo, bimestre);

            var anotacoesDoAluno = await consultasFechamentoAluno.ObterAnotacaoAlunoParaConselhoAsync(alunoCodigo, turmaCodigo, bimestre);
            if (anotacoesDoAluno == null)
                anotacoesDoAluno = new List<FechamentoAlunoAnotacaoConselhoDto>();

            if (conselhoClasseAluno == null)
            {
                return await ObterRecomendacoesIniciais(anotacoesDoAluno);
            }
            else return TransformaEntidadeEmConsultaDto(conselhoClasseAluno, anotacoesDoAluno);
        }

        private int ObterBimestreAtual(string codigoTurma)
        {
            var turma = repositorioTurma.ObterPorCodigo(codigoTurma);
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");

            return consultasPeriodoEscolar.ObterBimestre(DateTime.Today, turma.ModalidadeCodigo);
        }

        private async Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesIniciais(IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesPedagogicas)
        {
            var recomendacoes = await repositorioConselhoClasseRecomendacao.ObterTodosAsync();

            if (!recomendacoes.Any())
                throw new NegocioException("Não foi possível localizar as recomendações da família e aluno.");

            return new ConsultasConselhoClasseRecomendacaoConsultaDto()
            {
                RecomendacaoAluno = MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Aluno).Select(b => b.Recomendacao)),
                RecomendacaoFamilia = MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Familia).Select(b => b.Recomendacao)),
                AnotacoesPedagogicas = anotacoesPedagogicas
            };
        }

        private ConsultasConselhoClasseRecomendacaoConsultaDto TransformaEntidadeEmConsultaDto(ConselhoClasseAluno conselhoClasseAluno, IEnumerable<FechamentoAlunoAnotacaoConselhoDto> anotacoesPedagogicas)
        {
            return new ConsultasConselhoClasseRecomendacaoConsultaDto()
            {
                RecomendacaoAluno = conselhoClasseAluno.RecomendacoesAluno,
                RecomendacaoFamilia = conselhoClasseAluno.RecomendacoesFamilia,
                AnotacoesPedagogicas = anotacoesPedagogicas
            };
        }
    }
}