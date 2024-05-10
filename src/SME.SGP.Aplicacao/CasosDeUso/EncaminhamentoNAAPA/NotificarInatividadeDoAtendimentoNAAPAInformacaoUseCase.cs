﻿using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase : AbstractUseCase, INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase
    {
        private EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto informacaoNotificacao;
        private readonly IConfiguration configuration;

        public NotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase(IMediator mediator, IConfiguration configuration) : base(mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            informacaoNotificacao = param.ObterObjetoMensagem<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>();
            var reponsaveisNoticar = await mediator.Send(new ObterResponsaveisPorDreUeNAAPAQuery(informacaoNotificacao.DreCodigo, informacaoNotificacao.UeCodigo));

            foreach (var responsavel in reponsaveisNoticar)
            {
                await mediator.Send(new NotificarUsuarioCommand(
                                            ObterTitulo(),
                                            ObterMensagem(),
                                            responsavel.Login,
                                            NotificacaoCategoria.Aviso,
                                            NotificacaoTipo.NAAPA));
            }

            if (reponsaveisNoticar.Any())
                await mediator.Send(new AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommand(informacaoNotificacao.EncaminhamentoId));

            return true;
        }

        private string ObterTitulo()
        {
            return $"Encaminhamento NAAPA sem atendimento recente - {informacaoNotificacao.AlunoNome}";
        }

        private string ObterMensagem()
        {
            var mensagem = new StringBuilder();

            mensagem.Append($"O Encaminhamento do estudante {informacaoNotificacao.AlunoNome}({informacaoNotificacao.AlunoCodigo}) ");
            mensagem.Append($"da turma {informacaoNotificacao.TurmaNome} da {informacaoNotificacao.TipoEscola.ShortName()} {informacaoNotificacao.UeNome}({informacaoNotificacao.DreNome}) ");
            mensagem.Append($"está há 30 dias sem registro de atendimento. ");
            mensagem.Append($"Caso não seja mais necessário acompanhamento do estudante então o encaminhamento deve ser encerrado ou deve ser registrado o acompanhamento.");

            var hostAplicacao = configuration["UrlFrontEnd"];
            mensagem.Append($"<br/><a href='{hostAplicacao}naapa/encaminhamento/{informacaoNotificacao.EncaminhamentoId}'>Clique aqui para consultar o encaminhamento.</a>");

            return mensagem.ToString();
        }
    }
}
