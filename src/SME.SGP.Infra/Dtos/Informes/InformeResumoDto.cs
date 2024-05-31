using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Informes
{
    public class InformeResumoDto
    {
        public long Id { get; set; }
        public string DreNome { get; set; }
        public string UeNome { get; set; }
        public string DataEnvio { get; set; }
        public IEnumerable<GruposDeUsuariosDto> Perfis { get; set; }
        public string Titulo { get; set; }
        public string EnviadoPor { get; set; }
        public IEnumerable<ModalidadeRetornoDto> Modalidades { get; set; }
    }
}
