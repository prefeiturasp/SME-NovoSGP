using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase : AbstractUseCase, INotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase
    {
        public NotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<AtendimentoNAAPADto>();
            var notificacoesEnviadas = false;
            
            var alunosEol = (await mediator.Send(new ObterAlunosEolPorCodigosQuery(long.Parse(encaminhamentoNAAPADto.AlunoCodigo), true))).ToList();
            var matriculaVigenteAluno = FiltrarMatriculaVigenteAluno(alunosEol);
            var ultimaMatriculaAluno = FiltrarUltimaMatriculaAluno(alunosEol);

            if ((int)(encaminhamentoNAAPADto.SituacaoMatriculaAluno ?? 0) != 
                matriculaVigenteAluno.CodigoSituacaoMatricula)
            {
                if (UltimaMatriculaAlunoInativa(ultimaMatriculaAluno))
                {
                    var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(ultimaMatriculaAluno.CodigoTurma.ToString()));
                    await NotificarResponsaveisSobreInativacaoAluno(encaminhamentoNAAPADto.AlunoNome, encaminhamentoNAAPADto.AlunoCodigo,
                                                                    turma, ultimaMatriculaAluno.SituacaoMatricula);
                    notificacoesEnviadas = true;
                }
             
                await AtualizarEncaminhamento(encaminhamentoNAAPADto.Id ?? 0, matriculaVigenteAluno.CodigoSituacaoMatricula);
            }
            return notificacoesEnviadas;

        }

        private TurmasDoAlunoDto FiltrarMatriculaVigenteAluno(List<TurmasDoAlunoDto> matriculasAluno)
        {
            return matriculasAluno.Where(turma => turma.CodigoTipoTurma == (int)TipoTurma.Regular
                                                     && turma.AnoLetivo <= DateTimeExtension.HorarioBrasilia().Year
                                                     && turma.DataSituacao.Date <= DateTimeExtension.HorarioBrasilia().Date)
                                     .OrderByDescending(turma => turma.AnoLetivo)
                                     .ThenByDescending(turma => turma.DataSituacao)
                                     .FirstOrDefault();
        }

        private TurmasDoAlunoDto FiltrarUltimaMatriculaAluno(List<TurmasDoAlunoDto> matriculasAluno)
        {
            return matriculasAluno.Where(turma => turma.CodigoTipoTurma == (int)TipoTurma.Regular)
                                      .OrderByDescending(turma => turma.AnoLetivo)
                                      .ThenByDescending(turma => turma.DataSituacao)
                                      .FirstOrDefault();
        }

        private bool UltimaMatriculaAlunoInativa(TurmasDoAlunoDto matriculaAluno)
        {
            var situcoesMatriculasInativas = new int[] { (int)SituacaoMatriculaAluno.Concluido, (int)SituacaoMatriculaAluno.Desistente,
                                                        (int)SituacaoMatriculaAluno.VinculoIndevido, (int)SituacaoMatriculaAluno.Falecido,
                                                        (int)SituacaoMatriculaAluno.NaoCompareceu, (int)SituacaoMatriculaAluno.Deslocamento };

            return (situcoesMatriculasInativas.Contains(matriculaAluno.CodigoSituacaoMatricula)
                    && matriculaAluno.AnoLetivo <= DateTimeExtension.HorarioBrasilia().Year
                    && matriculaAluno.DataSituacao.Date <= DateTimeExtension.HorarioBrasilia().Date);
        }

        private async Task AtualizarEncaminhamento(long encaminhamentoNAAPAId, int codigoSituacaoMatriculaAluno)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterCabecalhoAtendimentoNAAPAQuery(encaminhamentoNAAPAId));
            encaminhamentoNAAPA.SituacaoMatriculaAluno = (SituacaoMatriculaAluno)codigoSituacaoMatriculaAluno;
            await mediator.Send(new SalvarAtendimentoNAAPACommand(encaminhamentoNAAPA));
        }

        private async Task NotificarResponsaveisSobreInativacaoAluno(string nomeAluno, string codigoAluno, Turma turma, string situacaoMatriculaAluno)
        {
            var titulo = $"Criança/Estudante inativa - {nomeAluno}({codigoAluno})";
            var mensagem = $"A criança/estudante {nomeAluno}({codigoAluno}) que está em acompanhamento pelo NAAPA da {turma.Ue.Dre.Abreviacao} e estava matriculado na turma {turma.NomeComModalidade()} na {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} teve a sua situação alterada para \"{situacaoMatriculaAluno}\" e não possui outras matrículas válidas na rede municipal de educação. O seu encaminhamento junto a esta DRE deverá ser encerrado.";

            var responsaveisNotificados = await RetornarReponsaveisDreUe(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe);

            foreach (var responsavel in responsaveisNotificados)
            {
                await mediator.Send(new NotificarUsuarioCommand(titulo,
                                                                mensagem,
                                                                responsavel.Login,
                                                                NotificacaoCategoria.Aviso,
                                                                NotificacaoTipo.NAAPA));
            }

        }

        private async Task<IEnumerable<FuncionarioUnidadeDto>> RetornarReponsaveisDreUe(string codigoDre, string codigoUe) 
        {
            var perfisDre = new Guid[] { Perfis.PERFIL_COORDENADOR_NAAPA };
            var tiposAtribuicaoUe = new TipoResponsavelAtribuicao[] { TipoResponsavelAtribuicao.Psicopedagogo,
                                        TipoResponsavelAtribuicao.PsicologoEscolar,
                                        TipoResponsavelAtribuicao.AssistenteSocial };

            var responsaveisDre = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoUe, perfisDre))).ToList();
            if (!responsaveisDre.Any())
                responsaveisDre = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, perfisDre))).ToList();

            var responsaveisUe = (await mediator.Send(new ObterResponsaveisAtribuidosUePorDreUeTiposQuery(codigoDre, codigoUe, tiposAtribuicaoUe)))
                                .Select(atribuicaoResponsavel => new FuncionarioUnidadeDto()
                                {
                                    Login = atribuicaoResponsavel.SupervisorId,
                                    NomeServidor = atribuicaoResponsavel.SupervisorNome,
                                    Perfil = ((TipoResponsavelAtribuicao)atribuicaoResponsavel.TipoAtribuicao).ToPerfil()
                                }).ToList();


            if (responsaveisDre.NaoEhNulo() && responsaveisDre.Any())
                responsaveisUe.AddRange(responsaveisDre);
            return responsaveisUe.DistinctBy(resp => resp.Login).ToList();
        }

    }
}
