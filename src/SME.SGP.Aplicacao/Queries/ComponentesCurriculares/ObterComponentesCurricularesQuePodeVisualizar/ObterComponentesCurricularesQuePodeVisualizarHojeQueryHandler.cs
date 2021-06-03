using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesQuePodeVisualizarHojeQueryHandler : IRequestHandler<ObterComponentesCurricularesQuePodeVisualizarHojeQuery, string[]>
    {
        private readonly IServicoEol servicoEol;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesQuePodeVisualizarHojeQueryHandler(IServicoEol servicoEol, IMediator mediator)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string[]> Handle(ObterComponentesCurricularesQuePodeVisualizarHojeQuery request, CancellationToken cancellationToken)
        {
            var componentesCurricularesParaVisualizar = new List<string>();

            var componentesCurricularesUsuarioLogado = await ObterComponentesCurricularesUsuarioLogado(request.TurmaCodigo, request.UsuarioLogado.CodigoRf, request.UsuarioLogado.PerfilAtual);
            var componentesCurricularesIdsUsuarioLogado = componentesCurricularesUsuarioLogado?.Select(b => b.Codigo.ToString());            

            foreach (var componenteParaVerificarAtribuicao in componentesCurricularesIdsUsuarioLogado)
            {
                if (await PodePersistirTurmaDisciplina(request.UsuarioLogado.CodigoRf, request.TurmaCodigo, componenteParaVerificarAtribuicao))
                    componentesCurricularesParaVisualizar.Add(componenteParaVerificarAtribuicao);
            }

            return componentesCurricularesParaVisualizar.ToArray();
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesUsuarioLogado(string turmaCodigo, string criadoRF, Guid perfilAtual)
        {
            return await mediator.Send(new ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQuery(turmaCodigo, criadoRF, perfilAtual));
        }
        public async Task<bool> PodePersistirTurmaDisciplina(string criadoRF, string turmaCodigo, string componenteParaVerificarAtribuicao)
        {
            var hoje = DateTime.Today;
            return await mediator.Send(new PodePersistirTurmaDisciplinaQuery(criadoRF, turmaCodigo, componenteParaVerificarAtribuicao, hoje));
        }
    }
}
