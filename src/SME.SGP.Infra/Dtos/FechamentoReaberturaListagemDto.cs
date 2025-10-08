using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FechamentoReaberturaListagemDto
    {
        public bool[] Bimestres { get; set; }
        public int BimestresQuantidadeTotal { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public AplicacaoSondagem Aplicacao { get; set; }
    }
}