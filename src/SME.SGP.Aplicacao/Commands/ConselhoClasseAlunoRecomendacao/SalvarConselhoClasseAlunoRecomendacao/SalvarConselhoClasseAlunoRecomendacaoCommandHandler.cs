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
    public class SalvarConselhoClasseAlunoRecomendacaoCommandHandler : AsyncRequestHandler<SalvarConselhoClasseAlunoRecomendacaoCommand>
    {
        private readonly IRepositorioConselhoClasseAlunoRecomendacao repositorioConselhoClasseAlunoRecomendacao;

        public SalvarConselhoClasseAlunoRecomendacaoCommandHandler(IRepositorioConselhoClasseAlunoRecomendacao repositorioConselhoClasseAlunoRecomendacao)
        {
            this.repositorioConselhoClasseAlunoRecomendacao = repositorioConselhoClasseAlunoRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoRecomendacao));
        }

        protected override async Task Handle(SalvarConselhoClasseAlunoRecomendacaoCommand request, CancellationToken cancellationToken)
        {
            var listaRecomendacoesExcluidas = new List<long>();
            var recomendacoesAlunoFamilia = new List<long>();

            recomendacoesAlunoFamilia.AddRange(request.RecomendacoesAlunoId);
            recomendacoesAlunoFamilia.AddRange(request.RecomendacoesFamiliaId);

            var recomendacoesAssociadas = await repositorioConselhoClasseAlunoRecomendacao.ObterRecomendacoesDoAlunoPorConselhoAlunoId(request.ConselhoClasseAlunoId);

            foreach(var recomendacao in recomendacoesAssociadas)
            {
                bool verificacao = recomendacoesAlunoFamilia.Contains(recomendacao);
                if (verificacao)
                    recomendacoesAlunoFamilia.Remove(recomendacao);
                else
                    listaRecomendacoesExcluidas.Add(recomendacao);
            }

            if (listaRecomendacoesExcluidas.Any())
                await repositorioConselhoClasseAlunoRecomendacao.ExcluirRecomendacoesPorConselhoAlunoIdRecomendacaoId(request.ConselhoClasseAlunoId, listaRecomendacoesExcluidas.ToArray());          

            if(recomendacoesAlunoFamilia.Any())
                repositorioConselhoClasseAlunoRecomendacao.InserirRecomendacaoAlunoFamilia(recomendacoesAlunoFamilia.ToArray(), request.ConselhoClasseAlunoId);
        }
    }
}
