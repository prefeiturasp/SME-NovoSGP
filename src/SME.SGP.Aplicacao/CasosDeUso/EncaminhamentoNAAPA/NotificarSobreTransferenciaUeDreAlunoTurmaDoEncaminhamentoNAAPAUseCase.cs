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
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<EncaminhamentoNAAPADto>();
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
                    /*await NotificarResponsaveisSobreInativacaoAluno(encaminhamentoNAAPADto.AlunoNome, encaminhamentoNAAPADto.AlunoCodigo,
                                                                    turma, ultimaMatriculaAluno.SituacaoMatricula);*/
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

        private async Task NotificarResponsaveisSobreInativacaoAluno(string nomeAluno, string codigoAluno, Turma turma, string situacaoMatriculaAluno)
        {
            var titulo = $"Criança / Estudante inativa - {nomeAluno}({codigoAluno})";
            var mensagem = $@"A criança/ estudante {nomeAluno}({codigoAluno}) que está em acompanhamento pelo NAAPA da {turma.Ue.Dre.Abreviacao} e estava matriculado na turma 
                           {turma.NomeComModalidade()} na {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} teve a sua situação alterada para {situacaoMatriculaAluno} e 
                           não possui outras matrículas válidas na rede municipal de educação.O seu encaminhamento junto a esta DRE deverá ser encerrado.";

            var responsaveisNotificados = await RetornarReponsaveisDreUe(turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre);

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
            var perfis = new Guid[] { Perfis.PERFIL_COORDENADOR_NAAPA,
                                        Perfis.PERFIL_PSICOPEDAGOGO,
                                        Perfis.PERFIL_PSICOLOGO_ESCOLAR,
                                        Perfis.PERFIL_ASSISTENTE_SOCIAL  };
            var responsaveisUe = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoUe, perfis))).ToList();
            var responsaveisDre = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, perfis))).ToList();
            
            if (responsaveisDre != null && responsaveisDre.Any())
                responsaveisUe.AddRange(responsaveisDre);
            return responsaveisUe.DistinctBy(resp => resp.Login);
        }

    }
}
