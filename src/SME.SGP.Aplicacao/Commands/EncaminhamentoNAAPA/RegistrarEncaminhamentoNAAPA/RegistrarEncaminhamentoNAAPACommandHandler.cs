using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarEncaminhamentoNAAPACommand, ResultadoAtendimentoNAAPADto>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;
        private readonly IMediator mediator;

        public RegistrarEncaminhamentoNAAPACommandHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA, IMediator mediator)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoAtendimentoNAAPADto> Handle(RegistrarEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var turmaCodigo = await mediator.Send(new ObterTurmaCodigoPorIdQuery(request.TurmaId));
            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turmaCodigo, request.AlunoCodigo, true));

            var encaminhamento = MapearParaEntidade(request, aluno?.CodigoSituacaoMatricula);
            var id = await repositorioEncaminhamentoNAAPA.SalvarAsync(encaminhamento);
            var resultado = new ResultadoAtendimentoNAAPADto(id);
            resultado.Auditoria = (AuditoriaDto)encaminhamento;
            return resultado;
        }

        private EncaminhamentoNAAPA MapearParaEntidade(RegistrarEncaminhamentoNAAPACommand request, SituacaoMatriculaAluno? situacaoAluno)
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
