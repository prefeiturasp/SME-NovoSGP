using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class EncaminhamentoEscolarObservacao : EntidadeBase
    {
        public EncaminhamentoEscolar EncaminhamentoEscolar { get; set; }
        public long EncaminhamentoEscolarId { get; set; }
        public string Observacao { get; set; }
        public bool Excluido { get; set; }
    }
}