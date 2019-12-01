using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Core.Exceptions
{
    public class ExcecaoServicoDesativado : Exception
    {
        public ExcecaoServicoDesativado(string mensagem)
            : base(mensagem) { }
    }
}
