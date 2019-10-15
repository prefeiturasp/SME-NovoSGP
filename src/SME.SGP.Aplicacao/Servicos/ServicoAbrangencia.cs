using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoAbrangencia : IServicoAbrangencia
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoEOL servicoEOL;
        private readonly IUnitOfWork unitOfWork;

        public ServicoAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IUnitOfWork unitOfWork, IServicoEOL servicoEOL)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public void Salvar(string login, Guid perfil, bool ehLogin)
        {
            if (ehLogin)
                TrataAbrangenciaLogin(login, perfil);
            else TrataAbrangenciaModificaoPerfil(login, perfil);
        }

        private async Task BuscaAbrangenciaEPersiste(string login, Guid perfil)
        {
            var abrangenciaRetornoEolDto = await servicoEOL.ObterAbrangencia(login, perfil);
            if (abrangenciaRetornoEolDto == null)
                throw new NegocioException("Não foi possível localizar registros de abrangencia para este usuário.");

            foreach (var abrangenciaDre in abrangenciaRetornoEolDto.Dres)
            {
                var idAbragenciaDre = await repositorioAbrangencia.SalvarDre(abrangenciaDre, 1, perfil);
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
                else throw new NegocioException("Não foi possível salvar a abrangecia Dre.");
            }
        }

        private async void TrataAbrangenciaLogin(string login, Guid perfil)
        {
            unitOfWork.IniciarTransacao();
            await repositorioAbrangencia.RemoverAbrangencias(login);
            await BuscaAbrangenciaEPersiste(login, perfil);
            unitOfWork.PersistirTransacao();
        }

        private async void TrataAbrangenciaModificaoPerfil(string login, Guid perfil)
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