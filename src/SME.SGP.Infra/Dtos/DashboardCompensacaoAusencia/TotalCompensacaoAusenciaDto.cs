using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TotalCompensacaoAusenciaDto
    {
        public string DescricaoAnoTurma { get; set; }
        public Modalidade ModalidadeCodigo { get; set; }
        public int Quantidade { get; set; }
        public string DescricaoAnoTurmaFormatado
        {
            get => $"{ModalidadeCodigo.ShortName()} - {DescricaoAnoTurma}";
        }
    }
}
