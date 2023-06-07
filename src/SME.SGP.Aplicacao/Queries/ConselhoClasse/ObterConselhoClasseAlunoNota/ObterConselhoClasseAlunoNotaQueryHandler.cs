using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoNotaQueryHandler: IRequestHandler<ObterConselhoClasseAlunoNotaQuery,IEnumerable<ConselhoClasseAlunoNotaDto>>
    {
        private readonly IRepositorioConselhoClasse repoConselhoClasse;

        public ObterConselhoClasseAlunoNotaQueryHandler(IRepositorioConselhoClasse repoConselhoClasse)
        {
            this.repoConselhoClasse = repoConselhoClasse ?? throw new ArgumentNullException(nameof(repoConselhoClasse));
        }

        public async Task<IEnumerable<ConselhoClasseAlunoNotaDto>> Handle(ObterConselhoClasseAlunoNotaQuery request, CancellationToken cancellationToken)
        {
            return await repoConselhoClasse.ObterConselhoClasseAlunoNota(request.TurmasCodigo,request.Bimestre);
        }
    }
}