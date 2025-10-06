using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterSondagemEscrita
{
    public class ObterSondagemEscritaQueryHandler : IRequestHandler<ObterSondagemEscritaQuery, IEnumerable<SondagemEscritaDto>>
    {
        private readonly IRepositorioSondagemEscrita repositorio;

        public ObterSondagemEscritaQueryHandler(
            IRepositorioSondagemEscrita repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<SondagemEscritaDto>> Handle(ObterSondagemEscritaQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterSondagemEscritaAsync(request.CodigoDre, request.CodigoUe, request.AnoLetivo, request.Bimestre, request.SerieAno);
        }
    }
}
