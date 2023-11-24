using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorFuncaoExternaHierarquicoQueryHandler : IRequestHandler<ObterFuncionariosPorFuncaoExternaHierarquicoQuery, IEnumerable<FuncionarioFuncaoExternaDTO>>
    {
        private readonly IMediator mediator;

        public ObterFuncionariosPorFuncaoExternaHierarquicoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FuncionarioFuncaoExternaDTO>> Handle(ObterFuncionariosPorFuncaoExternaHierarquicoQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<FuncionarioDTO> funcionarios = null;

            funcionarios = await ObterFuncionariosPorFuncaoExterna(request.CodigoUe, (int)request.FuncaoExterna);
            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);
            return funcionarios.Select(f => new FuncionarioFuncaoExternaDTO(f.CodigoRF, request.FuncaoExterna));
        }

        private FuncaoExterna? ObterProximoNivel(FuncaoExterna funcaoExterna, bool primeiroNivel)
        {
            switch (funcaoExterna)
            {
                case FuncaoExterna.CP:
                    return FuncaoExterna.AD;
                case FuncaoExterna.AD:
                    return FuncaoExterna.Diretor;
                case FuncaoExterna.Diretor:
                    return FuncaoExterna.AD;                       
                default:
                    return null;
            }
        }

        private async Task<IEnumerable<FuncionarioDTO>> ObterFuncionariosPorFuncaoExterna(string codigoUe, int funcaoExterna)
            => await mediator.Send(new ObterFuncionariosPorUeEFuncaoExternaQuery(codigoUe, funcaoExterna));
    }
}
