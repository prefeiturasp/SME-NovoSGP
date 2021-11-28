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

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registrosAjustar = await repositorio.ObterImagensParaAjusteRota();

            Parallel.ForEach(registrosAjustar.GroupBy(a => a.Id),
                new ParallelOptions { MaxDegreeOfParallelism = 10 },
                async registroAcompanhamento => 
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.AjusteImagesAcompanhamentoAprendizagemAlunoSync,
                                                                   registroAcompanhamento.AsEnumerable(),
                                                                   Guid.NewGuid(),
                                                                   null)));

            return true;
        }
    }
}
