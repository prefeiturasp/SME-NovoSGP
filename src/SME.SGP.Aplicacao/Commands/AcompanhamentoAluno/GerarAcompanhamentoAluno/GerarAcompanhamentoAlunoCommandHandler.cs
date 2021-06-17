using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoAlunoCommandHandler : IRequestHandler<GerarAcompanhamentoAlunoCommand, long>
    {
        private readonly IRepositorioAcompanhamentoAluno repositorioAcompanhamentoAluno;

        public GerarAcompanhamentoAlunoCommandHandler(IRepositorioAcompanhamentoAluno repositorioAcompanhamentoAluno)
        {
            this.repositorioAcompanhamentoAluno = repositorioAcompanhamentoAluno ?? throw new ArgumentNullException(nameof(repositorioAcompanhamentoAluno));
        }

        public async Task<long> Handle(GerarAcompanhamentoAlunoCommand request, CancellationToken cancellationToken)
            => await repositorioAcompanhamentoAluno.SalvarAsync(new Dominio.AcompanhamentoAluno()
            {
                TurmaId = request.TurmaId,
                AlunoCodigo = request.AlunoCodigo,
            });
    }
}
