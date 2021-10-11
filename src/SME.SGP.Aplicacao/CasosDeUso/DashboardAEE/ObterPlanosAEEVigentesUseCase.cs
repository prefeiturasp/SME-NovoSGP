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

            var lista = param.UeId > 0 ? MapearParaDtoTurmas(planos) : MapearParaDto(planos);
            return lista.OrderBy(a => a.Ordem)
                .ThenBy(a => a.Descricao);
        }

        private IEnumerable<AEETurmaDto> MapearParaDto(IEnumerable<AEETurmaDto> planos)
        {
            List<AEETurmaDto> retorno = new List<AEETurmaDto>();

            foreach(var plano in planos.GroupBy(a=> $"{a.Modalidade.ShortName()} - {a.AnoTurma}"))
            {
                retorno.Add(new AEETurmaDto()
                {
                    Modalidade = plano.FirstOrDefault().Modalidade,
                    Quantidade = plano.Sum(a => a.Quantidade),
                    AnoTurma = plano.FirstOrDefault().AnoTurma,
                    Descricao = plano.FirstOrDefault().AnoTurma != "0" ? $"{plano.FirstOrDefault().Modalidade.ShortName()} - {plano.FirstOrDefault().AnoTurma}" : plano.FirstOrDefault().Modalidade.ShortName(),
                });
            }

            return retorno;
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
