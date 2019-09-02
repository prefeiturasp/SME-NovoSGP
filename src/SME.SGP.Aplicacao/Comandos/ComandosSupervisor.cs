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
            var escolasAtribuidas = repositorioSupervisorEscolaDre.ObtemSupervisoresEscola(atribuicaoSupervisorEscolaDto.IdDre, atribuicaoSupervisorEscolaDto.SupervisorId);

            var codigosEscolasDto = atribuicaoSupervisorEscolaDto.Escolas.Select(a => a.Codigo);
            var codigosEscolasDominio = escolasAtribuidas?.Select(c => c.IdEscola);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var codigoEscolaDto in codigosEscolasDto)
                {
                    if (codigosEscolasDominio != null && !codigosEscolasDominio.Contains(codigoEscolaDto))
                    {
                        repositorioSupervisorEscolaDre.Salvar(new SupervisorEscolaDre()
                        {
                            DreId = atribuicaoSupervisorEscolaDto.IdDre,
                            SupervisorId = atribuicaoSupervisorEscolaDto.SupervisorId,
                            EscolaId = codigoEscolaDto
                        });
                    }
                }
                if (escolasAtribuidas != null)
                    foreach (var atribuicao in escolasAtribuidas)
                    {
                        if (!codigosEscolasDto.Contains(atribuicao.IdEscola))
                        {
                            repositorioSupervisorEscolaDre.Remover(atribuicao.Id);
                        }
                    }
                unitOfWork.PersistirTransacao();
            }
        }
    }
}