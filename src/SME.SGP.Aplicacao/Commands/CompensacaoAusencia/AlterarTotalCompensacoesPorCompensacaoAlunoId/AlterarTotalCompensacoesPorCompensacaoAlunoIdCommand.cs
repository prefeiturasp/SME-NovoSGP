using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarTotalCompensacoesPorCompensacaoAlunoIdCommand : IRequest<bool>
    {
        public long CompensacaoAlunoId { get; set; }
        public int Quantidade { get; set; }
    }
}
