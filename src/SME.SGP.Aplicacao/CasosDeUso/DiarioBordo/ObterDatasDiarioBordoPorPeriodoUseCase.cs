using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasDiarioBordoPorPeriodoUseCase : AbstractUseCase, IObterDatasDiarioBordoPorPeriodoUseCase
    {
        public ObterDatasDiarioBordoPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> Executar(string turmaCodigo, DateTime dataInicio, DateTime dataFim, long componenteCurricularId)
        {
            var retorno = new List<DiarioBordoPorPeriodoDto>();

            var componentePai = await mediator.Send(new ObterCodigoComponentePaiQuery(componenteCurricularId));
            var todasAulasPeriodo = await mediator.Send(new ObterAulasPorDataPeriodoQuery(dataInicio, dataFim, turmaCodigo, componentePai));
            var datasComDiarioBordo = await mediator.Send(new ObterDatasDiarioBordoPorPeriodoQuery(turmaCodigo, dataInicio, dataFim, componenteCurricularId));

            foreach(var aula in todasAulasPeriodo)
            {
                var dadosDiarioBordo = datasComDiarioBordo.Where(d => d.DataAula.Equals(aula.DataAula)).FirstOrDefault();

                if (dadosDiarioBordo != null)
                {
                    var dadosAula = await mediator.Send(new ObterAulaPorIdQuery(dadosDiarioBordo.AulaId));
                    var diarioBordo = await mediator.Send(new ObterDiarioBordoPorAulaIdQuery(dadosDiarioBordo.AulaId, componenteCurricularId));
                    dadosDiarioBordo.Titulo = dadosAula.TipoAula == TipoAula.Reposicao ? $"{aula.DataAula.ToString("dd/MM/yyyy")} - Reposição" : $"{aula.DataAula.ToString("dd/MM/yyyy")}";
                    dadosDiarioBordo.Auditoria = (AuditoriaDto)diarioBordo;
                    dadosDiarioBordo.Pendente = false;
                    retorno.Add(dadosDiarioBordo);
                }      
                else
                {
                    retorno.Add(new DiarioBordoPorPeriodoDto()
                    {
                        AulaId = aula.Id,
                        DiarioBordoId = null,
                        DataAula = aula.DataAula,
                        Pendente = true,
                        Titulo = aula.TipoAula == TipoAula.Reposicao ? $"{aula.DataAula.ToString("dd/MM/yyyy")} - Reposição" : $"{aula.DataAula.ToString("dd/MM/yyyy")}",
                        Planejamento = "",
                        ReflexoesReplanejamento = "",
                        Auditoria = null
                    });
                }
            }

            return retorno.OrderByDescending(a=> a.DataAula).Distinct();
        }
    }
}
