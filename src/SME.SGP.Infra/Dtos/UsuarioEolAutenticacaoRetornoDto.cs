using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class UsuarioEolAutenticacaoRetornoDto
    {
        public string CodigoRf { get; set; }
        public IEnumerable<Guid> Perfis { get; set; }
        public bool PossuiCargoCJ { get; set; }
        public bool PossuiPerfilCJ { get; set; }
        public AutenticacaoStatusEol Status { get; set; }
        public Guid UsuarioId { get; set; }
    }
}