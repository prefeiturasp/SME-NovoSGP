using MediatR;
using SME.SGP.Dominio;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoParecerConclusivoCommandHandler : NotificacaoParecerConclusivoConselhoClasseCommandBase<NotificarAprovacaoParecerConclusivoCommand, bool>
    {
        private NotificarAprovacaoParecerConclusivoCommand request;

        public NotificarAprovacaoParecerConclusivoCommandHandler(IMediator mediator) : base(mediator)
        {}
        
        public override async Task<bool> Handle(NotificarAprovacaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            this.request = request;
            var wfAprovacoes = request.PareceresEmAprovacao;
            await CarregarInformacoesParaNotificacao(wfAprovacoes);

            var turma = await ObterTurma(wfAprovacoes.FirstOrDefault().TurmaId);
            var mensagem = ObterMensagem(turma, wfAprovacoes.ToList());

            foreach (var usuario in Usuarios)
            {
                await mediator.Send(new NotificarUsuarioCommand(
                    ObterTitulo(turma),
                    mensagem,
                    usuario.CodigoRf,
                    NotificacaoCategoria.Aviso,
                    NotificacaoTipo.Fechamento,
                    turma.Ue.Dre.CodigoDre,
                    turma.Ue.CodigoUe,
                    turma.CodigoTurma), cancellationToken);
            }

            return true;
        }

        protected override string ObterTextoCabecalho(Turma turma)
        {
            var descricaoAprovadoRecusado = request.Aprovado ? "aprovada" : "recusada";
            return $@"A alteração de pareceres conclusivos dos estudantes abaixo da turma {turma.Nome} da {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) de {turma.AnoLetivo} foi {descricaoAprovadoRecusado}. Motivo: {request.Motivo}.";
        }
    }
}
