using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommandHandler : IRequestHandler<ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommand, bool>
    {
       
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioFrequencia)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<bool> Handle(ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioRegistroFrequenciaAluno.RemoverPorRegistroFrequenciaIdENumeroAula(request.RegistroFrequenciaId, request.NumeroAula, request.AlunoCodigo);
            return true;
        }
    }
}
