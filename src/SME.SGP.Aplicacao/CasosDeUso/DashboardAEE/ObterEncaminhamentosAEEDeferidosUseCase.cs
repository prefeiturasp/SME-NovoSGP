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
    public class ObterEncaminhamentosAEEDeferidosUseCase : AbstractUseCase, IObterEncaminhamentosAEEDeferidosUseCase
    {
        public ObterEncaminhamentosAEEDeferidosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AEETurmaDto>> Executar(FiltroDashboardAEEDto param)
        {
            if (param.AnoLetivo == 0)
                param.AnoLetivo = DateTime.Now.Year;
            var encaminhamentos =  await mediator.Send(new ObterEncaminhamentosAEEDeferidosQuery(param.AnoLetivo, param.DreId, param.UeId));
            return param.UeId > 0 ? MapearParaDtoTurmas(encaminhamentos) : MapearParaDto(encaminhamentos);
        }

        private IEnumerable<AEETurmaDto> MapearParaDto(IEnumerable<AEETurmaDto> encaminhamentos)
        {
            return encaminhamentos.Select(a => new AEETurmaDto()
            {
                Modalidade = a.Modalidade,
                Quantidade = a.Quantidade,
                Descricao = a.AnoTurma > 0 ? $"{a.Modalidade.ShortName()} - {a.AnoTurma}" : a.Modalidade.ShortName(),
            });
        }

        private IEnumerable<AEETurmaDto> MapearParaDtoTurmas(IEnumerable<AEETurmaDto> encaminhamentos)
        {
            return encaminhamentos.Select(a => new AEETurmaDto()
            {
                Modalidade = a.Modalidade,
                Quantidade = a.Quantidade,
                Descricao = a.Nome != "" ? $"{a.Modalidade.ShortName()} - {a.Nome}" : a.Modalidade.ShortName(),
            });
        }
    }
}
