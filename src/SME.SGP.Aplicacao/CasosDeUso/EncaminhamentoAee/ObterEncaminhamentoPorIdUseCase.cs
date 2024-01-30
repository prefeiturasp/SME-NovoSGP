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

            if(encaminhamentoAee.EhNulo())
                throw new NegocioException("Encaminhamento não localizado");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(encaminhamentoAee.AlunoCodigo, encaminhamentoAee.Turma.AnoLetivo, true));
            aluno.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(encaminhamentoAee.AlunoCodigo, encaminhamentoAee.Turma.AnoLetivo));

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var podeEditar = await VerificaPodeEditar(encaminhamentoAee, usuarioLogado);
            var podeAtribuirResponsavel = await VerificaPodeAtribuirResponsavel(encaminhamentoAee, usuarioLogado);
            
            var registroCadastradoEmOutraUE = !await VerificarUsuarioLogadoPertenceMesmaUEEncaminhamento(usuarioLogado, encaminhamentoAee.Turma);
            
            aluno.EhMatriculadoTurmaPAP = await BuscarAlunosTurmaPAP(aluno.CodigoAluno, encaminhamentoAee.Turma.AnoLetivo);
            return new EncaminhamentoAEERespostaDto()
            {
                Aluno = aluno,
                Turma = new TurmaAnoDto()
                {
                    Id = encaminhamentoAee.Turma.Id,
                    Codigo = encaminhamentoAee.Turma.CodigoTurma,
                    AnoLetivo = encaminhamentoAee.Turma.AnoLetivo,
                    CodigoUE = encaminhamentoAee.Turma.Ue.CodigoUe
                },
                Situacao = encaminhamentoAee.Situacao,
                SituacaoDescricao = encaminhamentoAee.Situacao.Name(),
                PodeEditar = podeEditar,
                PodeAtribuirResponsavel = podeAtribuirResponsavel,
                MotivoEncerramento = encaminhamentoAee.MotivoEncerramento,
                responsavelEncaminhamentoAEE = encaminhamentoAee.Responsavel.EhNulo() ? null :
                new ResponsavelEncaminhamentoAEEDto()
                {
                    Id = encaminhamentoAee.Responsavel.Id,
                    Nome = encaminhamentoAee.Responsavel.Nome,
                    Rf = encaminhamentoAee.Responsavel.CodigoRf
                },
                RegistroCadastradoEmOutraUE = registroCadastradoEmOutraUE
            };
        }

        private async Task<bool> VerificarUsuarioLogadoPertenceMesmaUEEncaminhamento(Usuario usuarioLogado, Turma turmaEncaminhamentoAee)
        {
            return await mediator.Send(new VerificarUsuarioLogadoPertenceMesmaUEQuery(usuarioLogado, turmaEncaminhamentoAee));
        }

        private async Task<bool> BuscarAlunosTurmaPAP(string alunoCodigo, int anoLetivo)
        {
            var consulta =  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, new []{alunoCodigo}));
            return consulta.Any(x => x.CodigoAluno.ToString() == alunoCodigo);
        }
        private async Task<bool> VerificaPodeAtribuirResponsavel(EncaminhamentoAEE encaminhamentoAee, Usuario usuarioLogado)
        {
            switch (encaminhamentoAee.Situacao)
            {
                case SituacaoAEE.AtribuicaoResponsavel:
                case SituacaoAEE.AtribuicaoPAAI:
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
            return funcionarios.Any(c => c.Login == usuarioLogado.CodigoRf);
        }

        private async Task<bool> VerificaPodeEditar(EncaminhamentoAEE encaminhamento, Usuario usuarioLogado)
        {
            return (await EhProfessorDaTurma(usuarioLogado, encaminhamento.Turma) &&
                    (encaminhamento.Situacao == SituacaoAEE.Rascunho || 
                     encaminhamento.Situacao == SituacaoAEE.Devolvido))
                || (await EhGestorDaEscolaDaTurma(usuarioLogado, encaminhamento.Turma) && 
                    (encaminhamento.Situacao == SituacaoAEE.Encaminhado || 
                     encaminhamento.Situacao == SituacaoAEE.Devolvido ||
                     encaminhamento.Situacao == SituacaoAEE.Rascunho))
                || (usuarioLogado.EhCoordenadorCEFAI() && encaminhamento.Situacao == SituacaoAEE.AtribuicaoPAAI)
                || ((usuarioLogado.EhProfessorPaee() || usuarioLogado.EhPerfilPaai()) && 
                    (encaminhamento.Situacao == SituacaoAEE.Analise || encaminhamento.Situacao == SituacaoAEE.Rascunho));
        }

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
            if (ue.EhNulo())
                throw new NegocioException($"Escola da turma [{turma.CodigoTurma}] não localizada.");

            return await mediator.Send(new EhGestorDaEscolaQuery(usuarioLogado.CodigoRf, ue.CodigoUe, usuarioLogado.PerfilAtual));
        }
    }
}
