using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosUesPorAbrangenciaUseCase : IObterFiltroRelatoriosUesPorAbrangenciaUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosUesPorAbrangenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> Executar(string codigoDre, int anoLetivo, bool consideraNovasUEs = false, bool consideraHistorico = false)
        {
            var ues = new List<AbrangenciaUeRetorno>();
            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre == "-99")
            {
                ues.Add(new AbrangenciaUeRetorno { Codigo = "-99", NomeSimples = "Todas" });
                return ues;
            }

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            return await mediator.Send(new ObterFiltroRelatoriosUesPorAbrangenciaQuery(usuarioLogado, codigoDre, anoLetivo, consideraNovasUEs, consideraHistorico));
        }
    }
}
