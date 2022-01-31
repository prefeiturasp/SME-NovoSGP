using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno;
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ExcluirAulaRecorrenteCommandHandler(IMediator mediator,
                                                   IRepositorioAulaConsulta repositorioAula,
                                                   IRepositorioNotificacaoAula repositorioNotificacaoAula,
                                                   IRepositorioPlanoAula repositorioPlanoAula,
                                                   IRepositorioDiarioBordo repositorioDiarioBordo,
                                                   IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno,
                                                   IRepositorioDevolutiva repositorioDevolutiva,
                                                   IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
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
            await ExcluirArquivoAnotacaoFrequencia(request.AulaId);
            await ExcluirArquivosPlanoAula(request.AulaId);
            await RemoverArquivosDiarioBordo(request.AulaId);
            return true;
        }
        private async Task ExcluirArquivosPlanoAula(long aulaId)
        {
            var plano = await repositorioPlanoAula.ObterPlanoAulaPorAulaRegistroExcluido(aulaId);

            if (plano != null)
            {
                await ExcluirArquivo(plano.Descricao, TipoArquivo.PlanoAula);
                await ExcluirArquivo(plano.RecuperacaoAula, TipoArquivo.PlanoAulaRecuperacao);
                await ExcluirArquivo(plano.LicaoCasa, TipoArquivo.PlanoAulaLicaoCasa); 
            }
        }

        private async Task RemoverArquivosDiarioBordo(long aulaId)
        {
            var diarioDeBordo = await repositorioDiarioBordo.ObterPorAulaIdRegistroExcluido(aulaId);
            if(diarioDeBordo?.Planejamento != null)
            {
                await ExcluirArquivo(diarioDeBordo.Planejamento,TipoArquivo.DiarioBordo);
            }
        }
        private async Task ExcluirArquivoAnotacaoFrequencia(long aulaId)
        {
            var anotacaoFrequencia = await repositorioAnotacaoFrequenciaAluno.ObterPorAulaIdRegistroExcluido(aulaId);
            foreach (var item in anotacaoFrequencia)
            {
                await ExcluirArquivo(item.Anotacao,TipoArquivo.FrequenciaAnotacaoEstudante);
            }
        }
        private async Task ExcluirArquivo(string mensagem,TipoArquivo tipo)
        {
            if (!string.IsNullOrEmpty(mensagem))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(mensagem, string.Empty, tipo.Name()));
            }
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

            if (aula.WorkflowAprovacaoId.HasValue)
                await PulicaFilaSgp(RotasRabbitSgp.WorkflowAprovacaoExcluir, aula.WorkflowAprovacaoId.Value, usuario);

            await PulicaFilaSgp(RotasRabbitSgp.NotificacoesDaAulaExcluir, aula.Id, usuario);
            await PulicaFilaSgp(RotasRabbitSgp.FrequenciaDaAulaExcluir, aula.Id, usuario);
            await PulicaFilaSgp(RotasRabbitSgp.PlanoAulaDaAulaExcluir, aula.Id, usuario);
            await PulicaFilaSgp(RotasRabbitSgp.AnotacoesFrequenciaDaAulaExcluir, aula.Id, usuario);
            await PulicaFilaSgp(RotasRabbitSgp.DiarioBordoDaAulaExcluir, aula.Id, usuario);
            await PulicaFilaSgp(RotasRabbitSgp.RotaExecutaExclusaoPendenciasAula, aula.Id, usuario);

            aula.Excluido = true;
            await repositorioAula.SalvarAsync(aula);
        }

        private async Task PulicaFilaSgp(string fila, long id, Usuario usuario)
        {
            await mediator.Send(new PublicarFilaSgpCommand(fila, new FiltroIdDto(id), Guid.NewGuid(), usuario));
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
                await mediator.Send(new SalvarLogViaRabbitCommand($"Exclusão de registro em manutenção de aula.", LogNivel.Critico, LogContexto.Aula, ex.Message));
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

            mensagemUsuario.Append($"Foram excluidas {aulasExcluidas.Count()} aulas do componente curricular {componenteCurricularNome} para a turma {turma.Nome} da {turma.Ue?.Nome} ({turma.Ue?.Dre?.Nome}).");

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

            unitOfWork.IniciarTransacao();
            try
            {
                // Salva Notificação
                var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(tituloMensagem, 
                                                               mensagemUsuario.ToString(), 
                                                               usuario.CodigoRf, 
                                                               NotificacaoCategoria.Aviso, 
                                                               NotificacaoTipo.Calendario, 
                                                               turma.Ue.Dre.CodigoDre,
                                                               turma.Ue.CodigoUe, 
                                                               turma.CodigoTurma, 
                                                               DateTime.Now.Year));

                // Gera vinculo Notificacao x Aula
                await repositorioNotificacaoAula.Inserir(notificacaoId, aulaId);

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
