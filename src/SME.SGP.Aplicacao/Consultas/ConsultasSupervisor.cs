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

        public IEnumerable<SupervisorEscolasDto> ObterPorDre(string dreId)
        {
            var escolasPorDre = servicoEOL.ObterEscolasPorDre(dreId);

            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemSupervisoresEscola(dreId, string.Empty);

            var listaRetorno = new List<SupervisorEscolasDto>();

            TratarRegistrosComSupervisores(escolasPorDre, supervisoresEscolasDres, listaRetorno);
            TrataEscolasSemSupervisores(escolasPorDre, listaRetorno);

            return listaRetorno;
        }

        public IEnumerable<SupervisorEscolasDto> ObterPorDreESupervisor(string supervisorId, string dreId)
        {
            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemSupervisoresEscola(dreId, supervisorId);

            List<SupervisorEscolasDto> lista = MapearSupervisorEscolaDre(supervisoresEscolasDres).ToList();

            return lista;
        }

        private static void TrataEscolasSemSupervisores(IEnumerable<EscolasRetornoDto> escolasPorDre, List<SupervisorEscolasDto> listaRetorno)
        {
            if (listaRetorno.Count != escolasPorDre.Count())
            {
                var escolasComSupervisor = listaRetorno
                    .SelectMany(a => a.Escolas.Select(b => b.Codigo))
                    .ToList();

                var escolasSemSupervisor = escolasPorDre.Where(a => !escolasComSupervisor.Contains(a.CodigoEscola)).ToList();

                var escolaSupervisorRetorno = new SupervisorEscolasDto() { SupervisorId = string.Empty, SupervisorNome = "Sem supervisor" };

                var escolas = from t in escolasSemSupervisor
                              select new UnidadeEscolarDto() { Codigo = t.CodigoEscola, Nome = t.NomeEscola };

                escolaSupervisorRetorno.Escolas = escolas.ToList();

                listaRetorno.Add(escolaSupervisorRetorno);
            }
        }

        private IEnumerable<SupervisorEscolasDto> MapearSupervisorEscolaDre(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres)
        {
            var listaEscolas = servicoEOL.ObterEscolasPorCodigo(supervisoresEscolasDres.Select(a => a.EscolaId.ToString()).ToArray());
            var listaSupervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresEscolasDres.Select(a => a.SupervisorId.ToString()).ToArray());

            if (!listaSupervisores.Any())
                throw new NegocioException("Não foi possível localizar supervisores.");

            var supervisoresIds = supervisoresEscolasDres
                .GroupBy(a => a.SupervisorId)
                .Select(g => g.Key);

            foreach (var supervisorId in supervisoresIds)
            {
                var idsEscolas = supervisoresEscolasDres.Where(a => a.SupervisorId == supervisorId).Select(a => a.EscolaId).ToList();

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

        private void TratarRegistrosComSupervisores(IEnumerable<EscolasRetornoDto> escolasPorDre, IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, List<SupervisorEscolasDto> listaRetorno)
        {
            if (supervisoresEscolasDres.Any())
            {
                var supervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresEscolasDres.Select(a => a.SupervisorId).ToArray());
                if (supervisores == null)
                    throw new System.Exception("Não foi possível localizar o nome dos supervisores na API Eol");

                foreach (var supervisorEscolaDre in supervisoresEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key).ToList())
                {
                    var supervisorEscolasDto = new SupervisorEscolasDto();
                    supervisorEscolasDto.SupervisorNome = supervisores.FirstOrDefault(a => a.CodigoRF == supervisorEscolaDre).NomeServidor;
                    supervisorEscolasDto.SupervisorId = supervisorEscolaDre;

                    var idsEscolasDoSupervisor = supervisoresEscolasDres.Where(a => a.SupervisorId == supervisorEscolaDre)
                        .Select(a => a.EscolaId)
                        .ToList();

                    var escolas = from t in escolasPorDre
                                  where idsEscolasDoSupervisor.Contains(t.CodigoEscola)
                                  select new UnidadeEscolarDto() { Codigo = t.CodigoEscola, Nome = t.NomeEscola };

                    supervisorEscolasDto.Escolas = escolas.ToList();

                    listaRetorno.Add(supervisorEscolasDto);
                }
            }
        }
    }
}