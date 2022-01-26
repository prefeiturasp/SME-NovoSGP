using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTurmaAbrangenciaQueryHandler : IRequestHandler<ObterProfessoresTurmaAbrangenciaQuery, IEnumerable<string>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterProfessoresTurmaAbrangenciaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }
        public async Task<IEnumerable<string>> Handle(ObterProfessoresTurmaAbrangenciaQuery request, CancellationToken cancellationToken)
         => await repositorioAbrangencia.ObterProfessoresTurmaPorAbrangencia(request.TurmaCodigo);
    }
}
