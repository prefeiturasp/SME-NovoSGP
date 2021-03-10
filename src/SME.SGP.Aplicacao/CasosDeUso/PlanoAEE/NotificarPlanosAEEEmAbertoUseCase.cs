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

            await EnviarNotificacao(planosAtivos.GroupBy(a => a.DRECodigo));

            return true;
        }

        private async Task EnviarNotificacao(IEnumerable<IGrouping<string, PlanoAEEReduzidoDto>> dresPlanos)
        {
            foreach (var dre in dresPlanos)
            {
                var dreCodigo = dre.Key;
                var dreAbreviacao = dre.FirstOrDefault().DREAbreviacao;

                await NotificarPlanoEmAberto(dre.GroupBy(a => $"{a.UETipo.ShortName()} {a.UENome}"), dreCodigo, dreAbreviacao);
            }
        }

        private async Task<List<DateTime>> ObterDatasParametroPlanoAEEEmAberto()
        {
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasParaNotificacarPlanoAEEAberto, DateTime.Today.Year));

            if (parametros != null)
            {
                return parametros.Where(a => a.Ativo).Select(a => Convert.ToDateTime($"{a.Valor}/{DateTime.Today.Year}")).ToList();
            }

            return null;
        }

        private async Task<bool> NotificarPlanoEmAberto(IEnumerable<IGrouping<string, PlanoAEEReduzidoDto>> planos, string dreCodigo, string dreAbreviacao)
        {
            var titulo = $"Acompanhamento dos planos AEE ({dreAbreviacao})";
            string descricao = $@"Segue a lista de Planos AEE das unidades da {dreAbreviacao} sob sua responsabilidade: <br/><br/>
                <table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>";

            foreach (var ue in planos)
            {
                descricao += $"<tr style='font-weight: bold; text-align:center;'><td colspan=4;>{ue.Key}</td></tr>";
                descricao += $@"<tr style='font-weight: bold'>
                    <td style='padding-left:5px;padding-right:5px;'>Estudante</td>
                    <td style='padding-left:5px;padding-right:5px;'>Turma Regular</td>
                    <td style='padding-left:5px;padding-right:5px;'>Vigência do Plano</td>
                    <td style='padding-left:5px;padding-right:5px;'>Situação</td></tr>";
                foreach (var plano in ue.OrderBy(a => a.EstudanteNome).ToList())
                {
                    descricao += $@"<tr><td style='padding-left:5px;padding-right:5px;'>{plano.EstudanteNome} ({plano.EstudanteCodigo})</td>
                        <td style='padding-left:5px;padding-right:5px;'>{plano.TurmaModalidade.ShortName()} - {plano.TurmaNome}</td>
                        <td style='padding-left:5px;padding-right:5px;'>{plano.VigenciaInicio.Date:d} - {plano.VigenciaFim.Date:d}</td>
                        <td style='padding-left:5px;padding-right:5px;'>{plano.Situacao.Name()}</td></tr>";
                }
            }

            descricao += "</table>";

            var supervisores = await ObterSupervisores(dreCodigo);

            if (supervisores == null)
                return false;

            foreach (var supervisor in supervisores)
            {
                await mediator.Send(new NotificarUsuarioCommand(titulo, descricao, supervisor, NotificacaoCategoria.Aviso, NotificacaoTipo.AEE));
            }
            return true;

        }

        private async Task<List<string>> ObterSupervisores(string codigoUe)
        {
            var supervisores = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Supervisor));
            if (supervisores.Any())
                return supervisores.Select(f => f.CodigoRF).ToList();

            var supervisoresTecnicos = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.SupervisorTecnico));
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
