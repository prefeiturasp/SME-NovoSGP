using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class UsuarioResumoCoreDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string NumeroDocumento { get; set; }
        public string Login { get; set; }
    }
}
