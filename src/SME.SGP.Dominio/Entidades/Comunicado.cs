using System;

namespace SME.SGP.Dominio
{
    public class Comunicado : EntidadeBase
    {
        public string ComunicadoGrupoId { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public string Titulo { get; set; }
    }
}