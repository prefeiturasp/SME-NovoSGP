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
    public class ObterRegistroAcaoPorIdUseCase : IObterRegistroAcaoPorIdUseCase
    {
        private readonly IMediator mediator;

        public ObterRegistroAcaoPorIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RegistroAcaoBuscaAtivaRespostaDto> Executar(long id)
        {
            var registroAcao = await mediator.Send(new ObterRegistroAcaoComTurmaPorIdQuery(id));
            if(registroAcao.EhNulo())
                throw new NegocioException(MensagemNegocioRegistroAcao.REGISTROACAO_NAO_ENCONTRADO);

            var aluno = await ObterAlunoPorCodigoETurma(registroAcao.AlunoCodigo, registroAcao.Turma.CodigoTurma,registroAcao.Turma.AnoLetivo);
            var nomeUe = registroAcao.Turma.Ue.TipoEscola == TipoEscola.Nenhum ? registroAcao.Turma.Ue.Nome : 
                            $"{registroAcao.Turma.Ue.TipoEscola.ObterNomeCurto()} {registroAcao.Turma.Ue.Nome}";

            return new RegistroAcaoBuscaAtivaRespostaDto()
            {
                Aluno = aluno,
                
                DreId = registroAcao.Turma.Ue.Dre.Id,
                DreCodigo = registroAcao.Turma.Ue.Dre.CodigoDre,
                DreNome = registroAcao.Turma.Ue.Dre.Nome,
                
                UeId = registroAcao.Turma.Ue.Id,
                UeCodigo = registroAcao.Turma.Ue.CodigoUe,
                UeNome = nomeUe,
                
                TurmaId = registroAcao.Turma.Id,
                TurmaCodigo = registroAcao.Turma.CodigoTurma,
                TurmaNome = registroAcao.Turma.Nome,
                
                AnoLetivo = registroAcao.Turma.AnoLetivo,
                Modalidade = (int)registroAcao.Turma.ModalidadeCodigo
            };
        }
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private async Task<AlunoTurmaReduzidoDto> ObterAlunoPorCodigoETurma(string alunoCodigo, string turmaCodigo,int anoLetivo)
        {
            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turmaCodigo, alunoCodigo, true));
            if (aluno.EhNulo()) throw new NegocioException(MensagemNegocioEOL.NAO_LOCALIZADO_INFORMACOES_ALUNO_TURMA_EOL);           

            var frequencia = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(alunoCodigo, turmaCodigo));
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(new []{aluno.CodigoAluno}, anoLetivo);
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
    }
}
