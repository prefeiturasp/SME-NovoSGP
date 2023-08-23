using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioPeriodicoAlunoPAPQueryHandler : IRequestHandler<ObterRelatorioPeriodicoAlunoPAPQuery, RelatorioPeriodicoPAPAluno>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPAluno repositorio;

        public ObterRelatorioPeriodicoAlunoPAPQueryHandler(IRepositorioRelatorioPeriodicoPAPAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<RelatorioPeriodicoPAPAluno> Handle(ObterRelatorioPeriodicoAlunoPAPQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterPorIdAsync(request.RelatorioAlunoId);
        }
    }
}
