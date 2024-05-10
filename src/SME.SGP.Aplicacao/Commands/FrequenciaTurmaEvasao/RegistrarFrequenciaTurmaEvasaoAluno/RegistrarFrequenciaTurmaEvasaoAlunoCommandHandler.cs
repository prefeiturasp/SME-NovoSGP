using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarFrequenciaTurmaEvasaoAlunoCommandHandler : IRequestHandler<RegistrarFrequenciaTurmaEvasaoAlunoCommand, long>
    {
        private readonly IRepositorioFrequenciaTurmaEvasaoAluno repositorioFrequenciaTurmaEvasaoAluno;

        public RegistrarFrequenciaTurmaEvasaoAlunoCommandHandler(IRepositorioFrequenciaTurmaEvasaoAluno repositorioFrequenciaTurmaEvasaoAluno)
        {
            this.repositorioFrequenciaTurmaEvasaoAluno = repositorioFrequenciaTurmaEvasaoAluno ?? throw new ArgumentNullException(nameof(repositorioFrequenciaTurmaEvasaoAluno));
        }

        public async Task<long> Handle(RegistrarFrequenciaTurmaEvasaoAlunoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaTurmaEvasaoAluno.Inserir(new FrequenciaTurmaEvasaoAluno()
            {
                FrequenciaTurmaEvasaoId = request.FrequenciaTurmaEvasaoId,
                AlunoCodigo = request.AlunoCodigo,  
                AlunoNome = request.AlunoNome,
                PercentualFrequencia = request.PercentualFrequencia,
            });
        }
    }
}
