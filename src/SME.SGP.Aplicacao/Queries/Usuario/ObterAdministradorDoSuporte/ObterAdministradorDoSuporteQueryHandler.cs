using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Usuario.ObterAdministradorDoSuporte
{
    public class ObterAdministradorDoSuporteQueryHandler : IRequestHandler<ObterAdministradorDoSuporteQuery, AdministradorSuporte>
    {
        private readonly IContextoAplicacao contextoAplicacao;

        public ObterAdministradorDoSuporteQueryHandler(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public Task<AdministradorSuporte> Handle(ObterAdministradorDoSuporteQuery request, CancellationToken cancellationToken)
        {
            var administrador = new AdministradorSuporte()
            {
                Login = this.contextoAplicacao.ObterVariavel<string>("Administrador") ?? string.Empty,
                Nome = this.contextoAplicacao.ObterVariavel<string>("NomeAdministrador") ?? string.Empty
            };

            return Task.FromResult(administrador);
        }
    }
}
