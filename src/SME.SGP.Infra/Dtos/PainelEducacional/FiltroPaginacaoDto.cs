using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroPaginacaoDto
    {
        public int NumeroPagina { get; set; } = 1;

        public int NumeroRegistros { get; set; } = 10;

        public Paginacao ObterPaginacao() => new Paginacao(NumeroPagina, NumeroRegistros);
    }
}
