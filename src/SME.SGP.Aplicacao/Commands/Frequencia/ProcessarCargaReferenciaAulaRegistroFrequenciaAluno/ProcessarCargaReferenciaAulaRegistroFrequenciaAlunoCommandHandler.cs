using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommandHandler : IRequestHandler<ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommand, bool>
    {
        IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<bool> Handle(ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommand request, CancellationToken cancellationToken)
        {
            foreach(var dto in request.ListaDeRegistroAula)
            {
               await this.repositorioRegistroFrequenciaAluno.AlterarRegistroAdicionandoAula(dto.RegistroFrequenciaId, dto.AulaId);
            }

            return true;
        }
    }
}
