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
    public class ObterTotalAulasSemFrequenciaPorTurmaQueryHandler : IRequestHandler<ObterTotalAulasSemFrequenciaPorTurmaQuery, IEnumerable<TotalAulasPorAlunoTurmaDto>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterTotalAulasSemFrequenciaPorTurmaQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public async Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> Handle(ObterTotalAulasSemFrequenciaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterTotalAulasSemFrequenciaPorTurma(request.DisciplinaId, request.CodigoTurma);
        }
    }
}
