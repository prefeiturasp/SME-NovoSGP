using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarPlanosAEEEmAbertoUseCase : AbstractUseCase, INotificarPlanosAEEEmAbertoUseCase
    {
        public NotificarPlanosAEEEmAbertoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (!await ParametroNotificarPlanosAEE())
                return false;

            var datasParametro = await ObterDatasParametroPlanoAEEEmAberto();
            if (datasParametro == null)
                return false;

            var enviaNotificacao = datasParametro.FirstOrDefault(d => d == DateTime.Now.Date);

            if (enviaNotificacao == null)
                return false;

            var planosAtivos = await mediator.Send(new ObterPlanosAEEAtivosComTurmaEVigenciaQuery());

            await EnviarNotificacao(planosAtivos.GroupBy(a => a.UECodigo));

            return true;
        }

        private async Task EnviarNotificacao(IEnumerable<IGrouping<string, PlanoAEEReduzidoDto>> uesPlanos)
        {
            var supervisoresPlanos = new List<PlanosAEEPorSupervisorDto>();

            foreach (var planosUe in uesPlanos)
            {
                var ueCodigo = planosUe.Key;
                var supervisores = await ObterSupervisores(ueCodigo);

                CarregarPlanosPorSupervisor(supervisoresPlanos, supervisores, planosUe);
            }

            foreach(var supervisor in supervisoresPlanos)
                await NotificarPlanoEmAberto(supervisor);

        }

        private void CarregarPlanosPorSupervisor(List<PlanosAEEPorSupervisorDto> supervisoresPlanos, List<string> supervisores, IGrouping<string, PlanoAEEReduzidoDto> planosUe)
        {
            foreach(var supervisor in supervisores)
            {
                var supervisorPlanos = ObterSupervisorComPlanos(supervisoresPlanos, supervisor);

                if (supervisorPlanos == null)
                    AdicionaSupervisorEPlanos(supervisoresPlanos, supervisor, planosUe);
                else
                    AdicionaPlanosAoSupervisor(supervisorPlanos, planosUe);
            }
        }

        private void AdicionaPlanosAoSupervisor(PlanosAEEPorSupervisorDto supervisorPlanos, IGrouping<string, PlanoAEEReduzidoDto> planosUe)
            => supervisorPlanos.Planos.Add(new PlanosAEEPorUEDto(planosUe.Key, planosUe.ToList()));

        private void AdicionaSupervisorEPlanos(List<PlanosAEEPorSupervisorDto> supervisoresPlanos, string supervisor, IGrouping<string, PlanoAEEReduzidoDto> planosUe)
            => supervisoresPlanos.Add(new PlanosAEEPorSupervisorDto(supervisor, planosUe.Key, planosUe.ToList()));

        private PlanosAEEPorSupervisorDto ObterSupervisorComPlanos(List<PlanosAEEPorSupervisorDto> supervisoresPlanos, string supervisor)
            => supervisoresPlanos.FirstOrDefault(c => c.Supervisor == supervisor);

        private async Task<List<DateTime>> ObterDatasParametroPlanoAEEEmAberto()
        {
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasParaNotificacarPlanoAEEAberto, DateTime.Today.Year));

            if (parametros != null)
            {
                return parametros.Where(a => a.Ativo).Select(a => Convert.ToDateTime($"{a.Valor}/{DateTime.Today.Year}")).ToList();
            }

            return null;
        }

        private async Task<bool> NotificarPlanoEmAberto(PlanosAEEPorSupervisorDto supervisor)
        {
            var dreAbreviacao = supervisor.Planos.First().Planos.First().DREAbreviacao;

            var titulo = $"Acompanhamento dos planos AEE ({dreAbreviacao})";
            string descricao = $@"Segue a lista de Planos AEE das unidades da {dreAbreviacao} sob sua responsabilidade: <br/><br/>
                <table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>";

            foreach (var planosUe in supervisor.Planos)
            {
                var ue = $"{planosUe.Planos.First().UETipo.ShortName()} {planosUe.Planos.First().UENome}";

                descricao += $"<tr style='font-weight: bold; text-align:center;'><td colspan=4;>{ue}</td></tr>";
                descricao += $@"<tr style='font-weight: bold'>
                    <td style='padding-left:5px;padding-right:5px;'>Estudante</td>
                    <td style='padding-left:5px;padding-right:5px;'>Turma Regular</td>
                    <td style='padding-left:5px;padding-right:5px;'>Vigência do Plano</td>
                    <td style='padding-left:5px;padding-right:5px;'>Situação</td></tr>";

                foreach (var plano in planosUe.Planos.OrderBy(a => a.EstudanteNome))
                {
                    descricao += $@"<tr><td style='padding-left:5px;padding-right:5px;'>{plano.EstudanteNome} ({plano.EstudanteCodigo})</td>
                        <td style='padding-left:5px;padding-right:5px;'>{plano.TurmaModalidade.ShortName()} - {plano.TurmaNome}</td>
                        <td style='padding-left:5px;padding-right:5px;'>{plano.VigenciaInicio.Date:d} - {plano.VigenciaFim.Date:d}</td>
                        <td style='padding-left:5px;padding-right:5px;'>{plano.Situacao.Name()}</td></tr>";
                }
            }

            descricao += "</table>";

            await mediator.Send(new NotificarUsuarioCommand(titulo, descricao, supervisor.Supervisor, NotificacaoCategoria.Aviso, NotificacaoTipo.AEE));

            return true;
        }

        private async Task<List<string>> ObterSupervisores(string codigoUE)
        {
            var supervisores = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUE, (int)Cargo.Supervisor));
            if (supervisores.Any())
                return supervisores.Select(f => f.CodigoRF).ToList();

            var supervisoresTecnicos = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUE, (int)Cargo.SupervisorTecnico));
            if (supervisoresTecnicos.Any())
                return supervisoresTecnicos.Select(f => f.CodigoRF).ToList();

            return null;
        }

        private async Task<bool> ParametroNotificarPlanosAEE()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarNotificacaoPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
