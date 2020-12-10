using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PlanoAulaDto
    {
        public PlanoAulaDto() { }

        public long Id { get; set; }
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Desenvolvimento da aula é obrigatório para o registro do plano.")]
        public string DesenvolvimentoAula { get; set; }
        public string RecuperacaoAula { get; set; }
        public string LicaoCasa { get; set; }

        [Required(ErrorMessage = "É necessário vincular o plano a uma aula.")]
        public long AulaId { get; set; }
        public List<ObjetivoAprendizagemComponenteDto> ObjetivosAprendizagemComponente { get; set; }

        public long? ComponenteCurricularId { get; set; }

        public bool ConsideraHistorico { get; set; }
    }
}
