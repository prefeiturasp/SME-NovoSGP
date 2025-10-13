using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesFrequenciaPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesFrequenciaPainelEducacionalUseCase
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ConsolidarInformacoesFrequenciaPainelEducacionalUseCase(IMediator mediator, IRepositorioFrequenciaConsulta repositorioFrequencia) : base(mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registrosFrequencia = await repositorioFrequencia.ObterInformacoesFrequenciaPainelEducacional(DateTime.Now.Year);



            //foreach (var regFrequencia in registrosFrequencia)
            //    await mediator.Send(new SalvarConsolidacaoProdutividadeFrequenciaCommand(regFrequencia.ToEntity()));
            return true;
        }
    }
}
