using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAulaRetornoDto
    {
        public string Descricao { get; set; }
        public string DesenvolvimentoAula { get; set; }
        public string RecuperacaoAula { get; set; }
        public string LicaoCasa { get; set; }
        public long AulaId { get; set; }

        public IEnumerable<ObjetivoAprendizagemDto> ObjetivosAprendizagem { get; set; }
        public IEnumerable<long> ObjetivosAprendizagemAula { get; set; }
    }
}
