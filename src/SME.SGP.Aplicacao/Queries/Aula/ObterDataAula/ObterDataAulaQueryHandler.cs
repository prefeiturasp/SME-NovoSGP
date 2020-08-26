using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataAulaQueryHandler : IRequestHandler<ObterDataAulaQuery, DateTime>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterDataAulaQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<DateTime> Handle(ObterDataAulaQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterDataAula(request.AulaId);
    }
}
