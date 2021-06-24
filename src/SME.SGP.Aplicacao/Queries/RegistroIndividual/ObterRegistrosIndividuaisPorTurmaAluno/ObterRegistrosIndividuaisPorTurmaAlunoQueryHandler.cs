using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosIndividuaisPorTurmaAlunoQueryHandler : IRequestHandler<ObterRegistrosIndividuaisPorTurmaAlunoQuery, IEnumerable<RegistroIndividualAlunoDTO>>
    {
        private readonly IRepositorioRegistroIndividual repositorio;

        public ObterRegistrosIndividuaisPorTurmaAlunoQueryHandler(IRepositorioRegistroIndividual repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<RegistroIndividualAlunoDTO>> Handle(ObterRegistrosIndividuaisPorTurmaAlunoQuery request, CancellationToken cancellationToken)
                => await repositorio.ObterRegistrosIndividuaisPorTurmaAlunoAsync(request.TurmaCodigo, request.AlunoCodigo, request.Modalidades);            
    }
}
