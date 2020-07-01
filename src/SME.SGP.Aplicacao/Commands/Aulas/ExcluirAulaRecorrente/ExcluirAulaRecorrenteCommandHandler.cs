using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaRecorrenteCommandHandler : IRequestHandler<ExcluirAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IUnitOfWork unitOfWork;

        public ExcluirAulaRecorrenteCommandHandler(IMediator mediator,
                                                   IRepositorioAula repositorioAula,
                                                   IRepositorioNotificacaoAula repositorioNotificacaoAula,
                                                   IServicoNotificacao servicoNotificacao,
                                                   IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            var aulaOrigem = await repositorioAula.ObterCompletoPorIdAsync(request.AulaId);
            if (aulaOrigem == null)
                throw new NegocioException("Não foi possível obter a aula.");

            var fimRecorrencia = await mediator.Send(new ObterFimPeriodoRecorrenciaQuery(aulaOrigem.TipoCalendarioId, aulaOrigem.DataAula.Date, request.Recorrencia));
            var aulasRecorrencia = await repositorioAula.ObterAulasRecorrencia(aulaOrigem.AulaPaiId ?? aulaOrigem.Id, aulaOrigem.Id, fimRecorrencia);
            var listaProcessos = await IncluirAulasEmManutencao(aulaOrigem, aulasRecorrencia);

            try
            {
                List<(DateTime data, bool existeFrequencia, bool existePlanoAula)> aulasComFrenciaOuPlano = new List<(DateTime data, bool existeFrequencia, bool existePlanoAula)>();
                var listaExclusoes = new List<(DateTime dataAula, bool sucesso, string mensagem, bool existeFrequente, bool existePlanoAula)>();

                listaExclusoes.Add(await TratarExclusaoAula(aulaOrigem, request.Usuario));
                foreach (var aulaRecorrente in aulasRecorrencia)
                {
                    listaExclusoes.Add(await TratarExclusaoAula(aulaRecorrente, request.Usuario));
                }

                //usuario.PerfilAtual = perfilSelecionado;
                await NotificarUsuario(aulaOrigem.Id, listaExclusoes, request.Usuario, request.ComponenteCurricularNome, aulaOrigem.Turma);
            }
            finally
            {
                await RemoverAulasEmManutencao(listaProcessos.Select(p => p.Id).ToArray());
            }

            return true;
        }

        private async Task<(DateTime dataAula, bool sucesso, string mensagem, bool existeFrequente, bool existePlanoAula)> TratarExclusaoAula(Aula aula, Usuario usuario)
        {
            try
            {
                var existeFrequencia = await mediator.Send(new ObterAulaPossuiFrequenciaQuery(aula.Id));
                var existePlanoAula = await mediator.Send(new ObterAulaPossuiPlanoAulaQuery(aula.Id));

                await ExcluirAula(aula, usuario);

                return (aula.DataAula, true, string.Empty, existeFrequencia, existePlanoAula);
            }
            catch (Exception ex)
            {
                return (aula.DataAula, false, ex.Message, false, false);
            }
        }

        private async Task ExcluirAula(Aula aula, Usuario usuario)
        {
            if (await mediator.Send(new AulaPossuiAvaliacaoQuery(aula, usuario.CodigoRf)))
                throw new NegocioException("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");

            await ValidarComponentesDoProfessor(aula.TurmaId, long.Parse(aula.DisciplinaId), aula.DataAula, usuario);

            unitOfWork.IniciarTransacao();
            try
            {
                // TODO validar transacao e conections 
                if (aula.WorkflowAprovacaoId.HasValue)
                    await mediator.Send(new ExcluirWorkflowCommand(aula.WorkflowAprovacaoId.Value));

                await mediator.Send(new ExcluirNotificacoesDaAulaCommand(aula.Id));
                await mediator.Send(new ExcluirFrequenciaDaAulaCommand(aula.Id));
                await mediator.Send(new ExcluirPlanoAulaDaAulaCommand(aula.Id));

                aula.Excluido = true;
                await repositorioAula.SalvarAsync(aula);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task ValidarComponentesDoProfessor(string codigoTurma, long componenteCurricularCodigo, DateTime dataAula, Usuario usuario)
        {
            var resultadoValidacao = await mediator.Send(new ValidarComponentesDoProfessorCommand(usuario, codigoTurma, componenteCurricularCodigo, dataAula));
            if (!resultadoValidacao.resultado)
                throw new NegocioException(resultadoValidacao.mensagem);
        }

        private async Task<IEnumerable<ProcessoExecutando>> IncluirAulasEmManutencao(Aula aulaOrigem, IEnumerable<Aula> aulasRecorrencia)
        {
            var listaProcessos = await mediator.Send(new InserirAulaEmManutencaoCommand(aulasRecorrencia.Select(a => a.Id)
                                                                                        .Union(new List<long>() { aulaOrigem.Id })));


            return listaProcessos;
        }

        private async Task RemoverAulasEmManutencao(long[] listaProcessosId)
        {
            try
            {
                await mediator.Send(new RemoverProcessoEmExecucaoCommand(listaProcessosId));
            }
            catch (Exception ex)
            {
                SentrySdk.AddBreadcrumb("Exclusao de Registro em Manutenção da Aula", "Alteração de Aula Recorrente");
                SentrySdk.CaptureException(ex);
            }
        }

        private async Task NotificarUsuario(long aulaId, IEnumerable<(DateTime dataAula, bool sucesso, string mensagem, bool existeFrequencia, bool existePlanoAula)> listaExclusoes, Usuario usuario, string componenteCurricularNome, Turma turma)
        {
            var perfilAtual = usuario.PerfilAtual;
            if (perfilAtual == Guid.Empty)
                throw new NegocioException($"Não foi encontrado o perfil do usuário informado.");

            var tituloMensagem = $"Exclusão de Aulas de {componenteCurricularNome} na turma {turma.Nome}";
            StringBuilder mensagemUsuario = new StringBuilder();

            var aulasExcluidas = listaExclusoes.Where(c => c.sucesso);
            var aulasComErro = listaExclusoes.Where(c => !c.sucesso);
            var aulasComFrequenciaOuPlanoAula = listaExclusoes.Where(a => a.existeFrequencia || a.existePlanoAula);

            mensagemUsuario.Append($"Foram excluidas {aulasExcluidas.Count()} aulas da disciplina {componenteCurricularNome} para a turma {turma.Nome} da {turma.Ue?.Nome} ({turma.Ue?.Dre?.Nome}).");

            if (aulasComFrequenciaOuPlanoAula.Any())
            {
                mensagemUsuario.Append($"<br><br>Nas seguintes datas haviam registros de plano de aula ou frequência:<br>");

                foreach (var aulaComFrequenciaOuPlanoAula in aulasComFrequenciaOuPlanoAula)
                {
                    var planoAula = aulaComFrequenciaOuPlanoAula.existePlanoAula ? " e Plano de Aula" : "";

                    var frequenciaPlano = aulaComFrequenciaOuPlanoAula.existeFrequencia ?
                                            $"Frequência{planoAula}"
                                            : "Plano de Aula";
                    mensagemUsuario.Append($"<br /> {aulaComFrequenciaOuPlanoAula.dataAula:dd/MM/yyyy} - {frequenciaPlano}");
                }
            }

            if (aulasComErro.Any())
            {
                mensagemUsuario.Append($"<br><br>Ocorreram erros na exclusão das seguintes aulas:<br>");
                foreach (var aula in aulasComErro.OrderBy(a => a.dataAula))
                {
                    mensagemUsuario.Append($"<br /> {aula.dataAula:dd/MM/yyyy} - {aula.mensagem};");
                }
            }

            var notificacao = new Notificacao()
            {
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = turma.Ue.Dre.CodigoDre,
                Mensagem = mensagemUsuario.ToString(),
                UsuarioId = usuario.Id,
                Tipo = NotificacaoTipo.Calendario,
                Titulo = tituloMensagem,
                TurmaId = turma.CodigoTurma,
                UeId = turma.Ue.CodigoUe,
            };

            unitOfWork.IniciarTransacao();
            try
            {
                // Salva Notificação
                servicoNotificacao.Salvar(notificacao);

                // Gera vinculo Notificacao x Aula
                await repositorioNotificacaoAula.Inserir(notificacao.Id, aulaId);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

    }
}
