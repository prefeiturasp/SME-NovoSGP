using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasPorAnoModalidadeUeQueryHandler : IRequestHandler<ObterCodigosTurmasPorAnoModalidadeUeQuery, IEnumerable<long>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        public ObterCodigosTurmasPorAnoModalidadeUeQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<long>> Handle(ObterCodigosTurmasPorAnoModalidadeUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterIdsTurmasPorAnoModalidadeUeTipoRegular(request.AnoLetivo,(int)request.Modalidade,request.UeId);
        }
    }
}
