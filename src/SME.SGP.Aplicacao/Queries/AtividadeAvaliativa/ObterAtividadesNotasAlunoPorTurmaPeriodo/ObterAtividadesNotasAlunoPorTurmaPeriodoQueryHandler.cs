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
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoQueryHandler : IRequestHandler<ObterAtividadesNotasAlunoPorTurmaPeriodoQuery, IEnumerable<AvaliacaoNotaAlunoDto>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterAtividadesNotasAlunoPorTurmaPeriodoQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public async Task<IEnumerable<AvaliacaoNotaAlunoDto>> Handle(ObterAtividadesNotasAlunoPorTurmaPeriodoQuery request, CancellationToken cancellationToken)
            =>  await repositorioAtividadeAvaliativa.ObterAtividadesNotasAlunoPorTurmaPeriodo(request.TurmaId, request.PeriodoEscolarId, request.AlunoCodigo, request.ComponenteCurricular);

    }
}