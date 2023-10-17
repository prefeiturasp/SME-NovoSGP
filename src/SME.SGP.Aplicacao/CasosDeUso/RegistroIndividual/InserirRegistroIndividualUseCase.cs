using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualUseCase : AbstractUseCase, IInserirRegistroIndividualUseCase
    {
        public InserirRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RegistroIndividual> Executar(InserirRegistroIndividualDto inserirRegistroIndividualDto)
        {
            if (inserirRegistroIndividualDto.Data.Date > DateTimeExtension.HorarioBrasilia().Date)
                throw new NegocioException(MensagemNegocioComponentesCurriculares.NAO_EH_PERMITIDO_FAZER_REGISTRO_INDIVIDUAL_EM_DATA_FUTURA);
            
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(inserirRegistroIndividualDto.TurmaId));
            
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            if (turma.EhAnoAnterior())
                throw new NegocioException(MensagemNegocioRegistroIndividual.NAO_EH_PERMITIDO_FAZER_REGISTRO_INDIVIDUAL_EM_TURMA_DE_ANO_ANTERIOR);
            
            var mesmoAnoLetivo = DateTime.Today.Year == inserirRegistroIndividualDto.Data.Year;

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(inserirRegistroIndividualDto.Data, turma));

            bool temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia().Date, bimestreAula, mesmoAnoLetivo));
   
            if (!temPeriodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);

            var periodoEscolar = await mediator.Send(new ObterUltimoPeriodoEscolarPorDataQuery(turma.AnoLetivo,turma.ModalidadeTipoCalendario, DateTimeExtension.HorarioBrasilia().Date));
            if (periodoEscolar.EhNulo())
                throw new NegocioException(MensagemNegocioPeriodo.PERIODO_ESCOLAR_NAO_ENCONTRADO);
            
            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turma.CodigoTurma, inserirRegistroIndividualDto.AlunoCodigo.ToString()));
            
            if (aluno.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo) 
                || aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Rematriculado
                || aluno.DataSituacao.Date >= periodoEscolar.PeriodoInicio.Date)
            {
                var auditoriaIndividual = await mediator.Send(new InserirRegistroIndividualCommand(inserirRegistroIndividualDto.TurmaId, inserirRegistroIndividualDto.AlunoCodigo, inserirRegistroIndividualDto.ComponenteCurricularId, inserirRegistroIndividualDto.Data, inserirRegistroIndividualDto.Registro));
                await PublicarAtualizacaoPendenciaRegistroIndividualAsync(inserirRegistroIndividualDto.TurmaId, inserirRegistroIndividualDto.AlunoCodigo, inserirRegistroIndividualDto.Data);
                return auditoriaIndividual;                
            }

            return null;
        }

        private async Task PublicarAtualizacaoPendenciaRegistroIndividualAsync(long turmaId, long codigoAluno, DateTime data)
        {
            try
            {
                await mediator.Send(new PublicarAtualizacaoPendenciaRegistroIndividualCommand(turmaId, codigoAluno, data));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível publicar o evento de atualização de pendências por ausência de registro individual. {ex.InnerException?.Message ?? ex.Message}", LogNivel.Negocio, LogContexto.RegistroIndividual));
            }
        }
    }
}
