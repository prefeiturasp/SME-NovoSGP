using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPaginadoCadastroAcessoABAEPorFiltroQueryHandler : ConsultasBase, IRequestHandler<ObterPaginadoCadastroAcessoABAEPorFiltroQuery, PaginacaoResultadoDto<DreUeNomeSituacaoTipoEscolaDataABAEDto>>
    {
        private readonly IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta;

        public ObterPaginadoCadastroAcessoABAEPorFiltroQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioCadastroAcessoABAEConsulta repositorioCadastroAcessoABAEConsulta) : base(contextoAplicacao)
        {
            this.repositorioCadastroAcessoABAEConsulta = repositorioCadastroAcessoABAEConsulta ?? throw new ArgumentNullException(nameof(repositorioCadastroAcessoABAEConsulta));
        }

        public async Task<PaginacaoResultadoDto<DreUeNomeSituacaoTipoEscolaDataABAEDto>> Handle(ObterPaginadoCadastroAcessoABAEPorFiltroQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCadastroAcessoABAEConsulta.ObterPaginado(request.FiltroDreIdUeIdNomeSituacaoABAEDto, Paginacao);
        }
    }
}
