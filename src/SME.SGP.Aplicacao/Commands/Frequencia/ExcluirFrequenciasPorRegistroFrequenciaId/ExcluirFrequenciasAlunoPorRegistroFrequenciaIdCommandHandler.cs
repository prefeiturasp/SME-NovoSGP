using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommandHandler : IRequestHandler<ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<bool> Handle(ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioRegistroFrequenciaAluno.RemoverPorRegistroFrequenciaId(request.RegistroFrequenciaId);
            return true;
        }
    }
}
