using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
  public  class ObterNotasFinaisBimestresAlunoQueryHandler : IRequestHandler<ObterNotasFinaisBimestresAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;
        private readonly IMediator mediator;

        public ObterNotasFinaisBimestresAlunoQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota,
            IMediator mediator)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFinaisBimestresAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseNota.ObterNotasFinaisBimestresAlunoAsync(request.AlunoCodigo,request.TurmasCodigos,request.Bimestre,request.DataMatricula,request.DataSituacao,request.ValidaMatricula);
        }
    }
}
