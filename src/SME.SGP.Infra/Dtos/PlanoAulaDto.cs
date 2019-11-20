using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PlanoAulaDto
    {
        public PlanoAulaDto() { }

        public string Descricao { get; set; }
        [Required(ErrorMessage = "Desenvolvimento da aula é obrigatório para o registro do plano.")]
        public string DesenvolvimentoAula { get; set; }
        public string RecuperacaoAula { get; set; }
        public string LicaoCasa { get; set; }

        [Required(ErrorMessage = "É necessário vincular o plano a uma aula.")]
        public long AulaId { get; set; }

        [ListaTemElementos(ErrorMessage = "Os objetivos de aprendizagem da aula devem ser informados.")]
        public List<long> ObjetivosAprendizagemAula { get; set; }
    }
}
