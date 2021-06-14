using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRegistroFrequenciaAlunoCommandHandler : IRequestHandler<SalvarRegistroFrequenciaAlunoCommand, long>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public SalvarRegistroFrequenciaAlunoCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<long> Handle(SalvarRegistroFrequenciaAlunoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.SalvarAsync(request.RegistroFrequenciaAluno);
        }
    }
}
