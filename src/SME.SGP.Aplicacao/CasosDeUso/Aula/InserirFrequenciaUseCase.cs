using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciaUseCase : AbstractUseCase, IInserirFrequenciaUseCase
    {
        public InserirFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(FrequenciaDto param)
        {
            var frequenciaAuditoriaAulaDto = await mediator.Send(new InserirFrequenciasAulaCommand(param));
            
            if (frequenciaAuditoriaAulaDto.AulaIdComErro.HasValue)
                throw new NegocioException(string.Format(MensagensNegocioFrequencia.Nao_foi_possivel_registrar_a_frequencia_do_dia_x,frequenciaAuditoriaAulaDto.DataAulaComErro.Value.ToString("dd/MM/yyyy")));

            return frequenciaAuditoriaAulaDto.Auditoria;
        }
    }
}
