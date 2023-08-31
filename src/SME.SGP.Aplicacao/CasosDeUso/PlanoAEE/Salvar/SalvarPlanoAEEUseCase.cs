using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class SalvarPlanoAEEUseCase : ISalvarPlanoAEEUseCase
    {
        private readonly IMediator mediator;

        public SalvarPlanoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoPlanoAEEDto> Executar(PlanoAEEPersistenciaDto planoAeeDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(planoAeeDto.TurmaCodigo));
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            if (!turma.EhTurmaRegular())
                throw new NegocioException(MensagemNegocioTurma.TURMA_DEVE_SER_TIPO_REGULAR);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(planoAeeDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            var planoAeePersistidoDto = await mediator.Send(new SalvarPlanoAeeCommand(planoAeeDto, turma.Id, aluno.NomeAluno, aluno.CodigoAluno, aluno.ObterNumeroAlunoChamada()));

            await mediator.Send(new SalvarPlanoAEETurmaAlunoCommand(planoAeePersistidoDto.PlanoId, aluno.CodigoAluno));

            return planoAeePersistidoDto;
        }     
    }
}