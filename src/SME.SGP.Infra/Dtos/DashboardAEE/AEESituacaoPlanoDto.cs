using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AEESituacaoPlanoDto
    {
        public long Quantidade { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string DescricaoSituacao { get => Situacao.Name(); }
    }
}
