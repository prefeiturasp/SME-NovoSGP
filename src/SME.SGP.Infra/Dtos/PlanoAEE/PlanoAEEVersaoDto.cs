using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class PlanoAEEVersaoDto
    {
        public long PlanoAEEId { get; set; }
        public PlanoAEE PlanoAEE { get; set; }
        public int Numero { get; set; }
        public bool Excluido { get; set; }
    }
}
