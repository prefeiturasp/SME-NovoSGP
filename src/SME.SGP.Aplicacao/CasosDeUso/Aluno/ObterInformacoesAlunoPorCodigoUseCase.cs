using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesAlunoPorCodigoUseCase : AbstractUseCase, IObterInformacoesAlunoPorCodigoUseCase
    {
        public ObterInformacoesAlunoPorCodigoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AlunoEnderecoRespostaDto> Executar(string codigoAluno)
        {
            return await mediator.Send(new ObterAlunoEnderecoEolQuery(codigoAluno));
        }
    }
}
