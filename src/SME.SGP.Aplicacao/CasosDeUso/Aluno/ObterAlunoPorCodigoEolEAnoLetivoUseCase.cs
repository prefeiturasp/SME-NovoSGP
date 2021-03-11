using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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

            if (alunoPorTurmaResposta == null)
                throw new NegocioException("Aluno não localizado");

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                NumeroAlunoChamada = alunoPorTurmaResposta.NumeroAlunoChamada,
                DataNascimento = alunoPorTurmaResposta.DataNascimento,
                DataSituacao = alunoPorTurmaResposta.DataSituacao,
                CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                Situacao = alunoPorTurmaResposta.SituacaoMatricula,
                TurmaEscola = await OberterNomeTurmaFormatado(alunoPorTurmaResposta.CodigoTurma.ToString()),
                NomeResponsavel = alunoPorTurmaResposta.NomeResponsavel,
                TipoResponsavel = alunoPorTurmaResposta.TipoResponsavel,
                CelularResponsavel = alunoPorTurmaResposta.CelularResponsavel,
                DataAtualizacaoContato = alunoPorTurmaResposta.DataAtualizacaoContato,
                EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(alunoPorTurmaResposta.CodigoAluno, anoLetivo))
            };

            return alunoReduzido;
        }

        private async Task<string> OberterNomeTurmaFormatado(string turmaId)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

            if (turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }

    }
}
