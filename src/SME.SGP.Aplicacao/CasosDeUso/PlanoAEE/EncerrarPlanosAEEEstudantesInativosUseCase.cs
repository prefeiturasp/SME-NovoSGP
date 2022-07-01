using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    /// <summary>
    /// Método de encerramento de planos para alunos que estejam inativos e com situação concluída.
    /// </summary>
    public class EncerrarPlanosAEEEstudantesInativosUseCase : AbstractUseCase, IEncerrarPlanosAEEEstudantesInativosUseCase
    {
        public EncerrarPlanosAEEEstudantesInativosUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var planosAtivos = await mediator.Send(new ObterPlanosAEEAtivosQuery());
            var anoLetivo = DateTime.Today.Year;

            if (planosAtivos != null && planosAtivos.Any())
            {
                foreach (var planoAEE in planosAtivos)
                {
                    var matriculas = await mediator
                        .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(planoAEE.AlunoCodigo, anoLetivo, filtrarSituacao: false));

                    var turma = await ObterTurma(planoAEE.TurmaId);                    

                    if (turma == null)
                        throw new NegocioException($"Não foi localizada a turma com id {planoAEE.TurmaId}.");

                    var etapaConcluida = false;
                    var transferenciaUe = false;
                    AlunoPorTurmaResposta ultimaMatricula = null;
                    AlunoPorTurmaResposta registroMatriculaTurmaAnterior = null;
                    
                    if (turma != null && (turma.AnoLetivo != anoLetivo))
                        etapaConcluida = DeterminaEtapaConcluida(matriculas, planoAEE.AlunoCodigo, turma, ref ultimaMatricula);

                    if (matriculas.Select(m => m.CodigoTurma).Distinct().Count() > 1)
                        transferenciaUe = DeterminaTransferenciaUe(matriculas, ref registroMatriculaTurmaAnterior);

                    if ((!matriculas?.Any(a => a.EstaAtivo(DateTime.Today)) ?? true) || etapaConcluida || transferenciaUe)
                    {
                        if (ultimaMatricula == null)
                            ultimaMatricula = matriculas?.OrderByDescending(a => a.DataSituacao).FirstOrDefault();

                        await EncerrarPlanoAEE(planoAEE, registroMatriculaTurmaAnterior?.SituacaoMatricula ?? ultimaMatricula?.SituacaoMatricula ?? "Inativo", registroMatriculaTurmaAnterior?.DataSituacao ?? ultimaMatricula?.DataSituacao ?? DateTime.Now);
                    }
                }
            }

            return true;
        }      

        private async Task EncerrarPlanoAEE(PlanoAEE planoAEE, string situacaoMatricula, DateTime dataSituacao)
        {
            planoAEE.Situacao = SituacaoPlanoAEE.EncerradoAutomaticamente;

            await mediator.Send(new PersistirPlanoAEECommand(planoAEE));
            await mediator.Send(new ResolverPendenciaPlanoAEECommand(planoAEE.Id));

            if (await ParametroNotificarPlanosAEE())
                await NotificarEncerramento(planoAEE, situacaoMatricula, dataSituacao);
        }

        private async Task<bool> ParametroNotificarPlanosAEE()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarNotificacaoPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }

        private async Task NotificarEncerramento(PlanoAEE plano, string situacaoMatricula, DateTime dataSituacao)
        {
            var turma = await ObterTurma(plano.TurmaId);

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE encerrado automaticamente - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi encerrado automaticamente. Motivo: {situacaoMatricula} em {dataSituacao}.";

            var usuarisoIds = await ObterUsuariosParaNotificacao(turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre);

            if (usuarisoIds != null && usuarisoIds.Any())
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(plano.Id, usuarisoIds, titulo, descricao, NotificacaoPlanoAEETipo.PlanoExpirado, NotificacaoCategoria.Aviso));
        }

        private async Task<IEnumerable<long>> ObterUsuariosParaNotificacao(string ueCodigo, string dreCodigo)
        {
            var coordenadoresUe = await ObterCoordenadoresUe(ueCodigo);

            var usuariosIds = await ObterUsuariosId(coordenadoresUe);
            var coordenadoresCEFAI = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dreCodigo));

            if (coordenadoresCEFAI != null && coordenadoresCEFAI.Any())
                foreach (var coordenadorCEFAI in coordenadoresCEFAI)
                    usuariosIds.Add(coordenadorCEFAI);

            return usuariosIds;
        }

        private async Task<List<string>> ObterCoordenadoresUe(string codigoUe)
        {
            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));
            if (funcionariosCP != null && funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.AD));
            if (funcionariosAD != null && funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Diretor));
            if (funcionariosDiretor != null && funcionariosDiretor.Any())
                return funcionariosDiretor.Select(f => f.CodigoRF).ToList();

            return null;
        }

        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            var usuariosIds = new List<long>();
            foreach (var functionario in funcionarios)
            {
                var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                if (usuarioId > 0)
                    usuariosIds.Add(usuarioId);
            }
            return usuariosIds;
        }

        private async Task<Turma> ObterTurma(long turmaId)
            => await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

        private bool DeterminaEtapaConcluida(IEnumerable<AlunoPorTurmaResposta> matriculas, string alunoCodigo, Turma turma, ref AlunoPorTurmaResposta ultimaMatricula)
        {
            var matriculasAnoTurma = mediator
                .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(alunoCodigo, turma?.AnoLetivo ?? DateTime.Today.Year)).Result;

            var concluiuTurma = matriculasAnoTurma
                .Any(m => m.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);

            if (concluiuTurma)
            {
                var ultimaMatriculaAtual = matriculas
                    .OrderBy(m => m.DataMatricula)
                    .LastOrDefault();

                var turmaAtual = ultimaMatriculaAtual != null ?
                    mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(ultimaMatriculaAtual.CodigoTurma.ToString())).Result : null;

                ultimaMatricula = matriculasAnoTurma
                    .OrderBy(m => m.DataSituacao)
                    .FirstOrDefault(m => m.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);

                return turma.Ue.CodigoUe != turmaAtual?.Ue.CodigoUe &&
                       (turma.EhTurmaInfantil && (!turmaAtual?.EhTurmaInfantil ?? turma.EhTurmaInfantil));
            }

            return false;
        }

        private bool DeterminaTransferenciaUe(IEnumerable<AlunoPorTurmaResposta> matriculas, ref AlunoPorTurmaResposta registroMatriculaTurmaAnterior)
        {
            var registroMatriculaMaisRecente = matriculas
                .OrderBy(m => m.DataSituacao)
                .Last();

            registroMatriculaTurmaAnterior = matriculas
                .Where(m => !m.CodigoTurma.Equals(registroMatriculaMaisRecente.CodigoTurma))
                .OrderBy(m => m.DataSituacao)
                .Last();

            return (registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Transferido ||
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Deslocamento ||
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.TransferidoSED) &&
                   !registroMatriculaTurmaAnterior.CodigoEscola.Equals(registroMatriculaMaisRecente.CodigoEscola);
        }
    }
}
