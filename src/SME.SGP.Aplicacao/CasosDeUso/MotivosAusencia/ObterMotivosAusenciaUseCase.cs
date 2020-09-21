using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries.MotivosAusencia.ObterMotivosAusencia;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMotivosAusenciaUseCase : AbstractUseCase, IObterMotivosAusenciaUseCase
    {
        public ObterMotivosAusenciaUseCase(IMediator mediator) : base(mediator)
        {

        }      

        public async Task<IEnumerable<OpcaoDropdownDto>> Executar()
            => MapearParaDto(await mediator.Send(new ObterMotivosAusenciaQuery()));

        private IEnumerable<OpcaoDropdownDto> MapearParaDto(IEnumerable<MotivoAusencia> motivos)
        {
            return motivos?.Select(m => MapearParaDto(m));
        }

        private OpcaoDropdownDto MapearParaDto(MotivoAusencia motivoAusencia)
            => motivoAusencia == null ? null :
            new OpcaoDropdownDto(motivoAusencia.Id.ToString(), motivoAusencia.Descricao);            
    }
}
