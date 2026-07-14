using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotificarResultadoInsatisfatorioDto
    {
        public NotificarResultadoInsatisfatorioDto()
        {
            ComponentesCurriculares = new List<NotificarResultadoInsatisfatorioCCDto>();
        }
        public string TurmaNome { get; set; }
        public List<NotificarResultadoInsatisfatorioCCDto> ComponentesCurriculares { get; set; }
        public string TurmaModalidade { get; set; }
    }
}
