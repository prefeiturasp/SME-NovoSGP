using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoQueryHandler : IRequestHandler<ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoQuery, IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>>
    {
        private readonly IRepositorioDiarioBordo repositorio;

        public ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoQueryHandler(IRepositorioDiarioBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>> Handle(ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
