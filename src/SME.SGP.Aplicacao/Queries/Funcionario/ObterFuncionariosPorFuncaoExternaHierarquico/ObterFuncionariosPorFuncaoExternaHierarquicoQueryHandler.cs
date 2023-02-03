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
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterFuncionariosPorFuncaoExternaHierarquicoQueryHandler(IMediator mediator, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<FuncionarioFuncaoExternaDTO>> Handle(ObterFuncionariosPorFuncaoExternaHierarquicoQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<SupervisorEscolasDreDto> supervisoresEscola = null;
            IEnumerable<FuncionarioDTO> funcionarios = null;

            /*if (request.FuncaoExterna == FuncaoExterna.Supervisor)
                supervisoresEscola = await repositorioSupervisorEscolaDre.ObtemSupervisoresPorUeAsync(request.CodigoUe);
            else*/
            funcionarios = await ObterFuncionariosPorFuncaoExterna(request.CodigoUe, (int)request.FuncaoExterna);
            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);

            /*if (request.FuncaoExterna == FuncaoExterna.Supervisor ?
                supervisoresEscola == null || !supervisoresEscola.Any() :
                funcionarios == null || !funcionarios.Any() || (!funcionariosDisponiveis.Any() && request.NotificacaoExigeAcao))
            {
                FuncaoExterna? funcaoExternaProximoNivel = ObterProximoNivel(request.FuncaoExterna, request.PrimeiroNivel);

                if (!funcaoExternaProximoNivel.HasValue)
                    return null;

                return await mediator.Send(new ObterFuncionariosPorFuncaoExternaHierarquicoQuery(request.CodigoUe, funcaoExternaProximoNivel.Value, false));
            }
            else*/
            {
                /*if (request.FuncaoExterna == FuncaoExterna.Supervisor)
                    return supervisoresEscola.Select(s => new FuncionarioFuncaoExternaDTO(s.SupervisorId, request.FuncaoExterna));
                else*/
                return funcionarios.Select(f => new FuncionarioFuncaoExternaDTO(f.CodigoRF, request.FuncaoExterna));
            }
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
                    /*if (!primeiroNivel)
                        return FuncaoExterna.Supervisor;
                    else*/
                    return FuncaoExterna.AD;                       
                /*case FuncaoExterna.Supervisor:
                    return FuncaoExterna.SupervisorTecnico;*/
                default:
                    return null;
            }
        }

        private async Task<IEnumerable<FuncionarioDTO>> ObterFuncionariosPorFuncaoExterna(string codigoUe, int funcaoExterna)
            => await mediator.Send(new ObterFuncionariosPorUeEFuncaoExternaQuery(codigoUe, funcaoExterna));
    }
}
