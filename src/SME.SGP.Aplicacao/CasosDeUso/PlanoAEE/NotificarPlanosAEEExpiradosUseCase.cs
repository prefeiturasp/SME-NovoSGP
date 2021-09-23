using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarPlanosAEEExpiradosUseCase : AbstractUseCase, INotificarPlanosAEEExpiradosUseCase
    {
        private readonly IConfiguration configuration;

        public NotificarPlanosAEEExpiradosUseCase(IMediator mediator, IConfiguration configuration) : base(mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (!await ParametroNotificarPlanosAEE())
                return false;

            var dataFim = await ObterDataFim();
            var planosExpirados = await mediator.Send(new ObterPlanosAEEPorDataFimQuery(dataFim, false, true, NotificacaoPlanoAEETipo.PlanoExpirado));

            foreach (var planoExpirado in planosExpirados)
                await NotificarPlanoExpirado(planoExpirado, dataFim);

            return true;
        }

        private async Task NotificarPlanoExpirado(PlanoAEE plano, DateTime dataFim)
        {
            var turma = await ObterTurma(plano.TurmaId);

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE Expirado - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} expirou em {dataFim:dd/MM/yyyy} e até o momento não teve sua vigência prorrogada ou foi encerrado.<br/>
                <a href='{hostAplicacao}aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano. ";

            var usuariosId = await ObterCEFAI(turma.Ue.Dre.CodigoDre);

            foreach(var usuarioId in usuariosId)
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(plano.Id, usuarioId, titulo, descricao, NotificacaoPlanoAEETipo.PlanoExpirado));
        }

        private async Task<IEnumerable<long>> ObterCEFAI(string codigoDre)
            => await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(codigoDre));

        private async Task<Turma> ObterTurma(long turmaId)
            => await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

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
