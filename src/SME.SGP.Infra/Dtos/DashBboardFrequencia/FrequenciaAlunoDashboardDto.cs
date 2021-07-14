using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoDashboardDto
    {
        public string DescricaoAnoTurma { get; set; }
        public Modalidade ModalidadeCodigo { get; set; }
        public TipoFrequenciaDashboard TipoFrequenciaAluno { get; set; }
        public int Quantidade { get; set; }

        public string DescricaoAnoTurmaFormatado
        {
            get => $"{ModalidadeCodigo.ShortName()} - {DescricaoAnoTurma}";
        }
    }
}
