using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class SupervisorEscolasDreDto : AuditoriaDto
    {
        public long AtribuicaoSupervisorId { get; set; }
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string EscolaId { get; set; }
        public int TipoAtribuicao { get; set; }
        public string SupervisorId { get; set; }
        public string SupervisorNome { get; set; }
        public bool AtribuicaoExcluida { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string UeNome { get; set; }
        public string DreNome { get; set; }
        public string NomeResponsavel { get; set; }

        public string Nome
        {
            get
            {
                if (TipoEscola == TipoEscola.Nenhum)
                    return UeNome;
                else return $"{TipoEscola.ShortName()} {UeNome}";
            }
        }
    }
}