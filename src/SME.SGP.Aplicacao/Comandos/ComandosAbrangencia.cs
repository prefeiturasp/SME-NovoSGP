using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAbrangencia : IComandosAbrangencia
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IUnitOfWork unitOfWork;

        public ComandosAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IUnitOfWork unitOfWork, IServicoUsuario servicoUsuario)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Salvar(AbrangenciaRetornoEolDto abrangenciaRetornoEolDto, long usuarioId)
        {
            unitOfWork.IniciarTransacao();

            await repositorioAbrangencia.RemoverAbrangencias(usuarioId);

            foreach (var abrangenciaDre in abrangenciaRetornoEolDto.Dres)
            {
                var idAbragenciaDre = await repositorioAbrangencia.SalvarDre(abrangenciaDre, usuarioId);
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