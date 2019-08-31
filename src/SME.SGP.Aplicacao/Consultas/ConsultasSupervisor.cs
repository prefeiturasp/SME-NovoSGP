using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasSupervisor : IConsultasSupervisor
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEOL servicoEOL;

        public ConsultasSupervisor(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre, IServicoEOL servicoEOL)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new System.ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public IEnumerable<SupervisorEscolasDto> ObterPorDreESupervisor(string supervisorId, string dreId)
        {
            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemSupervisoresEscola(dreId, supervisorId);

            List<SupervisorEscolasDto> lista = MapearSupervisorEscolaDre(supervisoresEscolasDres).ToList();

            return lista;
        }

        private IEnumerable<SupervisorEscolasDto> MapearSupervisorEscolaDre(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres)
        {
            var listaEscolas = servicoEOL.ObterEscolasPorCodigo(supervisoresEscolasDres.Select(a => a.IdEscola.ToString()).ToArray());
            var listaSupervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresEscolasDres.Select(a => a.IdSupervisor.ToString()).ToArray());

            if (!listaSupervisores.Any())
                throw new NegocioException("Não foi possível localizar supervisores.");

            var supervisoresIds = supervisoresEscolasDres
                .GroupBy(a => a.IdSupervisor)
                .Select(g => g.Key);

            foreach (var supervisorId in supervisoresIds)
            {
                var idsEscolas = supervisoresEscolasDres.Where(a => a.IdSupervisor == supervisorId).Select(a => a.IdEscola).ToList();

                IEnumerable<UnidadeEscolarDto> escolas = new List<UnidadeEscolarDto>();

                if (idsEscolas.Count > 0)
                {
                    escolas = from t in listaEscolas
                              where idsEscolas.Contains(t.CodigoEscola)
                              select new UnidadeEscolarDto() { Codigo = t.CodigoEscola, Nome = t.NomeEscola };
                }

                yield return new SupervisorEscolasDto()
                {
                    SupervisorNome = listaSupervisores.FirstOrDefault(a => a.CodigoRF == supervisorId).NomeServidor,
                    SupervisorId = supervisorId,
                    Escolas = escolas.ToList()
                };
            }
        }
    }
}