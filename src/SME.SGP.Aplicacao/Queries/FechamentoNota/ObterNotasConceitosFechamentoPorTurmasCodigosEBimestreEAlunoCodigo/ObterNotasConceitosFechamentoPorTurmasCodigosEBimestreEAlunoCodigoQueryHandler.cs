using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQueryHandler : IRequestHandler<ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery, IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterNotasFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> Handle(ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            return repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmasCodigosEBimestreEAlunoCodigoAsync(request.TurmasCodigos, request.Bimestre, request.AlunoCodigo);
        }
    }
}
