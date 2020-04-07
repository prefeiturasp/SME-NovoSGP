using Microsoft.Extensions.Configuration;
using SME.Background.Core;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoTurmaDisciplina : IServicoFechamentoTurmaDisciplina
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;

        public ServicoFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                                IRepositorioFechamentoAluno repositorioFechamentoAluno,
                                                IRepositorioFechamentoNota repositorioFechamentoNota,
                                                IRepositorioDre repositorioDre,
                                                IRepositorioTurma repositorioTurma,
                                                IRepositorioUe repositorioUe,
                                                IServicoPeriodoFechamento servicoPeriodoFechamento,
                                                IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                IRepositorioTipoCalendario repositorioTipoCalendario,
                                                IRepositorioTipoAvaliacao repositorioTipoAvaliacao,
                                                IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
                                                IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
                                                IRepositorioParametrosSistema repositorioParametrosSistema,
                                                IRepositorioConceito repositorioConceito,
                                                IConsultasDisciplina consultasDisciplina,
                                                IConsultasFrequencia consultasFrequencia,
                                                IServicoNotificacao servicoNotificacao,
                                                IServicoPendenciaFechamento servicoPendenciaFechamento,
                                                IServicoEOL servicoEOL,
                                                IServicoUsuario servicoUsuario,
                                                IUnitOfWork unitOfWork,
                                                IConfiguration configuration)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
        }

        public async Task<AuditoriaPersistenciaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto, bool componenteSemNota = false)
        {
            var fechamentoTurmaDisciplina = MapearParaEntidade(id, entidadeDto);
            var turma = repositorioTurma.ObterTurmaComUeEDrePorCodigo(entidadeDto.TurmaId);

            // Valida periodo de fechamento
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo
                                                                , turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio
                                                                , DateTime.Now.Month > 6 ? 2 : 1);

            var ue = turma.Ue;
            var periodoFechamento = await servicoPeriodoFechamento.ObterPorTipoCalendarioDreEUe(tipoCalendario.Id, ue.Dre, ue);
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == entidadeDto.Bimestre);

            if (periodoFechamento == null || periodoFechamentoBimestre == null)
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {entidadeDto.Bimestre}º Bimestre");

            await CarregaFechamentoTurma(fechamentoTurmaDisciplina, turma, periodoFechamentoBimestre.PeriodoEscolar);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            // Valida Permissão do Professor na Turma/Disciplina
            VerificaSeProfessorPodePersistirTurma(usuarioLogado.CodigoRf, entidadeDto.TurmaId, periodoFechamentoBimestre.PeriodoEscolar.PeriodoFim);

            var fechamentoAlunos = Enumerable.Empty<FechamentoAluno>();
            // reprocessar do fechamento de componente sem nota deve atualizar a sintise de frequencia
            if (componenteSemNota && id > 0)
            {
                var disciplinaEOL = await consultasDisciplina.ObterDisciplina(fechamentoTurmaDisciplina.DisciplinaId);
                fechamentoAlunos = await AtualizaSinteseAlunos(id, periodoFechamentoBimestre.PeriodoEscolar.PeriodoFim, disciplinaEOL);
            }
            else
                // Carrega notas alunos
                fechamentoAlunos = await CarregarFechamentoAlunoENota(id, entidadeDto.NotaConceitoAlunos);

            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = await repositorioFechamentoTurma.SalvarAsync(fechamentoTurmaDisciplina.FechamentoTurma);
                fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurmaId;

                var alunos = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
                var parametroDiasAlteracao = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, turma.AnoLetivo));
                var diasAlteracao = DateTime.Today.DayOfYear - fechamentoTurmaDisciplina.CriadoEm.DayOfYear;
                var acimaDiasPermidosAlteracao = diasAlteracao > parametroDiasAlteracao;
                var alunosComNotaAlterada = "";

                await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);
                foreach (var fechamentoAluno in fechamentoAlunos)
                {
                    fechamentoAluno.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplina.Id;
                    await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);
                    foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                    {

                        fechamentoNota.FechamentoAlunoId = fechamentoAluno.Id;
                        await repositorioFechamentoNota.SalvarAsync(fechamentoNota);

                    }

                    if (!componenteSemNota)
                    {
                        var notaAlunoAlterada = entidadeDto.NotaConceitoAlunos.FirstOrDefault(n => n.CodigoAluno.Equals(fechamentoAluno.AlunoCodigo));
                        if (id > 0 && acimaDiasPermidosAlteracao && notaAlunoAlterada != null && !alunosComNotaAlterada.Contains(fechamentoAluno.AlunoCodigo))
                        {
                            var aluno = alunos.FirstOrDefault(a => a.CodigoAluno == fechamentoAluno.AlunoCodigo);
                            alunosComNotaAlterada +=$"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>";
                        }
                    }
                }

                if (alunosComNotaAlterada.Length > 0)
                {
                    var dataAtual = DateTime.Now;
                    var mensagem = $"<p>A(s) nota(s)/conceito(s) final(is) da turma {turma.Nome} da {ue.Nome} (DRE {ue.Dre.Nome}) no bimestre {entidadeDto.Bimestre} de {turma.AnoLetivo} foram alterados pelo Professor " +
                        $"{usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) em  {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} para o(s) seguinte(s) aluno(s):</p><br/>{alunosComNotaAlterada} ";
                    var listaCPs = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.CP);
                    var listaDiretores = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.Diretor);
                    var listaSupervisores = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.Supervisor);
                    var usuariosNotificacao = new List<UsuarioEolRetornoDto>();

                    if (listaCPs != null)
                        usuariosNotificacao.AddRange(listaCPs);
                    if (listaDiretores != null)
                        usuariosNotificacao.AddRange(listaDiretores);
                    if (listaSupervisores != null)
                        usuariosNotificacao.AddRange(listaSupervisores);

                    GerarNotificacaoAlteracaoLimiteDias(mensagem, fechamentoTurmaDisciplina.CriadoEm.Year, periodoFechamento.DreId, turma, usuariosNotificacao);
                }
                unitOfWork.PersistirTransacao();

                Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.GerarPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId, turma, periodoFechamentoBimestre.PeriodoEscolar, fechamentoTurmaDisciplina, usuarioLogado, componenteSemNota));

                return (AuditoriaPersistenciaDto)fechamentoTurmaDisciplina;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
        }

        private void GerarNotificacaoAlteracaoLimiteDias(string mensagem, int ano, string dreId, Turma turma, List<UsuarioEolRetornoDto> usuarios)
        {
            foreach (var usuarioNotificacaoo in usuarios)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioNotificacaoo.CodigoRf);
                var notificacao = new Notificacao()
                {
                    Ano = ano,
                    Categoria = NotificacaoCategoria.Alerta,
                    DreId = dreId,
                    Mensagem = mensagem,
                    UsuarioId = usuario.Id,
                    Tipo = NotificacaoTipo.Notas,
                    Titulo = $"Alteração em nota/conceito final - Turma {turma.Nome}",
                    TurmaId = turma.Id.ToString(),
                    UeId = turma.UeId.ToString(),
                };
                servicoNotificacao.Salvar(notificacao);
            }
        }

        private async Task CarregaFechamentoTurma(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, Turma turma, PeriodoEscolar periodoEscolar)
        {
            if (fechamentoTurmaDisciplina.Id > 0)
            {
                // Alterando registro de fechamento
                fechamentoTurmaDisciplina.FechamentoTurma.Turma = turma;
                fechamentoTurmaDisciplina.FechamentoTurma.TurmaId = turma.Id;
                fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolar = periodoEscolar;
                fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId = periodoEscolar.Id;
            }
            else
            {
                // Incluindo registro de fechamento turma disciplina

                // Busca registro existente de fechamento da turma
                var fechamentoTurma = await repositorioFechamentoTurma.ObterPorTurmaPeriodo(turma.Id, periodoEscolar.Id);
                if (fechamentoTurma == null)
                    fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
            }

        }

        private async Task<IEnumerable<FechamentoAluno>> AtualizaSinteseAlunos(long fechamentoTurmaDisciplinaId, DateTime dataReferencia, DisciplinaDto disciplina)
        {
            var fechamentoAlunos = await repositorioFechamentoAluno.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId);
            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                {
                    var frequencia = consultasFrequencia.ObterPorAlunoDisciplinaData(fechamentoAluno.AlunoCodigo, fechamentoNota.DisciplinaId.ToString(), dataReferencia);
                    var sinteseDto = consultasFrequencia.ObterSinteseAluno(frequencia.PercentualFrequencia, disciplina);

                    fechamentoNota.SinteseId = (long)sinteseDto.SinteseId;
                }
            }

            return fechamentoAlunos;
        }

        public async Task GerarPendenciasFechamento(long disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, FechamentoTurmaDisciplina fechamento, Usuario usuarioLogado, bool componenteSemNota = false)
        {
            var situacaoFechamento = SituacaoFechamento.ProcessadoComSucesso;

            if (!componenteSemNota)
            {
                servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(fechamento.Id, turma.CodigoTurma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
                servicoPendenciaFechamento.ValidarPercentualAlunosAbaixoDaMedia(fechamento);
            }
            servicoPendenciaFechamento.ValidarAulasReposicaoPendente(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            servicoPendenciaFechamento.ValidarAulasSemPlanoAulaNaDataDoFechamento(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            servicoPendenciaFechamento.ValidarAulasSemFrequenciaRegistrada(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);

            var quantidadePendencias = servicoPendenciaFechamento.ObterQuantidadePendenciasGeradas();
            if (quantidadePendencias > 0)
            {
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;
                GerarNotificacaoFechamento(fechamento, turma, quantidadePendencias, usuarioLogado, periodoEscolar);
            }

            fechamento.AtualizarSituacao(situacaoFechamento);
            await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamento);
        }

        public async Task Reprocessar(long fechamentoTurmaDisciplinaId)
        {
            var fechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina.ObterPorId(fechamentoTurmaDisciplinaId);
            if (fechamentoTurmaDisciplina == null)
                throw new NegocioException("Fechamento ainda não realizado para essa turma.");

            var turma = repositorioTurma.ObterTurmaComUeEDrePorId(fechamentoTurmaDisciplina.FechamentoTurma.TurmaId);
            if (turma == null)
                throw new NegocioException("Turma não encontrada.");

            var disciplinaEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { fechamentoTurmaDisciplina.DisciplinaId }).FirstOrDefault();
            if (disciplinaEOL == null)
                throw new NegocioException("Componente Curricular não localizado.");

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId.Value);
            if (periodoEscolar == null)
                throw new NegocioException("Período escolar não encontrado.");

            fechamentoTurmaDisciplina.AdicionarPeriodoEscolar(periodoEscolar);
            fechamentoTurmaDisciplina.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            repositorioFechamentoTurmaDisciplina.Salvar(fechamentoTurmaDisciplina);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.GerarPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId, turma, periodoEscolar, fechamentoTurmaDisciplina, usuarioLogado, !disciplinaEOL.LancaNota));
        }

        private void GerarNotificacaoFechamento(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, Turma turma, int quantidadePendencias, Usuario usuarioLogado, PeriodoEscolar periodoEscolar)
        {
            var componentes = servicoEOL.ObterDisciplinasPorIds(new long[] { fechamentoTurmaDisciplina.DisciplinaId });
            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Componente curricular não encontrado.");
            }
            var ue = turma.Ue;
            if (ue == null)
                throw new NegocioException("UE não encontrada.");

            var dre = ue.Dre;
            if (dre == null)
                throw new NegocioException("DRE não encontrada.");

            var urlFrontEnd = configuration["UrlFrontEnd"];
            if (string.IsNullOrWhiteSpace(urlFrontEnd))
                throw new NegocioException("Url do frontend não encontrada.");

            var notificacao = new Notificacao()
            {
                UsuarioId = usuarioLogado.Id,
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Aviso,
                Titulo = $"Pendência no fechamento da turma {turma.Nome}",
                Tipo = NotificacaoTipo.Fechamento,
                Mensagem = $"O fechamento do {periodoEscolar.Bimestre}º bimestre de {componentes.FirstOrDefault().Nome} da turma {turma.Nome} da {ue.Nome} ({dre.Nome}) gerou {quantidadePendencias} pendência(s). " +
                $"Clique <a href='{urlFrontEnd}fechamento/pendencias-fechamento/{periodoEscolar.Bimestre}/{fechamentoTurmaDisciplina.DisciplinaId}'>aqui</a> para mais detalhes."
            };
            servicoNotificacao.Salvar(notificacao);

            var diretores = servicoEOL.ObterFuncionariosPorCargoUe(ue.CodigoUe, (long)Cargo.Diretor);

            if (diretores != null)
            {
                foreach (var diretor in diretores)
                {
                    var notificacaoDiretor = notificacao;
                    notificacaoDiretor.Id = 0;
                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);
                    notificacaoDiretor.UsuarioId = usuario.Id;
                    servicoNotificacao.Salvar(notificacaoDiretor);
                }
            }

            var cps = servicoEOL.ObterFuncionariosPorCargoUe(ue.CodigoUe, (long)Cargo.CP);

            if (cps != null)
            {
                foreach (var cp in cps)
                {
                    var notificacaoCp = notificacao;
                    notificacaoCp.Id = 0;
                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cp.CodigoRf);
                    notificacaoCp.UsuarioId = usuario.Id;
                    servicoNotificacao.Salvar(notificacaoCp);
                }
            }
        }

        private void VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime data)
        {
            if (!servicoUsuario.PodePersistirTurma(codigoRf, turmaId, data).Result)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma e data.");
        }

        private async Task<IEnumerable<FechamentoAluno>> CarregarFechamentoAlunoENota(long fechamentoTurmaDisciplinaId, IEnumerable<FechamentoNotaDto> fechamentoNotasDto)
        {
            var fechamentoAlunos = new List<FechamentoAluno>();

            if (fechamentoTurmaDisciplinaId > 0)
                fechamentoAlunos = (await repositorioFechamentoAluno.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId)).ToList();

            // Edita as notas existentes
            foreach (var agrupamentoNotasAluno in fechamentoNotasDto.GroupBy(g => g.CodigoAluno))
            {
                // Busca fechamento do aluno
                var fechamentoAluno = fechamentoAlunos.FirstOrDefault(c => c.AlunoCodigo == agrupamentoNotasAluno.Key);
                if (fechamentoAluno == null)
                    fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoNotasAluno.Key, FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId };

                foreach (var fechamentoNotaDto in agrupamentoNotasAluno)
                {
                    // busca nota do aluno
                    var notaFechamento = fechamentoAluno.FechamentoNotas.FirstOrDefault(x => x.DisciplinaId == fechamentoNotaDto.DisciplinaId);
                    if (notaFechamento != null)
                    {
                        notaFechamento.Nota = fechamentoNotaDto.Nota;
                        notaFechamento.ConceitoId = fechamentoNotaDto.ConceitoId;
                        notaFechamento.SinteseId = fechamentoNotaDto.SinteseId;
                    }
                    else
                        fechamentoAluno.AdicionarNota(MapearParaEntidade(fechamentoNotaDto));
                }
                fechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoAlunos;
        }

        private FechamentoNota MapearParaEntidade(FechamentoNotaDto fechamentoNotaDto)
            => fechamentoNotaDto == null ? null :
              new FechamentoNota()
              {
                  DisciplinaId = fechamentoNotaDto.DisciplinaId,
                  Nota = fechamentoNotaDto.Nota,
                  ConceitoId = fechamentoNotaDto.ConceitoId,
                  SinteseId = fechamentoNotaDto.SinteseId
              };

        private FechamentoTurmaDisciplina MapearParaEntidade(long id, FechamentoTurmaDisciplinaDto fechamentoDto)
        {
            var fechamento = new FechamentoTurmaDisciplina();
            if (id > 0)
                fechamento = repositorioFechamentoTurmaDisciplina.ObterPorId(id);

            fechamento.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            fechamento.DisciplinaId = fechamentoDto.DisciplinaId;
            fechamento.Justificativa = fechamentoDto.Justificativa;

            return fechamento;
        }

        private async Task<string> ValidaMinimoAvaliacoesBimestre(long tipoCalendarioId, string turmaId, long disciplinaId, int bimestre)
        {
            var validacoes = new StringBuilder();
            var tipoAvaliacaoBimestral = await repositorioTipoAvaliacao.ObterTipoAvaliacaoBimestral();

            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId });

            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Não foi possível localizar a disciplina no EOL.");

            if (disciplinasEOL.First().Regencia)
            {
                // Disciplinas Regencia de Classe
                disciplinasEOL = await consultasDisciplina.ObterDisciplinasParaPlanejamento(new FiltroDisciplinaPlanejamentoDto()
                {
                    CodigoTurma = long.Parse(turmaId),
                    CodigoDisciplina = disciplinaId,
                    Regencia = true
                });

                foreach (var disciplina in disciplinasEOL)
                {
                    var avaliacoes = await repositorioAtividadeAvaliativaRegencia.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaId, disciplina.CodigoComponenteCurricular.ToString(), bimestre);
                    if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        validacoes.AppendLine($"A disciplina [{disciplina.Nome}] não tem o número mínimo de avaliações bimestrais: Necessário {tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre}");
                }
            }
            else
            {
                var disciplinaEOL = disciplinasEOL.First();
                var avaliacoes = await repositorioAtividadeAvaliativaDisciplina.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaId, disciplinaEOL.CodigoComponenteCurricular.ToString(), bimestre);
                if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    validacoes.AppendLine($"A disciplina [{disciplinaEOL.Nome}] não tem o número mínimo de avaliações bimestrais: Necessário {tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre}");
            }

            return validacoes.ToString();
        }

        public void VerificaPendenciasFechamento(long fechamentoId)
        {
            // Verifica existencia de pendencia em aberto
            if (!servicoPendenciaFechamento.VerificaPendenciasFechamento(fechamentoId))
            {
                var fechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina.ObterPorId(fechamentoId);
                // Atualiza situação do fechamento
                fechamentoTurmaDisciplina.Situacao = SituacaoFechamento.ProcessadoComSucesso;
                repositorioFechamentoTurmaDisciplina.Salvar(fechamentoTurmaDisciplina);
            }
        }
    }
}