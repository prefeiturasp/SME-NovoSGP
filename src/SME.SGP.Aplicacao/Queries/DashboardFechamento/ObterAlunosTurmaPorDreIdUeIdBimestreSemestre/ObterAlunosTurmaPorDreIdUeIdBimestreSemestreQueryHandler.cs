using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class
        ObterAlunosTurmaPorDreIdUeIdBimestreSemestreQueryHandler : IRequestHandler<ObterAlunosTurmaPorDreIdUeIdBimestreSemestreQuery,
            IEnumerable<TurmaAlunoBimestreFechamentoDto>>
    {
        private readonly IRepositorioTurma repositorio;

        public ObterAlunosTurmaPorDreIdUeIdBimestreSemestreQueryHandler(IRepositorioTurma repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaAlunoBimestreFechamentoDto>> Handle(ObterAlunosTurmaPorDreIdUeIdBimestreSemestreQuery request,
            CancellationToken cancellationToken)
            => await repositorio.AlunosTurmaPorDreIdUeIdBimestreSemestre(request.UeId,
                request.Ano, request.DreId, request.Modalidade,
                request.Semestre, request.Bimestre);
    }
}