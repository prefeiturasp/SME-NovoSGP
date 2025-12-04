using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RelatorioDinamicoNAAPADto
    {
        public long TotalRegistro { get; set; }
        public IEnumerable<TotalRegistroPorModalidadeRelatorioDinamicoNAAPA> TotalRegistroPorModalidadesAno { get; set; }
        public PaginacaoResultadoDto<AtendimentoNAAPARelatorioDinamico> EncaminhamentosNAAPAPaginado { get; set; }
        public TotalDeAtendimentoDto TotalDeAtendimento { get; set; }
        public long[] EncaminhamentosNAAPAIds { get; set; }
    }
}
