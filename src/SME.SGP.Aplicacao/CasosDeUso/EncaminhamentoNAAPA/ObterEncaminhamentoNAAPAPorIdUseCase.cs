using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAPorIdUseCase : IObterEncaminhamentoNAAPAPorIdUseCase
    {
        private readonly IMediator mediator;

        public ObterEncaminhamentoNAAPAPorIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<EncaminhamentoNAAPARespostaDto> Executar(long id)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAComTurmaPorIdQuery(id));

            if(encaminhamentoNAAPA == null)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            var aluno = await ObterAlunoPorCodigoETurma(encaminhamentoNAAPA.AlunoCodigo, encaminhamentoNAAPA.Turma.CodigoTurma);
            var nomeUe = encaminhamentoNAAPA.Turma.Ue.TipoEscola == TipoEscola.Nenhum ? encaminhamentoNAAPA.Turma.Ue.Nome : 
                            $"{encaminhamentoNAAPA.Turma.Ue.TipoEscola.ObterNomeCurto()} {encaminhamentoNAAPA.Turma.Ue.Nome}";

            return new EncaminhamentoNAAPARespostaDto()
            {
                Aluno = aluno,
                
                DreId = encaminhamentoNAAPA.Turma.Ue.Dre.Id,
                DreCodigo = encaminhamentoNAAPA.Turma.Ue.Dre.CodigoDre,
                DreNome = encaminhamentoNAAPA.Turma.Ue.Dre.Nome,
                
                UeId = encaminhamentoNAAPA.Turma.Ue.Id,
                UeCodigo = encaminhamentoNAAPA.Turma.Ue.CodigoUe,
                UeNome = nomeUe,
                
                TurmaId = encaminhamentoNAAPA.Turma.Id,
                TurmaCodigo = encaminhamentoNAAPA.Turma.CodigoTurma,
                TurmaNome = encaminhamentoNAAPA.Turma.Nome,
                
                AnoLetivo = encaminhamentoNAAPA.Turma.AnoLetivo,
                Situacao = (int)encaminhamentoNAAPA.Situacao,
                DescricaoSituacao = encaminhamentoNAAPA.Situacao.Name(),
                Modalidade = (int)encaminhamentoNAAPA.Turma.ModalidadeCodigo
            };
        }

        private async Task<AlunoTurmaReduzidoDto> ObterAlunoPorCodigoETurma(string alunoCodigo, string turmaCodigo)
        {
            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turmaCodigo, alunoCodigo, true));
            var frequencia = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(alunoCodigo, turmaCodigo));
            var alunoReduzido = new AlunoTurmaReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(aluno.NomeAluno) ? aluno.NomeAluno : aluno.NomeSocialAluno,
                NumeroAlunoChamada = aluno.ObterNumeroAlunoChamada(),
                DataNascimento = aluno.DataNascimento,
                DataSituacao = aluno.DataSituacao,
                CodigoAluno = aluno.CodigoAluno,
                CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                Situacao = aluno.SituacaoMatricula,
                TurmaEscola = await ObterNomeTurmaFormatado(aluno.CodigoTurma.ToString()),
                CodigoTurma = aluno.CodigoTurma.ToString(),
                CelularResponsavel = aluno.CelularResponsavel,
                NomeResponsavel = aluno.NomeResponsavel,
                DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                TipoResponsavel = aluno.TipoResponsavel,
                Frequencia = frequencia
            };
            return alunoReduzido;
        }

        private async Task<string> ObterNomeTurmaFormatado(string turmaCodigo)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

            if (turma != null)
            {
                var nomeTurno = "";
                if (Enum.IsDefined(typeof(TipoTurnoEOL), turma.TipoTurno))
                {
                    TipoTurnoEOL tipoTurno = (TipoTurnoEOL)turma.TipoTurno;
                    nomeTurno = $"- {tipoTurno.GetAttribute<DisplayAttribute>()?.GetName()}";
                }
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} {nomeTurno}";
            }

            return turmaNome;
        }
    }
}
