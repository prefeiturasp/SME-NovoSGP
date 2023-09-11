using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class SalvarRelatorioPeriodicoTurmaPAPCommandHandler : IRequestHandler<SalvarRelatorioPeriodicoTurmaPAPCommand, RelatorioPeriodicoPAPTurma>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPTurma repositorio;

        public SalvarRelatorioPeriodicoTurmaPAPCommandHandler(IRepositorioRelatorioPeriodicoPAPTurma repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<RelatorioPeriodicoPAPTurma> Handle(SalvarRelatorioPeriodicoTurmaPAPCommand request, CancellationToken cancellationToken)
        {
            var relatorio = new RelatorioPeriodicoPAPTurma()
            {
                PeriodoRelatorioId = request.PeriodoRelatorioPAPId,
                TurmaId = request.TurmaId
            };

            await repositorio.SalvarAsync(relatorio);
            return relatorio;
        }
    }
}
