using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoPorIdUseCase : IObterEncaminhamentoPorIdUseCase
    {
        private readonly IMediator mediator;

        public ObterEncaminhamentoPorIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<EncaminhamentoAEERespostaDto> Executar(long id)
        {
            var encaminhamentoAee = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(id));
            var podeEditar = false;

            if (encaminhamentoAee == null)
                throw new NegocioException("Encaminhamento não localizado");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(encaminhamentoAee.AlunoCodigo, encaminhamentoAee.Turma.AnoLetivo));

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            
            if (encaminhamentoAee.Situacao == Dominio.Enumerados.SituacaoAEE.Rascunho && usuarioLogado.EhPerfilProfessor())
                podeEditar = true;

            if (encaminhamentoAee.Situacao != Dominio.Enumerados.SituacaoAEE.Rascunho && !usuarioLogado.EhPerfilProfessor())
                podeEditar = true;

            return new EncaminhamentoAEERespostaDto()
            {
                Aluno = aluno,
                Turma = new TurmaAnoDto()
                {
                    Id = encaminhamentoAee.Turma.Id,
                    Codigo = encaminhamentoAee.Turma.CodigoTurma,
                    AnoLetivo = encaminhamentoAee.Turma.AnoLetivo
                },
                Situacao = encaminhamentoAee.Situacao,
                PodeEditar = podeEditar,
                Auditoria = (AuditoriaDto)encaminhamentoAee
            };
        }
    }
}
