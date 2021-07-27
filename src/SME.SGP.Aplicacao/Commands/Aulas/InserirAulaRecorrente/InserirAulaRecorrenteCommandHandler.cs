using MediatR;
using Polly;
using Polly.Registry;
using SME.GoogleClassroom.Infra;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
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
    public class InserirAulaRecorrenteCommandHandler : IRequestHandler<InserirAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoLog servicoLog;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IUnitOfWork unitOfWork;        

        private const String MSG_NAO_PODE_CRIAR_AULAS_PARA_A_TURMA = "Você não pode criar aulas para essa Turma.";

        private const String MSG_NAO_PODE_ALTERAR_NESTA_TURMA =
            "Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.";

        public InserirAulaRecorrenteCommandHandler(IMediator mediator,
                                                   IServicoEol servicoEOL,
                                                   IServicoLog servicoLog,
                                                   IRepositorioAula repositorioAula,
                                                   IRepositorioNotificacaoAula repositorioNotificacaoAula,
                                                   IServicoNotificacao servicoNotificacao,
                                                   IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));            
        }

        public async Task<bool> Handle(InserirAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            AtribuicaoEsporadica atribuicao = new AtribuicaoEsporadica();
            if (!request.Usuario.EhGestorEscolar())
                atribuicao = await ValidarComponentesProfessor(request, request.Usuario, atribuicao);
            await GerarRecorrencia(request, request.Usuario, atribuicao);
            return true;
        }

        private async Task<AtribuicaoEsporadica> ValidarComponentesProfessor(InserirAulaRecorrenteCommand aulaRecorrente,
            Usuario usuarioLogado,
            AtribuicaoEsporadica atribuicao)
        {

            if (usuarioLogado.EhProfessorCj())
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aulaRecorrente.CodigoTurma));
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuarioLogado.CodigoRf));

                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuarioLogado.CodigoRf, false, aulaRecorrente.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && atribuicoesEsporadica.Any())
                {
                    var verificaAtribuicao = atribuicoesEsporadica.FirstOrDefault(a => a.DataInicio <= aulaRecorrente.DataAula.Date && a.DataFim >= aulaRecorrente.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe);
                    if (verificaAtribuicao == null)
                        throw new NegocioException(MSG_NAO_PODE_CRIAR_AULAS_PARA_A_TURMA);

                    atribuicao = verificaAtribuicao;
                }

                await ValidaComponentesQuandoCj(aulaRecorrente, usuarioLogado);
                return atribuicao;
            }

            var obterComponentesQuery = new ObterComponentesCurricularesDoProfessorNaTurmaQuery(
                aulaRecorrente.CodigoTurma,
                usuarioLogado.Login,
                usuarioLogado.PerfilAtual);
            var componentes = await mediator.Send(obterComponentesQuery);
            if (componentes == null || componentes.All(c => c.Codigo != aulaRecorrente.ComponenteCurricularId))
            {
                throw new NegocioException(MSG_NAO_PODE_CRIAR_AULAS_PARA_A_TURMA);
            }

            var obterUsuarioQuery = new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(
                aulaRecorrente.ComponenteCurricularId,
                aulaRecorrente.CodigoTurma, aulaRecorrente.DataAula, usuarioLogado);
            var usuarioPodePersistirTurmaNaData = await mediator.Send(obterUsuarioQuery);
            if (!usuarioPodePersistirTurmaNaData)
                throw new NegocioException(MSG_NAO_PODE_ALTERAR_NESTA_TURMA);

            return atribuicao;
        }

        private async Task ValidaComponentesQuandoCj(InserirAulaRecorrenteCommand aulaRecorrente,
            Usuario usuarioLogado)
        {
            var obterComponentesQuery = new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login);
            var componentes = await mediator.Send(obterComponentesQuery);
            bool FilterComponentesCompativeis(AtribuicaoCJ c) =>
                c.TurmaId == aulaRecorrente.CodigoTurma && c.DisciplinaId == aulaRecorrente.ComponenteCurricularId;

            if (componentes == null || !componentes.Any(FilterComponentesCompativeis))
            {
                throw new NegocioException(MSG_NAO_PODE_CRIAR_AULAS_PARA_A_TURMA);
            }
        }

        private async Task GerarRecorrencia(InserirAulaRecorrenteCommand aulaRecorrente, Usuario usuario, AtribuicaoEsporadica atribuicao)
        {
            var inicioRecorrencia = aulaRecorrente.DataAula;
            var obterFimPeriodoQuery = new ObterFimPeriodoRecorrenciaQuery(
                aulaRecorrente.TipoCalendarioId,
                aulaRecorrente.DataAula,
                aulaRecorrente.RecorrenciaAula);
            var fimRecorrencia = await mediator.Send(obterFimPeriodoQuery);
            await GerarRecorrenciaParaPeriodos(aulaRecorrente, inicioRecorrencia, fimRecorrencia, usuario, atribuicao);
        }

        private async Task GerarRecorrenciaParaPeriodos(InserirAulaRecorrenteCommand aulaRecorrente, DateTime inicioRecorrencia, DateTime fimRecorrencia, Usuario usuario, AtribuicaoEsporadica atribuicao)
        {
            var diasParaIncluirRecorrencia = ObterDiasDaRecorrencia(inicioRecorrencia, fimRecorrencia);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aulaRecorrente.CodigoTurma));

            var validacaoDatas = await ValidarDatasAula(diasParaIncluirRecorrencia, aulaRecorrente.CodigoTurma, 
                aulaRecorrente.ComponenteCurricularId, aulaRecorrente.TipoCalendarioId, aulaRecorrente.EhRegencia, 
                aulaRecorrente.Quantidade, usuario, turma, atribuicao);
            var datasPersistencia = validacaoDatas.datasPersistencia;
            var mensagensValidacao = validacaoDatas.mensagensValidacao;

            var geracaoRecorrencia = await GerarAulaDeRecorrenciaParaDias(aulaRecorrente, usuario, datasPersistencia);

            // Notificar usuário da conclusão da geração de aulas
            await NotificarUsuario(geracaoRecorrencia.aula, geracaoRecorrencia.aulasQueDeramErro, mensagensValidacao, usuario, datasPersistencia.Count(), aulaRecorrente.NomeComponenteCurricular, turma);
        }

        private async Task NotificarUsuario(Aula aula, IEnumerable<(DateTime dataAula, string errorMessage)> aulasQueDeramErro, IEnumerable<string> mensagensValidacao, Usuario usuario, int quantidadeAulasCriadas, string componenteCurricularNome, Turma turma)
        {
            var perfilAtual = usuario.PerfilAtual;
            if (perfilAtual == Guid.Empty)
                throw new NegocioException($"Não foi encontrado o perfil do usuário informado.");

            var tituloMensagem = $"Criação de Aulas de {componenteCurricularNome} na turma {turma.Nome}";
            StringBuilder mensagemUsuario = new StringBuilder();

            mensagemUsuario.Append($"Foram criadas {quantidadeAulasCriadas} aulas do componente curricular {componenteCurricularNome} para a turma {turma.Nome} da {turma.Ue?.Nome} ({turma.Ue?.Dre?.Nome}).");

            if (mensagensValidacao.Any())
            {
                mensagemUsuario
                    .Append($"<br><br>Não foi possível criar aulas nas seguintes datas:<br>")
                    .Append(mensagensValidacao
                        .OrderBy(data => data)
                        .Aggregate((mensagem, aggregation) => aggregation + $"<br /> {mensagem}")
                    );
            }

            if (aulasQueDeramErro.Any())
            {
                mensagemUsuario.Append($"<br><br>Ocorreram erros na criação das seguintes aulas:<br>");
                foreach (var aulaComErro in aulasQueDeramErro.OrderBy(data => data))
                {
                    mensagemUsuario.Append($"<br /> {aulaComErro.dataAula.ToString("dd/MM/yyyy")} - {aulaComErro.errorMessage};");
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
                await servicoNotificacao.SalvarAsync(notificacao);

                // Gera vinculo Notificacao x Aula
                await repositorioNotificacaoAula.Inserir(notificacao.Id, aula.Id);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task<(IEnumerable<DateTime> datasPersistencia, IEnumerable<string> mensagensValidacao)> ValidarDatasAula(IEnumerable<DateTime> diasParaIncluirRecorrencia, string turmaCodigo, long componenteCurricularCodigo, long tipoCalendarioId, bool ehRegencia, int quantidade, Usuario usuario, Turma turma, AtribuicaoEsporadica atribuicao)
        {
            // Aulas Existentes
            var validacaoAulasExistentes = await ValidarAulaExistenteNaData(diasParaIncluirRecorrencia, turmaCodigo, componenteCurricularCodigo, usuario.EhProfessorCj());
            var datasValidas = validacaoAulasExistentes.datasValidas;

            // Grade Curricular
            var validacaoGradeCurricular = await ValidarGradeCurricular(datasValidas, turmaCodigo, componenteCurricularCodigo, ehRegencia, quantidade, usuario.CodigoRf);

            // Dias Letivos
            var validacaoDiasLetivos = await ValidarDiasLetivos(validacaoGradeCurricular.datasValidas, turma, tipoCalendarioId);

            // Atribuição Professor
            var validacaoAtribuicaoProfessor = await ValidarAtribuicaoProfessor(validacaoDiasLetivos.diasLetivos, turmaCodigo, componenteCurricularCodigo, usuario, atribuicao);

            return (validacaoAtribuicaoProfessor.datasAtribuicao,
                    validacaoAtribuicaoProfessor.mensagensValidacao
                        .Union(validacaoDiasLetivos.mensagensValidacao
                        .Union(validacaoGradeCurricular.mensagensValidacao)
                        .Union(validacaoAulasExistentes.mensagensValidacao)));
        }

        private async Task<(IEnumerable<DateTime> diasLetivos, IEnumerable<string> mensagensValidacao)> ValidarDiasLetivos(IEnumerable<DateTime> datasConsulta, Turma turma, long tipoCalendarioId)
        {
            var diasLetivos = new List<DateTime>();
            var mensagensValidacao = new List<string>();

            foreach (var dataConsulta in datasConsulta)
            {
                var consultaPodeCadastrarAula = await mediator.Send(new ObterPodeCadastrarAulaPorDataQuery()
                {
                    UeCodigo = turma.Ue.CodigoUe,
                    DreCodigo = turma.Ue.Dre.CodigoDre,
                    TipoCalendarioId = tipoCalendarioId,
                    DataAula = dataConsulta,
                    Turma = turma
                });

                if (consultaPodeCadastrarAula.PodeCadastrar)
                    diasLetivos.Add(dataConsulta);
                else
                    IncluirMensagemValidacao(dataConsulta, "Não é possível cadastrar essa aula pois a data informada está fora do período letivo.", ref mensagensValidacao);
            }

            return (diasLetivos, mensagensValidacao);
        }

        private void IncluirMensagemValidacao(DateTime data, string mensagem, ref List<string> mensagensValidacao)
        {
            mensagensValidacao.Add($"<b>{data.ToString("dd/MM/yyyy")}</b> - {mensagem};");
        }

        private async Task<(IEnumerable<DateTime> datasValidas, IEnumerable<string> mensagensValidacao)> ValidarGradeCurricular(IEnumerable<DateTime> datasConsulta, string turmaCodigo, long componenteCurricularCodigo, bool ehRegencia, int quantidade, string codigoRf)
        {
            var datasValidas = new List<DateTime>();
            var mensagensValidacao = new List<string>();

            foreach (var dataConsulta in datasConsulta)
            {
                try
                {
                    var gradeAulas = await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, componenteCurricularCodigo, dataConsulta, codigoRf, ehRegencia));
                    var quantidadeAulasRestantes = gradeAulas == null ? int.MaxValue : gradeAulas.QuantidadeAulasRestante;

                    if (gradeAulas != null)
                    {
                        if (quantidadeAulasRestantes < quantidade)
                            throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
                        if (!gradeAulas.PodeEditar && (quantidade != gradeAulas.QuantidadeAulasRestante))
                            throw new NegocioException("Quantidade de aulas não pode ser diferente do valor da grade curricular.");

                    }
                    datasValidas.Add(dataConsulta);
                }
                catch (Exception e)
                {
                    IncluirMensagemValidacao(dataConsulta, e.Message, ref mensagensValidacao);
                }
            }

            return (datasValidas, mensagensValidacao);
        }

        private async Task<(IEnumerable<DateTime> datasValidas, IEnumerable<string> mensagensValidacao)> ValidarAulaExistenteNaData(IEnumerable<DateTime> diasParaIncluirRecorrencia, string turmaCodigo, long componenteCurricularCodigo, bool professorCJ)
        {
            var mensagensValidacao = new List<string>();
            var datasComRegistro = await repositorioAula.ObterDatasAulasExistentes(diasParaIncluirRecorrencia.ToList(), turmaCodigo, componenteCurricularCodigo.ToString(), professorCJ);

            if (datasComRegistro != null && datasComRegistro.Any())
            {
                diasParaIncluirRecorrencia.Where(a => datasComRegistro.Any(d => d == a)).ToList()
                                                .ForEach(a => IncluirMensagemValidacao(a, "Já existe aula cadastrada nesta data", ref mensagensValidacao));
                return (diasParaIncluirRecorrencia.Where(a => !datasComRegistro.Any(d => d == a)),
                        mensagensValidacao);
            }

            return (datasValidas: diasParaIncluirRecorrencia, mensagensValidacao: Enumerable.Empty<string>());
        }

        private async Task<(IEnumerable<DateTime> datasAtribuicao, IEnumerable<string> mensagensValidacao)> ValidarAtribuicaoProfessor(IEnumerable<DateTime> datasValidas, string turmaCodigo, long componenteCurricularCodigo, Usuario usuario, AtribuicaoEsporadica atribuicao)
        {
            var mensagensValidacao = new List<string>();

            if (usuario.EhProfessorCj() || usuario.EhGestorEscolar())
            {
                if(String.IsNullOrEmpty(atribuicao.DreId))
                    return (datasValidas, Enumerable.Empty<string>());

                datasValidas
                    .Where(d => d.Date > atribuicao.DataFim)
                    .OrderBy(a => a.Date)
                    .ToList()
                    .ForEach(dataInvalida => IncluirMensagemValidacao(
                        dataInvalida,
                        "Este professor não pode persistir nesta turma neste dia pois não possui abrangência",
                        ref mensagensValidacao)
                    );

                var datasAtribuicaoCJ = datasValidas.Where(d => d.Date <= atribuicao.DataFim);

                return (datasAtribuicaoCJ, mensagensValidacao);
            }
            var datasAtribuicaoEOL = await mediator.Send(new ObterValidacaoPodePersistirTurmaNasDatasQuery(
                usuario.CodigoRf,
                turmaCodigo,
                datasValidas.Select(a => a.Date).ToArray(),
                componenteCurricularCodigo));


            if (datasAtribuicaoEOL == null || !datasAtribuicaoEOL.Any())
                throw new NegocioException("Não foi possível validar datas para a atribuição do professor no EOL.");

            var datasAtribuicao = datasAtribuicaoEOL
                .Where(a => a.PodePersistir)
                .Select(a => a.Data);

            datasValidas
                .Where(d => !datasAtribuicao.Any(a => a.Date == d))
                .ToList()
                .ForEach(dataInvalida => IncluirMensagemValidacao(
                    dataInvalida,
                    "Este professor não pode persistir nesta turma neste dia.",
                    ref mensagensValidacao)
                );

            return (datasAtribuicao, mensagensValidacao);
        }

        private async Task<(Aula aula, IEnumerable<(DateTime dataAula, string mensagemDeErro)> aulasQueDeramErro)> GerarAulaDeRecorrenciaParaDias(InserirAulaRecorrenteCommand aulaRecorrente, Usuario usuario, IEnumerable<DateTime> datasParaPersistencia)
        {
            var aulasQueDeramErro = new List<(DateTime dataAula, string errorMessage)>();

            var aula = GerarNovaAula(aulaRecorrente, usuario);

            foreach (var dia in datasParaPersistencia)
            {
                if (aula.Id == 0)
                {
                    await repositorioAula.SalvarAsync(aula);
                    continue;
                }

                var aulaParaAdicionar = (Aula)aula.Clone();
                aulaParaAdicionar.DataAula = dia;
                aulaParaAdicionar.AdicionarAulaPai(aula);

                try
                {
                    await repositorioAula.SalvarAsync(aulaParaAdicionar);
                }
                catch (Exception ex)
                {
                    servicoLog.Registrar(ex);
                    aulasQueDeramErro.Add((dia, $"Erro Interno: {ex.Message}"));
                }
            }

            return (aula, aulasQueDeramErro);
        }

        private Aula GerarNovaAula(InserirAulaRecorrenteCommand aulaRecorrente, Usuario usuario)
        {
            var entidadeAula = new Aula();

            entidadeAula.ProfessorRf = usuario.CodigoRf;
            entidadeAula.RecorrenciaAula = aulaRecorrente.RecorrenciaAula;

            entidadeAula.UeId = aulaRecorrente.CodigoUe;
            entidadeAula.DisciplinaId = aulaRecorrente.ComponenteCurricularId.ToString();
            entidadeAula.DisciplinaNome = aulaRecorrente.NomeComponenteCurricular;
            entidadeAula.TurmaId = aulaRecorrente.CodigoTurma;
            entidadeAula.TipoCalendarioId = aulaRecorrente.TipoCalendarioId;
            entidadeAula.DataAula = aulaRecorrente.DataAula.Date;
            entidadeAula.Quantidade = aulaRecorrente.Quantidade;
            entidadeAula.TipoAula = aulaRecorrente.TipoAula;
            entidadeAula.AulaCJ = usuario.EhProfessorCj();
            return entidadeAula;
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
    }
}
