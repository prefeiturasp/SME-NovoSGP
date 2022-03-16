using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisPorAlunoTurmasQueryHandler : IRequestHandler<ObterNotasFinaisPorAlunoTurmasQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamento;

        public ObterNotasFinaisPorAlunoTurmasQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamento)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFinaisPorAlunoTurmasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamento.ObterNotasFinaisAlunoAsync(request.TurmasCodigos, request.AlunoCodigo);
        }
    }
}
