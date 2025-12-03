using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase : AbstractUseCase, INotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase
    {
        public NotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<AtendimentoNAAPADto>();
            var notificacoesEnviadas = false;
            
            var alunosEol = (await mediator.Send(new ObterAlunosEolPorCodigosQuery(long.Parse(encaminhamentoNAAPADto.AlunoCodigo), true))).ToList();
            var matriculaVigenteAluno = FiltrarMatriculaVigenteAluno(alunosEol);

            var turmaAnterior = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoNAAPADto.TurmaId));

            if (matriculaVigenteAluno.CodigoTurma.ToString() !=
                turmaAnterior.CodigoTurma)
            {
                var turmaNova = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(matriculaVigenteAluno.CodigoTurma.ToString()));

                if (turmaNova.Ue.Dre.CodigoDre != turmaAnterior.Ue.Dre.CodigoDre || turmaNova.Ue.CodigoUe != turmaAnterior.Ue.CodigoUe)
                {
                    await NotificarResponsaveisSobreTransferenciaUeDreAluno(encaminhamentoNAAPADto.AlunoNome, encaminhamentoNAAPADto.AlunoCodigo,
                                                                    turmaAnterior, turmaNova);
                    notificacoesEnviadas = true;
                }
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

        private async Task NotificarResponsaveisSobreTransferenciaUeDreAluno(string nomeAluno, string codigoAluno, Turma turmaAnterior, 
                                                                             Turma turmaNova)
        {
            var responsaveisUeAnterior = await RetornarReponsaveisDreUe(turmaAnterior.Ue.Dre.CodigoDre, turmaAnterior.Ue.CodigoUe);
            var responsaveisUeNova = await RetornarReponsaveisDreUe(turmaNova.Ue.Dre.CodigoDre, turmaNova.Ue.CodigoUe);
            var responsaveisNotificados = responsaveisUeAnterior;
            responsaveisNotificados.AddRange(responsaveisUeNova);

            var titulo = $"Criança/Estudante transferida - {nomeAluno}({codigoAluno})";
            var mensagem = $@"A criança/estudante {nomeAluno}({codigoAluno}) que está em acompanhamento pelo NAAPA da {turmaAnterior.Ue.Dre.Abreviacao} e estava matriculada na turma {turmaAnterior.NomeComModalidade()} na {turmaAnterior.Ue.TipoEscola.ObterNomeCurto()} {turmaAnterior.Ue.Nome} foi transferida para a turma {turmaNova.NomeComModalidade()} na {turmaNova.Ue.TipoEscola.ObterNomeCurto()} {turmaNova.Ue.Nome}{(turmaAnterior.Ue.Dre.CodigoDre != turmaNova.Ue.Dre.CodigoDre ? $"({turmaNova.Ue.Dre.Abreviacao})" : "")}. {MontarMensagemResponsaveisNovaUe(responsaveisUeNova)}";

            
            foreach (var responsavel in responsaveisNotificados.DistinctBy(resp => resp.Login))
            {
                await mediator.Send(new NotificarUsuarioCommand(titulo,
                                                                mensagem,
                                                                responsavel.Login,
                                                                NotificacaoCategoria.Aviso,
                                                                NotificacaoTipo.NAAPA));
            }

        }

        private string MontarMensagemResponsaveisNovaUe(List<FuncionarioUnidadeDto> responsaveis)
        {
            var perfisMsg = new Guid[] { Perfis.PERFIL_PSICOLOGO_ESCOLAR, Perfis.PERFIL_PSICOPEDAGOGO, Perfis.PERFIL_ASSISTENTE_SOCIAL };
            if (!responsaveis.Any(resp => perfisMsg.Contains(resp.Perfil)))
                return string.Empty;

            var msg = "O encaminhamento NAAPA agora é de responsabilidade dos seguintes profissionais:";
            foreach (var responsavel in responsaveis.Where(resp => perfisMsg.Contains(resp.Perfil) ))
                msg += $"</br>{RetornarNomePerfil(responsavel.Perfil)}: {responsavel.NomeServidor}({responsavel.Login})";
            return msg;
        }

        private string RetornarNomePerfil(Guid perfil)
        {
            if (perfil == Perfis.PERFIL_PSICOLOGO_ESCOLAR)
                return "Psicólogo Escolar";
            if (perfil == Perfis.PERFIL_PSICOPEDAGOGO)
                return "Psicopedagogo";
            if (perfil == Perfis.PERFIL_ASSISTENTE_SOCIAL)
                return "Assistente Social";
            return string.Empty;            
        }

        private async Task<List<FuncionarioUnidadeDto>> RetornarReponsaveisDreUe(string codigoDre, string codigoUe) 
        {
            var perfisDre = new Guid[] { Perfis.PERFIL_COORDENADOR_NAAPA };
            var tiposAtribuicaoUe = new TipoResponsavelAtribuicao[] { TipoResponsavelAtribuicao.Psicopedagogo,
                                        TipoResponsavelAtribuicao.PsicologoEscolar,
                                        TipoResponsavelAtribuicao.AssistenteSocial };

            var responsaveisDre = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoUe, perfisDre))).ToList();
            if (!responsaveisDre.Any())
                responsaveisDre = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, perfisDre))).ToList();

            var responsaveisUe = (await mediator.Send(new ObterResponsaveisAtribuidosUePorDreUeTiposQuery(codigoDre, codigoUe, tiposAtribuicaoUe)))
                                .Select(atribuicaoResponsavel => new FuncionarioUnidadeDto(){  Login = atribuicaoResponsavel.SupervisorId,
                                                                                               NomeServidor = atribuicaoResponsavel.SupervisorNome,
                                                                                               Perfil = ((TipoResponsavelAtribuicao)atribuicaoResponsavel.TipoAtribuicao).ToPerfil()
                                }).ToList();
            
            
            if (responsaveisDre.NaoEhNulo() && responsaveisDre.Any())
                responsaveisUe.AddRange(responsaveisDre);
            return responsaveisUe.DistinctBy(resp => resp.Login).ToList();
        }

    }
}
