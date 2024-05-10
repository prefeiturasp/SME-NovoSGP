using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
