using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMiniaturasFotosSemestreAlunoQueryHandler : IRequestHandler<ObterMiniaturasFotosSemestreAlunoQuery, IEnumerable<Arquivo>>
    {
        private readonly IRepositorioAcompanhamentoAlunoFoto repositorio;

        public ObterMiniaturasFotosSemestreAlunoQueryHandler(IRepositorioAcompanhamentoAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<Arquivo>> Handle(ObterMiniaturasFotosSemestreAlunoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterFotosPorSemestreId(request.AcompanhamentoSemestreId);
    }
}
