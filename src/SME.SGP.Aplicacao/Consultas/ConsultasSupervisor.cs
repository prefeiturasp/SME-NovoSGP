using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasSupervisor : IConsultasSupervisor
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioUe repositorioUe;

        public ConsultasSupervisor(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                   IServicoEol servicoEOL,
                                   IRepositorioAbrangencia repositorioAbrangencia,
                                   IServicoUsuario servicoUsuario,
                                   IRepositorioUe repositorioUe)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new System.ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<SupervisorEscolasDto>> ObterPorDre(string dreId)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var escolasPorDre = await repositorioAbrangencia.ObterUes(dreId, login, perfil);

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

            IEnumerable<SupervisorEscolasDto> lista = new List<SupervisorEscolasDto>();

            if (supervisoresEscolasDres.Any())
                lista = MapearSupervisorEscolaDre(supervisoresEscolasDres).ToList();

            return lista;
        }

        public IEnumerable<SupervisorEscolasDto> ObterPorDreESupervisores(string[] supervisoresId, string dreId)
        {
            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemPorDreESupervisores(dreId, supervisoresId);

            if (supervisoresEscolasDres == null || supervisoresEscolasDres.Any())
                return Enumerable.Empty<SupervisorEscolasDto>();
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

        private static void TrataEscolasSemSupervisores(IEnumerable<AbrangenciaUeRetorno> escolasPorDre, List<SupervisorEscolasDto> listaRetorno)
        {
            if (listaRetorno.Count != escolasPorDre.Count())
            {
                var escolasComSupervisor = listaRetorno
                    .SelectMany(a => a.Escolas.Select(b => b.Codigo))
                    .ToList();

                var escolasSemSupervisor = escolasPorDre.Where(a => !escolasComSupervisor.Contains(a.Codigo)).ToList();

                var escolaSupervisorRetorno = new SupervisorEscolasDto() { SupervisorId = string.Empty, SupervisorNome = "NÃO ATRIBUÍDO" };

                var escolas = from t in escolasSemSupervisor
                              select new UnidadeEscolarDto() { Codigo = t.Codigo, Nome = t.NomeSimples };

                escolaSupervisorRetorno.Escolas = escolas.ToList();

                listaRetorno.Add(escolaSupervisorRetorno);
            }
        }

        private IEnumerable<SupervisorEscolasDto> MapearSupervisorEscolaDre(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres)
        {
            var listaEscolas = repositorioUe.ListarPorCodigos(supervisoresEscolasDres.Select(a => a.EscolaId).ToArray());

            IEnumerable<SupervisoresRetornoDto> listaSupervisores;

            if (supervisoresEscolasDres.Count() == 1 && string.IsNullOrEmpty(supervisoresEscolasDres.FirstOrDefault().SupervisorId))
            {
                listaSupervisores = new List<SupervisoresRetornoDto>() { new SupervisoresRetornoDto() { CodigoRF = "", NomeServidor = "NÃO ATRIBUÍDO" } };
            }
            else
            {
                var supervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresEscolasDres.Select(a => a.SupervisorId.ToString()).ToArray());

                if (supervisores == null || (supervisores.Count() != supervisoresEscolasDres.Count()))
                {
                    RemoverSupervisorSemAtribuicao(supervisoresEscolasDres, supervisores);

                    if (supervisores != null)
                        supervisoresEscolasDres = supervisoresEscolasDres.Where(s => supervisores.Select(e => e.CodigoRF).Contains(s.SupervisorId));
                    else
                        supervisoresEscolasDres = Enumerable.Empty<SupervisorEscolasDreDto>();
                }

                listaSupervisores = supervisores;
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
                              where idsEscolas.Contains(t.CodigoUe)
                              select new UnidadeEscolarDto() { Codigo = t.CodigoUe, Nome = $"{t.TipoEscola.ShortName()} {t.Nome}" };
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

        private SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
        {
            return new SupervisorEscolaDre()
            {
                DreId = dto.DreId,
                SupervisorId = dto.SupervisorId,
                EscolaId = dto.EscolaId,
                Id = dto.Id,
                Excluido = dto.Excluido,
                AlteradoEm = dto.AlteradoEm,
                AlteradoPor = dto.AlteradoPor,
                AlteradoRF = dto.AlteradoRF,
                CriadoEm = dto.CriadoEm,
                CriadoPor = dto.CriadoPor,
                CriadoRF = dto.CriadoRF
            };
        }


        private void TratarRegistrosComSupervisores(IEnumerable<AbrangenciaUeRetorno> escolasPorDre, IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, List<SupervisorEscolasDto> listaRetorno)
        {
            if (supervisoresEscolasDres.Any())
            {
                var supervisoresIds = supervisoresEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key);
                var supervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresIds.ToArray());

                if (supervisores != null && supervisores.Any())
                {
                    if (supervisores.Count() != supervisoresIds.Count())
                        RemoverSupervisorSemAtribuicao(supervisoresEscolasDres, supervisores);

                    foreach (var supervisorEscolaDre in supervisoresIds)
                    {
                        var supervisorEscolasDto = new SupervisorEscolasDto();
                        supervisorEscolasDto.SupervisorNome = supervisores.FirstOrDefault(a => a.CodigoRF == supervisorEscolaDre)?.NomeServidor;
                        supervisorEscolasDto.SupervisorId = supervisorEscolaDre;

                        var idsEscolasDoSupervisor = supervisoresEscolasDres.Where(a => a.SupervisorId == supervisorEscolaDre)
                            .Select(a => a.EscolaId)
                            .ToList();

                        var escolas = from t in escolasPorDre
                                      where idsEscolasDoSupervisor.Contains(t.Codigo)
                                      select new UnidadeEscolarDto() { Codigo = t.Codigo, Nome = t.NomeSimples };

                        supervisorEscolasDto.Escolas = escolas.ToList();

                        listaRetorno.Add(supervisorEscolasDto);
                    }
                }
            }
        }

        private void RemoverSupervisorSemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, IEnumerable<SupervisoresRetornoDto> supervisoresEol)
        {
            var supervisoresSemAtribuicao = supervisoresEscolasDres;

            if (supervisoresEol != null)
                supervisoresSemAtribuicao = supervisoresEscolasDres.Where(s => !supervisoresEol.Select(e => e.CodigoRF).Contains(s.SupervisorId));

            if (supervisoresSemAtribuicao != null && supervisoresSemAtribuicao.Any())
            {
                foreach (var supervisor in supervisoresSemAtribuicao)
                {
                    var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                    supervisorEntidadeExclusao.Excluir();

                    repositorioSupervisorEscolaDre.Salvar(supervisorEntidadeExclusao);
                }
            }
        }

    }
}