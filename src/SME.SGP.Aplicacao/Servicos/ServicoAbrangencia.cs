using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoAbrangencia : IServicoAbrangencia
    {
        private const string PERFIL_SUPERVISOR = "4EE1E074-37D6-E911-ABD6-F81654FE895D";
        private readonly IConsultasSupervisor consultasSupervisor;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoEOL servicoEOL;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioTurma repositorioTurma;

        public ServicoAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IUnitOfWork unitOfWork, IServicoEOL servicoEOL, IConsultasSupervisor consultasSupervisor, IRepositorioDre repositorioDre, IRepositorioUe repositorioUe, IRepositorioTurma repositorioTurma)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasSupervisor = consultasSupervisor ?? throw new ArgumentNullException(nameof(consultasSupervisor));
            this.repositorioDre = repositorioDre;
            this.repositorioUe = repositorioUe;
            this.repositorioTurma = repositorioTurma;
        }

        public async Task Salvar(string login, Guid perfil, bool ehLogin)
        {
            if (ehLogin)
                await TrataAbrangenciaLogin(login, perfil);
            else await TrataAbrangenciaModificaoPerfil(login, perfil);
        }

        private async Task BuscaAbrangenciaEPersiste(string login, Guid perfil)
        {
            Task<AbrangenciaRetornoEolDto> consultaEol = null;
            var ehSupervisor = perfil == Guid.Parse(PERFIL_SUPERVISOR);

            if (ehSupervisor)
                consultaEol = ObterAbrangenciaEolSupervisor(login);
            else
                consultaEol = servicoEOL.ObterAbrangencia(login, perfil);

            // Enquanto o EOl consulta, tentamos ganhar tempo obtendo a consulta sintetica
            var consultaAbrangenciaSintetica = repositorioAbrangencia.ObterAbrangenciaSintetica(login, perfil);

            var abrangenciaEol = await consultaEol;
            var abrangenciaSintetica = await consultaAbrangenciaSintetica;

            if (abrangenciaEol == null)
                throw new NegocioException("Não foi possível localizar registros de abrangência para este usuário.");

            IEnumerable<Dre> dres = Enumerable.Empty<Dre>();
            IEnumerable<Ue> ues = Enumerable.Empty<Ue>();
            IEnumerable<Turma> turmas = Enumerable.Empty<Turma>();

            // sincronizamos as dres, ues e turmas
            SincronizarEstruturaInstitucional(abrangenciaEol.Dres, out dres, out ues, out turmas);

            // sincronizamos a abrangencia do login + perfil
            SincronizarAbrangencia(abrangenciaSintetica, abrangenciaEol.Abrangencia.Abrangencia, ehSupervisor, dres, ues, turmas, login, perfil);
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

        private void SincronizarAbragenciaPorTurmas(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Turma> turmas, string login, Guid perfil)
        {
            repositorioAbrangencia.RemoverAbrangenciasForaEscopo(login, perfil, TipoAbrangencia.PorTurma);

            var novas = turmas.Where(x => !abrangenciaSintetica.Select(y => y.TurmaId).Contains(x.Id));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() { Perfil = perfil, TurmaId = x.Id }), login);

            var paraExcluir = abrangenciaSintetica.Where(x => !turmas.Select(y => y.Id).Contains(x.TurmaId)).Select(x => x.Id);

            repositorioAbrangencia.ExcluirAbrangencias(paraExcluir);
        }

        private void SincronizarAbrangenciPorDres(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Dre> dres, string login, Guid perfil)
        {
            repositorioAbrangencia.RemoverAbrangenciasForaEscopo(login, perfil, TipoAbrangencia.PorDre);

            var novas = dres.Where(x => !abrangenciaSintetica.Select(y => y.DreId).Contains(x.Id));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() { Perfil = perfil, DreId = x.Id }), login);

            var paraExcluir = abrangenciaSintetica.Where(x => !dres.Select(y => y.Id).Contains(x.DreId)).Select(x => x.Id);

            repositorioAbrangencia.ExcluirAbrangencias(paraExcluir);
        }

        private void SincronizarAbrangenciaPorUes(IEnumerable<AbrangenciaSinteticaDto> abrangenciaSintetica, IEnumerable<Ue> ues, string login, Guid perfil)
        {
            repositorioAbrangencia.RemoverAbrangenciasForaEscopo(login, perfil, TipoAbrangencia.PorUe);

            var novas = ues.Where(x => !abrangenciaSintetica.Select(y => y.UeId).Contains(x.Id));

            repositorioAbrangencia.InserirAbrangencias(novas.Select(x => new Abrangencia() { Perfil = perfil, UeId = x.Id }), login);

            var paraExcluir = abrangenciaSintetica.Where(x => !ues.Select(y => y.Id).Contains(x.UeId)).Select(x => x.Id);

            repositorioAbrangencia.ExcluirAbrangencias(paraExcluir);
        }

        private Task<AbrangenciaRetornoEolDto> ObterAbrangenciaEolSupervisor(string login)
        {
            Task<AbrangenciaRetornoEolDto> consultaEol;
            var listaEscolasDresSupervior = consultasSupervisor.ObterPorDreESupervisor(login, string.Empty);

            if (!listaEscolasDresSupervior.Any())
                throw new NegocioException($"Não foi possível obter as escolas atribuidas ao supervisor {login}.");

            var escolas = from a in listaEscolasDresSupervior
                          from b in a.Escolas
                          select b.Codigo;

            consultaEol = servicoEOL.ObterAbrangenciaParaSupervisor(escolas.ToArray());
            return consultaEol;
        }

        private void SincronizarEstruturaInstitucional(IEnumerable<AbrangenciaDreRetornoEolDto> estrutura, out IEnumerable<Dre> dres, out IEnumerable<Ue> ues, out IEnumerable<Turma> turmas)
        {
            IEnumerable<Dre> dresSync = estrutura.Select(x => new Dre() { Abreviacao = x.Abreviacao, CodigoDre = x.Codigo, Nome = x.Nome });
            IEnumerable<Ue> uesSync = estrutura.SelectMany(x => x.Ues.Select(y => new Ue { CodigoUe = y.Codigo, TipoEscola = y.CodTipoEscola, Nome = y.Nome, Dre = new Dre() { CodigoDre = x.Codigo } }));
            IEnumerable<Turma> turmasSync = estrutura.SelectMany(x => x.Ues.SelectMany(y => y.Turmas.Select(z =>
            new Turma
            {
                Ano = z.Ano,
                AnoLetivo = z.AnoLetivo,
                CodigoTurma = z.Codigo,
                ModalidadeCodigo = Convert.ToInt32(z.CodigoModalidade),
                QuantidadeDuracaoAula = z.DuracaoTurno,
                Nome = z.NomeTurma,
                Semestre = z.Semestre,
                TipoTurno = z.TipoTurno,
                Ue = new Ue() { CodigoUe = y.Codigo }
            })));

            dres = repositorioDre.Sincronizar(dresSync);
            ues = repositorioUe.Sincronizar(uesSync, dres);
            turmas = repositorioTurma.Sincronizar(turmasSync, ues);

        }


        private async Task TrataAbrangenciaLogin(string login, Guid perfil)
        {
            unitOfWork.IniciarTransacao();
            await BuscaAbrangenciaEPersiste(login, perfil);
            unitOfWork.PersistirTransacao();
        }

        private async Task TrataAbrangenciaModificaoPerfil(string login, Guid perfil)
        {
            if (!(await repositorioAbrangencia.JaExisteAbrangencia(login, perfil)))
            {
                unitOfWork.IniciarTransacao();
                await BuscaAbrangenciaEPersiste(login, perfil);
                unitOfWork.PersistirTransacao();
            }
        }
    }
}