using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasNaoLancamNotaQueryHandler : IRequestHandler<ObterTotalAulasNaoLancamNotaQuery, IEnumerable<TotalAulasNaoLancamNotaDto>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterTotalAulasNaoLancamNotaQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }

        public async Task<IEnumerable<TotalAulasNaoLancamNotaDto>> Handle(ObterTotalAulasNaoLancamNotaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterTotalAulasNaoLancamNotaPorBimestreTurma(request.CodigoTurma, request.Bimestre, request.CodigoAluno);
        }
    }
}
