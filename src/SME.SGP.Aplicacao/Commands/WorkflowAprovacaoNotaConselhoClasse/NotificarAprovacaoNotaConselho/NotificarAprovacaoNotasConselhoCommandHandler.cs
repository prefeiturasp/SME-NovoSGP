using MediatR;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoNotasConselhoCommandHandler : AprovacaoNotaConselhoCommandBase<NotificarAprovacaoNotasConselhoCommand>
    {

        private NotificarAprovacaoNotasConselhoCommand notificarAprovacaoNotasConselhoCommand;
        public NotificarAprovacaoNotasConselhoCommandHandler(IMediator mediator)
            :base(mediator)
        {
        }

        protected override async Task Handle(NotificarAprovacaoNotasConselhoCommand request, CancellationToken cancellationToken)
        {
            notificarAprovacaoNotasConselhoCommand = request;

            await IniciarAprovacao(notificarAprovacaoNotasConselhoCommand.NotasEmAprovacao);

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
        }

        protected override string ObterTexto(Ue ue, Turma turma, PeriodoEscolar periodoEscolar)
        {
            if (notificarAprovacaoNotasConselhoCommand.Aprovada)
            {
                return String.Empty;
            }

            return $@"A alteração de notas/conceitos pós-conselho do bimestre { periodoEscolar.Bimestre } 
                      de { turma.AnoLetivo } da turma { turma.NomeFiltro } da { ue.Nome } ({ ue.Dre.Abreviacao }) 
                      abaixo foi recusada. Motivo: { notificarAprovacaoNotasConselhoCommand.Justificativa }.";
        }

        protected override string ObterTitulo(Ue ue, Turma turma)
        {
            if (notificarAprovacaoNotasConselhoCommand.Aprovada)
            {
                return String.Empty;
            }

            return $@"Alteração em nota/conceito final pós-conselho - { ue.Nome } ({ ue.Dre.Abreviacao }) - { turma.NomeFiltro } (ano anterior)";
        }
    }
}
