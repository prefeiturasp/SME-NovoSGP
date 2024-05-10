using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTextoRecomendacoesAlunoFamiliaQueryHandler : IRequestHandler<ObterTextoRecomendacoesAlunoFamiliaQuery, (string recomendacoesAluno, string recomendacoesFamilia)>
    {
        private readonly IRepositorioConselhoClasseRecomendacao repositorio;

        public ObterTextoRecomendacoesAlunoFamiliaQueryHandler(IRepositorioConselhoClasseRecomendacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<(string recomendacoesAluno, string recomendacoesFamilia)> Handle(ObterTextoRecomendacoesAlunoFamiliaQuery request, CancellationToken cancellationToken)
        {
            var recomendacoes = await repositorio.ObterTodosAsync();
            if (!recomendacoes.Any())
                throw new NegocioException("Não foi possível localizar as recomendações da família e aluno.");

            return (MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Aluno).Select(b => b.Recomendacao)),
                    MontaTextUlLis(recomendacoes.Where(a => a.Tipo == ConselhoClasseRecomendacaoTipo.Familia).Select(b => b.Recomendacao)));
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
    }

}
