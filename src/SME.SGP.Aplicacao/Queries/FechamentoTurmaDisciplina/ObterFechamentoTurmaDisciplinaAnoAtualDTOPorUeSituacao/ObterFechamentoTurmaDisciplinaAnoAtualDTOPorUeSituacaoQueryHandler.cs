using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

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
