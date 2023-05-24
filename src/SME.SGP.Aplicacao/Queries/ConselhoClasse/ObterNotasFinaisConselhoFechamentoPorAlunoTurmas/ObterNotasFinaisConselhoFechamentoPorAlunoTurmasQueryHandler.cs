using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQueryHandler : IRequestHandler<ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery, IEnumerable<NotaConceitoFechamentoConselhoFinalDto>>
    {
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta;

        public ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta)
        {
            this.repositorioConselhoClasseAlunoConsulta = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
        }

        public async Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> Handle(ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAlunoConsulta.ObterNotasFinaisAlunoAsync(request.TurmasCodigos, request.AlunoCodigo);
        }
    }
}
