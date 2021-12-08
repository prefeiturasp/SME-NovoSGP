using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUePorLoginPerfilProfessorOuAbrangenciaUeQueryHandler : IRequestHandler<ObterUePorLoginPerfilProfessorOuAbrangenciaUeQuery, string>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        public ObterUePorLoginPerfilProfessorOuAbrangenciaUeQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }
        public async Task<string> Handle(ObterUePorLoginPerfilProfessorOuAbrangenciaUeQuery request, CancellationToken cancellationToken)
         => await repositorioAbrangencia.ObterUe(request.Login, request.Perfil);
    }
}
