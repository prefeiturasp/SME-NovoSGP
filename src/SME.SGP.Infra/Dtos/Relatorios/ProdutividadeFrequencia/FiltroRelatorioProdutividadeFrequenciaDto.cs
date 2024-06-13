using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioProdutividadeFrequenciaDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int? Bimestre { get; set; }
        public string RfProfessor { get; set; }
        public TipoRelatorioProdutividadeFrequencia TipoRelatorioProdutividade { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }

    public enum TipoRelatorioProdutividadeFrequencia
    {
        [Display(Name = "Média por UE")]
        MédiaPorUE = 1,
        [Display(Name = "Média por Professor")]
        MédiaPorProfessor = 2,
        [Display(Name = "Analítico")]
        Analitico = 3,
    }
}
