using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosUesPorAbrangenciaUseCase : IObterFiltroRelatoriosUesPorAbrangenciaUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosUesPorAbrangenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        
        public async Task<IEnumerable<AbrangenciaUeRetorno>> Executar(string codigoDre)
        {
            var ues = new List<AbrangenciaUeRetorno>();
            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre == "-99")
            {
                ues.Add(new AbrangenciaUeRetorno { Codigo = "-99", NomeSimples = "Todas" });
                return ues;
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            return await mediator.Send(new ObterFiltroRelatoriosUesPorAbrangenciaQuery(usuarioLogado, codigoDre));
        }
    }
}
