using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorTurmaComponenteDataUseCase: AbstractUseCase, IObterAulasPorTurmaComponenteDataUseCase
    {
        public ObterAulasPorTurmaComponenteDataUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<IEnumerable<AulaQuantidadeTipoDto>> Executar(FiltroObterAulasPorTurmaComponenteDataDto filtro)
        {
            var aulas = await mediator.Send(new ObterAulasPorDataTurmaComponenteCurricularQuery(filtro.DataAula,filtro.TurmaCodigo, filtro.ComponenteCurricular));
            
            return  aulas.Select(s=> new AulaQuantidadeTipoDto
            {
                Id = s.Id,
                Quantidade = s.Quantidade,
                Tipo = (int)s.TipoAula,
                EhCj = s.AulaCJ
            });
        }

        private async Task<bool> AlterandoDataAula(long aulaId, DateTime dataAula)
        {
            var dataOriginalAula = await mediator.Send(new ObterDataAulaQuery(aulaId));
            return dataAula != dataOriginalAula;
        }

        private static bool CriandoAula(long aulaId)
            => aulaId == 0;
    }
}
