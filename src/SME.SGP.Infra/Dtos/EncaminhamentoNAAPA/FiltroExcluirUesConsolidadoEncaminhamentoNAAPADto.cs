﻿using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos
{
    public class FiltroExcluirUesConsolidadoEncaminhamentoNAAPADto
    {
        public FiltroExcluirUesConsolidadoEncaminhamentoNAAPADto(long ueId, int anoLetivo, SituacaoNAAPA[] situacoesIgnoradas)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            SituacoesIgnoradas = situacoesIgnoradas;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public SituacaoNAAPA[] SituacoesIgnoradas { get; set; }
    }
}