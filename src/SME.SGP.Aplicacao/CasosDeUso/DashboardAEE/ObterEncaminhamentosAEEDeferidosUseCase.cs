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
           
            var lista = param.UeId > 0 ? MapearParaDtoTurmas(encaminhamentos) : MapearParaDto(encaminhamentos);
            return lista.OrderBy(a => a.Ordem)
                .ThenBy(a => a.Descricao);
        }

        private IEnumerable<AEETurmaDto> MapearParaDto(IEnumerable<AEETurmaDto> encaminhamentos)
        {
            List<AEETurmaDto> retorno = new List<AEETurmaDto>();

            foreach (var encaminhamento in encaminhamentos.GroupBy(a => $"{a.Modalidade.ShortName()} - {a.AnoTurma}"))
            {
                retorno.Add(new AEETurmaDto()
                {
                    Modalidade = encaminhamento.FirstOrDefault().Modalidade,
                    Quantidade = encaminhamento.Sum(a => a.Quantidade),
                    AnoTurma = encaminhamento.FirstOrDefault().AnoTurma,
                    Descricao = encaminhamento.FirstOrDefault().AnoTurma != "0" ? $"{encaminhamento.FirstOrDefault().Modalidade.ShortName()} - {encaminhamento.FirstOrDefault().AnoTurma}" : encaminhamento.FirstOrDefault().Modalidade.ShortName(),
                });
            }

            return retorno;
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
