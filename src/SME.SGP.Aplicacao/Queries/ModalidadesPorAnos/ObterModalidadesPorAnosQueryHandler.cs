using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnosQueryHandler : IRequestHandler<ObterModalidadesPorAnosQuery, IEnumerable<ModalidadesPorAnoDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterModalidadesPorAnosQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<ModalidadesPorAnoDto>> Handle(ObterModalidadesPorAnosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterModalidadesPorAnos(request.AnoLetivo, request.DreId, request.UeId, request.modalidade, request.semestre);
        }
                
    }
}
