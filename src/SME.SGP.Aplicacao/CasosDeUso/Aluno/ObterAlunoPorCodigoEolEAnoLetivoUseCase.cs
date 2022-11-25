using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEolEAnoLetivoUseCase : AbstractUseCase, IObterAlunoPorCodigoEolEAnoLetivoUseCase
    {
        public ObterAlunoPorCodigoEolEAnoLetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AlunoReduzidoDto> Executar(string codigoAluno, int anoLetivo, string codigoTurma)
        {
            var alunoPorTurmaResposta = await mediator.Send(new ObterAlunoPorCodigoEolQuery(codigoAluno, anoLetivo, false, true, codigoTurma));

            if (alunoPorTurmaResposta == null)
                throw new NegocioException("Aluno não localizado");

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                NumeroAlunoChamada = alunoPorTurmaResposta.ObterNumeroAlunoChamada(),
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
            {
                TipoTurno? tipoTurno = (TipoTurno)turma.TipoTurno;
                var nomeTurno = tipoTurno != null ? $"- {tipoTurno.GetAttribute<DisplayAttribute>()?.GetName()}" : "";
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} {nomeTurno}";
            }

            return turmaNome;
        }

    }
}
