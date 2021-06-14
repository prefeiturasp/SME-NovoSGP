using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMiniaturaFotoEstudantePorAlunoCodigoQueryHandler : IRequestHandler<ObterMiniaturaFotoEstudantePorAlunoCodigoQuery, MiniaturaFotoDto>
    {
        private readonly IRepositorioAlunoFoto repositorio;

        public ObterMiniaturaFotoEstudantePorAlunoCodigoQueryHandler(IRepositorioAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<MiniaturaFotoDto> Handle(ObterMiniaturaFotoEstudantePorAlunoCodigoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterFotosPorAlunoCodigo(request.AlunoCodigo);
    }
}