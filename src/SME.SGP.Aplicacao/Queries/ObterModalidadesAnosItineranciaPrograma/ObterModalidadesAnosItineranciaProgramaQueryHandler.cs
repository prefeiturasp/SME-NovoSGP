using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesAnosItineranciaProgramaQueryHandler : IRequestHandler<ObterModalidadesAnosItineranciaProgramaQuery, IEnumerable<ModalidadesPorAnoItineranciaProgramaDto>>
    {
        private readonly IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma;

        public ObterModalidadesAnosItineranciaProgramaQueryHandler(IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma)
        {
            this.repositorioConsolidacaoMatriculaTurma = repositorioConsolidacaoMatriculaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoMatriculaTurma));
        }

        public async  Task<IEnumerable<ModalidadesPorAnoItineranciaProgramaDto>> Handle(ObterModalidadesAnosItineranciaProgramaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoMatriculaTurma.ObterModalidadesPorAnos(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade, request.Semestre);
        }
    }
}
