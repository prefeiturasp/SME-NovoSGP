using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeUseCase : IObterFiltroRelatoriosModalidadesPorUeUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosModalidadesPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, bool filtraPorAbrangencia = false)
        {

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o usuario logado.");

            if (filtraPorAbrangencia)
                return await mediator.Send(new ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery(codigoUe));
            else return await mediator.Send(new ObterFiltroRelatoriosModalidadesPorUeQuery(codigoUe));
        }

    }
}
