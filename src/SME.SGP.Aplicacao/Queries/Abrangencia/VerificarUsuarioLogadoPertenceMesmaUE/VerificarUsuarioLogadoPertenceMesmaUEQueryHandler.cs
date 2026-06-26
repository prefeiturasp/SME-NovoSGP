using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarUsuarioLogadoPertenceMesmaUEQueryHandler : IRequestHandler<VerificarUsuarioLogadoPertenceMesmaUEQuery, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public VerificarUsuarioLogadoPertenceMesmaUEQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<bool> Handle(VerificarUsuarioLogadoPertenceMesmaUEQuery request,CancellationToken cancellationToken)
        {
           return await repositorioAbrangencia.VerificarUsuarioLogadoPertenceMesmaUE(request.CodigoUe, request.Login, request.Perfil,
                request.Modalidade, request.AnoLetivo, request.Periodo,request.ConsideraHistorico);
        }
    }
}
