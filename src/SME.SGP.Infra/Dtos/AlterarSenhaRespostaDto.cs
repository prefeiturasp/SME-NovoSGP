using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlterarSenhaRespostaDto
    {
        public bool SenhaAlterada { get; set; }
        public string Mensagem { get; set; }
        public int StatusRetorno { get; set; }
    }
}
