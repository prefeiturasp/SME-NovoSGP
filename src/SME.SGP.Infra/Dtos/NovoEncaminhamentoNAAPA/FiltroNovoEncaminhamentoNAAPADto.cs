using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class FiltroNovoEncaminhamentoNAAPADto
    {
        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string CodigoNomeAluno { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataAberturaQueixaFim { get; set; }
        public int Situacao { get; set; }
        public int Prioridade { get; set; }
        public bool ExibirEncerrados { get; set; }
        public OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] Ordenacao { get; set; }
    }
}