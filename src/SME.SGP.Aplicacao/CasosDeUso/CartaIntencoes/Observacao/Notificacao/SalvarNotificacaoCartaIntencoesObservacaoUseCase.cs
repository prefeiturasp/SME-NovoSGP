using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
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
    public class SalvarNotificacaoCartaIntencoesObservacaoUseCase : ISalvarNotificacaoCartaIntencoesObservacaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao;
        private readonly IServicoNotificacao servicoNotificacao;

        public SalvarNotificacaoCartaIntencoesObservacaoUseCase(IMediator mediator, IServicoNotificacao servicoNotificacao,
            IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioNotificacaoCartaIntencoesObservacao = repositorioNotificacaoCartaIntencoesObservacao ?? throw new ArgumentNullException(nameof(repositorioNotificacaoCartaIntencoesObservacao));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<SalvarNotificacaoCartaIntencoesObservacaoDto>();

            var turma = dadosMensagem.Turma;
            var usuarioLogado = dadosMensagem.Usuario;
            var cartaIntencoesObservacaoId = dadosMensagem.CartaIntencoesObservacaoId;

            var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));
            var titulares = professoresTitulares?.Select(x => x.ProfessorRf);
            if (titulares.NaoEhNulo())
            {
                var mensagem = new StringBuilder($"O usuário {usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) inseriu uma nova observação na Carta de intenções da turma <strong>{turma.Nome}</strong> da <strong>{turma.Ue.TipoEscola}-{turma.Ue.Nome}</strong> ({turma.Ue.Dre.Abreviacao}).");
                                                
                if (dadosMensagem.Observacao.Trim().Length > 200)
                {
                    mensagem.AppendLine($"<br/><br/>Observação: Acesse Carta de intenções para consultar o conteúdo da observação.");
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
                        if (usuario.NaoEhNulo())
                        {
                            var notificacao = new Notificacao()
                            {
                                Ano = DateTime.Now.Year,
                                Categoria = NotificacaoCategoria.Aviso,
                                Tipo = NotificacaoTipo.Planejamento,
                                Titulo = $"Nova observação na Carta de intenções da turma {turma.Nome}",
                                Mensagem = mensagem.ToString(),
                                UsuarioId = usuario.Id,
                                TurmaId = "",
                                UeId = "",
                                DreId = "",
                            };

                            await servicoNotificacao.Salvar(notificacao);

                            var notificacaoObservacao = new NotificacaoCartaIntencoesObservacao()
                            {
                                NotificacaoId = notificacao.Id,
                                CartaIntencoesObservacaoId = cartaIntencoesObservacaoId
                            };

                            await repositorioNotificacaoCartaIntencoesObservacao.Salvar(notificacaoObservacao);

                        }

                    }
                }
                return true;
            }
            return false;
        }
    }
}
