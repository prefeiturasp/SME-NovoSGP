using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
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

        public ServicoAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IUnitOfWork unitOfWork, IServicoEol servicoEOL, IConsultasSupervisor consultasSupervisor,
            IRepositorioDre repositorioDre, IRepositorioUe repositorioUe, IRepositorioTurma repositorioTurma, IRepositorioCicloEnsino repositorioCicloEnsino, IRepositorioTipoEscola repositorioTipoEscola)
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

        public async Task Salvar(string login, Guid perfil, bool ehLogin)
        {
            if (ehLogin)
                await TrataAbrangenciaLogin(login, perfil);
            else await TrataAbrangenciaModificaoPerfil(login, perfil);
        }

        public void SalvarAbrangencias(IEnumerable<Abrangencia> abrangencias, string login)
        {
            repositorioAbrangencia.InserirAbrangencias(abrangencias, login);
        }

        public async Task SincronizarEstruturaInstitucionalVigenteCompleta()
        {
            var estruturaInstitucionalVigente = servicoEOL.ObterEstruturaInstuticionalVigentePorDre();

            if (estruturaInstitucionalVigente != null && estruturaInstitucionalVigente.Dres != null && estruturaInstitucionalVigente.Dres.Count > 0)
                await SincronizarEstruturaInstitucional(estruturaInstitucionalVigente);
            else
            {
                var erro = new NegocioException("Não foi possível obter dados de estrutura institucional do EOL");
                SentrySdk.CaptureException(erro);
                throw erro;
            }

            var tiposEscolas = servicoEOL.BuscarTiposEscola();
            if (tiposEscolas.Any())
            {
                SincronizarTiposEscola(tiposEscolas);
            }
            else
            {
                var erro = new NegocioException("Não foi possível obter dados de tipos de escolas do EOL");
                SentrySdk.CaptureException(erro);
                throw erro;
            }
            var ciclos = servicoEOL.BuscarCiclos();
            if (ciclos.Any())
            {
                SincronizarCiclos(ciclos);
            }
            else
            {
                var erro = new NegocioException("Não foi possível obter dados de ciclos de ensino do EOL");
                SentrySdk.CaptureException(erro);
                throw erro;
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

        private async Task BuscaAbrangenciaEPersiste(string login, Guid perfil)
        {
            try
            {
                const string breadcrumb = "SGP API - Tratamento de Abrangência";

                Task<AbrangenciaCompactaVigenteRetornoEOLDTO> consultaEol = null;

                var ehSupervisor = perfil == Perfis.PERFIL_SUPERVISOR;
                var ehProfessorCJ = perfil == Perfis.PERFIL_CJ || perfil == Perfis.PERFIL_CJ_INFANTIL;

                SentrySdk.AddBreadcrumb($"{breadcrumb} - Chamada BuscaAbrangenciaEPersiste - Login: {login}, perfil {perfil} - EhSupervisor: {ehSupervisor}, EhProfessorCJ: {ehProfessorCJ}", "SGP Api - Negócio");

                if (ehSupervisor)
                    consultaEol = TratarRetornoSupervisor(ObterAbrangenciaEolSupervisor(login));
                else if (ehProfessorCJ)
                    return;
                else
                    consultaEol = servicoEOL.ObterAbrangenciaCompactaVigente(login, perfil);

                if (consultaEol != null)
                {
                    // Enquanto o EOl consulta, tentamos ganhar tempo obtendo a consulta sintetica
                    var consultaAbrangenciaSintetica = repositorioAbrangencia.ObterAbrangenciaSintetica(login, perfil, string.Empty);

                    var abrangenciaEol = await consultaEol;
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
                        unitOfWork.IniciarTransacao();

                        SincronizarAbrangencia(abrangenciaSintetica, abrangenciaEol.Abrangencia.Abrangencia, ehSupervisor, dres, ues, turmas, login, perfil);

                        unitOfWork.PersistirTransacao();
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
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
                dres = repositorioDre.MaterializarCodigosDre(abrangenciaEol.IdDres, out codigosNaoEncontrados);

            if (abrangenciaEol.IdUes != null && abrangenciaEol.IdUes.Length > 0)
                ues = repositorioUe.MaterializarCodigosUe(abrangenciaEol.IdUes, out codigosNaoEncontrados);

            if (abrangenciaEol.IdTurmas != null && abrangenciaEol.IdTurmas.Length > 0)
            {
                turmas = repositorioTurma.MaterializarCodigosTurma(abrangenciaEol.IdTurmas, out codigosNaoEncontrados)
                    .Union(await ImportarTurmasNaoEncontradas(codigosNaoEncontrados));
            }

            return new Tuple<IEnumerable<Dre>, IEnumerable<Ue>, IEnumerable<Turma>>(dres, ues, turmas);
        }

        private Task<AbrangenciaRetornoEolDto> ObterAbrangenciaEolSupervisor(string login)
        {
            Task<AbrangenciaRetornoEolDto> consultaEol = null;
            var listaEscolasDresSupervior = consultasSupervisor.ObterPorDreESupervisor(login, string.Empty);

            if (listaEscolasDresSupervior.Any())
            {
                var escolas = from a in listaEscolasDresSupervior
                              from b in a.Escolas
                              select b.Codigo;

                consultaEol = servicoEOL.ObterAbrangenciaParaSupervisor(escolas.ToArray());
            }

            return consultaEol;
        }

        private void SincronizarAbragenciaPorTurmas(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Turma> turmas, string login, Guid perfil)
        {
            var novas = turmas.Where(x => !abrangenciaSintetica.Select(y => y.TurmaId).Contains(x.Id));

            var paraAtualizar = abrangenciaSintetica.Where(x => !turmas.Select(y => y.Id).Contains(x.TurmaId)).Select(x => x.Id);

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() { Perfil = perfil, TurmaId = x.Id }), login);

            repositorioAbrangencia.AtualizaAbrangenciaHistorica(paraAtualizar);
        }

        private void SincronizarAbrangencia(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, Infra.Enumerados.Abrangencia abrangencia, bool ehSupervisor, IEnumerable<Dre> dres, IEnumerable<Ue> ues, IEnumerable<Turma> turmas, string login, Guid perfil)
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
                    case Infra.Enumerados.Abrangencia.UE:
                        SincronizarAbrangenciaPorUes(abrangenciaSintetica, ues, login, perfil);
                        break;

                    case Infra.Enumerados.Abrangencia.UeTurmasDisciplinas:
                    case Infra.Enumerados.Abrangencia.Professor:
                        SincronizarAbragenciaPorTurmas(abrangenciaSintetica, turmas, login, perfil);
                        break;
                }
            }
        }

        private void SincronizarAbrangenciaPorUes(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Ue> ues, string login, Guid perfil)
        {
            var novas = ues.Where(x => !abrangenciaSintetica.Select(y => y.UeId).Contains(x.Id));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() { Perfil = perfil, UeId = x.Id }), login);

            var paraAtualizar = abrangenciaSintetica.Where(x => !ues.Select(y => y.Id).Contains(x.UeId)).Select(x => x.Id);

            repositorioAbrangencia.AtualizaAbrangenciaHistorica(paraAtualizar);
        }

        private void SincronizarAbrangenciPorDres(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Dre> dres, string login, Guid perfil)
        {
            var novas = dres.Where(x => !abrangenciaSintetica.Select(y => y.DreId).Contains(x.Id));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() { Perfil = perfil, DreId = x.Id }), login);

            var paraAtualizar = abrangenciaSintetica.Where(x => !dres.Select(y => y.Id).Contains(x.DreId)).Select(x => x.Id);

            repositorioAbrangencia.AtualizaAbrangenciaHistorica(paraAtualizar);
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
            var dres = estrutura.Dres.Select(x => new Dre() { Abreviacao = x.Abreviacao, CodigoDre = x.Codigo, Nome = x.Nome });
            var ues = estrutura.Dres.SelectMany(x => x.Ues.Select(y => new Ue { CodigoUe = y.Codigo, TipoEscola = y.CodTipoEscola, Nome = y.Nome, Dre = new Dre() { CodigoDre = x.Codigo } }));
            var turmas = estrutura.Dres.SelectMany(x => x.Ues.SelectMany(y => y.Turmas.Select(z =>
             new Turma
             {
                 Ano = z.Ano,
                 AnoLetivo = z.AnoLetivo,
                 CodigoTurma = z.Codigo,
                 ModalidadeCodigo = (Modalidade)Convert.ToInt32(z.CodigoModalidade),
                 QuantidadeDuracaoAula = z.DuracaoTurno,
                 Nome = z.NomeTurma,
                 Semestre = z.Semestre,
                 TipoTurno = z.TipoTurno,
                 Ue = new Ue() { CodigoUe = y.Codigo },
                 EnsinoEspecial = z.EnsinoEspecial
             })));

            dres = repositorioDre.Sincronizar(dres);
            ues = repositorioUe.Sincronizar(ues, dres);
            await repositorioTurma.Sincronizar(turmas, ues);
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
            if (!(await repositorioAbrangencia.JaExisteAbrangencia(login, perfil)))
            {
                await BuscaAbrangenciaEPersiste(login, perfil);
            }
        }

        private Task<AbrangenciaCompactaVigenteRetornoEOLDTO> TratarRetornoSupervisor(Task<AbrangenciaRetornoEolDto> consultaEol)
        {
            if (consultaEol != null)
            {
                return Task.Factory.StartNew(() =>
                {
                    var resultado = consultaEol.Result;
                    return new AbrangenciaCompactaVigenteRetornoEOLDTO()
                    {
                        Abrangencia = resultado.Abrangencia,
                        IdUes = resultado.Dres.SelectMany(x => x.Ues.Select(y => y.Codigo)).ToArray()
                    };
                });
            }
            return Task.FromResult<AbrangenciaCompactaVigenteRetornoEOLDTO>(null);
        }
    }
}