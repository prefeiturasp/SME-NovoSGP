using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosSupervisor : IComandosSupervisor
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IUnitOfWork unitOfWork;

        public ComandosSupervisor(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new System.ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public void AtribuirEscola(AtribuicaoSupervisorEscolaDto atribuicaoSupervisorEscolaDto)
        {
            var escolasAtribuidas = repositorioSupervisorEscolaDre.ObtemSupervisoresEscola(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.SupervisorId);

            var codigosEscolasDominio = escolasAtribuidas?.Select(c => c.EscolaId);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                if (escolasAtribuidas != null)
                    foreach (var atribuicao in escolasAtribuidas)
                    {
                        if (atribuicaoSupervisorEscolaDto.EscolasIds == null || !atribuicaoSupervisorEscolaDto.EscolasIds.Contains(atribuicao.EscolaId))
                        {
                            repositorioSupervisorEscolaDre.Remover(atribuicao.Id);
                        }
                    }
                if (atribuicaoSupervisorEscolaDto.EscolasIds != null)
                    foreach (var codigoEscolaDto in atribuicaoSupervisorEscolaDto.EscolasIds)
                    {
                        if (codigosEscolasDominio != null && !codigosEscolasDominio.Contains(codigoEscolaDto))
                        {
                            repositorioSupervisorEscolaDre.Salvar(new SupervisorEscolaDre()
                            {
                                DreId = atribuicaoSupervisorEscolaDto.DreId,
                                SupervisorId = atribuicaoSupervisorEscolaDto.SupervisorId,
                                EscolaId = codigoEscolaDto
                            });
                        }
                    }

                unitOfWork.PersistirTransacao();
            }
        }
    }
}