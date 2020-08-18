using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Devolutiva.ObterUltimaDataDevolutivaPorTurmaComponente
{
    public class ObterUltimaDataDevolutivaPorTurmaComponenteQueryHandler: IRequestHandler<ObterUltimaDataDevolutivaPorTurmaComponenteQuery, DateTime>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterUltimaDataDevolutivaPorTurmaComponenteQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<DateTime> Handle(ObterUltimaDataDevolutivaPorTurmaComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ObterUltimaDataDevolutiva(request.TurmaCodigo, request.ComponenteCurricularCodigo);
    }
}
