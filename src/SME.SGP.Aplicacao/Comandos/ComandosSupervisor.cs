using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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

        public void AtribuirUE(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto)
        {
            var escolasAtribuidas = repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.SupervisorId, true);

            var codigosEscolasDominio = escolasAtribuidas?.Where(e => !e.Excluido).Select(c => c.EscolaId);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                RemoverEscolas(atribuicaoSupervisorEscolaDto, escolasAtribuidas);
                AtribuirEscolas(atribuicaoSupervisorEscolaDto, codigosEscolasDominio);

                unitOfWork.PersistirTransacao();
            }
        }

        private void AtribuirEscolas(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto, System.Collections.Generic.IEnumerable<string> codigosEscolasDominio)
        {
            if (atribuicaoSupervisorEscolaDto.UESIds != null)
                foreach (var codigoEscolaDto in atribuicaoSupervisorEscolaDto.UESIds)
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
        }

        private void RemoverEscolas(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto, System.Collections.Generic.IEnumerable<SupervisorEscolasDreDto> escolasAtribuidas)
        {
            if (escolasAtribuidas != null)
                foreach (var atribuicao in escolasAtribuidas)
                {
                    if (atribuicaoSupervisorEscolaDto.UESIds == null || !atribuicaoSupervisorEscolaDto.UESIds.Contains(atribuicao.EscolaId) || atribuicao.Excluido)
                    {
                        repositorioSupervisorEscolaDre.Remover(atribuicao.Id);
                    }
                }
        }
    }
}