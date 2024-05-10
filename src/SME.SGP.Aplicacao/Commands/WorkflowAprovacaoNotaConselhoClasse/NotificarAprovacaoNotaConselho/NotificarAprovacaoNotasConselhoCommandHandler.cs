using MediatR;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoNotasConselhoCommandHandler : AprovacaoNotaConselhoCommandBase<NotificarAprovacaoNotasConselhoCommand, bool>
    {

        private NotificarAprovacaoNotasConselhoCommand notificarAprovacaoNotasConselhoCommand;
        public NotificarAprovacaoNotasConselhoCommandHandler(IMediator mediator)
            :base(mediator)
        {
        }

        public override async Task<bool> Handle(NotificarAprovacaoNotasConselhoCommand request, CancellationToken cancellationToken)
        {
            notificarAprovacaoNotasConselhoCommand = request;

            await CarregarInformacoesParaNotificacao(notificarAprovacaoNotasConselhoCommand.NotasEmAprovacao);

            var turma = WFAprovacoes.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
            var ue = Ues.Find(ue => ue.Id == turma.UeId);
            var titulo = ObterTitulo(ue, turma);
            var mensagem = ObterMensagem(ue, turma, WFAprovacoes);

            foreach (var usuario in Usuarios)
            {
                await mediator.Send(new NotificarUsuarioCommand(
                                                                titulo,
                                                                mensagem,
                                                                usuario.CodigoRf,
                                                                NotificacaoCategoria.Aviso,
                                                                NotificacaoTipo.Notas,
                                                                ue.Dre.CodigoDre,
                                                                ue.CodigoUe,
                                                                turma.CodigoTurma,
                                                                DateTime.Today.Year,
                                                                request.CodigoDaNotificacao ?? 0,
                                                                usuarioId: usuario.Id));
            }
            return true;
        }

        protected override string ObterTexto(Ue ue, Turma turma, PeriodoEscolar periodoEscolar)
        {
            var descricaoAprovadoRecusado = notificarAprovacaoNotasConselhoCommand.Aprovada ? "aprovada" : "recusada";

            return $@"A alteração de notas/conceitos pós-conselho do bimestre {(periodoEscolar.NaoEhNulo() ? periodoEscolar.Bimestre : "final")} 
                      de { turma.AnoLetivo } da turma { turma.NomeFiltro } da { ue.Nome } ({ ue.Dre.Abreviacao }) 
                      abaixo foi { descricaoAprovadoRecusado }. Motivo: { notificarAprovacaoNotasConselhoCommand.Justificativa }.";
        }

    }
}
