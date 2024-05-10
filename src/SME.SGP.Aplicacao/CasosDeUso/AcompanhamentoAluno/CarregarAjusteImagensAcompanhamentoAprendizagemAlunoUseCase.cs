using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase : AbstractUseCase, ICarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase
    {
        private readonly IRepositorioAcompanhamentoAlunoSemestre repositorio;

        public CarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase(IMediator mediator, IRepositorioAcompanhamentoAlunoSemestre repositorio) : base(mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroPaginacaoAjusteImagensRAADto>();
            var pagina = filtro?.Pagina ?? 1;

            var registrosAjustar = await repositorio.ObterRAAsParaAjusteRota(pagina);
            if (!registrosAjustar.Any())
                return true;

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.AjusteImagesAcompanhamentoAprendizagemAlunoCarregar,
                                                           new FiltroPaginacaoAjusteImagensRAADto(++pagina),
                                                           Guid.NewGuid(),
                                                           null));

            foreach (var registroAcompanhamento in registrosAjustar)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.AjusteImagesAcompanhamentoAprendizagemAlunoSync,
                                                                new FiltroAjusteImagensRAADto(registroAcompanhamento),
                                                                Guid.NewGuid(),
                                                                null));

            return true;
        }
    }
}
