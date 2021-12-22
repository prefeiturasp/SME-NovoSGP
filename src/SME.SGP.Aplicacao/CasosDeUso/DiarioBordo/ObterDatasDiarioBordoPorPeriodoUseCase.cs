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

            foreach(var aula in todasAulasPeriodo.GroupBy(t=> t.DataAula))
            {
                var itensAula = aula.FirstOrDefault();
                var dadosDiarioBordo = datasComDiarioBordo.Where(d => d.DataAula.Equals(itensAula.DataAula)).FirstOrDefault();

                if (dadosDiarioBordo != null)
                {
                    var dadosAula = await mediator.Send(new ObterAulaPorIdQuery(dadosDiarioBordo.AulaId));
                    dadosDiarioBordo.Titulo = dadosAula.TipoAula == TipoAula.Reposicao ? $"{itensAula.DataAula.ToString("dd/MM/yyyy")} - Reposição" : $"{itensAula.DataAula.ToString("dd/MM/yyyy")}";
                    dadosDiarioBordo.Auditoria = (AuditoriaDto)dadosAula;
                    retorno.Add(dadosDiarioBordo);
                }      
                else
                {
                    retorno.Add(new DiarioBordoPorPeriodoDto()
                    {
                        AulaId = itensAula.Id,
                        DiarioBordoId = null,
                        DataAula = itensAula.DataAula,
                        Pendente = true,
                        Titulo = itensAula.TipoAula == TipoAula.Reposicao ? $"{itensAula.DataAula.ToString("dd/MM/yyyy")} - Reposição" : $"{itensAula.DataAula.ToString("dd/MM/yyyy")}",
                        Planejamento = "",
                        ReflexoesReplanejamento = "",
                        Auditoria = null
                    });
                }
            }

            return retorno.OrderByDescending(a=> a.DataAula);
        }
    }
}
