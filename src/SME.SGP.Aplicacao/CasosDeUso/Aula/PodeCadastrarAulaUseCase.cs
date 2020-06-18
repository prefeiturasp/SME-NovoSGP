using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public static class PodeCadastrarAulaUseCase
    {
        public static async Task<CadastroAulaDto> Executar(IMediator mediator, long aulaId, string turmaCodigo, long componenteCurricular, DateTime dataAula, bool ehRegencia = false)
        {
            bool podeCadastrarAula;

            if (aulaId > 0)
            {
                var dataAulaExistente = await mediator.Send(new ObterDataAulaQuery(aulaId));
                podeCadastrarAula = dataAula.Date == dataAulaExistente.Date;
            }
            else
                podeCadastrarAula = await mediator.Send(new PodeCadastrarAulaNoDiaQuery(dataAula, turmaCodigo, componenteCurricular));
             
            if (!podeCadastrarAula)
                    throw new NegocioException("Não é possível cadastrar aula pois já existe aula cadastrada no dia para esse componente curricular!");

            return new CadastroAulaDto()
            {
                PodeCadastrarAula = true,
                Grade = await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, componenteCurricular, dataAula, ehRegencia: ehRegencia))
            };
        }
    }
}
