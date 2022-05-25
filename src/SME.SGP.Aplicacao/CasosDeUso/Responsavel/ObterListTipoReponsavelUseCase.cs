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
    public class ObterListTipoReponsavelUseCase : AbstractUseCase, IObterListTipoReponsavelUseCase
    {
        public ObterListTipoReponsavelUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoReponsavelRetornoDto>> Executar(bool exibirTodos)
        {
            var tipos = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                .Cast<TipoResponsavelAtribuicao>()
                .Select(d => new TipoReponsavelRetornoDto() { Codigo = (int)d, Descricao = d.Name() }).OrderBy(x => x.Descricao)
                .ToList();

            if (exibirTodos)
                return tipos;

            var perfil = await mediator.Send(new ObterPerfilAtualQuery());

            if (perfil == Perfis.PERFIL_CEFAI)
                return tipos.Where(c => c.Codigo == 2);
            else if (perfil == Perfis.PERFIL_COORDENADOR_NAAPA)
                return tipos.Where(c => c.Codigo == 3 || c.Codigo == 4 || c.Codigo == 5);
            else
                return Enumerable.Empty<TipoReponsavelRetornoDto>();
        }
    }
}
