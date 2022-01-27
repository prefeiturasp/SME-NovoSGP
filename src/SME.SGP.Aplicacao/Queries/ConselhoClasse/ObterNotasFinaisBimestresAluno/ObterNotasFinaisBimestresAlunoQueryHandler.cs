using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
  public  class ObterNotasFinaisBimestresAlunoQueryHandler : IRequestHandler<ObterNotasFinaisBimestresAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;

        public ObterNotasFinaisBimestresAlunoQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFinaisBimestresAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseNota.ObterNotasFinaisBimestresAlunoAsync(request.AlunoCodigo, request.TurmasCodigos, request.Bimestre);
        }
    }
}
