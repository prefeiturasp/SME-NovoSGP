using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoParecerConclusivoCommandHandler : AsyncRequestHandler<NotificarAprovacaoParecerConclusivoCommand>
    {
        private readonly IMediator mediator;

        public NotificarAprovacaoParecerConclusivoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(NotificarAprovacaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var aprovacao = request.Aprovado ? "aprovada" : "recusada";
            var turma = await ObterTurma(request.TurmaCodigo);
            var nomeTurma = $"{turma.Nome} da {turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) de {turma.AnoLetivo}";
            var aluno = await ObterAluno(request.ParecerEmAprovacao.ConselhoClasseAluno.AlunoCodigo, turma.AnoLetivo);
            var nomeAluno = $"{aluno.Nome} ({aluno.CodigoAluno})";
            var data = $"{request.ParecerEmAprovacao.CriadoEm:dd/MM/yyyy} às {request.ParecerEmAprovacao.CriadoEm:HH:mm}";

            var parecerAnterior = request.ParecerEmAprovacao.ConselhoClasseAluno.ConselhoClasseParecer?.Nome ?? "Nenhum";
            var parecerNovo = request.ParecerEmAprovacao.ConselhoClasseParecer?.Nome;

            var titulo = $"Alteração de parecer conclusivo - Turma {turma.Nome} ({turma.AnoLetivo})";
            var descricao = $"A alteração do parecer conclusivo do estudante {nomeAluno} da turma {nomeTurma} realizada pelo Professor {request.CriadorNome} ({request.CriadorRf}) em {data} de '{parecerAnterior}' para '{parecerNovo}' foi {aprovacao}.<br/>";
            if (!request.Aprovado)
                descricao += $"Motivo: {request.Motivo}.";

            await mediator.Send(new NotificarUsuarioCommand(titulo,
                                                            descricao,
                                                            request.CriadorRf,
                                                            NotificacaoCategoria.Aviso,
                                                            NotificacaoTipo.Fechamento,
                                                            turma.Ue.Dre.CodigoDre,
                                                            turma.Ue.CodigoUe,
                                                            turma.CodigoTurma));
        }

        private async Task<AlunoReduzidoDto> ObterAluno(string alunoCodigo, int anoLetivo)
            => await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(alunoCodigo, anoLetivo));

        private async Task<Turma> ObterTurma(string turmaCodigo)
            => await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
    }
}
