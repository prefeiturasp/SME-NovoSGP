using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAulaRetornoDto
    {
        public PlanoAulaRetornoDto()
        {
            ObjetivosAprendizagem = new List<ObjetivoAprendizagemDto>();
            ObjetivosAprendizagemAula = new List<long>();
        }

        public string Descricao { get; set; }
        public string DesenvolvimentoAula { get; set; }
        public string RecuperacaoAula { get; set; }
        public string LicaoCasa { get; set; }
        public long AulaId { get; set; }

        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagem { get; set; }
        public List<long> ObjetivosAprendizagemAula { get; set; }
    }
}
