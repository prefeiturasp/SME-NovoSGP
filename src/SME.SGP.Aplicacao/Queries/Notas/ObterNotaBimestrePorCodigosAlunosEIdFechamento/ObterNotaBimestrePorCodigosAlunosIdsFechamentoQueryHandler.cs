using MediatR;
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

    public class ObterNotaBimestrePorCodigosAlunosIdsFechamentoQueryHandler : IRequestHandler<ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery, IEnumerable<FechamentoNotaDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplinaConsulta;
        public ObterNotaBimestrePorCodigosAlunosIdsFechamentoQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplinaConsulta)
        {
            this.repositorioFechamentoTurmaDisciplinaConsulta = repositorioFechamentoTurmaDisciplinaConsulta ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplinaConsulta));
        }

        public Task<IEnumerable<FechamentoNotaDto>> Handle(ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery request, CancellationToken cancellationToken)
        {
            return repositorioFechamentoTurmaDisciplinaConsulta.ObterNotasBimestrePorCodigosAlunosIdsFechamentos(request.CodigosAlunos, request.FechamentoId);
        }
    }
}
