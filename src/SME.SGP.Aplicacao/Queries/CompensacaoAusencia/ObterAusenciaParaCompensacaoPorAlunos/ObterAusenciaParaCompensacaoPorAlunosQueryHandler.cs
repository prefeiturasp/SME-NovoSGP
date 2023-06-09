using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoPorAlunosQueryHandler : IRequestHandler<ObterAusenciaParaCompensacaoPorAlunosQuery, IEnumerable<CompensacaoDataAlunoDto>>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ObterAusenciaParaCompensacaoPorAlunosQueryHandler(IRepositorioCompensacaoAusencia compensacaoAusencia)
        {
            repositorioCompensacaoAusencia = compensacaoAusencia ?? throw new ArgumentNullException(nameof(compensacaoAusencia));
        }

        public async Task<IEnumerable<CompensacaoDataAlunoDto>> Handle(ObterAusenciaParaCompensacaoPorAlunosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCompensacaoAusencia.ObterAusenciaParaCompensacaoPorAlunos(request.CompensacaoAusenciaId, request.CodigosAlunos, request.DisciplinasId, request.Bimestre, request.Turmacodigo, request.Professor);
        }
    }
}