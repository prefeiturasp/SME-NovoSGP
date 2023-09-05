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
        private readonly IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusenciaConsulta;

        public ObterAusenciaParaCompensacaoPorAlunosQueryHandler(IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusenciaConsulta)
        {
            this.repositorioCompensacaoAusenciaConsulta = repositorioCompensacaoAusenciaConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaConsulta));
        }

        public async Task<IEnumerable<CompensacaoDataAlunoDto>> Handle(ObterAusenciaParaCompensacaoPorAlunosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCompensacaoAusenciaConsulta.ObterAusenciaParaCompensacaoPorAlunos(request.CompensacaoAusenciaId, request.CodigosAlunos, request.DisciplinasId, request.Bimestre, request.Turmacodigo, request.Professor);
        }
    }
}