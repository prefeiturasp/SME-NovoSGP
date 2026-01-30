using Newtonsoft.Json;
using System;

namespace SME.SGP.Infra.Dtos
{
    public class AutenticacaoSSODto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("payload")]
        public PayloadAutenticacaoSSODto Payload { get; set; }
    }

    public class PayloadAutenticacaoSSODto
    {
        [JsonProperty("usuario_id")]
        public Guid UsuarioId { get; set; }
        public string CodigoRf { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public Guid Perfil { get; set; }
    }
}
