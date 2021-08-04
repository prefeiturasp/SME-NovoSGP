using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosComunicadoUseCase : AbstractUseCase, IObterAnosLetivosComunicadoUseCase
    {
        public ObterAnosLetivosComunicadoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AnoLetivoComunicadoDto> Executar(int anoMinimo)
        {

            if (anoMinimo > DateTime.Now.Year)
                throw new NegocioException("O ano letivo não pode ser maior que o atual");

            var AnoLetivoComunicado = new AnoLetivoComunicadoDto();

            DateTime? dataInicial = null;

            DateTime dataAtual = DateTime.Now.Date;

            if (anoMinimo > 0)
                dataInicial = new DateTime(anoMinimo, 1, 1).Date;

            var anosLetivosHistorico = await mediator.Send(new ObterAnosLetivosHistoricoDeComunicadosQuery(dataInicial, dataAtual));

            if (anosLetivosHistorico != null && anosLetivosHistorico.Any())
            {
                AnoLetivoComunicado.TemHistorico = true;
                AnoLetivoComunicado.AnosLetivosHistorico = anosLetivosHistorico;
            }

            return AnoLetivoComunicado;
        }

    }
}
