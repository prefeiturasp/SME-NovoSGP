using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Core.Enumerados
{
    public enum EstadoProcessamento
    {
        Indeterminado = 0,
        Registrado,
        Enfileirado,
        EmProcessamento,
        Sucesso,
        Falha
    }
}
