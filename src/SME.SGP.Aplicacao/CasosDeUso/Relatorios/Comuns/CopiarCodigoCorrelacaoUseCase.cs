using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CopiarCodigoCorrelacaoUseCase : ICopiarCodigoCorrelacaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio;

        public CopiarCodigoCorrelacaoUseCase(IMediator mediator, IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio ?? throw new ArgumentNullException(nameof(repositorioCorrelacaoRelatorio));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            var relatorioCorrelacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(mensagemRabbit.CodigoCorrelacao));
            if (relatorioCorrelacao == null)
                throw new NegocioException("Não foi possível obter a correlação para copiar.");


            var novoRelatorioCorrelacao = (RelatorioCorrelacao)relatorioCorrelacao.Clone();
            novoRelatorioCorrelacao.Id = 0;
            novoRelatorioCorrelacao.Codigo = Guid.Parse(mensagemRabbit.Mensagem.ToString());

            await repositorioCorrelacaoRelatorio.SalvarAsync(novoRelatorioCorrelacao);

            return await Task.FromResult(true);
        }


    }
}
