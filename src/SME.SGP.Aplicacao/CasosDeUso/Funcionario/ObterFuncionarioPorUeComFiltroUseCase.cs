using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionarioPorUeComFiltroUseCase : AbstractUseCase, IObterFuncionarioPorUeComFiltroUseCase
    {
        public ObterFuncionarioPorUeComFiltroUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar((string codigoUe, string filtro) param)
        {
            var servidores = await mediator.Send(new ObterFuncionariosPorUeQuery(param.codigoUe));

            if (!string.IsNullOrEmpty(param.filtro))
                return servidores.Where(servidor => servidor.NomeServidor.IndexOf(param.filtro, StringComparison.OrdinalIgnoreCase) != -1 ||
                                                    servidor.CodigoRf.IndexOf(param.filtro, StringComparison.OrdinalIgnoreCase) != -1);

            return servidores;
        }
    }
}
