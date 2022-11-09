using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class ObterFuncionariosPorUeQueryFake : IRequestHandler<ObterFuncionariosPorUeQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorUeQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<UsuarioEolRetornoDto>();

            foreach (var servidor in request.CodigosRfs.ToList())
            {
                lista.Add(new UsuarioEolRetornoDto
                {
                    CodigoRf = servidor,
                    NomeServidor = $"Nome Servidor ${servidor}",
                    CodigoFuncaoAtividade = 1,
                    UsuarioId = 1,
                    EstaAfastado = false,
                    Login = servidor,
                    PodeEditar = true
                });
            }
            return lista;
        }
    }
}