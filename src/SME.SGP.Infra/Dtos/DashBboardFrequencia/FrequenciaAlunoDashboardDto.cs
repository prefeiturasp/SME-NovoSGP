using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoDashboardDto
    {
        //public string Ano { get; set; }
        //public string DescricaoAnoTurma { get; set; }
        //public string DreAbreviacao { get; set; }
        public string Descricao { get; set; }
        public string DreCodigo { get; set; }
        // public Modalidade ModalidadeCodigo { get; set; }
        public TipoFrequencia TipoFrequenciaAluno { get; set; }
        public int Presentes { get; set; } 
        public int Remotos { get; set; } 
        public int Ausentes { get; set; }        

        //public string DescricaoAnoTurmaFormatado
        //{
        //    get => $"{ModalidadeCodigo.ShortName()} - {DescricaoAnoTurma}";
        //}
    }
}
