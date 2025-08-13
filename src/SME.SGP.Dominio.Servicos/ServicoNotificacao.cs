using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoNotificacao : IServicoNotificacao
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacaoConsulta;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IMediator mediator;

        public ServicoNotificacao(IRepositorioNotificacao repositorioNotificacao,
                                  IRepositorioNotificacaoConsulta repositorioNotificacaoConsulta,
                                  IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                  IMediator mediator)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioNotificacaoConsulta = repositorioNotificacaoConsulta ?? throw new ArgumentNullException(nameof(repositorioNotificacaoConsulta));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task ExcluirFisicamenteAsync(long[] ids)
        {
            var notificacoes = await repositorioNotificacaoConsulta.ObterUsuariosNotificacoesPorIds(ids);
            await repositorioNotificacao.ExcluirPorIdsAsync(ids);

            foreach(var notificacao in notificacoes)
            {
                await mediator.Send(new NotificarExclusaoNotificacaoCommand(notificacao.Codigo, notificacao.Status, notificacao.UsuarioRf));
            }
        }

        public void GeraNovoCodigo(Notificacao notificacao)
        {
            if (notificacao.Codigo == 0)
                notificacao.Codigo = ObtemNovoCodigo();
        }

        public async Task GeraNovoCodigoAsync(Notificacao notificacao)
        {
            if (notificacao.Codigo == 0)
                notificacao.Codigo = await ObtemNovoCodigoAsync();
        }

        public long ObtemNovoCodigo()
        {
            return repositorioNotificacaoConsulta.ObterUltimoCodigoPorAno(DateTime.Now.Year) + 1;
        }

        public async Task<long> ObtemNovoCodigoAsync()
        {
            return await mediator.Send(new ObterNotificacaoUltimoCodigoPorAnoQuery(DateTime.Now.Year)) + 1;
        }

        public async Task Salvar(Notificacao notificacao)
        {
            await GeraNovoCodigoAsync(notificacao);

            await repositorioNotificacao.SalvarAsync(notificacao);
            await mediator.Send(new NotificarCriacaoNotificacaoCommand(notificacao));
        }

        public IEnumerable<(Cargo? Cargo, string Id)> ObterFuncionariosPorNivel(string codigoUe, Cargo? cargo, bool primeiroNivel = true, bool? notificacaoExigeAcao = false)
        {
            IEnumerable<SupervisorEscolasDreDto> supervisoresEscola = null;
            IEnumerable<UsuarioEolRetornoDto> funcionarios = null;

            if (cargo == Cargo.Supervisor)
                supervisoresEscola = repositorioSupervisorEscolaDre.ObtemSupervisoresPorUe(codigoUe).Result;
            else
                funcionarios = mediator.Send(
                    new ObterFuncionariosPorCargoUeQuery(codigoUe, (int) cargo)).Result;

            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);

            if (cargo == Cargo.Supervisor ? 
                supervisoresEscola.EhNulo() || !supervisoresEscola.Any() :
                funcionarios.EhNulo() || !funcionarios.Any() || (!funcionariosDisponiveis.Any() && notificacaoExigeAcao.Value))
            {
                Cargo? cargoProximoNivel = ObterProximoNivel(cargo, primeiroNivel);

                if (!cargoProximoNivel.HasValue)
                    return Enumerable.Empty<(Cargo?, string)>();

                return  ObterFuncionariosPorNivel(codigoUe, cargoProximoNivel, false);
            }
            else
            {
                if (cargo == Cargo.Supervisor)
                    return supervisoresEscola.Select(s => (Cargo: cargo, Id: s.SupervisorId));
                else
                    return funcionarios.Select(f => (Cargo: cargo, Id: f.CodigoRf));
            }

        }

        public async Task<IEnumerable<(Cargo? Cargo, string Id)>> ObterFuncionariosPorNivelAsync(string codigoUe, Cargo? cargo, bool primeiroNivel = true, bool? notificacaoExigeAcao = false)
        {
            IEnumerable<SupervisorEscolasDreDto> supervisoresEscola = null;
            IEnumerable<UsuarioEolRetornoDto> funcionarios = null;

            if (cargo == Cargo.Supervisor)
                supervisoresEscola = await repositorioSupervisorEscolaDre.ObtemSupervisoresPorUeAsync(codigoUe);
            else
                funcionarios = await mediator.Send(new ObterFuncionariosPorCargoUeQuery(codigoUe, (int)cargo));

            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);

            if (cargo == Cargo.Supervisor ?
                supervisoresEscola.EhNulo() || !supervisoresEscola.Any() :
                funcionarios.EhNulo() || !funcionarios.Any() || (!funcionariosDisponiveis.Any() && notificacaoExigeAcao.Value))
            {
                Cargo? cargoProximoNivel = ObterProximoNivel(cargo, primeiroNivel);

                if (!cargoProximoNivel.HasValue)
                    return Enumerable.Empty<(Cargo?, string)>();

                return await ObterFuncionariosPorNivelAsync(codigoUe, cargoProximoNivel, false);
            }
            else
            {
                if (cargo == Cargo.Supervisor)
                    return supervisoresEscola.Select(s => (Cargo: cargo, Id: s.SupervisorId));
                else
                    return funcionarios.Select(f => (Cargo: cargo, Id: f.CodigoRf));
            }

        }


        public async Task<IEnumerable<(FuncaoAtividade? FuncaoAtividade, string Id)>> ObterFuncionariosPorNivelFuncaoAtividadeAsync(string codigoUe, FuncaoAtividade? funcaoAtividade, bool primeiroNivel = true, bool? notificacaoExigeAcao = false)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosPorFuncaoAtividadeUeQuery(codigoUe, (int)funcaoAtividade));
            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);
            return funcionarios.Select(f => (CodigoFuncaoAtividade: funcaoAtividade, Id: f.CodigoRf));
        }

        public Cargo? ObterProximoNivel(Cargo? cargo, bool primeiroNivel)
        {
            if (!cargo.HasValue)
                return null;

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

        public async Task<Notificacao> ObterPorCodigo(long codigo)
        {
            return await mediator.Send(new ObterNotificacaoPorCodigoQuery(codigo));
        }        

        public async Task ExcluirPeloSistemaAsync(long[] ids)
        {
            await repositorioNotificacao.ExcluirPeloSistemaAsync(ids);
        }
    }
}