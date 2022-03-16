using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAlunosComAcompanhamentoQueryHandler : IRequestHandler<ObterTotalAlunosComAcompanhamentoQuery, int>
    {
        private readonly IRepositorioAcompanhamentoAlunoConsulta repositorio;

        public ObterTotalAlunosComAcompanhamentoQueryHandler(IRepositorioAcompanhamentoAlunoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<int> Handle(ObterTotalAlunosComAcompanhamentoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterTotalAlunosComAcompanhamentoPorTurmaSemestre(request.TurmaId, request.Semestre);
        }
    }
}
