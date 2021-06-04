using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CodigosAlunosComRegistroFrequenciaAlunoQueryHandler : IRequestHandler<CodigosAlunosComRegistroFrequenciaAlunoQuery, IEnumerable<string>>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public CodigosAlunosComRegistroFrequenciaAlunoQueryHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<string>> Handle(CodigosAlunosComRegistroFrequenciaAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ObterCodigosAlunosComRegistroFrequenciaAlunoAsync(request.RegistroFrequenciaId, request.CodigosAlunos, request.NumeroAula);
        }
    }
}
