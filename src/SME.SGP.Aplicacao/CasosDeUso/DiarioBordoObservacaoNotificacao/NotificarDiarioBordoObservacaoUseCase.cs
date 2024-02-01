using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dados;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class NotificarDiarioBordoObservacaoUseCase : INotificarDiarioBordoObservacaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IUnitOfWork unitOfWork;

        public NotificarDiarioBordoObservacaoUseCase(IMediator mediator,
                                                      IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao,
                                                      IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
            if (diarioBordo.EhNulo())
                return false;

            var mensagem = new StringBuilder($"O usuário {usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) inseriu uma nova observação no Diário de bordo do dia {dataAtual} da turma <strong>{diarioBordo.Aula.Turma.Nome}</strong> da <strong>{diarioBordo.Aula.Turma.Ue.TipoEscola}-{diarioBordo.Aula.Turma.Ue.Nome}</strong> ({diarioBordo.Aula.Turma.Ue.Dre.Abreviacao}).");

            mensagem.AppendLine($"<br/><br/>Observação: {dadosMensagem.Observacao}.");

            var titulo = $"Nova observação no Diário de bordo da turma {diarioBordo.Aula.Turma.Nome} ({dataAtual})";

            if (dadosMensagem.UsuariosNotificacao.PossuiRegistros())
            {
                await GerarNotificacoesUsuarioDiarioClasse(dadosMensagem.UsuariosNotificacao.Where(rf => rf != usuarioLogado.CodigoRf),
                                                          titulo, mensagem.ToString(), dadosMensagem.ObservacaoId);
                return true;
            }

            var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(diarioBordo.Aula.Turma.CodigoTurma));
            var titulares = professoresTitulares?.Select(x => x.ProfessorRf.Trim()).Where(x => !string.IsNullOrEmpty(x));
            if (titulares.PossuiRegistros())
            {
                if (titulares.Count() == 1)
                    titulares = titulares.FirstOrDefault().Split(',').Select(rf => rf.Trim()).Where(rf => !string.IsNullOrEmpty(rf));

                await GerarNotificacoesUsuarioDiarioClasse(titulares.Where(rf => rf != usuarioLogado.CodigoRf),
                                                           titulo, mensagem.ToString(), dadosMensagem.ObservacaoId);
                return true;
            }
            return false;
        }

        private async Task GerarNotificacoesUsuarioDiarioClasse(IEnumerable<string> rfUsuariosNotificacao, string titulo, string mensagem, long observacaoId)
        {
            foreach (var usuarioRf in rfUsuariosNotificacao)
            {
                unitOfWork.IniciarTransacao();
                try
                {
                    var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(titulo,
                                                                        mensagem,
                                                                        usuarioRf,
                                                                        NotificacaoCategoria.Aviso,
                                                                        NotificacaoTipo.Planejamento));


                    var diarioBordoObservacaoNotificacao = new DiarioBordoObservacaoNotificacao(observacaoId, notificacaoId);

                    await repositorioDiarioBordoObservacaoNotificacao.Salvar(diarioBordoObservacaoNotificacao);
                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                }
            }
        }
    }
}
