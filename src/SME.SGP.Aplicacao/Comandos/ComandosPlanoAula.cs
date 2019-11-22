using System;
using System.Threading.Tasks;
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

        public async Task Salvar(PlanoAulaDto planoAulaDto)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var objetivoAprendizagem in planoAulaDto.ObjetivosAprendizagemAula)
                {
                    PlanoAula planoAula = await repositorio.ObterPlanoAulaPorAula(planoAulaDto.AulaId);
                    planoAula = MapearParaDominio(planoAulaDto, planoAula);

                    repositorio.Salvar(planoAula);
                    // TODO: Salvar Objetivos
                }
                unitOfWork.PersistirTransacao();
            }
        }

        private PlanoAula MapearParaDominio(PlanoAulaDto planoDto, PlanoAula planoAula = null)
        {
            if (planoAula == null)
                planoAula = new PlanoAula();

            planoAula.AulaId = planoDto.AulaId;
            planoAula.Descricao = planoDto.Descricao;
            planoAula.DesenvolvimentoAula = planoDto.DesenvolvimentoAula;
            planoAula.RecuperacaoAula = planoDto.RecuperacaoAula;
            planoAula.LicaoCasa = planoDto.LicaoCasa;

            return planoAula;
        }
    }
}
