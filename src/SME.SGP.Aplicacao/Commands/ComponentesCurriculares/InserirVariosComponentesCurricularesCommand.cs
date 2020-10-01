using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirVariosComponentesCurricularesCommand: IRequest<bool>
    {
        public IEnumerable<ComponenteCurricularDto> ComponentesCurriculares { get; set; }

        public InserirVariosComponentesCurricularesCommand(IEnumerable<ComponenteCurricularDto> componentesCurriculares)
        {
            ComponentesCurriculares = componentesCurriculares;
        }
    }
}
