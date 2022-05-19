using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorAlunoTurmaQueryHandler : IRequestHandler<ObterTotalAulasPorAlunoTurmaQuery, IEnumerable<TotalAulasPorAlunoTurmaDto>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterTotalAulasPorAlunoTurmaQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public async Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> Handle(ObterTotalAulasPorAlunoTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterTotalAulasPorAlunoTurma(request.DisciplinaId, request.CodigoTurma);
        }
    }
}
