using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUnicaCommandHandler : IRequestHandler<ExcluirAulaUnicaCommand, RetornoBaseDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorioAula;
        private readonly IUnitOfWork unitOfWork;

        public ExcluirAulaUnicaCommandHandler(IMediator mediator,
                                              IRepositorioAula repositorioAula,
                                              IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RetornoBaseDto> Handle(ExcluirAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            var aula = await repositorioAula.ObterPorIdAsync(request.AulaId);

            if (await mediator.Send(new AulaPossuiAvaliacaoQuery(aula, request.Usuario.CodigoRf)))
                throw new NegocioException("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");

            await ValidarComponentesDoProfessor(aula.TurmaId, long.Parse(aula.DisciplinaId), aula.DataAula, request.Usuario);

            unitOfWork.IniciarTransacao();
            try
            {
                if (aula.WorkflowAprovacaoId.HasValue)
                    await mediator.Send(new ExcluirWorkflowCommand(aula.WorkflowAprovacaoId.Value));

                await mediator.Send(new ExcluirNotificacoesDaAulaCommand(aula.Id));
                await mediator.Send(new ExcluirFrequenciaDaAulaCommand(aula.Id));
                await mediator.Send(new ExcluirPlanoAulaDaAulaCommand(aula.Id));

                aula.Excluido = true;
                await repositorioAula.SalvarAsync(aula);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add("Aula excluída com sucesso.");
            return retorno;
        }

        private async Task ValidarComponentesDoProfessor(string codigoTurma, long componenteCurricularCodigo, DateTime dataAula, Usuario usuario)
        {
            var resultadoValidacao = await mediator.Send(new ValidarComponentesDoProfessorCommand(usuario, codigoTurma, componenteCurricularCodigo, dataAula));
            if (!resultadoValidacao.resultado)
                throw new NegocioException(resultadoValidacao.mensagem);
        }

    }
}
