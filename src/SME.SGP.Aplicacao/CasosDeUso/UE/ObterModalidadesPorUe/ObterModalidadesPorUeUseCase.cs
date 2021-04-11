using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorUeUseCase : IObterModalidadesPorUeUseCase
    {
        private readonly IMediator mediator;

        public ObterModalidadesPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<ModalidadeRetornoDto>> Executar(string ueCodigo, int anoLetivo, bool consideraNovasModalidades)
        {
            var modadlidadesQueSeraoIgnoradas = await ObterModalidadesQueSeraoIgnoradas(anoLetivo, consideraNovasModalidades);
            return await mediator.Send(new ObterModalidadesPorUeEAnoLetivoQuery(ueCodigo, anoLetivo, modadlidadesQueSeraoIgnoradas));
        }

        private async Task<IEnumerable<Modalidade>> ObterModalidadesQueSeraoIgnoradas(int anoLetivo, bool consideraNovasModalidades)
        {
            if (consideraNovasModalidades) return null;
            return await mediator.Send(new ObterNovasModalidadesPorAnoQuery(anoLetivo));
        }
    }
}