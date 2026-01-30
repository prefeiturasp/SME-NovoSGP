using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RetornoDadosAcessoUsuarioSgpDto
    {
        public string Token { get; set; }
        public DateTime DataExpiracaoToken { get; set; }
        public IEnumerable<int> Permissoes { get; set; }
    }
}