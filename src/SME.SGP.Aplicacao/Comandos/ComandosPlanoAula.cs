using System;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAula : IComandosPlanoAula
    {
        private readonly IRepositorioPlanoAula repositorio;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAula(IRepositorioPlanoAula repositorioPlanoAula, IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorioPlanoAula;
            this.unitOfWork = unitOfWork;
        }

        public void Salvar(PlanoAulaDto planoAulaDto)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var objetivoAprendizagem in planoAulaDto.ObjetivosAprendizagemAula)
                {
                    //PlanoAnual planoAnual = ObterPlanoAnualSimplificado(planoAnualDto, bimestrePlanoAnual.Bimestre.Value);
                    //planoAnual = MapearParaDominio(planoAnualDto, planoAnual, bimestrePlanoAnual);
                    //Salvar(planoAulaDto, planoAula, bimestrePlanoAnual);
                }
                unitOfWork.PersistirTransacao();
            }
        }
    }
}
