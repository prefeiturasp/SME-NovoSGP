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
    public class ObterFuncionariosPorCargoHierarquicoQueryHandler : IRequestHandler<ObterFuncionariosPorCargoHierarquicoQuery, IEnumerable<FuncionarioCargoDTO>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterFuncionariosPorCargoHierarquicoQueryHandler(IMediator mediator, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<FuncionarioCargoDTO>> Handle(ObterFuncionariosPorCargoHierarquicoQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<SupervisorEscolasDreDto> supervisoresEscola = null;
            IEnumerable<FuncionarioDTO> funcionarios = null;

            if (request.Cargo == Cargo.Supervisor)
                supervisoresEscola = await repositorioSupervisorEscolaDre.ObtemSupervisoresPorUeAsync(request.CodigoUe);
            else
                funcionarios = await ObterFuncionariosPorCargo(request.CodigoUe, (int)request.Cargo);

            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);

            if (request.Cargo == Cargo.Supervisor ?
                supervisoresEscola == null || !supervisoresEscola.Any() :
                funcionarios == null || !funcionarios.Any() || (!funcionariosDisponiveis.Any() && request.NotificacaoExigeAcao))
            {
                Cargo? cargoProximoNivel = ObterProximoNivel(request.Cargo, request.PrimeiroNivel);

                if (!cargoProximoNivel.HasValue)
                    return null;

                return await mediator.Send(new ObterFuncionariosPorCargoHierarquicoQuery(request.CodigoUe, cargoProximoNivel.Value, false));
            }
            else
            {
                if (request.Cargo == Cargo.Supervisor)
                    return supervisoresEscola.Select(s => new FuncionarioCargoDTO(s.SupervisorId, request.Cargo));
                else
                    return funcionarios.Select(f => new FuncionarioCargoDTO(f.CodigoRF, request.Cargo));
            }
        }

        private Cargo? ObterProximoNivel(Cargo cargo, bool primeiroNivel)
        {
            switch (cargo)
            {
                case Cargo.CP:
                    return Cargo.AD;
                case Cargo.AD:
                    return Cargo.Diretor;
                case Cargo.Diretor:
                    if (primeiroNivel)
                        return Cargo.AD;
                    else
                        return Cargo.Supervisor;
                case Cargo.Supervisor:
                    return Cargo.SupervisorTecnico;
                default:
                    return null;
            }
        }

        private async Task<IEnumerable<FuncionarioDTO>> ObterFuncionariosPorCargo(string codigoUe, int cargo)
            => await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, cargo));
    }
}
