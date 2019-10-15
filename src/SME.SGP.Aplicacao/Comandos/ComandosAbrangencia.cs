using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAbrangencia : IComandosAbrangencia
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoEOL servicoEOL;
        private readonly IUnitOfWork unitOfWork;

        public ComandosAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IUnitOfWork unitOfWork, IServicoEOL servicoEOL)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task Salvar()
        {
            var perfil = "5AE1E074-37D6-E911-ABD6-F81654FE895D";

            var abrangenciaRetornoEolDto = await servicoEOL.ObterAbrangencia("6941583", perfil);

            unitOfWork.IniciarTransacao();

            await repositorioAbrangencia.RemoverAbrangencias(1);

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

            unitOfWork.PersistirTransacao();
        }
    }
}