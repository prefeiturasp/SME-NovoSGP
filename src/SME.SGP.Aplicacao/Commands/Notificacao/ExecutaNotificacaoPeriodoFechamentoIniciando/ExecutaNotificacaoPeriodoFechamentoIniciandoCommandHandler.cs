using MediatR;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoPeriodoFechamentoIniciandoCommandHandler : IRequestHandler<ExecutaNotificacaoPeriodoFechamentoIniciandoCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoPeriodoFechamentoIniciandoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoPeriodoFechamentoIniciandoCommand request, CancellationToken cancellationToken)
        {

            var funcionarios = await mediator.Send(new ObterFuncionariosQuery(request.PeriodoIniciandoBimestre.PeriodoFechamento.Dre.CodigoDre, request.PeriodoIniciandoBimestre.PeriodoFechamento.Ue.CodigoUe, "", ""));
            await EnviarNotificacoes(funcionarios, request.PeriodoIniciandoBimestre.PeriodoEscolar, request.PeriodoIniciandoBimestre, request.PeriodoIniciandoBimestre.PeriodoFechamento.Ue);
            return true;
        }

        private async Task EnviarNotificacoes(IEnumerable<UsuarioEolRetornoDto> funcionarios, PeriodoEscolar periodoEscolar, PeriodoFechamentoBimestre periodoFechamentoBimestre, Ue ue)
        {
            var titulo = $"Início do período de fechamento do {periodoEscolar.Bimestre}º bimestre - {ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var mensagem = @$"O fechamento do <b>{periodoEscolar.Bimestre}º bimestre</b> na <b>{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})</b> irá iniciar no dia <b>{periodoFechamentoBimestre.InicioDoFechamento.Date}</b>.";

            foreach (var funcionario in funcionarios)
                await mediator.Send(new NotificarUsuarioCommand(titulo, mensagem, funcionario.CodigoRf, NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario));

        }
    }
}
