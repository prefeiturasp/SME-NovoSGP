using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterDisciplinasTurmasEolQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Descricao = "Português",
                    LancaNota = true,
                    GrupoMatriz = new GrupoMatriz()
                    {
                        Id = 1,
                        Nome = "Base Nacional Comum"
                    }
                }
            });
        }
    }
}
