using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AEESituacaoEncaminhamentoDto
    {
        public long Quantidade { get; set; }
        public SituacaoAEE Situacao { get; set; }
        public string DescricaoSituacao { get => Situacao.Name(); }
    }
}
