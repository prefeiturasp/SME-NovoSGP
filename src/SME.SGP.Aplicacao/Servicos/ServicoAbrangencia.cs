using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
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

        public ServicoAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IUnitOfWork unitOfWork, IServicoEOL servicoEOL, IConsultasSupervisor consultasSupervisor)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasSupervisor = consultasSupervisor ?? throw new ArgumentNullException(nameof(consultasSupervisor));
        }

        public async Task Salvar(string login, Guid perfil, bool ehLogin)
        {
            if (ehLogin)
                await TrataAbrangenciaLogin(login, perfil);
            else await TrataAbrangenciaModificaoPerfil(login, perfil);
        }

        private async Task BuscaAbrangenciaEPersiste(string login, Guid perfil)
        {
            AbrangenciaRetornoEolDto abrangenciaRetornoEolDto;

            if (perfil == Guid.Parse(PERFIL_SUPERVISOR))
            {
                var listaEscolasDresSupervior = consultasSupervisor.ObterPorDreESupervisor(login, string.Empty);

                if (!listaEscolasDresSupervior.Any())
                    throw new NegocioException($"Não foi possível obter as escolas atribuidas ao supervisor {login}.");

                var escolas = from a in listaEscolasDresSupervior
                              from b in a.Escolas
                              select b.Codigo;

                abrangenciaRetornoEolDto = await servicoEOL.ObterAbrangenciaParaSupervisor(escolas.ToArray());
            }
            else
            {
                abrangenciaRetornoEolDto = await servicoEOL.ObterAbrangencia(login, perfil);
            }

            if (abrangenciaRetornoEolDto == null)
                throw new NegocioException("Não foi possível localizar registros de abrangência para este usuário.");

            foreach (var abrangenciaDre in abrangenciaRetornoEolDto.Dres)
            {
                var idAbragenciaDre = await repositorioAbrangencia.SalvarDre(abrangenciaDre, login, perfil);
                if (idAbragenciaDre > 0)
                {
                    foreach (var abrangenciaUe in abrangenciaDre.Ues)
                    {
                        var idAbragenciaUe = await repositorioAbrangencia.SalvarUe(abrangenciaUe, idAbragenciaDre);

                        if (idAbragenciaUe > 0)
                        {
                            foreach (var abrangenciaTurma in abrangenciaUe.Turmas)
                            {
                                await repositorioAbrangencia.SalvarTurma(abrangenciaTurma, idAbragenciaUe);
                            }
                        }
                    }
                }
                else throw new NegocioException("Não foi possível salvar a abrangência Dre.");
            }
        }

        private async Task TrataAbrangenciaLogin(string login, Guid perfil)
        {
            unitOfWork.IniciarTransacao();
            await repositorioAbrangencia.RemoverAbrangencias(login);
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