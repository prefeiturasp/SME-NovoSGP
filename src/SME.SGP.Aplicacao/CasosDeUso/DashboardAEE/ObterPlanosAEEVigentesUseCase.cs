using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEVigentesUseCase : AbstractUseCase, IObterPlanosAEEVigentesUseCase
    {
        public ObterPlanosAEEVigentesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AEETurmaDto>> Executar(FiltroDashboardAEEDto param)
        {
            if (param.AnoLetivo == 0)
                param.AnoLetivo = DateTime.Now.Year;
            var planos = await mediator.Send(new ObterPlanosAEEVigentesQuery(param.AnoLetivo, param.DreId, param.UeId));

            return param.UeId > 0 ? MapearParaDtoTurmas(planos) : MapearParaDto(planos);
        }

        private IEnumerable<AEETurmaDto> MapearParaDto(IEnumerable<AEETurmaDto> planos)
        {
            return planos.Select(a => new AEETurmaDto()
            {
                Modalidade = a.Modalidade,
                Quantidade = a.Quantidade,
                Descricao = a.AnoTurma > 0 ? $"{a.Modalidade.ShortName()} - {a.AnoTurma}" : a.Modalidade.ShortName(),
            });
        }

        private IEnumerable<AEETurmaDto> MapearParaDtoTurmas(IEnumerable<AEETurmaDto> planos)
        {
            return planos.Select(a => new AEETurmaDto()
            {
                Modalidade = a.Modalidade,
                Quantidade = a.Quantidade,
                Descricao = a.Nome != "" ? $"{a.Modalidade.ShortName()} - {a.Nome}" : a.Modalidade.ShortName(),
            });
        }
    }
}
