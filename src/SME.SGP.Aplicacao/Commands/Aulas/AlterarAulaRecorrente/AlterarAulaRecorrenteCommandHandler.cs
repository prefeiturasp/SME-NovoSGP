using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaRecorrenteCommandHandler : IRequestHandler<AlterarAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;
        private readonly IUnitOfWork unitOfWork;

        public AlterarAulaRecorrenteCommandHandler(IMediator mediator,
                                                   IRepositorioAula repositorioAula,
                                                   IRepositorioNotificacaoAula repositorioNotificacaoAula,
                                                   IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(AlterarAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            await ValidarComponentesProfessor(request);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.CodigoTurma));
            var aulaOrigem = await mediator.Send(new ObterAulaPorIdQuery(request.AulaId));

            await AlterarRecorrencia(request, aulaOrigem, turma);

            return true;
        }

        private async Task ValidarComponentesProfessor(AlterarAulaRecorrenteCommand aulaRecorrente)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aulaRecorrente.CodigoTurma));
            var componentesCurricularesDoProfessor = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(aulaRecorrente.CodigoTurma, aulaRecorrente.Usuario.Login, aulaRecorrente.Usuario.PerfilAtual, turma.EhTurmaInfantil));

            if (aulaRecorrente.Usuario.EhProfessorCj())
            {
                var componentesCurricularesDoProfessorCJ = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(aulaRecorrente.Usuario.Login));
                ValidarProfCJSemPermissaoCriacaoAulas(componentesCurricularesDoProfessor, componentesCurricularesDoProfessorCJ, aulaRecorrente.CodigoTurma, aulaRecorrente.ComponenteCurricularId);
            }
            else
            {
                ValidarProfSemPermissaoCriacaoAulas(componentesCurricularesDoProfessor, aulaRecorrente.ComponenteCurricularId);
                await ValidarUsuarioSemPermissaoTurmaDisciplina(aulaRecorrente.ComponenteCurricularId, aulaRecorrente.CodigoTurma, aulaRecorrente.DataAula, aulaRecorrente.Usuario);
            }
        }

        private async Task ValidarUsuarioSemPermissaoTurmaDisciplina(long componenteCurricularId, string codigoTurma, DateTime dataAula, Usuario usuario)
        {
            var usuarioPodePersistirTurmaNaData = await mediator
                    .Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(componenteCurricularId, codigoTurma, dataAula, usuario));

            if (!usuarioPodePersistirTurmaNaData)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }

        private void ValidarProfSemPermissaoCriacaoAulas(IEnumerable<ComponenteCurricularEol> componentesCurricularesDoProfessor,
                                                         long componenteCurricularId)
        {
            if (!ContemComponenteCurricularProfTurmaDisciplina(componentesCurricularesDoProfessor, componenteCurricularId))
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);
        }

        private void ValidarProfCJSemPermissaoCriacaoAulas(IEnumerable<ComponenteCurricularEol> componentesCurricularesDoProfessor,
                                                           IEnumerable<AtribuicaoCJ> atribuicoesProfessorCJ, string codigoTurma, long componenteCurricularId)
        {
            if (!ContemComponenteCurricularProfCJTurmaDisciplina(atribuicoesProfessorCJ, codigoTurma, componenteCurricularId))
                if (!ContemComponenteCurricularProfTurmaDisciplina(componentesCurricularesDoProfessor, componenteCurricularId))
                    throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);
        }

        private bool ContemComponenteCurricularProfTurmaDisciplina(IEnumerable<ComponenteCurricularEol> componentesCurricularesDoProfessor, long componenteCurricularId)
        {
            return componentesCurricularesDoProfessor.NaoEhNulo() &&
                   componentesCurricularesDoProfessor.Any(c => c.Codigo == componenteCurricularId || c.CodigoComponenteTerritorioSaber == componenteCurricularId);
        }

        private bool ContemComponenteCurricularProfCJTurmaDisciplina(IEnumerable<AtribuicaoCJ> atribuicoesProfessorCJ, string codigoTurma, long componenteCurricularId)
        {
            return atribuicoesProfessorCJ.NaoEhNulo() &&
                  atribuicoesProfessorCJ.Any(c => c.TurmaId == codigoTurma && c.DisciplinaId == componenteCurricularId);
        }

        private async Task AlterarRecorrencia(AlterarAulaRecorrenteCommand request, Aula aulaOrigem, Turma turma)
        {
            var listaAlteracoes = new List<(bool sucesso, bool erro, DateTime data, string mensagem)>();
            var listaAlteracoesFrequencia = new List<(long Id, DateTime Data, int QdadeAnterior)>()
            {
                (aulaOrigem.Id, aulaOrigem.DataAula, aulaOrigem.Quantidade)
            };

            var dataAula = request.DataAula;
            var aulaPaiIdOrigem = aulaOrigem.AulaPaiId ?? aulaOrigem.Id;
            var aulaAnteriorQnt = aulaOrigem.Quantidade;
            var fimRecorrencia = await mediator.Send(new ObterFimPeriodoRecorrenciaQuery(request.TipoCalendarioId, dataAula, request.RecorrenciaAula));

            var aulasDaRecorrencia = await mediator.Send(new ObterRepositorioAulaPorAulaRecorrenteQuery(aulaPaiIdOrigem, aulaOrigem.Id, fimRecorrencia));
            listaAlteracoesFrequencia.AddRange(aulasDaRecorrencia.Select(aula => (aula.Id, aula.DataAula, aula.Quantidade)));
            var listaProcessos = await IncluirAulasEmManutencao(aulaOrigem, aulasDaRecorrencia);

            try
            {
                listaAlteracoes.Add(await TratarAlteracaoAula(request, aulaOrigem, dataAula, turma));

                var diasRecorrencia = ObterDiasDaRecorrencia(dataAula.AddDays(7), fimRecorrencia);
                foreach (var diaAula in diasRecorrencia)
                {
                    // Obter a aula na mesma semana da nova data
                    var aulaRecorrente = aulasDaRecorrencia.FirstOrDefault(c => UtilData.ObterSemanaDoAno(c.DataAula) == UtilData.ObterSemanaDoAno(diaAula));

                    // Se não existir aula da recorrencia na semana cria uma nova
                    if (aulaRecorrente.NaoEhNulo())
                        listaAlteracoes.Add(await TratarAlteracaoAula(request, aulaRecorrente, diaAula, turma));
                    else
                        listaAlteracoes.Add(await TratarAlteracaoAula(request, (Aula)aulaOrigem.Clone(), diaAula, turma));
                }

                if (request.Quantidade != aulaAnteriorQnt)
                    await TrataAlteracaoDeFrequencia(request.Usuario, listaAlteracoesFrequencia);
            }
            finally
            {
                await RemoverAulasEmManutencao(listaProcessos.Select(p => p.Id).ToArray());
            }

            await NotificarUsuario(request.AulaId, listaAlteracoes, request.Usuario, request.NomeComponenteCurricular, turma);
        }

        private async Task TrataAlteracaoDeFrequencia(Usuario usuarioLogado, List<(long Id, DateTime Data, int QdadeAnterior)> listaAlteracoesFrequencia)
        {
            foreach(var aula in listaAlteracoesFrequencia)
            {
                var trataFrequenciaAulaModificada = new AulaAlterarFrequenciaRequestDto(aula.Id, aula.QdadeAnterior);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaAlterarAulaFrequenciaTratar, trataFrequenciaAulaModificada, Guid.NewGuid(), usuarioLogado));
            }
        }

        private IEnumerable<DateTime> ObterDiasDaRecorrencia(DateTime inicioRecorrencia, DateTime fimRecorrencia)
        {
            if (inicioRecorrencia.Date == fimRecorrencia.Date)
                return new List<DateTime>() { inicioRecorrencia };

            return ObterDiaEntreDatas(inicioRecorrencia, fimRecorrencia);
        }

        private IEnumerable<DateTime> ObterDiaEntreDatas(DateTime inicio, DateTime fim)
        {
            for (DateTime i = inicio; i <= fim; i = i.AddDays(7))
            {
                yield return i;
            }
        }

        private async Task RemoverAulasEmManutencao(long[] listaProcessosId)
        {
            try
            {
                await mediator.Send(new RemoverProcessoEmExecucaoCommand(listaProcessosId));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Exclusao de Registro em Manutenção da Aula", LogNivel.Negocio, LogContexto.Aula, ex.Message));
            }
        }

        private async Task<IEnumerable<ProcessoExecutando>> IncluirAulasEmManutencao(Aula aulaOrigem, IEnumerable<Aula> aulasDaRecorrencia)
        {
            var listaProcessos = await mediator.Send(new InserirAulaEmManutencaoCommand(aulasDaRecorrencia.Select(a => a.Id)
                                                                                        .Union(new List<long>() { aulaOrigem.Id })));


            return listaProcessos;
        }

        private async Task<(bool sucesso, bool erro, DateTime data, string mensagem)> TratarAlteracaoAula(AlterarAulaRecorrenteCommand request, Aula aula, DateTime dataAula, Turma turma)
        {
            try
            {
                await AplicarValidacoes(request, dataAula, turma, aula);

                await AlterarAula(request, aula, dataAula, turma);
            }
            catch (NegocioException ne)
            {
                // retorna erro = false
                return (false, false, dataAula, ne.Message);
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro alterando aula recorrente", LogNivel.Negocio, LogContexto.Aula, e.Message));
                return (false, true, dataAula, e.Message);
            }

            // Retorna sucesso = true
            return (true, false, dataAula, string.Empty);
        }

        private async Task AlterarAula(AlterarAulaRecorrenteCommand request, Aula aula, DateTime dataAula, Turma turma)
        {
            aula.DataAula = dataAula;
            aula.Quantidade = request.Quantidade;
            aula.RecorrenciaAula = request.RecorrenciaAula;
            aula.DisciplinaId = request.ComponenteCurricularId.ToString();

            if (request.AulaId == aula.Id)
                aula.AulaPaiId = null;
            else
                aula.AulaPaiId = request.AulaId;

            await repositorioAula.SalvarAsync(aula);
        }

        private async Task AplicarValidacoes(AlterarAulaRecorrenteCommand request, DateTime dataAula, Turma turma, Aula aula)
        {
            await ValidarSeEhDiaLetivo(request.TipoCalendarioId, dataAula, turma);
            var codigosComponentesConsiderados = new List<long>() { request.ComponenteCurricularId };
            var aulasExistentes = await mediator.Send(new ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery(request.DataAula, request.CodigoTurma, codigosComponentesConsiderados.ToArray()));
            aulasExistentes = aulasExistentes.Where(a => a.Id != request.AulaId);
            if (aulasExistentes.NaoEhNulo() && aulasExistentes.Any(c => c.TipoAula == request.TipoAula))
                throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");

            await ValidarGrade(request, dataAula, aulasExistentes, turma, request.Quantidade - aula.Quantidade);
        }

        private async Task ValidarSeEhDiaLetivo(long tipoCalendarioId, DateTime dataAula, Turma turma)
        {
            var consultaPodeCadastrarAula = await mediator.Send(new ObterPodeCadastrarAulaPorDataQuery()
            {
                UeCodigo = turma.Ue.CodigoUe,
                DreCodigo = turma.Ue.Dre.CodigoDre,
                TipoCalendarioId = tipoCalendarioId,
                DataAula = dataAula,
                Turma = turma
            });

            if (!consultaPodeCadastrarAula.PodeCadastrar)
                throw new NegocioException(consultaPodeCadastrarAula.MensagemPeriodo);
        }

        private async Task ValidarGrade(AlterarAulaRecorrenteCommand request, DateTime dataAula, IEnumerable<AulaConsultaDto> aulasExistentes, Turma turma, int quantidadeAdicional)
        {
            var codigosComponentesConsiderados = new List<long>() { request.ComponenteCurricularId };
            var retornoValidacao = await mediator.Send(new ValidarGradeAulaCommand(turma.CodigoTurma,
                                                                                   turma.ModalidadeCodigo,
                                                                                   codigosComponentesConsiderados.ToArray(),
                                                                                   dataAula,
                                                                                   request.Usuario.CodigoRf,
                                                                                   request.Quantidade,
                                                                                   request.EhRegencia,
                                                                                   aulasExistentes));
            if (!retornoValidacao.resultado)
                throw new NegocioException(retornoValidacao.mensagem);
        }

        private async Task NotificarUsuario(long aulaId, IEnumerable<(bool sucesso, bool erro, DateTime data, string mensagem)> listaAlteracoes, Usuario usuario, string componenteCurricularNome, Turma turma)
        {
            var perfilAtual = usuario.PerfilAtual;
            if (perfilAtual == Guid.Empty)
                throw new NegocioException($"Não foi encontrado o perfil do usuário informado.");

            var tituloMensagem = $"Alteração de Aulas de {componenteCurricularNome} na turma {turma.Nome}";
            StringBuilder mensagemUsuario = new StringBuilder();

            var aulasAlteradas = listaAlteracoes.Where(c => c.sucesso);
            var aulasComConsistencia = listaAlteracoes.Where(c => !c.sucesso && !c.erro);
            var aulasComErro = listaAlteracoes.Where(c => !c.sucesso && c.erro);

            mensagemUsuario.Append($"Foram alteradas {aulasAlteradas.Count()} aulas do componente curricular {componenteCurricularNome} para a turma {turma.Nome} da {turma.Ue?.Nome} ({turma.Ue?.Dre?.Nome}).");

            if (aulasComConsistencia.Any())
            {
                mensagemUsuario.Append($"<br><br>Não foi possível alterar aulas nas seguintes datas:<br>");
                foreach (var aula in aulasComConsistencia.OrderBy(a => a.data))
                {
                    mensagemUsuario.Append($"<br /> {aula.data:dd/MM/yyyy} - {aula.mensagem}");
                }
            }

            if (aulasComErro.Any())
            {
                mensagemUsuario.Append($"<br><br>Ocorreram erros na alteração das seguintes aulas:<br>");
                foreach (var aula in aulasComErro.OrderBy(a => a.data))
                {
                    mensagemUsuario.Append($"<br /> {aula.data:dd/MM/yyyy} - {aula.mensagem};");
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
