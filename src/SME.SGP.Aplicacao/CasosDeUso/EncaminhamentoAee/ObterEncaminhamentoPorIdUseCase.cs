using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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

            if(encaminhamentoAee == null)
                throw new NegocioException("Encaminhamento não localizado");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(encaminhamentoAee.AlunoCodigo, encaminhamentoAee.Turma.AnoLetivo));

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var podeEditar = await VerificaPodeEditar(encaminhamentoAee, usuarioLogado);            

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
                MotivoEncerramento = encaminhamentoAee.MotivoEncerramento,
                Auditoria = (AuditoriaDto)encaminhamentoAee,
                responsavelEncaminhamentoAEE = new ResponsavelEncaminhamentoAEEDto()
                {
                    Id = encaminhamentoAee.ResponsavelId == null ? 0 : encaminhamentoAee.ResponsavelId,
                    Nome = encaminhamentoAee.ResponsavelNome == null ? "" 
                    :
                    string.IsNullOrEmpty(encaminhamentoAee.ResponsavelNome) ? "" : encaminhamentoAee.ResponsavelNome,
                    Rf = encaminhamentoAee.ResponsavelRf == null ? ""
                    :
                    string.IsNullOrEmpty(encaminhamentoAee.ResponsavelRf) ? "" : encaminhamentoAee.ResponsavelRf,
                }
            };
        }

        private async Task<bool> VerificaPodeEditar(EncaminhamentoAEE encaminhamento, Usuario usuarioLogado)
        {
            switch (encaminhamento.Situacao)
            {
                case SituacaoAEE.Rascunho:
                    return usuarioLogado.CodigoRf == encaminhamento.CriadoRF;
                case SituacaoAEE.Encaminhado:
                    return usuarioLogado.EhGestorEscolar() && await EhGestorDaEscolaDaTurma(usuarioLogado, encaminhamento.Turma);
                case SituacaoAEE.Analise:
                case SituacaoAEE.Finalizado:
                case SituacaoAEE.Encerrado:
                    return false;
                default:
                    return false;
            }
        }

        private async Task<bool> EhGestorDaEscolaDaTurma(Usuario usuarioLogado, Turma turma)
        {
            var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turma.CodigoTurma));
            if (ue == null)
                throw new NegocioException($"Escola da turma [{turma.CodigoTurma}] não localizada.");

            return await mediator.Send(new EhGestorDaEscolaQuery(usuarioLogado.CodigoRf, ue.CodigoUe, usuarioLogado.PerfilAtual));
        }
    }
}
