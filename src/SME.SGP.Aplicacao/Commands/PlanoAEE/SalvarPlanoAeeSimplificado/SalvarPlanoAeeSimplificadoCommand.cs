using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAeeSimplificadoCommand : IRequest<bool>
    {
        public SalvarPlanoAeeSimplificadoCommand(PlanoAEE planoAEE)
        {
            PlanoAEE = planoAEE;
        }

        public PlanoAEE PlanoAEE { get; set; }
    }
}
