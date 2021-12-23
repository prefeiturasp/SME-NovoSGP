using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    public class ObterNotificacaoUltimoCodigoPorAnoQueryHandler : IRequestHandler<ObterNotificacaoUltimoCodigoPorAnoQuery, long>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacaoConsulta;

        public ObterNotificacaoUltimoCodigoPorAnoQueryHandler(IRepositorioNotificacaoConsulta repositorio)
        {
            this.repositorioNotificacaoConsulta = repositorio;
        }

        public async Task<long> Handle(ObterNotificacaoUltimoCodigoPorAnoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNotificacaoConsulta.ObterUltimoCodigoPorAnoAsync(request.Ano);
        }
    }
}