using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class ObterPaginadoCadastroAcessoABAEUseCase: AbstractUseCase, IObterPaginadoCadastroAcessoABAEUseCase
{
    public ObterPaginadoCadastroAcessoABAEUseCase(IMediator mediator) : base(mediator)
    {}

    public async Task<PaginacaoResultadoDto<DreUeNomeSituacaoABAEDto>> Executar(FiltroDreIdUeIdNomeSituacaoABAEDto filtro)
    {
        var retorno = await mediator.Send(new ObterPaginadoCadastroAcessoABAEPorFiltroQuery(filtro));
        
        return new PaginacaoResultadoDto<DreUeNomeSituacaoABAEDto>()
        {
            Items = retorno.Items.Select(s => new DreUeNomeSituacaoABAEDto()
            {
                Id = s.Id,
                Dre = s.Dre,
                Ue = $"{s.TipoEscola.ObterNomeCurto()} {s.Ue}",
                Nome = s.Nome,
                Situacao = s.Situacao
            }),
            TotalRegistros = retorno.TotalRegistros,
            TotalPaginas = retorno.TotalPaginas
        };
    }
}