using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarListaDto
    {
        [ListaTemElementos(ErrorMessage = "Nenhum período foi informado")]
        public List<PeriodoEscolarDto> Periodos { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "É necessário informar o tipo de calendário")]
        public long TipoCalendario { get; set; }
    }
}