using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System.Linq;

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
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            if (!turma.EhTurmaRegular())
                throw new NegocioException(MensagemNegocioTurma.TURMA_DEVE_SER_TIPO_REGULAR);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(planoAeeDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            var planoAeePersistidoDto = await mediator.Send(new SalvarPlanoAeeCommand(planoAeeDto, turma.Id, aluno.NomeAluno, aluno.CodigoAluno, aluno.ObterNumeroAlunoChamada()));

            await mediator.Send(new SalvarPlanoAEETurmaAlunoCommand(planoAeePersistidoDto.PlanoId, aluno.CodigoAluno));

            await ValidaQuestaoPeriodoEscolarSeEstaNoPeriodoCorreto(planoAeePersistidoDto);

            return planoAeePersistidoDto;
        }

        private async Task ValidaQuestaoPeriodoEscolarSeEstaNoPeriodoCorreto(RetornoPlanoAEEDto planoAeePersistidoDto)
        {
            var planoAee = await mediator.Send(new ObterPlanoAEEPorIdQuery(planoAeePersistidoDto.PlanoId));
            
            if(planoAee.EhNulo())
                throw new NegocioException(MensagemNegocioPlanoAee.Plano_aee_nao_encontrado);

            var ultimaVersaoPlanoAee = await mediator.Send(new ObterVersaoPlanoAEEPorIdQuery(planoAeePersistidoDto.PlanoVersaoId));

            if(ultimaVersaoPlanoAee.NaoEhNulo() && ultimaVersaoPlanoAee.Numero > 1)
            {
                var turma = await mediator.Send(new ObterTurmaPorIdQuery(planoAee.TurmaId));

                if (turma.EhNulo())
                    throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

                var periodoEscolarPlano = await mediator.Send(new ObterPeriodoEscolarAtualPorTurmaQuery(turma, DateTime.Now));

                if(periodoEscolarPlano.EhNulo())
                    throw new NegocioException(MensagemNegocioPeriodo.PERIODO_ESCOLAR_NAO_ENCONTRADO);

                var respostaQuestoesPlanoAee = await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(ultimaVersaoPlanoAee.Id));

                if(respostaQuestoesPlanoAee.NaoEhNulo() && respostaQuestoesPlanoAee.Any())
                {
                    var questionarioId = await mediator.Send(new ObterQuestionarioPlanoAEEIdQuery());
                    var questaoPeriodoEscolar = (await mediator.Send(new ObterQuestoesPlanoAEEPorVersaoQuery(questionarioId, ultimaVersaoPlanoAee.Id, turma.CodigoTurma)))?.FirstOrDefault(x => x.TipoQuestao == TipoQuestao.PeriodoEscolar);

                    if (questaoPeriodoEscolar.NaoEhNulo())
                    {                    
                        //Obtem resposta da questao Bimestre de vigência do plano
                        var respostaPeriodoEscolar = respostaQuestoesPlanoAee.FirstOrDefault(x => x.QuestaoId == questaoPeriodoEscolar.Id);

                        if (respostaPeriodoEscolar.NaoEhNulo() && (long)Convert.ToDouble(respostaPeriodoEscolar.Texto) != periodoEscolarPlano.Id)
                        {
                            await mediator.Send(new AtualizarPlanoAEERespostaPeriodoEscolarCommand(respostaPeriodoEscolar.Id, periodoEscolarPlano.Id.ToString()));
                        }
                    }
                }
            }
        }
    }
}