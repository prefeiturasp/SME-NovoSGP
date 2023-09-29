using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommand : IRequest<bool>
    {
        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }

        public GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommand(EncaminhamentoAEE encaminhamentoAEE)
        {
            EncaminhamentoAEE = encaminhamentoAEE;
        }
    }
}
