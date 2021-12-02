using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PlanoAulaDto
    {
        public PlanoAulaDto() { }

        public long Id { get; set; }
        public string Descricao { get; set; }
        public string RecuperacaoAula { get; set; }
        public string LicaoCasa { get; set; }
        [Required(ErrorMessage = "É necessário vincular o plano a uma aula.")]
        public long AulaId { get; set; }
        public List<ObjetivoAprendizagemComponenteDto> ObjetivosAprendizagemComponente { get; set; }

        public long? ComponenteCurricularId { get; set; }

        public bool ConsideraHistorico { get; set; }
    }

    public class PlanoAulaResumidoDto
    {
        public string DescricaoAtual { get; set; }
        public string RecuperacaoAulaAtual { get; set; }
        public string LicaoCasaAtual { get; set; }

        public string DescricaoNovo { get; set; }
        public string RecuperacaoAulaNovo { get; set; }
        public string LicaoCasaNovo { get; set; }
    }
}
