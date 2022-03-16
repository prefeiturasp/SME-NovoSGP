using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoAlunoIDPorTurmaQueryHandler : IRequestHandler<ObterAcompanhamentoAlunoIDPorTurmaQuery, long>
    {
        private readonly IRepositorioAcompanhamentoAlunoConsulta repositorio;

        public ObterAcompanhamentoAlunoIDPorTurmaQueryHandler(IRepositorioAcompanhamentoAlunoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(ObterAcompanhamentoAlunoIDPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterPorTurmaEAluno(request.TurmaId, request.AlunoCodigo);
    }
}
