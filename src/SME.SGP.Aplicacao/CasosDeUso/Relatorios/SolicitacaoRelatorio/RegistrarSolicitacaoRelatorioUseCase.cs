using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio;
using SME.SGP.Dominio.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public class RegistrarSolicitacaoRelatorioUseCase : AbstractUseCase, IRegistrarSolicitacaoRelatorioUseCase
    {
        public RegistrarSolicitacaoRelatorioUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<long> Executar(SolicitacaoRelatorioDto solicitacaoRelatorio)
        {
            var solicitacaoRelatorioId = await mediator.Send(new InserirSolicitacaoRelatorioCommand(solicitacaoRelatorio));
            await mediator.Send(new InserirCodigoCorrelacaoCommand(solicitacaoRelatorio.CodigoCorrelacao, solicitacaoRelatorio.UsuarioQueSolicitou, solicitacaoRelatorio.TipoRelatorio, solicitacaoRelatorio.ExtensaoRelatorio));

            return solicitacaoRelatorioId;
        }
    }
}