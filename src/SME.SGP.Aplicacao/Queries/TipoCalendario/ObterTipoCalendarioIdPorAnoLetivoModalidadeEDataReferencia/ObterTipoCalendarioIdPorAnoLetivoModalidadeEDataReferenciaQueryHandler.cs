using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQueryHandler : IRequestHandler<ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery, long>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<long> Handle(ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery request, CancellationToken cancellationToken)
            => await repositorioTipoCalendario.ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferencia(request.AnoLetivo, request.Modalidade, request.DataReferencia);        
    }
}
