using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase : AbstractUseCase, IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorio;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase(IMediator mediator, IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado) : base(mediator)
        {
            this.repositorio = repositorioConselhoClasseConsolidado;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidarTurmaConselhoClasseAlunoPorTurmaDto>();

            try
            {
                var alunoConsolidacoes = await repositorio.ObterConselhoClasseConsolidadoPorTurmaAsync(filtro.TurmaId);

                foreach (var alunoConsolidacao in alunoConsolidacoes)
                {
                    var mensagemPorAluno = new MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto(alunoConsolidacao.ConsolidacaoId, alunoConsolidacao.TurmaId, alunoConsolidacao.AlunoCodigo);

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoTratar, JsonConvert.SerializeObject(mensagemPorAluno), mensagemRabbit.CodigoCorrelacao, null));
                }
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno anos anteriores por turma.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
