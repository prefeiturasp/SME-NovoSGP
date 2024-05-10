using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TurmaModalidadeCodigoDto
    {
        public long TurmaCodigo { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string ModalidadeDescricao { get { return ((Modalidade)this.ModalidadeCodigo).Name(); } }
    }
}
