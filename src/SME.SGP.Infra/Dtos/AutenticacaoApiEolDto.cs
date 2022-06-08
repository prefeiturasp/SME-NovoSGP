using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AutenticacaoApiEolDto
    {
        public string CodigoRf { get; set; }
        public AutenticacaoStatusEol Status { get; set; }
        public Guid UsuarioId { get; set; }
    }
}