using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarWFAprovacaoParecerConclusivoCommandHandler : AsyncRequestHandler<GerarWFAprovacaoParecerConclusivoCommand>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorio;
        private readonly IMediator mediator;

        public GerarWFAprovacaoParecerConclusivoCommandHandler(IRepositorioWFAprovacaoParecerConclusivo repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(GerarWFAprovacaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacaoId = await GerarWFAprovacao(request);

            await repositorio.Salvar(new WFAprovacaoParecerConclusivo()
            {
                ConselhoClasseAlunoId = request.ConselhoClasseAlunoId,
                WfAprovacaoId = wfAprovacaoId,
                ConselhoClasseParecerId = request.ParecerConclusivoId
            });
        }

        private async Task<long> GerarWFAprovacao(GerarWFAprovacaoParecerConclusivoCommand request)
        {
            var ue = await ObterUe(request.Turma.UeId);
            var turma = $"{request.Turma.Nome} da {ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao}) de {request.Turma.AnoLetivo}";
            var usuarioLogado = await ObterUsuarioLogado();
            var professor = $"{usuarioLogado.Nome} ({usuarioLogado.CodigoRf})";
            var data = $"{DateTime.Today:dd/MM/yyyy} às {DateTime.Now:hh:mm}";
            var aluno = await ObterAluno(request.AlunoCodigo, request.Turma.AnoLetivo);

            var titulo = $"Alteração de parecer conclusivo - Turma {request.Turma.Nome} ({request.Turma.AnoLetivo})";
            var descricao = $"O parecer conclusivo do estudante {aluno.Nome} ({aluno.CodigoAluno}) da turma {turma} foi alterada pelo Professor {professor}) em {data} de '{request.ParecerAnterior}' para '{request.ParecerNovo}'.<br/>";
            descricao += "Você precisa aceitar esta notificação para que a alteração seja considerada válida.";

            return await mediator.Send(new EnviarNotificacaoCommand(titulo,
                                                                    descricao.ToString(),
                                                                    Dominio.NotificacaoCategoria.Workflow_Aprovacao,
                                                                    Dominio.NotificacaoTipo.Fechamento,
                                                                    new Cargo[] { Cargo.CP, Cargo.Supervisor },
                                                                    ue.Dre.CodigoDre,
                                                                    ue.CodigoUe,
                                                                    request.Turma.CodigoTurma,
                                                                    WorkflowAprovacaoTipo.AlteracaoParecerConclusivo,
                                                                    request.ConselhoClasseAlunoId));
        }

        private async Task<Usuario> ObterUsuarioLogado()
            => await mediator.Send(new ObterUsuarioLogadoQuery());

        private async Task<Ue> ObterUe(long ueId)
            => await mediator.Send(new ObterUeComDrePorIdQuery(ueId));

        private async Task<AlunoReduzidoDto> ObterAluno(string alunoCodigo, int anoLetivo)
            => await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(alunoCodigo, anoLetivo));
    }
}
