using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEAnoPlanoAeeQueryHandler : IRequestHandler<ObterAlunoPorCodigoEAnoPlanoAeeQuery, AlunoReduzidoDto>
    {
        private readonly IMediator mediator;

        public ObterAlunoPorCodigoEAnoPlanoAeeQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AlunoReduzidoDto> Handle(ObterAlunoPorCodigoEAnoPlanoAeeQuery request, CancellationToken cancellationToken)
        {
            var alunoPorTurmaResposta = (await mediator.Send(new ObterTurmasAlunoPorFiltroPlanoAeeQuery(request.CodigoAluno, request.AnoLetivo, false, request.TipoTurma), cancellationToken))
                                                       .OrderByDescending(a => a.DataSituacao).ThenByDescending(a => a.NumeroAlunoChamada)?.FirstOrDefault();

            if (alunoPorTurmaResposta == null)
                return default;

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta?.NomeAluno) ? alunoPorTurmaResposta?.NomeAluno : alunoPorTurmaResposta?.NomeSocialAluno,
                NumeroAlunoChamada = alunoPorTurmaResposta.ObterNumeroAlunoChamada(),
                DataNascimento = alunoPorTurmaResposta.DataNascimento,
                DataSituacao = alunoPorTurmaResposta.DataSituacao,
                CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                CodigoSituacaoMatricula = alunoPorTurmaResposta.CodigoSituacaoMatricula,
                Situacao = alunoPorTurmaResposta.SituacaoMatricula,               
                TurmaEscola = await ObterNomeTurmaFormatado(alunoPorTurmaResposta.CodigoTurma.ToString()),
                CodigoTurma = alunoPorTurmaResposta.CodigoTurma.ToString(),
                CelularResponsavel = alunoPorTurmaResposta.CelularResponsavel,
                NomeResponsavel = alunoPorTurmaResposta.NomeResponsavel,
                DataAtualizacaoContato = alunoPorTurmaResposta.DataAtualizacaoContato,
                TipoResponsavel = alunoPorTurmaResposta.TipoResponsavel
            };

            return alunoReduzido;

        }
        
        private async Task<string> ObterNomeTurmaFormatado(string turmaCodigo)
        {
            var turmaNome = string.Empty;
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

            if (turma.NaoEhNulo())
            {
                var nomeTurno = string.Empty;
                if (Enum.IsDefined(typeof(TipoTurnoEOL), turma.TipoTurno))
                {
                    var tipoTurno = (TipoTurnoEOL)turma.TipoTurno;
                    nomeTurno = $"- {tipoTurno.GetAttribute<DisplayAttribute>()?.GetName()}";
                }
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} {nomeTurno}";
            }

            return turmaNome;
        }
    }
}