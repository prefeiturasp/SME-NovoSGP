using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterParametroSistemaPorTipoEAnoQueryHanlerFake : IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>
    {
        public async Task<ParametrosSistema> Handle(ObterParametroSistemaPorTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            return new ParametrosSistema()
            {
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "GerarPendenciasPlanoAEE",
                Descricao = "Controle de geração de pendências para os processos do Plano AEE",
                Valor = string.Empty,
                Ativo = true,
                Tipo = TipoParametroSistema.GerarPendenciasPlanoAEE
            };
        }
    }
}