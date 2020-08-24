using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoUseCase : AbstractUseCase, IObterAnotacaoFrequenciaAlunoUseCase
    {
        public ObterAnotacaoFrequenciaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AnotacaoFrequenciaAlunoDto> Executar(FiltroAnotacaoFrequenciaAlunoDto param)
            => MapearParaDto(await mediator.Send(new ObterAnotacaoFrequenciaAlunoQuery(param.CodigoAluno, param.AulaId)));

        private AnotacaoFrequenciaAlunoDto MapearParaDto(AnotacaoFrequenciaAluno anotacao)
            => anotacao == null ? null :
            new AnotacaoFrequenciaAlunoDto()
            {
                Id = anotacao.Id,
                CodigoAluno = anotacao.CodigoAluno,
                AulaId = anotacao.AulaId,
                MotivoAusenciaId = anotacao.MotivoAusenciaId ?? 0,
                Anotacao = anotacao.Anotacao,
                Auditoria = (AuditoriaDto)anotacao
            };

    }
}
