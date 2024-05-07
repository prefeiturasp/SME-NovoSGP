using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CopiarRelatorioPAPUseCase : AbstractUseCase, ICopiarRelatorioPAPUseCase
    {
        public CopiarRelatorioPAPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ResultadoRelatorioPAPDto> Executar(CopiarPapDto copiarPapDto)
        {
            var retorno = new ResultadoRelatorioPAPDto();
            //Buscar o relatorio Pap
            //Obter as questoes com respostas selecionadas para copia
            //Montar RelatorioPAPDto por aluno e verificando se o aluno ja possui relatório caso positivo deve ser alterado os dados.
            /*
             *  ISalvarRelatorioPAPUseCase
                IObterSecoesPAPUseCase
                IObterQuestionarioPAPUseCase
             */


            return retorno;
        }
    }
}