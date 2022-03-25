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
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase : AbstractUseCase, IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorio;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase(IMediator mediator, IRepositorioFechamentoNotaConsulta repositorioFechamentoNotaConsulta) : base(mediator)
        {
            this.repositorio = repositorioFechamentoNotaConsulta;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto>();

            try
            {
                var alunoNotas = await repositorio.ObterFechamentoNotaAlunoAsync(filtro.TurmaId, filtro.AlunoCodigo);

                var migracaoConsolidacaoTurmaCommand = new ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand()
                {
                    ConsolidacaoId = filtro.ConsolidacaoId,
                    AlunoNotas = alunoNotas,
                };
                await mediator.Send(migracaoConsolidacaoTurmaCommand);
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno anos anteriores por aluno.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
