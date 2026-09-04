using System;

namespace SME.SGP.Infra
{
    public class ObterFiltroParametrosEncaminhamentoNaapaDto
    {
        public string CodigoNomeAluno { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataAberturaQueixaFim { get; set; }
        public int Situacao { get; set; }
        public long Prioridade { get; set; }
        public long[] TurmasIds { get; set; }
        public string CodigoUe { get; set; }
        public bool ExibirEncerrados { get; set; }
    }
}
