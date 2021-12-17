using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{ 
    public class ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommandHandler : IRequestHandler<ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommand, bool>
    {
       
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IMediator mediator;
        public ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioFrequencia, IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioRegistroFrequenciaAluno.RemoverPorRegistroFrequenciaIdENumeroAula(request.RegistroFrequenciaId, request.NumeroAula, request.AlunoCodigo);
            return true;
        }
    }
}
