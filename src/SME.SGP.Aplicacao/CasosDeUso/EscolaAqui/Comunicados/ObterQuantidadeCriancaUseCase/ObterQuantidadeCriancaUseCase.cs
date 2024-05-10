using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeCriancaUseCase : AbstractUseCase, IObterQuantidadeCriancaUseCase
    {
        public ObterQuantidadeCriancaUseCase(IMediator mediator) : base(mediator) { }

        public async Task<QuantidadeCriancaDto> Executar(int anoLetivo, string[] turma, string dreId, string ueId, int[] modalidade, string[] anoTurma)
        {
            var quantidade = await mediator.Send(new ObterQuantidadeCriancaQuery(anoLetivo,turma,dreId,ueId,modalidade,anoTurma));

            if (quantidade.EhNulo())
                return default;

            return quantidade;
        }
    }
}
