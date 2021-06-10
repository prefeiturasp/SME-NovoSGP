using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoNotificacao : IServicoNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEol servicoEOL;

        public ServicoNotificacao(IRepositorioNotificacao repositorioNotificacao,
                                  IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                  IServicoEol servicoEOL)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task ExcluirFisicamenteAsync(long[] ids)
        {
            await repositorioNotificacao.ExcluirPorIdsAsync(ids);
        }

        public void GeraNovoCodigo(Notificacao notificacao)
        {
            if (notificacao.Codigo == 0)
                notificacao.Codigo = ObtemNovoCodigo();
        }

        public long ObtemNovoCodigo()
        {
            return repositorioNotificacao.ObterUltimoCodigoPorAno(DateTime.Now.Year) + 1;
        }

        public void Salvar(Notificacao notificacao)
        {
            GeraNovoCodigo(notificacao);

            repositorioNotificacao.Salvar(notificacao);
        }

        public async Task SalvarAsync(Notificacao notificacao)
        {
            GeraNovoCodigo(notificacao);

            await repositorioNotificacao.SalvarAsync(notificacao);
        }

        public IEnumerable<(Cargo? Cargo, string Id)> ObterFuncionariosPorNivel(string codigoUe, Cargo? cargo, bool primeiroNivel = true, bool? notificacaoExigeAcao = false)
        {
            IEnumerable<SupervisorEscolasDreDto> supervisoresEscola = null;
            IEnumerable<UsuarioEolRetornoDto> funcionarios = null;

            if (cargo == Cargo.Supervisor)
                supervisoresEscola = repositorioSupervisorEscolaDre.ObtemSupervisoresPorUe(codigoUe);
            else
                funcionarios = servicoEOL.ObterFuncionariosPorCargoUe(codigoUe, (int)cargo);

            var funcionariosDisponiveis = funcionarios.Where(f => !f.EstaAfastado);

            if (cargo == Cargo.Supervisor ? 
                supervisoresEscola == null || !supervisoresEscola.Any() :
                funcionarios == null || !funcionarios.Any() || (!funcionariosDisponiveis.Any() && notificacaoExigeAcao.Value))
            {
                Cargo? cargoProximoNivel = ObterProximoNivel(cargo, primeiroNivel);

                if (!cargoProximoNivel.HasValue)
                    return Enumerable.Empty<(Cargo?, string)>();

                return ObterFuncionariosPorNivel(codigoUe, cargoProximoNivel, false);
            }
            else
            {
                if (cargo == Cargo.Supervisor)
                    return supervisoresEscola.Select(s => (Cargo: cargo, Id: s.SupervisorId));
                else
                    return funcionarios.Select(f => (Cargo: cargo, Id: f.CodigoRf));
            }

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

        public Notificacao ObterPorCodigo(long codigo)
        {
            return repositorioNotificacao.ObterPorCodigo(codigo);
        }        

        public async Task ExcluirPeloSistemaAsync(long[] ids)
        {
            await repositorioNotificacao.ExcluirPeloSistemaAsync(ids);
        }

    }
}