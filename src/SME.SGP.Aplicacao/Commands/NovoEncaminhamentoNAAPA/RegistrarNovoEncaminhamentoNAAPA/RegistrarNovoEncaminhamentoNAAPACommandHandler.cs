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
using SME.SGP.Dominio.Entidades;

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
            SituacaoMatriculaAluno? situacaoAluno = null;

            if (request.Tipo == (int)TipoQuestionario.EncaminhamentoNAAPAIndividual && request.TurmaId.HasValue)
            {
                var turmaCodigo = await mediator.Send(new ObterTurmaCodigoPorIdQuery(request.TurmaId.Value));
                var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turmaCodigo, request.AlunoCodigo, true));
                situacaoAluno = aluno?.CodigoSituacaoMatricula;
            }

            var encaminhamento = MapearParaEntidade(request, situacaoAluno);
            var id = await repositorioNovoEncaminhamentoNAAPA.SalvarAsync(encaminhamento);

            var resultado = new ResultadoNovoEncaminhamentoNAAPADto(id);
            resultado.Auditoria = (AuditoriaDto)encaminhamento;

            return resultado;
        }

        private EncaminhamentoEscolar MapearParaEntidade(RegistrarNovoEncaminhamentoNAAPACommand request, SituacaoMatriculaAluno? situacaoAluno)
            => new()
            {
                Tipo = request.Tipo,
                DreId = request.DreId,
                UeId = request.UeId,
                Situacao = request.Situacao,
                TurmaId = request.TurmaId,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNome = request.AlunoNome,
                SituacaoMatriculaAluno = situacaoAluno
            };
    }
}