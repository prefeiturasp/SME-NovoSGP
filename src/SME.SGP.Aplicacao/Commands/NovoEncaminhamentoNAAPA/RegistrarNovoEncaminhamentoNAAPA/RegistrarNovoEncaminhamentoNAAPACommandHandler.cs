using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;


namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPA
{
    public class RegistrarNovoEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarNovoEncaminhamentoNAAPACommand, ResultadoNovoEncaminhamentoNAAPADto>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA;
        private readonly IMediator mediator;

        public RegistrarNovoEncaminhamentoNAAPACommandHandler(IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA, IMediator mediator)
        {
            this.repositorioNovoEncaminhamentoNAAPA = repositorioNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPA));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<ResultadoNovoEncaminhamentoNAAPADto> Handle(RegistrarNovoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var turmaCodigo = await mediator.Send(new ObterTurmaCodigoPorIdQuery(request.TurmaId));
            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turmaCodigo, request.AlunoCodigo, true));

            var encaminhamento = MapearParaEntidade(request, aluno?.CodigoSituacaoMatricula);
            var id = await repositorioNovoEncaminhamentoNAAPA.SalvarAsync(encaminhamento);
            var resultado = new ResultadoNovoEncaminhamentoNAAPADto(id);
            resultado.Auditoria = (AuditoriaDto)encaminhamento;
            return resultado;
        }

        private SME.SGP.Dominio.EncaminhamentoNAAPA MapearParaEntidade(RegistrarNovoEncaminhamentoNAAPACommand request, SituacaoMatriculaAluno? situacaoAluno)
            => new()
            {
                TurmaId = request.TurmaId,
                Situacao = request.Situacao,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNome = request.AlunoNome,
                SituacaoMatriculaAluno = situacaoAluno
            };
    }
}