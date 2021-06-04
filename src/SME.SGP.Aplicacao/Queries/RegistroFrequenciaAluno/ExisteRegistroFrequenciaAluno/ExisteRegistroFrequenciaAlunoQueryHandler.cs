using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteRegistroFrequenciaAlunoQueryHandler : IRequestHandler<ExisteRegistroFrequenciaAlunoQuery, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ExisteRegistroFrequenciaAlunoQueryHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<bool> Handle(ExisteRegistroFrequenciaAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ExisteRegistroFrequenciaAlunoAsync(request.RegistroFrequenciaId, request.CodigoAluno, request.NumeroAula);
        }
    }
}
