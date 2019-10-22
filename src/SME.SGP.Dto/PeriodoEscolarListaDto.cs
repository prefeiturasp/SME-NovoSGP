using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class PeriodoEscolarListaDto
    {
        [ListaTemElementos(ErrorMessage = "Nenhum período foi informado")]
        public List<PeriodoEscolarDto> Periodos { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "É necessario informar o tipo de calendario")]
        public long TipoCalendario { get; set; }
    }
}