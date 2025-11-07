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
    public class GravarHistoricoReinicioSenhaCommandHandler : IRequestHandler<GravarHistoricoReinicioSenhaCommand, bool>
    {
        private readonly IRepositorioHistoricoReinicioSenha repositorioHistoricoReinicioSenha;

        public GravarHistoricoReinicioSenhaCommandHandler(IRepositorioHistoricoReinicioSenha repositorioHistoricoReinicioSenha)
        {
            this.repositorioHistoricoReinicioSenha = repositorioHistoricoReinicioSenha ?? throw new ArgumentNullException(nameof(repositorioHistoricoReinicioSenha));
        }

        public async Task<bool> Handle(GravarHistoricoReinicioSenhaCommand request, CancellationToken cancellationToken)
        {

            await repositorioHistoricoReinicioSenha.SalvarAsync(new Dominio.HistoricoReinicioSenha()
            {
                UsuarioRf = request.UsuarioRf,
                DreCodigo = request.DreCodigo ?? "sme",
                UeCodigo = request.UeCodigo ?? ""
            });

            return true;
        }
    }
}
