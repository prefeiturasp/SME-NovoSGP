using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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
            aluno.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(encaminhamentoAee.AlunoCodigo, encaminhamentoAee.Turma.AnoLetivo));

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var podeEditar = await VerificaPodeEditar(encaminhamentoAee, usuarioLogado);
            var podeAtribuirResponsavel = await VerificaPodeAtribuirResponsavel(encaminhamentoAee, usuarioLogado);

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
                SituacaoDescricao = encaminhamentoAee.Situacao.Name(),
                PodeEditar = podeEditar,
                PodeAtribuirResponsavel = podeAtribuirResponsavel,
                MotivoEncerramento = encaminhamentoAee.MotivoEncerramento,
                responsavelEncaminhamentoAEE = encaminhamentoAee.Responsavel == null ? null :
                new ResponsavelEncaminhamentoAEEDto()
                {
                    Id = encaminhamentoAee.Responsavel.Id,
                    Nome = encaminhamentoAee.Responsavel.Nome,
                    Rf = encaminhamentoAee.Responsavel.CodigoRf
                }
            };
        }

        private async Task<bool> VerificaPodeAtribuirResponsavel(EncaminhamentoAEE encaminhamentoAee, Usuario usuarioLogado)
        {
            switch (encaminhamentoAee.Situacao)
            {
                case SituacaoAEE.AtribuicaoResponsavel:
                case SituacaoAEE.Analise:
                    return await EhGestorDaEscolaDaTurma(usuarioLogado, encaminhamentoAee.Turma) 
                        || await EhCoordenadorCEFAI(usuarioLogado, encaminhamentoAee.Turma);
                default:
                    return false;
            }
        }

        private async Task<bool> EhCoordenadorCEFAI(Usuario usuarioLogado, Turma turma)
        {
            if (!usuarioLogado.EhCoordenadorCEFAI())
                return false;

            var codigoDre = await mediator.Send(new ObterCodigoDREPorUeIdQuery(turma.UeId));
            if (string.IsNullOrEmpty(codigoDre))
                return false;

            return await UsuarioTemFuncaoCEFAINaDRE(usuarioLogado, codigoDre);
        }

        private async Task<bool> UsuarioTemFuncaoCEFAINaDRE(Usuario usuarioLogado, string codigoDre)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, new List<Guid>() { Perfis.PERFIL_CEFAI }));
            return funcionarios.Any(c => c == usuarioLogado.CodigoRf);
        }

        private async Task<bool> VerificaPodeEditar(EncaminhamentoAEE encaminhamento, Usuario usuarioLogado)
        {
            switch (encaminhamento.Situacao)
            {
                case SituacaoAEE.Rascunho:
                case SituacaoAEE.Devolvido:
                    return true;
                case SituacaoAEE.Encaminhado:
                    return await EhGestorDaEscolaDaTurma(usuarioLogado, encaminhamento.Turma);
                case SituacaoAEE.Analise:
                    return await EhUsuarioResponsavelPeloEncaminhamento(usuarioLogado, encaminhamento.ResponsavelId);
                
                default:
                    return false;
            }
        }

        private Task<bool> EhUsuarioResponsavelPeloEncaminhamento(Usuario usuarioLogado, long? responsavelId)
            => Task.FromResult(responsavelId.HasValue && usuarioLogado.Id == responsavelId.Value);

        private async Task<bool> EhProfessorDaTurma(Usuario usuarioLogado, Turma turma)
        {
            if (!usuarioLogado.EhProfessor())
                return false;

            var professores = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turma.CodigoTurma));

            return professores.Any(a => a.ProfessorRf.ToString() == usuarioLogado.CodigoRf);
        }

        private async Task<bool> EhGestorDaEscolaDaTurma(Usuario usuarioLogado, Turma turma)
        {
            if (!usuarioLogado.EhGestorEscolar())
                return false;

            var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turma.CodigoTurma));
            if (ue == null)
                throw new NegocioException($"Escola da turma [{turma.CodigoTurma}] não localizada.");

            return await mediator.Send(new EhGestorDaEscolaQuery(usuarioLogado.CodigoRf, ue.CodigoUe, usuarioLogado.PerfilAtual));
        }
    }
}
