using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class RegistroReclassificacaoPainelEducacionalDto
    {
        public string Dre { get; set; }
        public string Ue { get; set; }
        public int Ano { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int QuantidadeAlunosReclassificados { get; set; }
    }
}
