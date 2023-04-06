using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaDiciplinaRegenciaCommandHandler : IRequestHandler<SalvarCompensacaoAusenciaDiciplinaRegenciaCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public SalvarCompensacaoAusenciaDiciplinaRegenciaCommandHandler(IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia)
        {
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia;
        }

        public Task<bool> Handle(SalvarCompensacaoAusenciaDiciplinaRegenciaCommand request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaDisciplinaRegencia.SalvarAsync(request.CompensacaoAusenciaDisciplinaRegencia);
        }
    }
}
