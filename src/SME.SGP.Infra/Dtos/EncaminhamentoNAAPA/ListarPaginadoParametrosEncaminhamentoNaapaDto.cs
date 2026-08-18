using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class ListarPaginadoParametrosEncaminhamentoNaapaDto
    {
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoNomeAluno { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataAberturaQueixaFim { get; set; }
        public int Situacao { get; set; }
        public long Prioridade { get; set; }
        public long[] TurmasIds { get; set; }
        public Paginacao Paginacao { get; set; }
        public bool ExibirEncerrados { get; set; }
        public OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] Ordenacao { get; set; }
    }
}
