using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualUseCase : AbstractUseCase, IInserirRegistroIndividualUseCase
    {
        public InserirRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RegistroIndividual> Executar(InserirRegistroIndividualDto dto)
        {
            var auditoria = await mediator.Send(new InserirRegistroIndividualCommand(dto.TurmaId, dto.AlunoCodigo, dto.ComponenteCurricularId, dto.Data, dto.Registro));
            await PublicarAtualizacaoPendenciaRegistroIndividualAsync(dto.TurmaId, dto.AlunoCodigo, dto.Data);
            return auditoria;
        }

        private async Task PublicarAtualizacaoPendenciaRegistroIndividualAsync(long turmaId, long codigoAluno, DateTime data)
        {
            try
            {
                await mediator.Send(new PublicarAtualizacaoPendenciaRegistroIndividualCommand(turmaId, codigoAluno, data));
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureMessage($"Não foi possível publicar o evento de atualização de pendências por ausência de registro individual. {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
