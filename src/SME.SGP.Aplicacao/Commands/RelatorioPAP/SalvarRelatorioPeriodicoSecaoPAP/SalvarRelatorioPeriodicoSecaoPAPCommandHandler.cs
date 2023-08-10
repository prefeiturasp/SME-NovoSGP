using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoSecaoPAPCommandHandler : IRequestHandler<SalvarRelatorioPeriodicoSecaoPAPCommand, RelatorioPeriodicoPAPSecao>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPSecao repositorio;

        public SalvarRelatorioPeriodicoSecaoPAPCommandHandler(IRepositorioRelatorioPeriodicoPAPSecao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<RelatorioPeriodicoPAPSecao> Handle(SalvarRelatorioPeriodicoSecaoPAPCommand request, CancellationToken cancellationToken)
        {
            var relatorioSecao = new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = request.RelatorioPeriodicoAlunoId,
                SecaoRelatorioPeriodicoId = request.SecaoRelatorioPeriodicoId
            };

            await repositorio.SalvarAsync(relatorioSecao);

            return relatorioSecao;
        }
    }
}
