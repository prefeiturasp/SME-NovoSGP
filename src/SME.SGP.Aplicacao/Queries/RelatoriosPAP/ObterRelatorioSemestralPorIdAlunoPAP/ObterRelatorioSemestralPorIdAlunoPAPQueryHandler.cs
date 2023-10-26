using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralPorIdAlunoPAPQueryHandler : IRequestHandler<ObterRelatorioSemestralPorIdAlunoPAPQuery, RelatorioSemestralPAPAluno>
    {
        private readonly IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno;

        public ObterRelatorioSemestralPorIdAlunoPAPQueryHandler(IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno)
        {
            this.repositorioRelatorioSemestralAluno = repositorioRelatorioSemestralAluno ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAluno));
        }

        public async Task<RelatorioSemestralPAPAluno> Handle(ObterRelatorioSemestralPorIdAlunoPAPQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRelatorioSemestralAluno.ObterCompletoPorIdAsync(request.IdAlunoPAP);
        }
    }
}
