using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralPorTurmaSemestreAlunoUseCase
    {
        public static async Task<RelatorioSemestralAlunoDto> Executar(IMediator mediator, string alunoCodigo, string turmaCodigo, int semestre)
        {            
            var relatorio = await mediator.Send(new ObterRelatorioSemestralPorTurmaSemestreAlunoQuery(alunoCodigo, turmaCodigo, semestre));

            return relatorio;
        }
    }
}
