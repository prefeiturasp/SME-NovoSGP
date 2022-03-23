using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AtualizarComponenteCurricularCommand: IRequest<bool>
    {
        public ComponenteCurricularDto ComponenteCurricular { get; set; }

        public AtualizarComponenteCurricularCommand(ComponenteCurricularDto componenteCurricular)
        {
            ComponenteCurricular = componenteCurricular;
        }
    }
}
