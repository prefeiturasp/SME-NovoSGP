using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterTodosAlunosNasTurmasQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTodosAlunosNasTurmasQuery(string anoEscolar, int modalidadeTurma, int anoLetivo)
        {
            AnoLetivo = anoLetivo;
            AnoEscolar = anoEscolar;
            ModalidadeTurma = modalidadeTurma;
        }

        public int ModalidadeTurma { get; set; }
        public int AnoLetivo { get; set; }
        public string AnoEscolar { get; set;}
    }
}
