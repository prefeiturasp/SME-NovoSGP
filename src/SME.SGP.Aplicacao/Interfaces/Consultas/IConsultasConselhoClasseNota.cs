using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasseNota
    {
        ConselhoClasseNota ObterPorId(long id);
    }
}
