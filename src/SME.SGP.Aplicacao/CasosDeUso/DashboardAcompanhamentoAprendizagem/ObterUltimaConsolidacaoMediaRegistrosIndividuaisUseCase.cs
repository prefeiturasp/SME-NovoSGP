using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase : AbstractUseCase, IObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase
    {
        public ObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DateTime?> Executar(int anoLetivo)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma, anoLetivo));
            if (!string.IsNullOrEmpty(parametroSistema.Valor))
                return DateTime.Parse(parametroSistema.Valor);
            return null;
        }
    }
}
