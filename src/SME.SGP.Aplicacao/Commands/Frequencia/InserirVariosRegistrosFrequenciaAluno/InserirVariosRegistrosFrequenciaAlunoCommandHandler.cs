using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirVariosRegistrosFrequenciaAlunoCommandHandler : IRequestHandler<InserirVariosRegistrosFrequenciaAlunoCommand, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public InserirVariosRegistrosFrequenciaAlunoCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<bool> Handle(InserirVariosRegistrosFrequenciaAlunoCommand request, CancellationToken cancellationToken)
            => await repositorioRegistroFrequenciaAluno.InserirVarios(request.FrequenciasPersistir);
    }
}
