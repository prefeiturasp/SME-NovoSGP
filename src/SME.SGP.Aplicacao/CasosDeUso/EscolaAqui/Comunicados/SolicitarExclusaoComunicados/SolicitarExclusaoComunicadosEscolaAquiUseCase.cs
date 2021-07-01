using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarExclusaoComunicadosEscolaAquiUseCase : AbstractUseCase, ISolicitarExclusaoComunicadosEscolaAquiUseCase
    {
        public SolicitarExclusaoComunicadosEscolaAquiUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<string> Executar(long[] ids)
        {
            var retorno = await mediator.Send(new ExcluirComunicadoCommand(ids));

            if (retorno)
                return "Comunicado excluído com sucesso";
            else
                return "Erro na exclusão do Comunicado";
        }
    }
}
