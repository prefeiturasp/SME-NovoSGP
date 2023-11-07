using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos
{
    public class TelefonesDto
    {
        public string DDD { get; set; }
        public string Numero { get; set; }
        public TipoTelefone TipoTelefone { get; set; }
    }
}
