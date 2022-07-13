using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorCargoUeQueryHandler : IRequestHandler<ObterFuncionariosPorCargoUeQuery,IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IServicoEol servicoEol;

        public ObterFuncionariosPorCargoUeQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorCargoUeQuery request, CancellationToken cancellationToken)
        {
            return await servicoEol.ObterFuncionariosPorCargoUe(request.UeId,request.CargoId);
        }
    }
}