using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoDto : AuditoriaDto
    {
        public FechamentoDto()
        {
            FechamentosBimestres = new List<FechamentoBimestreDto>();
        }

        public bool ConfirmouAlteracaoHierarquica { get; set; }
        public string DreId { get; set; }
        public IEnumerable<FechamentoBimestreDto> FechamentosBimestres { get; set; }
        public long Id { get; set; }
        public bool Migrado { get; set; }
        public long TipoCalendarioId { get; set; }
        public string UeId { get; set; }
    }
}