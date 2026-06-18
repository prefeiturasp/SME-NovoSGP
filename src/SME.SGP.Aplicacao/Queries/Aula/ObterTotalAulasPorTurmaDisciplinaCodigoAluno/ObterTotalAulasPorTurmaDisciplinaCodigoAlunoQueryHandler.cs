using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQueryHandler : IRequestHandler<ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery, IEnumerable<TotalAulasNaoLancamNotaDto>>
    {
        private readonly IRepositorioAulaConsulta repositorioAulaConsulta;
        public ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQueryHandler(IRepositorioAulaConsulta repositorioAulaConsulta)
        {
            this.repositorioAulaConsulta = repositorioAulaConsulta;
        }

        public async Task<IEnumerable<TotalAulasNaoLancamNotaDto>> Handle(ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAulaConsulta.ObterTotalAulasPorTurmaDisciplinaAluno(request.DisciplinaId, request.CodigoTurma, request.CodigoAluno);
        }
    }
}
