using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorFuncaoAtividadeHierarquicoQueryHandler : IRequestHandler<ObterFuncionariosPorFuncaoAtividadeHierarquicoQuery, IEnumerable<FuncionarioFuncaoAtividadeDTO>>
    {
        private readonly IMediator mediator;

        public ObterFuncionariosPorFuncaoAtividadeHierarquicoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FuncionarioFuncaoAtividadeDTO>> Handle(ObterFuncionariosPorFuncaoAtividadeHierarquicoQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<UsuarioEolRetornoDto> funcionarios = null;

            funcionarios = await ObterFuncionariosPorFuncaoAtividade(request.CodigoUe, (int)request.CodigoFuncaoAtividade);
            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);
            if (funcionariosDisponiveis == null)
                return Enumerable.Empty<FuncionarioFuncaoAtividadeDTO>();
            return funcionarios.Select(f => new FuncionarioFuncaoAtividadeDTO(f.CodigoRf, request.CodigoFuncaoAtividade));
        }

        private async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorFuncaoAtividade(string codigoUe, int funcaoAtividade)
            => await mediator.Send(new ObterFuncionariosPorFuncaoAtividadeUeQuery(codigoUe, funcaoAtividade));
    }
}
