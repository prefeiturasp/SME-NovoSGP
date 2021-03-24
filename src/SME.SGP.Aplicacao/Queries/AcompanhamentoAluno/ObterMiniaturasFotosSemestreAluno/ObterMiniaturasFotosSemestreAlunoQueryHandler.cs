using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMiniaturasFotosSemestreAlunoQueryHandler : IRequestHandler<ObterMiniaturasFotosSemestreAlunoQuery, IEnumerable<MiniaturaFotoDto>>
    {
        private readonly IRepositorioAcompanhamentoAlunoFoto repositorio;

        public ObterMiniaturasFotosSemestreAlunoQueryHandler(IRepositorioAcompanhamentoAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<MiniaturaFotoDto>> Handle(ObterMiniaturasFotosSemestreAlunoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterFotosPorSemestreId(request.AcompanhamentoSemestreId, request.QuantidadeFotos);
    }
}
