using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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

            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(dreId, string.Empty);

            var listaRetorno = new List<SupervisorEscolasDto>();

            TratarRegistrosComSupervisores(escolasPorDre, supervisoresEscolasDres, listaRetorno);
            TrataEscolasSemSupervisores(escolasPorDre, listaRetorno);

            return listaRetorno;
        }

        public IEnumerable<SupervisorDto> ObterPorDreENomeSupervisor(string supervisorNome, string dreId)
        {
            var supervisoresEol = servicoEOL.ObterSupervisoresPorDre(dreId);

            if (string.IsNullOrEmpty(supervisorNome))
            {
                return supervisoresEol?.Select(a => new SupervisorDto() { SupervisorId = a.CodigoRF, SupervisorNome = a.NomeServidor });
            }
            else
            {
                return from a in supervisoresEol
                       where a.NomeServidor.ToLower().Contains(supervisorNome.ToLower())
                       select new SupervisorDto() { SupervisorId = a.CodigoRF, SupervisorNome = a.NomeServidor };
            }
        }

        public IEnumerable<SupervisorEscolasDto> ObterPorDreESupervisor(string supervisorId, string dreId)
        {
            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(dreId, supervisorId);

            List<SupervisorEscolasDto> lista = MapearSupervisorEscolaDre(supervisoresEscolasDres).ToList();

            return lista;
        }

        public IEnumerable<SupervisorEscolasDto> ObterPorDreESupervisores(string[] supervisoresId, string dreId)
        {
            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemPorDreESupervisores(dreId, supervisoresId);

            if (supervisoresEscolasDres == null || supervisoresEscolasDres.Count() == 0)
                return null;
            else return MapearSupervisorEscolaDre(supervisoresEscolasDres).ToList();
        }

        public SupervisorEscolasDto ObterPorUe(string ueId)
        {
            var supervisorEscolaDreDto = repositorioSupervisorEscolaDre.ObtemPorUe(ueId);
            if (supervisorEscolaDreDto == null)
                supervisorEscolaDreDto = new SupervisorEscolasDreDto() { EscolaId = ueId };

            return MapearSupervisorEscolaDre(new[] { supervisorEscolaDreDto })
                .FirstOrDefault();
        }

        private static void TrataEscolasSemSupervisores(IEnumerable<EscolasRetornoDto> escolasPorDre, List<SupervisorEscolasDto> listaRetorno)
        {
            if (listaRetorno.Count != escolasPorDre.Count())
            {
                var escolasComSupervisor = listaRetorno
                    .SelectMany(a => a.Escolas.Select(b => b.Codigo))
                    .ToList();

                var escolasSemSupervisor = escolasPorDre.Where(a => !escolasComSupervisor.Contains(a.CodigoEscola)).ToList();

                var escolaSupervisorRetorno = new SupervisorEscolasDto() { SupervisorId = string.Empty, SupervisorNome = "NÃO ATRIBUÍDO" };

                var escolas = from t in escolasSemSupervisor
                              select new UnidadeEscolarDto() { Codigo = t.CodigoEscola, Nome = t.NomeEscola };

                escolaSupervisorRetorno.Escolas = escolas.ToList();

                listaRetorno.Add(escolaSupervisorRetorno);
            }
        }

        private IEnumerable<SupervisorEscolasDto> MapearSupervisorEscolaDre(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres)
        {
            var listaEscolas = servicoEOL.ObterEscolasPorCodigo(supervisoresEscolasDres.Select(a => a.EscolaId.ToString()).ToArray());

            List<SupervisoresRetornoDto> listaSupervisores;

            if (supervisoresEscolasDres.Count() == 1 && string.IsNullOrEmpty(supervisoresEscolasDres.FirstOrDefault().SupervisorId))
            {
                listaSupervisores = new List<SupervisoresRetornoDto>() { new SupervisoresRetornoDto() { CodigoRF = "", NomeServidor = "NÃO ATRIBUÍDO" } };
            }
            else
            {
                listaSupervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresEscolasDres.Select(a => a.SupervisorId.ToString()).ToArray())
                .ToList();
            }

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

                var auditoria = supervisoresEscolasDres.FirstOrDefault(c => c.SupervisorId == supervisorId);

                yield return new SupervisorEscolasDto()
                {
                    SupervisorNome = listaSupervisores.FirstOrDefault(a => a.CodigoRF == (supervisorId ?? string.Empty)).NomeServidor,
                    SupervisorId = supervisorId,
                    Escolas = escolas.ToList(),
                    AlteradoEm = auditoria.AlteradoEm,
                    AlteradoPor = auditoria.AlteradoPor,
                    AlteradoRF = auditoria.AlteradoRF,
                    CriadoEm = auditoria.CriadoEm,
                    CriadoPor = auditoria.CriadoPor,
                    CriadoRF = auditoria.CriadoRF
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