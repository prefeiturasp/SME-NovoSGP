using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CartaIntencoes.ServicosFakes
{
    public class ObterUsuariosNotificarCartaIntencoesObservacaoQueryHandlerFake : IRequestHandler<ObterUsuariosNotificarCartaIntencoesObservacaoQuery, IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>
    {
        public ObterUsuariosNotificarCartaIntencoesObservacaoQueryHandlerFake()
        { }

        public async Task<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>> Handle(ObterUsuariosNotificarCartaIntencoesObservacaoQuery request, CancellationToken cancellationToken)
        {
            return new List<UsuarioNotificarCartaIntencoesObservacaoDto>()
            {
               new UsuarioNotificarCartaIntencoesObservacaoDto()
               {  
                   Nome = "Professor Teste",
                   PodeRemover = false,
                   UsuarioId = 9999999
               }
            };
        }
    }
}
