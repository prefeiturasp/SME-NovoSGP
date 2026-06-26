using System;

namespace SME.SGP.Infra
{
    public class RevalidacaoTokenDto
    {
        public DateTime DataHoraExpiracao { get; set; }
        public string Token { get; set; }
    }
}
