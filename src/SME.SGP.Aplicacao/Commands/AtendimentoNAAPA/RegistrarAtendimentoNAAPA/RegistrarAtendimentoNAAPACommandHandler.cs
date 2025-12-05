using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarAtendimentoNAAPACommandHandler : IRequestHandler<RegistrarAtendimentoNAAPACommand, ResultadoAtendimentoNAAPADto>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA;
        private readonly IMediator mediator;

        public RegistrarAtendimentoNAAPACommandHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA, IMediator mediator)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoAtendimentoNAAPADto> Handle(RegistrarAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var turmaCodigo = await mediator.Send(new ObterTurmaCodigoPorIdQuery(request.TurmaId));
            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turmaCodigo, request.AlunoCodigo, true));

            var encaminhamento = MapearParaEntidade(request, aluno?.CodigoSituacaoMatricula);
            var id = await repositorioEncaminhamentoNAAPA.SalvarAsync(encaminhamento);
            var resultado = new ResultadoAtendimentoNAAPADto(id);
            resultado.Auditoria = (AuditoriaDto)encaminhamento;
            return resultado;
        }

        private EncaminhamentoNAAPA MapearParaEntidade(RegistrarAtendimentoNAAPACommand request, SituacaoMatriculaAluno? situacaoAluno)
            => new ()
            {
                TurmaId = request.TurmaId,
                Situacao = request.Situacao,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNome = request.AlunoNome,
                SituacaoMatriculaAluno = situacaoAluno
            };
    }
}
