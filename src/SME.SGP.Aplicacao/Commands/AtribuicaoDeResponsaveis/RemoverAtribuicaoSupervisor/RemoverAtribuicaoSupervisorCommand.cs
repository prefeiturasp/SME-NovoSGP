using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoSupervisorCommand : IRequest<long>
    {
        public RemoverAtribuicaoSupervisorCommand(SupervisorEscolaDre supervisorEscolar)
        {
            SuperVisorEscolar = supervisorEscolar;
        }

        public SupervisorEscolaDre SuperVisorEscolar { get; set; }
    }
}
