using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesQueryHandler : IRequestHandler<ObterNotificacoesQuery, PaginacaoResultadoDto<Notificacao>>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacaoConsulta;

        public ObterNotificacoesQueryHandler(IRepositorioNotificacaoConsulta repositorio)
        {
            this.repositorioNotificacaoConsulta = repositorio;
        }

        public Task<PaginacaoResultadoDto<Notificacao>> Handle(ObterNotificacoesQuery request, CancellationToken cancellationToken)
        {
            return repositorioNotificacaoConsulta.Obter(request.DreId, request.UeId, request.Status, request.TurmaId, request.UsuarioRf, request.Tipo, request.Categoria, request.Titulo, request.Codigo, request.AnoLetivo, request.Paginacao);
        }
    }    
}