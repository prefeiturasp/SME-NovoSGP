using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class CarregarDadosAcessoPorLoginPerfilQueryHandlerFake : IRequestHandler<CarregarDadosAcessoPorLoginPerfilQuery, RetornoDadosAcessoUsuarioSgpDto>
    {
        public async Task<RetornoDadosAcessoUsuarioSgpDto> Handle(CarregarDadosAcessoPorLoginPerfilQuery request, CancellationToken cancellationToken)
        {
            return new RetornoDadosAcessoUsuarioSgpDto()
            {
                Permissoes = new List<int>() { 1 },
                Token = "",
                DataExpiracaoToken = DateTime.Now
            };
        }
    }
}
