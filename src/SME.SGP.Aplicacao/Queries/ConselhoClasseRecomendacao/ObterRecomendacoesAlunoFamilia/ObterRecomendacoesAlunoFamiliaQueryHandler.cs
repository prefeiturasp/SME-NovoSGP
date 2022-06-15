using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesAlunoFamiliaQueryHandler : IRequestHandler<ObterRecomendacoesAlunoFamiliaQuery, IEnumerable<RecomendacoesAlunoFamiliaDto>>
    {
        private IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao;
        public ObterRecomendacoesAlunoFamiliaQueryHandler(IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao)
        {
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
        }
        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> Handle(ObterRecomendacoesAlunoFamiliaQuery request, CancellationToken cancellationToken)
         => await repositorioConselhoClasseRecomendacao.ObterIdRecomendacoesETipoAsync();
    }
}
