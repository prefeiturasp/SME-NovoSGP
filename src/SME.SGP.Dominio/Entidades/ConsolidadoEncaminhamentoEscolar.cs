using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class ConsolidadoEncaminhamentoEscolar : EntidadeBase
    {
        public ConsolidadoEncaminhamentoEscolar()
        {

        }

        public ConsolidadoEncaminhamentoEscolar(int anoLetivo, long ueId, long quantidade, SituacaoNAAPA situacao, Modalidade modalidade)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
            Quantidade = quantidade;
            Situacao = situacao;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
        public long Quantidade { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}