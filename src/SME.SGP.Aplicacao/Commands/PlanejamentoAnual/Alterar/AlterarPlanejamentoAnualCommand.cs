using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PlanejamentoAnual.Alterar
{
    public class AlterarPlanejamentoAnualCommand : IRequest<AuditoriaDto>
    {
        public long Id { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long PlanejamentoAnualPeriodoEscolarId { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public bool EhAlteracao => Id > 0;
        public IEnumerable<ComponentePlanejamentoAnualDto> Componentes { get; set; }
    }
}
