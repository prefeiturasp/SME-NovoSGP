using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class AlterarCartaIntencoesCommandHandler : IRequestHandler<AlterarCartaIntencoesCommand, AuditoriaDto>
    {
        private readonly IRepositorioCartaIntencoes repositorioCartaIntencoes;

        public AlterarCartaIntencoesCommandHandler(IRepositorioCartaIntencoes repositorioCartaIntencoes)
        {
            this.repositorioCartaIntencoes = repositorioCartaIntencoes ?? throw new ArgumentNullException(nameof(repositorioCartaIntencoes));
        }

        public async Task<AuditoriaDto> Handle(AlterarCartaIntencoesCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoes = MapearParaEntidade(request);

            await repositorioCartaIntencoes.SalvarAsync(cartaIntencoes);

            return (AuditoriaDto)cartaIntencoes;
        }

        private CartaIntencoes MapearParaEntidade(AlterarCartaIntencoesCommand request)
            => new CartaIntencoes()
            {
                Id = request.Carta.Id,
                TurmaId = request.TurmaId,
                PeriodoEscolarId = request.Carta.PeriodoEscolarId,
                ComponenteCurricularId = request.ComponenteCurricularId,
                Planejamento = request.Carta.Planejamento,
                AlteradoEm = request.Existente.AlteradoEm,
                AlteradoPor = request.Existente.AlteradoPor,
                AlteradoRF = request.Existente.AlteradoRF,
                CriadoEm = request.Existente.CriadoEm,
                CriadoPor = request.Existente.CriadoPor,
                CriadoRF = request.Existente.CriadoRF,
                Excluido = request.Existente.Excluido
            };
    }
}
