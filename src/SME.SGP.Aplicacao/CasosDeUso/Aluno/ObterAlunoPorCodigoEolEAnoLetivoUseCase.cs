using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEolEAnoLetivoUseCase : AbstractUseCase, IObterAlunoPorCodigoEolEAnoLetivoUseCase
    {
        public ObterAlunoPorCodigoEolEAnoLetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AlunoReduzidoDto> Executar(string codigoAluno, int anoLetivo)
        {            
            var alunoPorTurmaResposta = await mediator.Send(new ObterAlunoPorCodigoEolQuery(codigoAluno, anoLetivo));

            var alunoReduzido = new AlunoReduzidoDto()
            {
                NomeAluno = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : "",
                NumeroAlunoChamada = alunoPorTurmaResposta.NumeroAlunoChamada,
                DataNascimento = alunoPorTurmaResposta.DataNascimento.ToString("dd/MM/yyyy"),
                CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                SituacaoMatricula = $@"{alunoPorTurmaResposta.SituacaoMatricula} em {alunoPorTurmaResposta.DataSituacao.ToString("dd/MM/yyyy")}",
                TurmaEscola = await OberterNomeTurmaFormatado(alunoPorTurmaResposta.CodigoTurma)
            };

            return alunoReduzido;
        }

        private async Task<string> OberterNomeTurmaFormatado(long turmaId)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

            if(turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }

    }
}
