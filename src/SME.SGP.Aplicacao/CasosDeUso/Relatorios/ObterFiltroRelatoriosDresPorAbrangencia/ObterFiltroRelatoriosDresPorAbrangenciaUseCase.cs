using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosDresPorAbrangenciaUseCase : IObterFiltroRelatoriosDresPorAbrangenciaUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterFiltroRelatoriosDresPorAbrangenciaUseCase(IMediator mediator,
                                              IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }
        public async Task<IEnumerable<AbrangenciaDreRetorno>> Executar()
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var dres = (await repositorioAbrangencia.ObterDres(usuarioLogado.Login, usuarioLogado.PerfilAtual))?.ToList();
            var possuiAbrangenciaEmTodasAsDres = await mediator.Send(new ObterUsuarioPossuiAbrangenciaEmTodasAsDresQuery(usuarioLogado.PerfilAtual));
            if (possuiAbrangenciaEmTodasAsDres)
            {
                dres?.Insert(0, new AbrangenciaDreRetorno { Abreviacao = "Todas", Codigo = "-99", Nome = "Todas" });
            }
            return dres;
        }
    }
}
