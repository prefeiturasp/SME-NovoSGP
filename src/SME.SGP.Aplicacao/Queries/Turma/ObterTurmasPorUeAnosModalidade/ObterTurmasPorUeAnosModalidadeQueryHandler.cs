using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeAnosModalidadeQueryHandler : IRequestHandler<ObterTurmasPorUeAnosModalidadeQuery, IEnumerable<long>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorUeAnosModalidadeQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<long>> Handle(ObterTurmasPorUeAnosModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasPorUeAnos(request.UeCodigo, request.AnoLetivo, request.Anos, request.ModalidadeId);
        }
    }
}
