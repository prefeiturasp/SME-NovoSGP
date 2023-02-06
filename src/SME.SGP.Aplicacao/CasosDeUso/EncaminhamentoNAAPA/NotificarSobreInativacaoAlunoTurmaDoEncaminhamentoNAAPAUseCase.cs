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
    public class NotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase : AbstractUseCase, INotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase
    {
        public NotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var situcoesMatriculaInativas = new int[] { (int)SituacaoMatriculaAluno.Concluido, (int)SituacaoMatriculaAluno.Desistente,
                                                        (int)SituacaoMatriculaAluno.VinculoIndevido, (int)SituacaoMatriculaAluno.Falecido,
                                                        (int)SituacaoMatriculaAluno.NaoCompareceu, (int)SituacaoMatriculaAluno.Deslocamento } ;
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<EncaminhamentoNAAPADto>();
            var codigoTurma = await mediator.Send(new ObterTurmaCodigoPorIdQuery(encaminhamentoNAAPADto.TurmaId));

            var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosQuery(long.Parse(encaminhamentoNAAPADto.AlunoCodigo), true));
            var alunoTurma = alunosEol.Where(turma => turma.CodigoTipoTurma == (int)TipoTurma.Regular)
                                      .OrderByDescending(turma => turma.AnoLetivo)
                                      .ThenByDescending(turma => turma.DataSituacao)
                                      .FirstOrDefault();

            if (codigoTurma == alunoTurma.CodigoTurma.ToString() && 
                (int)(encaminhamentoNAAPADto.SituacaoMatriculaAluno ?? 0) != alunoTurma.CodigoSituacaoMatricula &&
                situcoesMatriculaInativas.Contains(alunoTurma.CodigoSituacaoMatricula))
            {

                return true;
            }
            return false;

        }

    }
}
