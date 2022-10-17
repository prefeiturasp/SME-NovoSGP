using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComplementaresPorAlunoQueryHandler : IRequestHandler<ObterTurmasComplementaresPorAlunoQuery, IEnumerable<TurmaComplementarDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        public ObterTurmasComplementaresPorAlunoQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta)); 
        }

        public async Task<IEnumerable<TurmaComplementarDto>> Handle(ObterTurmasComplementaresPorAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaConsulta.ObterTurmasComplementaresPorAlunos(request.AlunosCodigos);
        }
    }
}
