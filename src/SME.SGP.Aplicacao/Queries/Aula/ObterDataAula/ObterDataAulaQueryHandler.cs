using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataAulaQueryHandler : IRequestHandler<ObterDataAulaQuery, DateTime>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterDataAulaQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<DateTime> Handle(ObterDataAulaQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterDataAula(request.AulaId);
    }
}
