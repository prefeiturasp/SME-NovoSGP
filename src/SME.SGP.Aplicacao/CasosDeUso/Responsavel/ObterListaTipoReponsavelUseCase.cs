using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaTipoReponsavelUseCase : AbstractUseCase, IObterListaTipoReponsavelUseCase
    {
        public ObterListaTipoReponsavelUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoReponsavelRetornoDto>> Executar(bool exibirTodos)
        {
            var tipos = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                .Cast<TipoResponsavelAtribuicao>()
                .Select(d => new TipoReponsavelRetornoDto() { Codigo = (int)d, Descricao = d.Name() }).OrderBy(x => x.Descricao)
                .OrderBy(c => c.Codigo);

            var perfil = await mediator.Send(ObterPerfilAtualQuery.Instance);

            if (exibirTodos || (perfil == Perfis.PERFIL_ADMDRE))
                return await Task.FromResult(tipos);

            if (perfil == Perfis.PERFIL_CEFAI)
                return await Task.FromResult(tipos.Where(c => c.Codigo == 2));
            else if (perfil == Perfis.PERFIL_COORDENADOR_NAAPA)
                return await Task.FromResult(tipos.Where(c => c.Codigo == 3 || c.Codigo == 4 || c.Codigo == 5));
            else
                return await Task.FromResult(Enumerable.Empty<TipoReponsavelRetornoDto>());
        }
    }
}
