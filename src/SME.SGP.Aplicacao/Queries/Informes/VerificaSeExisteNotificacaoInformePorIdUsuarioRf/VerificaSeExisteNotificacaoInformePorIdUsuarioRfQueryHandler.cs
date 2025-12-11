using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Informes.VerificaSeExisteNotificacaoInformePorIdUsuarioRf
{
    public class VerificaSeExisteNotificacaoInformePorIdUsuarioRfQueryHandler : IRequestHandler<VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery, bool>
    {
        private readonly IRepositorioInformativoNotificacao repositorio;
        public VerificaSeExisteNotificacaoInformePorIdUsuarioRfQueryHandler(IRepositorioInformativoNotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery request, CancellationToken cancellationToken)
        {
            return repositorio.VerificaSeExisteNotificacaoInformePorIdUsuarioRfAsync(request.InformativoId, request.UsuarioRf);
        }
    }
}
