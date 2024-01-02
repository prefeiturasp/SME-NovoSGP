using MediatR;
using SME.SGP.Infra.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosAtribuicaoCJUseCase : AbstractUseCase, IObterAnosLetivosAtribuicaoCJUseCase
    {
        public ObterAnosLetivosAtribuicaoCJUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<int[]> Executar()
        {
            var codigoRf = await mediator.Send(ObterUsuarioLogadoRFQuery.Instance);

            var dto = new AtribuicoesPorTurmaEProfessorDto()
            {
                UsuarioRf = codigoRf,
                Substituir = true
            };

            var listaRetorno = (await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(dto))).ToList();

            if (!listaRetorno.Any())
                return null;

            var anosLetivos = listaRetorno.Select(a => a.Turma.AnoLetivo).Distinct().OrderByDescending(a => a);

            return anosLetivos.ToArray();
        }
    }
}
