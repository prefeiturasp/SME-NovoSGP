using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQueryHandler : IRequestHandler<ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery, IEnumerable<FechamentoTurmaDisciplinaPendenciaDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public Task<IEnumerable<FechamentoTurmaDisciplinaPendenciaDto>> Handle(ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery request, CancellationToken cancellationToken)
            => repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinaDTOPorUeSituacao(request.IdUe, request.SituacoesFechamento, request.IdsFechamentoTurmaDisciplinaIgnorados);
    }
}
