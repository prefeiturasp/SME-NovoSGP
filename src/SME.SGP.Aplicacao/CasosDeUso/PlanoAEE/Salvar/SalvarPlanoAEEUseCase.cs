using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class SalvarPlanoAEEUseCase : ISalvarPlanoAEEUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPlanoAEEUseCase(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RetornoPlanoAEEDto> Executar(PlanoAEEPersistenciaDto planoAeeDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(planoAeeDto.TurmaCodigo));
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            if (!turma.EhTurmaRegular())
                throw new NegocioException(MensagemNegocioTurma.TURMA_DEVE_SER_TIPO_REGULAR);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(planoAeeDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            using var transacao = unitOfWork.IniciarTransacao();

            try
            {
                var planoAeePersistidoDto = await mediator
                    .Send(new SalvarPlanoAeeCommand(planoAeeDto, turma.Id, aluno.NomeAluno, aluno.CodigoAluno, aluno.ObterNumeroAlunoChamada()));

                await mediator
                    .Send(new SalvarPlanoAEETurmaAlunoCommand(planoAeePersistidoDto.PlanoId, aluno.CodigoAluno));

                unitOfWork.PersistirTransacao();

                return planoAeePersistidoDto;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível salvar o Plano AEE {planoAeeDto.Id}", LogNivel.Negocio, LogContexto.Geral, ex.Message, rastreamento: ex.StackTrace, innerException: ex.InnerException.ToString()));
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
