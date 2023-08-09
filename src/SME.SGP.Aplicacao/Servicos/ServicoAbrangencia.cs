﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoAbrangencia : IServicoAbrangencia
    {
        private readonly IConsultasSupervisor consultasSupervisor;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioCicloEnsino repositorioCicloEnsino;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioTipoEscola repositorioTipoEscola;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoEol servicoEOL;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioUsuarioConsulta repositorioUsuario;

        public ServicoAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IUnitOfWork unitOfWork, IServicoEol servicoEOL, IConsultasSupervisor consultasSupervisor,
            IRepositorioDre repositorioDre, IRepositorioUe repositorioUe, IRepositorioTurma repositorioTurma, IRepositorioCicloEnsino repositorioCicloEnsino, IRepositorioTipoEscola repositorioTipoEscola,
            IMediator mediator, IRepositorioUsuarioConsulta repositorioUsuario)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasSupervisor = consultasSupervisor ?? throw new ArgumentNullException(nameof(consultasSupervisor));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoEscola = repositorioTipoEscola ?? throw new ArgumentNullException(nameof(repositorioTipoEscola));
            this.repositorioCicloEnsino = repositorioCicloEnsino ?? throw new ArgumentNullException(nameof(repositorioCicloEnsino));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public bool DreEstaNaAbrangencia(string login, Guid perfilId, string codigoDre)
        {
            if (string.IsNullOrWhiteSpace(login) || perfilId == Guid.Empty || string.IsNullOrWhiteSpace(codigoDre))
                throw new NegocioException("É necessário informar login, perfil e código da DRE");

            var dres = repositorioAbrangencia
                .ObterDres(login, perfilId).Result;

            return dres.Any(dre => dre.Codigo.Equals(codigoDre, StringComparison.InvariantCultureIgnoreCase));
        }

        public void RemoverAbrangencias(long[] ids)
        {
            repositorioAbrangencia.ExcluirAbrangencias(ids);
        }

        public async Task RemoverAbrangenciasHistoricas(long[] ids)
        {
            await repositorioAbrangencia.ExcluirAbrangenciasHistoricas(ids);
        }

        public async Task RemoverAbrangenciasHistoricasIncorretas(string login, List<Guid> perfis)
        {
            var abrangenciasHistorica = await ObterAbrangenciaHistorica(login);
            long[] idsRemover = abrangenciasHistorica
                .Where(a => a.perfil != Perfis.PERFIL_PAEE
                            && a.perfil != Perfis.PERFIL_PAP
                            && a.perfil != Perfis.PERFIL_PROFESSOR
                            && a.perfil != Perfis.PERFIL_CJ
                            && a.perfil != Perfis.PERFIL_POED
                            && a.perfil != Perfis.PERFIL_POSL
                            && a.perfil != Perfis.PERFIL_PROFESSOR_INFANTIL
                            && a.perfil != Perfis.PERFIL_CJ_INFANTIL
                            && !perfis.Contains(a.perfil)
                ).Select(a => a.Id).ToArray();

            if (idsRemover.Any())
                await RemoverAbrangenciasHistoricas(idsRemover);
        }

        public async Task<IEnumerable<AbrangenciaHistoricaDto>> ObterAbrangenciaHistorica(string login)
        {
            return await repositorioAbrangencia.ObterAbrangenciaHistoricaPorLogin(login);
        }

        public async Task Salvar(string login, Guid perfil, bool ehLogin)
        {
            if (ehLogin)
            {
                await TrataAbrangenciaLogin(login, perfil);
            }
            else
            {
                await TrataAbrangenciaModificaoPerfil(login, perfil);
            }
        }

        public void SalvarAbrangencias(IEnumerable<Abrangencia> abrangencias, string login)
        {
            repositorioAbrangencia.InserirAbrangencias(abrangencias, login);
        }

        public async Task SincronizarEstruturaInstitucionalVigenteCompleta()
        {
            EstruturaInstitucionalRetornoEolDTO estruturaInstitucionalVigente;

            try
            {
                estruturaInstitucionalVigente = servicoEOL.ObterEstruturaInstuticionalVigentePorDre();
            }
            catch (Exception ex)
            {
                throw new NegocioException($"Erro ao obter estrutura organizacional vigente no EOL. Detalhe: {ex}");
            }

            if (estruturaInstitucionalVigente != null && estruturaInstitucionalVigente.Dres != null && estruturaInstitucionalVigente.Dres.Count > 0)
                await SincronizarEstruturaInstitucional(estruturaInstitucionalVigente);
            else
            {
                var erro = $"Não foi possível obter dados de estrutura institucional do EOL. {estruturaInstitucionalVigente?.Dres?.Count}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Abrangencia, string.Empty));

                throw new NegocioException(erro);
            }

            var tiposEscolas = servicoEOL.BuscarTiposEscola();
            if (tiposEscolas.Any())
            {
                SincronizarTiposEscola(tiposEscolas);
            }
            else
            {
                var erro = "Não foi possível obter dados de tipos de escolas do EOL";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Abrangencia, string.Empty));

                throw new NegocioException(erro);
            }

            var ciclos = servicoEOL.BuscarCiclos();
            if (ciclos.Any())
            {
                SincronizarCiclos(ciclos);
            }
            else
            {
                var erro = "Não foi possível obter dados de ciclos de ensino do EOL";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Abrangencia, string.Empty));

                throw new NegocioException(erro);
            }
        }

        public bool UeEstaNaAbrangecia(string login, Guid perfilId, string codigoDre, string codigoUE)
        {
            if (string.IsNullOrWhiteSpace(login) || perfilId == Guid.Empty || string.IsNullOrWhiteSpace(codigoDre) || string.IsNullOrWhiteSpace(codigoUE))
                throw new NegocioException("É necessário informar login, perfil, código da DRE e da UE");

            var ues = repositorioAbrangencia
                .ObterUes(codigoDre, login, perfilId).Result;

            return ues.Any(dre => dre.Codigo.Equals(codigoUE, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> SincronizarAbrangenciaHistorica(int anoLetivo, string professorRf, long turmaId = 0)
        {
            try
            {
                var turmasHistoricasEOL = await mediator.Send(new ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQuery(anoLetivo, professorRf));

                var usuario = await repositorioUsuario.ObterUsuarioPorCodigoRfAsync(professorRf);

                if (usuario == null)
                    throw new NegocioException("Usuário não encontrado no SGP");

                var abrangenciaGeralSGP = await repositorioAbrangencia.ObterAbrangenciaGeralPorUsuarioId(usuario.Id);

                if (anoLetivo == DateTimeExtension.HorarioBrasilia().Year)
                {
                    List<Abrangencia> abrangenciaTurmasHistoricasEOL = new List<Abrangencia>();

                    foreach (AbrangenciaTurmaRetornoEolDto turma in turmasHistoricasEOL)
                    {
                        Abrangencia abrangencia = new Abrangencia();

                        var turmaSGP = await mediator.Send(new ObterTurmaPorCodigoQuery(turma.Codigo));
                        if (turmaSGP == null)
                            throw new NegocioException($"Turma não encontrada no SGP - [{turma.Codigo} - {turma.NomeTurma}]");

                        abrangencia.DreId = turmaSGP.Ue.DreId;
                        abrangencia.UeId = turmaSGP.Ue.Id;
                        abrangencia.UsuarioId = usuario.Id;
                        abrangencia.TurmaId = turmaSGP.Id;
                        abrangencia.Perfil = ((Modalidade) int.Parse(turma.CodigoModalidade) == Modalidade.EducacaoInfantil) ? Perfis.PERFIL_PROFESSOR_INFANTIL : Perfis.PERFIL_PROFESSOR;

                        abrangenciaTurmasHistoricasEOL.Add(abrangencia);
                    }

                    var novas = abrangenciaTurmasHistoricasEOL.Where(ath => !abrangenciaGeralSGP.Any(x => ath.DreId == x.DreId && ath.UeId == x.UeId && ath.TurmaId == x.TurmaId && ath.UsuarioId == x.UsuarioId));

                    repositorioAbrangencia.InserirAbrangencias(novas, usuario.Login);

                    abrangenciaGeralSGP = await repositorioAbrangencia.ObterAbrangenciaGeralPorUsuarioId(usuario.Id);

                    var paraAtualizar = abrangenciaGeralSGP.Where(x => abrangenciaTurmasHistoricasEOL.Any(ath => ath.DreId == x.DreId && ath.UeId == x.UeId && ath.TurmaId == x.TurmaId && ath.UsuarioId == x.UsuarioId));


                    repositorioAbrangencia.AtualizaAbrangenciaHistorica(paraAtualizar.Select(x => x.Id));
                }
                else
                {
                    var paraAtualizarAbrangencia = new List<Abrangencia>();

                    if (turmaId > 0)
                    {
                        var abragenciaSGP = abrangenciaGeralSGP.Where(a => a.TurmaId == turmaId && !a.Historico).FirstOrDefault();
                        if (abragenciaSGP != null)
                        {
                            var virouHistorica = await mediator.Send(new VerificaSeTurmaVirouHistoricaQuery(abragenciaSGP.TurmaId.Value));
                            if (virouHistorica && !abragenciaSGP.Historico)
                                paraAtualizarAbrangencia.Add(abragenciaSGP);
                        }

                        repositorioAbrangencia.AtualizaAbrangenciaHistoricaAnosAnteriores(paraAtualizarAbrangencia.Select(x => x.Id), anoLetivo);
                    }
                }


                return true;
            }
            catch (Exception e)
            {
                var erro = $"Erro ao sincronizar abrangência histórica SGP - Chamada SincronizarAbrangenciaHistorica - anoLetivo: {anoLetivo}, professorRf {professorRf} - erro: {e.Message}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Abrangencia, string.Empty));

                throw new NegocioException($"Erro ao sincronizar abrangência histórica - Ano({anoLetivo}), RF({professorRf})");
            }
        }

        public async Task<IEnumerable<string>> ObterLoginsAbrangenciaUePorPerfil(long ueId, Guid perfil, bool historica = false)
        {
            var ue = await mediator.Send(new ObterUePorIdQuery(ueId));

            if (ue == null)
                throw new NegocioException("UE não localizada.");

            return await repositorioAbrangencia
                .ObterLoginsAbrangenciaUePorPerfil(ueId, perfil, historica);
        }

        private async Task BuscaAbrangenciaEPersiste(string login, Guid perfil)
        {
            Task<AbrangenciaCompactaVigenteRetornoEOLDTO> consultaEol = null;
            AbrangenciaCompactaVigenteRetornoEOLDTO abrangenciaEol = null;

            var ehSupervisor = perfil == Perfis.PERFIL_SUPERVISOR;
            var ehProfessorCJ = perfil == Perfis.PERFIL_CJ || perfil == Perfis.PERFIL_CJ_INFANTIL;

            if (ehSupervisor)
            {
                var uesIds = await ObterAbrangenciaEolSupervisor(login);
                if (!uesIds.Any())
                    return;
                var abrangenciaSupervisor = await servicoEOL.ObterAbrangenciaParaSupervisor(uesIds.ToArray());
                abrangenciaEol = new AbrangenciaCompactaVigenteRetornoEOLDTO()
                {
                    Abrangencia = abrangenciaSupervisor.Abrangencia,
                    IdUes = abrangenciaSupervisor.Dres.SelectMany(x => x.Ues.Select(y => y.Codigo)).ToArray()
                };
            }
            else if (ehProfessorCJ)
                return;
            else
                consultaEol = servicoEOL.ObterAbrangenciaCompactaVigente(login, perfil);

            if (consultaEol != null || abrangenciaEol != null)
            {
                // Enquanto o EOl consulta, tentamos ganhar tempo obtendo a consulta sintetica
                var consultaAbrangenciaSintetica = repositorioAbrangencia.ObterAbrangenciaSintetica(login, perfil, string.Empty);

                if (abrangenciaEol == null)
                    abrangenciaEol = await consultaEol;
                var abrangenciaSintetica = await consultaAbrangenciaSintetica;

                if (abrangenciaEol != null)
                {
                    IEnumerable<Dre> dres = Enumerable.Empty<Dre>();
                    IEnumerable<Ue> ues = Enumerable.Empty<Ue>();
                    IEnumerable<Turma> turmas = Enumerable.Empty<Turma>();

                    // sincronizamos as dres, ues e turmas
                    var estrutura = await MaterializarEstruturaInstitucional(abrangenciaEol, dres, ues, turmas);

                    dres = estrutura.Item1;
                    ues = estrutura.Item2;
                    turmas = estrutura.Item3;

                    // sincronizamos a abrangencia do login + perfil
                    await unitOfWork.IniciarTransacaoAsync();

                    SincronizarAbrangencia(abrangenciaSintetica, abrangenciaEol.Abrangencia?.Abrangencia, ehSupervisor, dres, ues, turmas, login, perfil);

                    await unitOfWork.PersistirTransacaoAsync();

                    //nao tem rollback essa logica ?
                }
            }
        }

        private async Task<IEnumerable<Turma>> ImportarTurmasNaoEncontradas(string[] codigosNaoEncontrados)
        {
            if (codigosNaoEncontrados != null && codigosNaoEncontrados.Length > 0)
            {
                var turmasEol = servicoEOL.ObterEstruturaInstuticionalVigentePorTurma(codigosTurma: codigosNaoEncontrados);
                if (turmasEol != null)
                    await SincronizarEstruturaInstitucional(turmasEol);
            }

            return repositorioTurma.MaterializarCodigosTurma(codigosNaoEncontrados, out codigosNaoEncontrados);
        }

        private async Task<Tuple<IEnumerable<Dre>, IEnumerable<Ue>, IEnumerable<Turma>>> MaterializarEstruturaInstitucional(AbrangenciaCompactaVigenteRetornoEOLDTO abrangenciaEol, IEnumerable<Dre> dres, IEnumerable<Ue> ues, IEnumerable<Turma> turmas)
        {
            string[] codigosNaoEncontrados;

            if (abrangenciaEol.IdDres != null && abrangenciaEol.IdDres.Length > 0)
            {
                var retorno = await mediator.Send(new ObterDreMaterializarCodigosQuery(abrangenciaEol.IdDres));
                dres = retorno.Item1;
                codigosNaoEncontrados = retorno.Item2;
            }

            if (abrangenciaEol.IdUes != null && abrangenciaEol.IdUes.Length > 0)
            {
                var retorno = await mediator.Send(new ObterUeMaterializarCodigosQuery(abrangenciaEol.IdUes));
                ues = retorno.Item1;
                codigosNaoEncontrados = retorno.Item2;
            }

            if (abrangenciaEol.IdTurmas != null && abrangenciaEol.IdTurmas.Length > 0)
            {
                turmas = repositorioTurma.MaterializarCodigosTurma(abrangenciaEol.IdTurmas, out codigosNaoEncontrados)
                    .Union(await ImportarTurmasNaoEncontradas(codigosNaoEncontrados));
            }

            return new Tuple<IEnumerable<Dre>, IEnumerable<Ue>, IEnumerable<Turma>>(dres, ues, turmas);
        }

        private async Task<string[]> ObterAbrangenciaEolSupervisor(string login)
        {
            var listaEscolasDresSupervior = await consultasSupervisor.ObterPorDreESupervisor(login, string.Empty);

            if (listaEscolasDresSupervior.Any())
                return listaEscolasDresSupervior.Select(escola => escola.UeId).ToArray();
           
            return Array.Empty<string>();
        }


        private IEnumerable<AbrangenciaSinteticaDto> RemoverAbrangenciaSinteticaDuplicada(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica)
        {
            var retorno = new List<AbrangenciaSinteticaDto>();
            var abrangencia = abrangenciaSintetica.GroupBy(x => x.CodigoTurma).Select(y => y.OrderBy(a => a.CodigoTurma));
            foreach (var item in abrangencia)
            { 
                retorno.Add(item.FirstOrDefault());
            }

            return retorno;
        }

        private void SincronizarAbragenciaPorTurmas(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Turma> turmas, string login, Guid perfil)
        {
            abrangenciaSintetica = RemoverAbrangenciaSinteticaDuplicada(abrangenciaSintetica);
            var abr = abrangenciaSintetica.GroupBy(x => x.CodigoTurma).Select(y => y.OrderBy(a => a.CodigoTurma));
            var idsParaAtualizar = new List<long>();

            if (!turmas.Any() && abrangenciaSintetica.Any())
                idsParaAtualizar = abrangenciaSintetica.Select(x => x.Id).ToList();

            var novas = turmas.Where(x => !abrangenciaSintetica.Select(y => y.TurmaId).Contains(x.Id));

            var paraAtualizar = abrangenciaSintetica.GroupBy(x => x.CodigoTurma).SelectMany(y => y.OrderBy(a => a.CodigoTurma).Take(1));

            var listaAbrangenciaSintetica = new List<AbrangenciaSinteticaDto>();
            var listaParaAtualizar = new List<AbrangenciaSinteticaDto>();

            listaAbrangenciaSintetica.AddRange(abrangenciaSintetica.ToList());
            listaParaAtualizar.AddRange(paraAtualizar.ToList());
            var registrosDuplicados = listaAbrangenciaSintetica.Except(listaParaAtualizar);
            
            if(registrosDuplicados.Any())
                idsParaAtualizar = registrosDuplicados.Select(x => x.Id).ToList();

            if(abrangenciaSintetica.Any() && turmas.Any())
                if(abrangenciaSintetica.Count() != turmas.Count())
                    idsParaAtualizar.AddRange(VerificaTurmasAbrangenciaAtualParaHistorica(abrangenciaSintetica, turmas));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() {Perfil = perfil, TurmaId = x.Id}), login);

            repositorioAbrangencia.AtualizaAbrangenciaHistorica(idsParaAtualizar);
        }

        public IEnumerable<long> VerificaTurmasAbrangenciaAtualParaHistorica(IEnumerable<AbrangenciaSinteticaDto> abrangenciaAtual, IEnumerable<Turma> turmasAbrangenciaEol)
        {
            var turmasNaAbrangenciaAtualExistentesEol = from ta in turmasAbrangenciaEol
                                                        join aa in abrangenciaAtual
                                                        on ta.Id equals aa.TurmaId into turmasIguais
                                                        from tI in turmasIguais.DefaultIfEmpty()
                                                        select tI;

            return abrangenciaAtual.Except(turmasNaAbrangenciaAtualExistentesEol).Select(t=> t.Id);
        }

        private void SincronizarAbrangencia(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, Infra.Enumerados.Abrangencia? abrangencia, bool ehSupervisor, IEnumerable<Dre> dres, IEnumerable<Ue> ues, IEnumerable<Turma> turmas, string login, Guid perfil)
        {
            if (ehSupervisor)
                SincronizarAbrangenciaPorUes(abrangenciaSintetica, ues, login, perfil);
            else
            {
                switch (abrangencia)
                {
                    case Infra.Enumerados.Abrangencia.Dre:
                    case Infra.Enumerados.Abrangencia.SME:
                        SincronizarAbrangenciPorDres(abrangenciaSintetica, dres, login, perfil);
                        break;

                    case Infra.Enumerados.Abrangencia.DreEscolasAtribuidas:
                    case Infra.Enumerados.Abrangencia.UeTurmasDisciplinas:
                    case Infra.Enumerados.Abrangencia.UE:
                        SincronizarAbrangenciaPorUes(abrangenciaSintetica, ues, login, perfil);
                        break;

                    case Infra.Enumerados.Abrangencia.Professor:
                        SincronizarAbragenciaPorTurmas(abrangenciaSintetica, turmas, login, perfil);
                        break;
                }
            }
        }

        private void SincronizarAbrangenciaPorUes(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Ue> ues, string login, Guid perfil)
        {
            var novas = ues.Where(x => !abrangenciaSintetica.Select(y => y.UeId).Contains(x.Id));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() {Perfil = perfil, UeId = x.Id}), login);

            var paraAtualizar = abrangenciaSintetica.Where(x => !ues.Select(y => y.Id).Contains(x.UeId));

            var perfisHistorico = paraAtualizar.Where(x => x.EhPerfilProfessor()).Select(x => x.Id);

            repositorioAbrangencia.AtualizaAbrangenciaHistorica(perfisHistorico);

            var perfisGestao = paraAtualizar.Where(x => !x.EhPerfilProfessor()).Select(x => x.Id);

            repositorioAbrangencia.ExcluirAbrangencias(perfisGestao);
        }

        private void SincronizarAbrangenciPorDres(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Dre> dres, string login, Guid perfil)
        {
            var novas = dres.Where(x => !abrangenciaSintetica.Select(y => y.DreId).Contains(x.Id));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() {Perfil = perfil, DreId = x.Id}), login);

            var paraAtualizar = abrangenciaSintetica.Where(x => !dres.Select(y => y.Id).Contains(x.DreId));

            var perfisHistorico = paraAtualizar.Where(x => x.EhPerfilProfessor()).Select(x => x.Id);

            repositorioAbrangencia.AtualizaAbrangenciaHistorica(perfisHistorico);

            var perfisGestao = paraAtualizar.Where(x => !x.EhPerfilProfessor()).Select(x => x.Id);

            repositorioAbrangencia.ExcluirAbrangencias(perfisGestao);
        }

        private void SincronizarCiclos(IEnumerable<CicloRetornoDto> ciclos)
        {
            IEnumerable<CicloEnsino> ciclosEnsino = ciclos.Select(x =>
                new CicloEnsino
                {
                    Descricao = x.Descricao,
                    DtAtualizacao = x.DtAtualizacao,
                    CodEol = x.Codigo,
                    CodigoEtapaEnsino = x.CodigoEtapaEnsino,
                    CodigoModalidadeEnsino = x.CodigoModalidadeEnsino
                });

            repositorioCicloEnsino.Sincronizar(ciclosEnsino);
        }

        private async Task SincronizarEstruturaInstitucional(EstruturaInstitucionalRetornoEolDTO estrutura)
        {
            var dres = estrutura.Dres.Select(x => new Dre() {Abreviacao = x.Abreviacao, CodigoDre = x.Codigo, Nome = x.Nome});
            var ues = estrutura.Dres.SelectMany(x => x.Ues.Select(y => new Ue {CodigoUe = y.Codigo, TipoEscola = y.CodTipoEscola, Nome = y.Nome, Dre = new Dre() {CodigoDre = x.Codigo}}));
            var turmas = estrutura.Dres.SelectMany(x => x.Ues.SelectMany(y => y.Turmas.Select(z =>
                new Turma
                {
                    Ano = z.Ano,
                    AnoLetivo = z.AnoLetivo,
                    CodigoTurma = z.Codigo,
                    //Para turma do tipo 7 (Itinerarios 2A Ano) a modalidade é definida como Médio
                    ModalidadeCodigo = z.TipoTurma == Dominio.Enumerados.TipoTurma.Itinerarios2AAno ? Modalidade.Medio : (Modalidade) Convert.ToInt32(z.CodigoModalidade),
                    QuantidadeDuracaoAula = z.DuracaoTurno,
                    Nome = z.NomeTurma,
                    Semestre = z.Semestre,
                    TipoTurno = z.TipoTurno,
                    Ue = new Ue() {CodigoUe = y.Codigo},
                    EnsinoEspecial = z.EnsinoEspecial,
                    EtapaEJA = z.EtapaEJA,
                    DataInicio = z.DataInicioTurma,
                    SerieEnsino = z.SerieEnsino,
                    DataFim = z.DataFim,
                    Extinta = z.Extinta,
                    TipoTurma = z.TipoTurma
                })));

            dres = await repositorioDre.SincronizarAsync(dres);
            ues = await repositorioUe.SincronizarAsync(ues, dres);
            await repositorioTurma.SincronizarAsync(turmas, ues);
        }

        private void SincronizarTiposEscola(IEnumerable<TipoEscolaRetornoDto> tiposEscolasDto)
        {
            IEnumerable<TipoEscolaEol> tiposEscolas = tiposEscolasDto.Select(x =>
                new TipoEscolaEol
                {
                    Descricao = x.DescricaoSigla,
                    DtAtualizacao = x.DtAtualizacao,
                    CodEol = x.Codigo
                });

            repositorioTipoEscola.Sincronizar(tiposEscolas);
        }

        private async Task TrataAbrangenciaLogin(string login, Guid perfil)
        {
            await BuscaAbrangenciaEPersiste(login, perfil);
        }

        private async Task TrataAbrangenciaModificaoPerfil(string login, Guid perfil)
        {
            await BuscaAbrangenciaEPersiste(login, perfil);
        }
    }
}