using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPaginadoCadastroAcessoABAEPorFiltroQuery : IRequest<PaginacaoResultadoDto<DreUeNomeSituacaoTipoEscolaDataABAEDto>>
    {
        public ObterPaginadoCadastroAcessoABAEPorFiltroQuery(FiltroDreIdUeIdNomeSituacaoABAEDto filtroDreIdUeIdNomeSituacaoAbaeDto)
        {
            FiltroDreIdUeIdNomeSituacaoABAEDto = filtroDreIdUeIdNomeSituacaoAbaeDto;
        }
        public FiltroDreIdUeIdNomeSituacaoABAEDto FiltroDreIdUeIdNomeSituacaoABAEDto { get; set; }
    }
}

