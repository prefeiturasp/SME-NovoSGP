using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery : IRequest<long>
    {
        public ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery(int anoLetivo, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            DataReferencia = dataReferencia;
        }

        public int AnoLetivo { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
