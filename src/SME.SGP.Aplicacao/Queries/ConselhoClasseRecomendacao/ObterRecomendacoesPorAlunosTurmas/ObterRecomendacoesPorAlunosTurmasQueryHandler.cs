using MediatR;
using MimeKit.Text;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesPorAlunosTurmasQueryHandler : IRequestHandler<ObterRecomendacoesPorAlunosTurmasQuery, IEnumerable<RecomendacaoConselhoClasseAlunoDTO>>
    {
        private readonly IRepositorioConselhoClasseAlunoRecomendacaoConsulta recomendacaoAlunoRepositorio;
        private readonly IMediator mediator;
        public ObterRecomendacoesPorAlunosTurmasQueryHandler(IMediator mediator, IRepositorioConselhoClasseAlunoRecomendacaoConsulta recomendacaoAlunoRepositorio)
        {
            this.recomendacaoAlunoRepositorio = recomendacaoAlunoRepositorio ?? throw new ArgumentNullException(nameof(recomendacaoAlunoRepositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<RecomendacaoConselhoClasseAlunoDTO>> Handle(ObterRecomendacoesPorAlunosTurmasQuery request, CancellationToken cancellationToken)
        {
            var recomendacoes = await recomendacaoAlunoRepositorio.ObterRecomendacoesPorAlunoTurma(request.CodigoAluno, request.CodigoTurma, request.AnoLetivo, request.Modalidade, request.Semestre);
            if (recomendacoes != null || recomendacoes.Any())
            {
                var recomendacoesGeral = await mediator.Send(new ObterRecomendacoesAlunoFamiliaQuery());
                foreach (var recomendacao in recomendacoes)
                {
                    recomendacao.RecomendacoesAluno = recomendacao?.RecomendacoesAluno ?? MontaTextUlLis(recomendacoesGeral.Where(a => (ConselhoClasseRecomendacaoTipo)a.Tipo == ConselhoClasseRecomendacaoTipo.Aluno).Select(b => b.Recomendacao));
                    recomendacao.RecomendacoesFamilia = recomendacao?.RecomendacoesFamilia ?? MontaTextUlLis(recomendacoesGeral.Where(a => (ConselhoClasseRecomendacaoTipo)a.Tipo == ConselhoClasseRecomendacaoTipo.Familia).Select(b => b.Recomendacao));
                }
            }
            await FormatarRecomendacoes(recomendacoes);

            return recomendacoes;
        }


        private async Task FormatarRecomendacoes(IEnumerable<RecomendacaoConselhoClasseAlunoDTO> recomendacoesConselho)
        {
            foreach (var recomendacao in recomendacoesConselho)
            {
                var recomendacoesDoAluno = await recomendacaoAlunoRepositorio.ObterRecomendacoesAlunoFamiliaPorAlunoETurma(recomendacao.AlunoCodigo, recomendacao.TurmaCodigo);

                var concatenaRecomendacaoAluno = new StringBuilder();
                foreach (var aluno in recomendacoesDoAluno.Where(r => r.Tipo == (int)ConselhoClasseRecomendacaoTipo.Aluno).ToList())
                    concatenaRecomendacaoAluno.AppendLine("- " + aluno.Recomendacao);

                concatenaRecomendacaoAluno.AppendLine("<br/>");
                concatenaRecomendacaoAluno.AppendLine(FormatarHtmlParaTexto(recomendacao.RecomendacoesAluno));

                var concatenaRecomendacaoFamilia = new StringBuilder();
                foreach (var aluno in recomendacoesDoAluno.Where(r => r.Tipo == (int)ConselhoClasseRecomendacaoTipo.Familia).ToList())
                    concatenaRecomendacaoFamilia.AppendLine("- " + aluno.Recomendacao);

                concatenaRecomendacaoFamilia.AppendLine("<br/>");
                concatenaRecomendacaoFamilia.AppendLine(FormatarHtmlParaTexto(recomendacao.RecomendacoesFamilia));


                recomendacao.AnotacoesPedagogicas = FormatarHtmlParaTexto(recomendacao.AnotacoesPedagogicas);
                recomendacao.RecomendacoesAluno = concatenaRecomendacaoAluno.ToString();
                recomendacao.RecomendacoesFamilia = concatenaRecomendacaoFamilia.ToString();
            }

        }

        private string FormatarHtmlParaTexto(string textoHtml)
        {
            if (!string.IsNullOrEmpty(textoHtml))
            {
                string semTags = UtilRegex.RemoverTagsHtmlMidia(textoHtml);
                semTags = UtilRegex.RemoverTagsHtml(semTags);
                semTags = UtilRegex.AdicionarEspacos(semTags);

                return semTags;
            }
            else
            {
                return textoHtml;
            }
        }

        private string MontaTextUlLis(IEnumerable<string> textos)
        {
            var str = new StringBuilder();

            foreach (var item in textos)
            {
                str.AppendFormat(item);
            }

            return str.ToString().Trim();
        }
    }
}
