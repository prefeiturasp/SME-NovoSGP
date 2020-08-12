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
    public class InserirCartaIntencoesCommandHandler : IRequestHandler<InserirCartaIntencoesCommand, AuditoriaDto>
    {
        private readonly IRepositorioCartaIntencoes repositorioCartaIntencoes;

        public InserirCartaIntencoesCommandHandler(IRepositorioCartaIntencoes repositorioCartaIntencoes)
        {
            this.repositorioCartaIntencoes = repositorioCartaIntencoes ?? throw new ArgumentNullException(nameof(repositorioCartaIntencoes));
        }

        public async Task<AuditoriaDto> Handle(InserirCartaIntencoesCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoes = MapearParaEntidade(request);

            await repositorioCartaIntencoes.SalvarAsync(cartaIntencoes);

            return (AuditoriaDto)cartaIntencoes;
        }

        private CartaIntencoes MapearParaEntidade(InserirCartaIntencoesCommand request)
            => new CartaIntencoes()
            {
                TurmaId = request.TurmaId,
                PeriodoEscolarId = request.Carta.PeriodoEscolarId,
                ComponenteCurricularId = request.ComponenteCurricularId,
                Planejamento = request.Carta.Planejamento,
            };
    }
}
