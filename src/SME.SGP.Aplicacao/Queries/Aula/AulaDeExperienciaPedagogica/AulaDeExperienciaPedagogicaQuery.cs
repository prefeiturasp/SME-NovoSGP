using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AulaDeExperienciaPedagogicaQuery : IRequest<bool>
    {
        public AulaDeExperienciaPedagogicaQuery(long componenteCurricular)
        {
            ComponenteCurricular = componenteCurricular;
        }

        public long ComponenteCurricular { get; set; }
    }
}
