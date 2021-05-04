using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TurmaModalidadeDto
    {
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public bool ModalidadeInfantil { get => Modalidade == Modalidade.InfantilCEI || Modalidade == Modalidade.InfantilPreEscola; }
    }
}
