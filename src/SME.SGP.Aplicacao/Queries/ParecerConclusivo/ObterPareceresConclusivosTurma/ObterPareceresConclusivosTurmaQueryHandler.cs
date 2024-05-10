using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosTurmaQueryHandler : IRequestHandler<ObterPareceresConclusivosTurmaQuery, IEnumerable<ParecerConclusivoDto>>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorio;
        
        public ObterPareceresConclusivosTurmaQueryHandler(IRepositorioConselhoClasseParecerConclusivo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<ParecerConclusivoDto>> Handle(ObterPareceresConclusivosTurmaQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterPareceresConclusivos(request.Turma.AnoLetivo, request.Turma.AnoTurmaInteiro, request.Turma.ModalidadeCodigo);
        }
    }
}
