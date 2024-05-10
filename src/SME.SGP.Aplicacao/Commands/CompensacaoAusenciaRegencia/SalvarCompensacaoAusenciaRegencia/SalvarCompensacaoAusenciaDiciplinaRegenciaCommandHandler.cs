using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaDiciplinaRegenciaCommandHandler : IRequestHandler<SalvarCompensacaoAusenciaDiciplinaRegenciaCommand, long>
    {
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public SalvarCompensacaoAusenciaDiciplinaRegenciaCommandHandler(IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia)
        {
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia;
        }

        public Task<long> Handle(SalvarCompensacaoAusenciaDiciplinaRegenciaCommand request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaDisciplinaRegencia.SalvarAsync(request.CompensacaoAusenciaDisciplinaRegencia);
        }
    }
}
