using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class SupervisorEscolasDreDto : AuditoriaDto
    {
        public long Id { get; set; }
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string EscolaId { get; set; }
        public int TipoAtribuicao { get; set; }
        public string SupervisorId { get; set; }
        public bool Excluido { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string UeNome { get; set; }
        public string DreNome { get; set; }
    }
}