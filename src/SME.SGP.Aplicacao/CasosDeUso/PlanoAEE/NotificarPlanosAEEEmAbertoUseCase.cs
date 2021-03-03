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
        private readonly IConfiguration configuration;

        public NotificarPlanosAEEEmAbertoUseCase(IMediator mediator, IConfiguration configuration) : base(mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

            foreach (var planoExpirado in planosExpirados)
                await NotificarPlanoExpirado(planoExpirado, dataFim);

            return true;
        }

        private async Task<List<DateTime>> ObterDatasParametroPlanoAEEEmAberto()
        {
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasParaNotificacarPlanoAEEAberto, DateTime.Today.Year));

            if(parametros != null)
            {
                return parametros.Where(a => a.Ativo).Select(a => Convert.ToDateTime($"{a.Valor}/{DateTime.Today.Year}")).ToList();
            }

            return null;
        }

        private async Task NotificarPlanoExpirado(PlanoAEE plano, DateTime dataFim)
        {
            var turma = await ObterTurma(plano.TurmaId);

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.Infantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE Expirado - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} expirou em {dataFim:dd/MM/yyyy}. <br/>
                <a href='{hostAplicacao}relatorios/aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano. ";

            var usuarioId = await ObterCEFAI(turma.Ue.Dre.CodigoDre);

            if (usuarioId > 0)
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(plano.Id, usuarioId, titulo, descricao, NotificacaoPlanoAEETipo.PlanoExpirado));
        }

        private async Task<long> ObterCEFAI(string codigoDre)
            => await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(codigoDre));

        private async Task<Turma> ObterTurma(long turmaId)
        {
            throw new NotImplementedException();
        }

        private async Task<DateTime> ObterDataFim()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasGeracaoNotificacoesPlanoAEEExpirado, DateTime.Today.Year));

            if (parametro == null)
                throw new NegocioException("Parâmetro de Dias para notificar plano AEE expirados não localizado");

            return DateTime.Today.AddDays(int.Parse(parametro.Valor) * -1);
        }

        private async Task<bool> ParametroNotificarPlanosAEE()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarNotificacaoPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
