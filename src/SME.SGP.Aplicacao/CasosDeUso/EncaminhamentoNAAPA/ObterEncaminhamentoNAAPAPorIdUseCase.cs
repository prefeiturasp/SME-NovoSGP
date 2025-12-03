using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
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

        public async Task<AtendimentoNAAPARespostaDto> Executar(long id)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAComTurmaPorIdQuery(id));

            if(encaminhamentoNAAPA.EhNulo())
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            var aluno = await ObterAlunoPorCodigoETurma(encaminhamentoNAAPA.AlunoCodigo, encaminhamentoNAAPA.Turma.CodigoTurma,encaminhamentoNAAPA.Turma.AnoLetivo);
            var nomeUe = encaminhamentoNAAPA.Turma.Ue.TipoEscola == TipoEscola.Nenhum ? encaminhamentoNAAPA.Turma.Ue.Nome : 
                            $"{encaminhamentoNAAPA.Turma.Ue.TipoEscola.ObterNomeCurto()} {encaminhamentoNAAPA.Turma.Ue.Nome}";

            if(encaminhamentoNAAPA.Situacao == Dominio.Enumerados.SituacaoNAAPA.AguardandoAtendimento)
                await VerificaSeEstaEmAguardandoAtendimentoIndevidamente(encaminhamentoNAAPA);

            return new AtendimentoNAAPARespostaDto()
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
                Modalidade = (int)encaminhamentoNAAPA.Turma.ModalidadeCodigo,
                MotivoEncerramento = encaminhamentoNAAPA.MotivoEncerramento
            };
        }
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private async Task<AlunoTurmaReduzidoDto> ObterAlunoPorCodigoETurma(string alunoCodigo, string turmaCodigo,int anoLetivo)
        {
            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(alunoCodigo, anoLetivo, false, true, turmaCodigo));
            if (aluno.EhNulo()) throw new NegocioException(MensagemNegocioEOL.NAO_LOCALIZADO_INFORMACOES_ALUNO_TURMA_EOL);           

            var frequencia = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(alunoCodigo, turmaCodigo));
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(new []{aluno.CodigoAluno}, anoLetivo);
            var alunoReduzido = new AlunoTurmaReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(aluno.NomeAluno) ? aluno.NomeAluno : aluno.NomeSocialAluno,
                NumeroAlunoChamada = aluno.ObterNumeroAlunoChamada(),
                DataNascimento = aluno.DataNascimento,
                DataSituacao = aluno.DataSituacao,
                Idade = aluno.Idade,
                DocumentoCpf = aluno.DocumentoCpf,
                CodigoAluno = aluno.CodigoAluno,
                CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                Situacao = aluno.SituacaoMatricula,
                TurmaEscola = await ObterNomeTurmaFormatado(aluno.CodigoTurma.ToString()),
                CodigoTurma = aluno.CodigoTurma.ToString(),
                CelularResponsavel = aluno.CelularResponsavel,
                NomeResponsavel = aluno.NomeResponsavel,
                DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                TipoResponsavel = aluno.TipoResponsavel,
                Frequencia = frequencia,
                EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
            };
            return alunoReduzido;
        }

        private async Task<string> ObterNomeTurmaFormatado(string turmaCodigo)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

            if (turma.NaoEhNulo())
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

        public async Task VerificaSeEstaEmAguardandoAtendimentoIndevidamente(EncaminhamentoNAAPA encaminhamento)
        {
            bool necessitaAlteracao = await mediator.Send(new VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery(encaminhamento.Id));
            if (necessitaAlteracao)
                await mediator.Send(new AlterarSituacaoNAAPACommand(encaminhamento, Dominio.Enumerados.SituacaoNAAPA.EmAtendimento));        
        }
    }
}
