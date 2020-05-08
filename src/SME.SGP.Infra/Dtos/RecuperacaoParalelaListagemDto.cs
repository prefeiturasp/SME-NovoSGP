using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaListagemDto
    {
        public IEnumerable<EixoDto> Eixos { get; set; }
        public IEnumerable<ObjetivoDto> Objetivos { get; set; }
        public RecuperacaoParalelaOrdenacao? Ordenacao { get; set; }
        public RecuperacaoParalelaPeriodoListagemDto Periodo { get; set; }
        public IEnumerable<RespostaDto> Respostas { get; set; }
        public bool SomenteLeitura { get; set; }
    }
}