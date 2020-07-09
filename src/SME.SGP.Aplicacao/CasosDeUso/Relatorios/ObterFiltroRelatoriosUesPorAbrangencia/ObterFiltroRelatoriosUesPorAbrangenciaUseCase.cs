using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosUesPorAbrangenciaUseCase : IObterFiltroRelatoriosUesPorAbrangenciaUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterFiltroRelatoriosUesPorAbrangenciaUseCase(IMediator mediator,
                                              IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }
        public async Task<IEnumerable<AbrangenciaUeRetorno>> Executar(string codigoDre)
        {
            var ues = new List<AbrangenciaUeRetorno>();
            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre=="-99")
            {
                ues.Add(new AbrangenciaUeRetorno { Codigo = "-99", Nome = "Todas" });
                return ues;
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            
            ues = (await repositorioAbrangencia.ObterUes(codigoDre, usuarioLogado.Login, usuarioLogado.PerfilAtual))?.ToList();
            
            var possuiAbrangenciaEmTodasAsUes = await mediator.Send(new ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery(usuarioLogado.PerfilAtual));
            if (possuiAbrangenciaEmTodasAsUes)
            {
                ues?.Insert(0, new AbrangenciaUeRetorno { Codigo = "-99", Nome = "Todas" });
            }
            return ues;
        }
    }
}
