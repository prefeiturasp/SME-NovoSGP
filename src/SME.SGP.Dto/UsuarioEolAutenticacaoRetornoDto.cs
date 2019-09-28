using System;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class UsuarioEolAutenticacaoRetornoDto
    {
        public string CodigoRf { get; set; }
        public IEnumerable<Guid> Perfis { get; set; }
        public AutenticacaoStatusEol Status { get; set; }
    }
}