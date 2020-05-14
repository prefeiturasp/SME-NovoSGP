using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class AbrangenciaFiltroRetorno
    {
        public string AbreviacaoModalidade { get { return this.Modalidade.GetAttribute<DisplayAttribute>().ShortName; } }
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public int CodigoModalidade { get { return (int)this.Modalidade; } }
        public string CodigoTurma { get; set; }
        public string CodigoUe { get; set; }
        public string DescricaoFiltro { get { return $"{AbreviacaoModalidade} - {NomeTurma} - {NomeUe} "; } }
        public Modalidade Modalidade { get; set; }
        public string NomeDre { get; set; }
        public string NomeModalidade { get { return this.Modalidade.GetAttribute<DisplayAttribute>().Name; } }
        public string NomeTurma { get; set; }
        public string NomeUe { get; set; }
        public int QtDuracaoAula { get; set; }
        public int Semestre { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public int TipoTurno { get; set; }
    }
}