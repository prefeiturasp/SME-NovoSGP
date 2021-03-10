using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEEUseCase : AbstractUseCase, IObterVersoesPlanoAEEUseCase
    {
        public ObterVersoesPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PlanoAEEDescricaoVersaoDto>> Executar(FiltroVersoesPlanoAEEDto filtro)
        {
            var versoesPlanoAEE = await mediator.Send(new ObterVersoesPlanoAEESemReestruturacaoQuery(filtro.PlanoId, filtro.ReestruturacaoId));
            return MapearPlanoAEEDescricao(versoesPlanoAEE);
        }

        private static List<PlanoAEEDescricaoVersaoDto> MapearPlanoAEEDescricao(IEnumerable<PlanoAEEVersaoDto> versoesPlanoAEE)
        {
            var planosAEEDescricaoVersaoDto = new List<PlanoAEEDescricaoVersaoDto>();
            foreach (var item in versoesPlanoAEE)
            {
                var planoAEEDescricaoVersaoDto = new PlanoAEEDescricaoVersaoDto
                {
                    Id = item.Id,
                    Descricao = $"v{item.Numero} - {item.CriadoEm:dd/MM/yyyy}"
                };
                planosAEEDescricaoVersaoDto.Add(planoAEEDescricaoVersaoDto);
            }

            return planosAEEDescricaoVersaoDto;
        }
    }
}
