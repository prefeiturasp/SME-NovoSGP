using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoAlunoPAPCommandHandler : IRequestHandler<SalvarRelatorioPeriodicoAlunoPAPCommand, RelatorioPeriodicoPAPAluno>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPAluno repositorio;

        public SalvarRelatorioPeriodicoAlunoPAPCommandHandler(IRepositorioRelatorioPeriodicoPAPAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<RelatorioPeriodicoPAPAluno> Handle(SalvarRelatorioPeriodicoAlunoPAPCommand request, CancellationToken cancellationToken)
        {
            var relatorioAluno = new RelatorioPeriodicoPAPAluno()
            {
                CodigoAluno = request.AlunoCodigo,
                NomeAluno = request.AlunoNome,
                RelatorioPeriodicoTurmaId = request.RelatorioPeriodicoTurmaPAPId
            };

            await repositorio.SalvarAsync(relatorioAluno);

            return relatorioAluno;
        }
    }
}
