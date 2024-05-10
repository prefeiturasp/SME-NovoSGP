using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirVariosCompensacaoAusenciaRegenciaCommandHandler : IRequestHandler<InserirVariosCompensacaoAusenciaRegenciaCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public InserirVariosCompensacaoAusenciaRegenciaCommandHandler(IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia)
        {
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaDisciplinaRegencia));
        }

        public Task<bool> Handle(InserirVariosCompensacaoAusenciaRegenciaCommand request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaDisciplinaRegencia.InserirVarios(request.CompensacaoAusenciaDisciplinaRegencias, request.UsuarioLogado);
        }
    }
}
