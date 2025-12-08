using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PlanoAEE.EncerrarPlano
{
    public class EncerramentoManualPlanoAEEUseCase : IEncerramentoManualPlanoAEEUseCase
    {
        private readonly IMediator mediator;

        public EncerramentoManualPlanoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoEncerramentoPlanoAEEDto> Executar(long planoId)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(planoId));
            if (planoAEE.EhNulo())
                throw new NegocioException(MensagemNegocioPlanoAee.Plano_aee_nao_encontrado);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoPlanoAeeQuery(planoAEE.AlunoCodigo, DateTime.Now.Year, false));
            if(aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            if(!PermitirEncerramentoManual(aluno))
                throw new NegocioException(MensagemNegocioPlanoAee.PLANO_AEE_ENCERRAMENTO_MANUAL_NAO_PERMITIDO);

            var retorno = await mediator.Send(new EncerramentoManualPlanoAEECommand(planoId));
            return retorno;
        }

        private static bool PermitirEncerramentoManual(AlunoReduzidoDto aluno)
        {
            return !(new[] { SituacaoMatriculaAluno.Ativo,
                             SituacaoMatriculaAluno.PendenteRematricula,
                             SituacaoMatriculaAluno.Rematriculado,
                             SituacaoMatriculaAluno.SemContinuidade}.Contains(aluno.CodigoSituacaoMatricula))

                   || aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido;
        }

    }
}
