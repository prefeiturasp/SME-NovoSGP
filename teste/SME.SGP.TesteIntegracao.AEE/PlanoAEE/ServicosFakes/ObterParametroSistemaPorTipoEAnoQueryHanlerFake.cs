using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterParametroSistemaPorTipoEAnoQueryHanlerFake : IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>
    {
        public Task<ParametrosSistema> Handle(ObterParametroSistemaPorTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ParametrosSistema()
            {
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "GerarPendenciasPlanoAEE",
                Descricao = "Controle de geração de pendências para os processos do Plano AEE",
                Valor = string.Empty,
                Ativo = true,
                Tipo = TipoParametroSistema.GerarPendenciasPlanoAEE
            });
        }
    }
}