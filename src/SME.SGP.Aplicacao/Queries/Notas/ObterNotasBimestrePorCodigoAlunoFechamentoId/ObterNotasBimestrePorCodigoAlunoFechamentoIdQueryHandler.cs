using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasBimestrePorCodigoAlunoFechamentoIdQueryHandler : IRequestHandler<ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery, IEnumerable<FechamentoNotaDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterNotasBimestrePorCodigoAlunoFechamentoIdQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<FechamentoNotaDto>> Handle(ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery request, CancellationToken cancellationToken)
         => await repositorioFechamentoTurmaDisciplina.ObterNotasBimestre(request.CodigoAluno, request.FechamentoTurmaId);
    }
}
