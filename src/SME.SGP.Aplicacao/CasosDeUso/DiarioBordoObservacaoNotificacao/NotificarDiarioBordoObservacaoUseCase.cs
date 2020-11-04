﻿using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarDiarioBordoObservacaoUseCase : INotificarDiarioBordoObservacaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IUnitOfWork unitOfWork;

        public NotificarDiarioBordoObservacaoUseCase(IMediator mediator,
                                                      IConfiguration configuration,
                                                      IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao,
                                                      IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacaoNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<NotificarDiarioBordoObservacaoDto>();
            var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");
            var usuarioLogado = dadosMensagem.Usuario;
            long diarioBordoId = (long)dadosMensagem.DiarioBordoId;
            var diarioBordo = await mediator.Send(new ObterDiarioBordoComAulaETurmaPorCodigoQuery(diarioBordoId));

            var titulares = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(diarioBordo.Aula.Turma.CodigoTurma));

            if (titulares != null)
            {
                var mensagem = new StringBuilder($"O usuário {usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) inseriu uma nova observação no Diário de bordo do dia {dataAtual} da turma <strong>{diarioBordo.Aula.Turma.Nome}</strong> da <strong>{diarioBordo.Aula.Turma.Ue.TipoEscola}-{diarioBordo.Aula.Turma.Ue.Nome}</strong> ({diarioBordo.Aula.Turma.Ue.Dre.Abreviacao}).");
                
                if (dadosMensagem.Observacao.Length > 200)
                {
                    mensagem.AppendLine($"<br/><br/>Observação: Acesse o Diário de bordo de uma das aulas para consultar a observação.");
                }
                else
                {
                    mensagem.AppendLine($"<br/><br/>Observação: {dadosMensagem.Observacao.TrimEnd('.').Trim()}.");
                }


                if (titulares.Count() == 1)
                    titulares = titulares.FirstOrDefault().Split(',');

                foreach (var titular in titulares)
                {
                    var codigoRf = titular.Trim();

                    if (codigoRf != usuarioLogado.CodigoRf)
                    {
                        var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRf));
                        //if (usuario != null)
                        {
                            unitOfWork.IniciarTransacao();
                            var notificacaoId = await mediator.Send(new NotificarUsuarioCommand($"Nova observação no Diário de bordo da turma {diarioBordo.Aula.Turma.Nome} ({dataAtual})",
                                                                             mensagem.ToString(),
                                                                             codigoRf,
                                                                             NotificacaoCategoria.Aviso,
                                                                             NotificacaoTipo.Planejamento));


                            var diarioBordoObservacaoNotificacao = new DiarioBordoObservacaoNotificacao(dadosMensagem.ObservacaoId, notificacaoId);

                            await repositorioDiarioBordoObservacaoNotificacao.Salvar(diarioBordoObservacaoNotificacao);
                            unitOfWork.PersistirTransacao();
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
