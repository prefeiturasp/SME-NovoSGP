using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FechamentoDto : AuditoriaDto
    {
        public FechamentoDto()
        {
            FechamentosBimestres = new List<FechamentoBimestreDto>();
        }

        public bool ConfirmouAlteracaoHierarquica { get; set; }
        public long? DreId { get; set; }
        public IEnumerable<FechamentoBimestreDto> FechamentosBimestres { get; set; }
        public bool Migrado { get; set; }

        [Required(ErrorMessage = "O tipo de calendário é obrigatório")]
        public long? TipoCalendarioId { get; set; }
        public Dominio.Aplicacao Aplicacao { get; set; }
        public long? UeId { get; set; }
        public bool EhRegistroExistente { get; set; }
    }
}