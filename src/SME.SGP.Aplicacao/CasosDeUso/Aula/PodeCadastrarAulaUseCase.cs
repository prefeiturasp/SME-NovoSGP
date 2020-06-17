using SME.SGP.Infra;
using System.Threading.Tasks;
using MediatR;
using System;
using SME.SGP.Dominio;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public static class PodeCadastrarAulaUseCase
    {
        public static async Task<CadastroAulaDto> Executar(IMediator mediator, long aulaId, string turmaCodigo, long componenteCurricular, DateTime dataAula, bool ehRegencia = false)
        {

            if ((aulaId == 0) || (dataAula != await mediator.Send(new ObterDataAulaQuery(aulaId))))
                if (!await mediator.Send(new PodeCadastrarAulaNoDiaQuery(dataAula, turmaCodigo, componenteCurricular)))
                    throw new NegocioException("Não é possível cadastrar aula pois já existe aula cadastrada no dia para esse componente curricular!");

            return new CadastroAulaDto()
            {
                PodeCadastrarAula = true,
                Grade = await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, componenteCurricular, dataAula, ehRegencia: ehRegencia))
            };
        }
    }
}
