using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
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
        private readonly IRepositorioUeConsulta repositorioUe;

        public ConsultasSupervisor(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
            IServicoEol servicoEOL,
            IRepositorioAbrangencia repositorioAbrangencia,
            IServicoUsuario servicoUsuario,
            IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDre(string dreId)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var escolasPorDre = await repositorioAbrangencia.ObterUes(dreId, login, perfil);

            var supervisoresEscolasDres = await repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(dreId, TipoResponsavelAtribuicao.SupervisorEscolar);

            var listaRetorno = new List<ResponsavelEscolasDto>();

            TratarRegistrosComResponsaveis(escolasPorDre, supervisoresEscolasDres, listaRetorno);
            TrataEscolasSemResponsaveis(escolasPorDre, listaRetorno);

            return listaRetorno;
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDreESupervisor(string supervisorId, string dreId)
        {
            var responsaveisEscolasDres = await repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(dreId, supervisorId);

            IEnumerable<ResponsavelEscolasDto> lista = new List<ResponsavelEscolasDto>();

            if (responsaveisEscolasDres.Any())
                lista = MapearResponsavelEscolaDre(responsaveisEscolasDres).ToList();

            return lista;
        }

        public IEnumerable<ResponsavelEscolasDto> ObterPorDreESupervisores(string[] supervisoresId, string dreId)
        {
            var responsaveisEscolasDres = repositorioSupervisorEscolaDre.ObtemPorDreESupervisores(dreId, supervisoresId);

            if (responsaveisEscolasDres == null || !responsaveisEscolasDres.Any())
                return Enumerable.Empty<ResponsavelEscolasDto>();
            else
                return MapearResponsavelEscolaDre(responsaveisEscolasDres).ToList();
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterAtribuicaoResponsavel(FiltroObterSupervisorEscolasDto filtro)
        {

            if (string.IsNullOrEmpty(filtro.DreCodigo))
                throw new NegocioException("Necessário informar o Codigo da DRE");

            var responsavelEscolaDreDto = await repositorioSupervisorEscolaDre.ObterAtribuicaoResponsavel(filtro);

            if (responsavelEscolaDreDto == null)
                responsavelEscolaDreDto = new List<SupervisorEscolasDreDto>() { new SupervisorEscolasDreDto() { EscolaId = filtro.UeCodigo } };

            return MapearResponsavelEscolaDre(responsavelEscolaDreDto);
        }

        private static void TrataEscolasSemResponsaveis(IEnumerable<AbrangenciaUeRetorno> escolasPorDre, List<ResponsavelEscolasDto> listaRetorno)
        {
            if (listaRetorno.Count != escolasPorDre.Count())
            {
                var escolasComResponsavel = listaRetorno
                    .SelectMany(a => a.Escolas.Select(b => b.Codigo))
                    .ToList();

                var escolasSemResponsavel = escolasPorDre.Where(a => !escolasComResponsavel.Contains(a.Codigo)).ToList();

                var escolaResponsavelRetorno = new ResponsavelEscolasDto() { ResponsavelId = string.Empty, Responsavel = "NÃO ATRIBUÍDO" };

                var escolas = from t in escolasSemResponsavel
                              select new UnidadeEscolarDto() { Codigo = t.Codigo, Nome = t.NomeSimples };

                escolaResponsavelRetorno.Escolas = escolas.ToList();

                listaRetorno.Add(escolaResponsavelRetorno);
            }
        }

        private IEnumerable<ResponsavelEscolasDto> MapearResponsavelEscolaDre(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres)
        {
            var listaEscolas = repositorioUe.ListarPorCodigos(supervisoresEscolasDres.Select(a => a.EscolaId).ToArray());

            var listaResponsaveis = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            if (supervisoresEscolasDres.Count() == 1 && string.IsNullOrEmpty(supervisoresEscolasDres.FirstOrDefault().SupervisorId))
                listaResponsaveis.Add(new ResponsavelRetornoDto() { CodigoRfOuLogin = "", NomeServidor = "NÃO ATRIBUÍDO" });

            var supervisoresTipo = supervisoresEscolasDres
                .GroupBy(a => new { a.SupervisorId, a.Tipo })
                .Select(g => g.Key);

            foreach (var supervisor in supervisoresTipo)
            {
                var supervisorEscolas = supervisoresEscolasDres.Where(a => a.SupervisorId == supervisor.SupervisorId).ToList();

                var idsEscolas = supervisorEscolas.Select(a => a.EscolaId).ToList();

                IEnumerable<UnidadeEscolarDto> escolas = new List<UnidadeEscolarDto>();

                if (idsEscolas.Count > 0)
                {
                    escolas = from t in listaEscolas
                              where idsEscolas.Contains(t.CodigoUe)
                              select new UnidadeEscolarDto() { Codigo = t.CodigoUe, Nome = $"{t.TipoEscola.ShortName()} {t.Nome}" };
                }

                var auditoria = supervisoresEscolasDres.FirstOrDefault(c => c.SupervisorId == supervisor.SupervisorId);

                switch (supervisor.Tipo)
                {
                    case (int)TipoResponsavelAtribuicao.PsicologoEscolar:
                    case (int)TipoResponsavelAtribuicao.Psicopedagogo:
                    case (int)TipoResponsavelAtribuicao.AssistenteSocial:
                        {
                            var nomesFuncionariosAtribuidos = servicoEOL.ObterListaNomePorListaLogin(new List<string> { supervisor.SupervisorId }).Result;

                            foreach (var funcionario in nomesFuncionariosAtribuidos)
                            {
                                listaResponsaveis.Add(new ResponsavelRetornoDto()
                                {
                                    CodigoRfOuLogin = funcionario.Login,
                                    NomeServidor = funcionario.NomeServidor
                                });
                            }

                            break;
                        }
                    default:
                        {
                            var nomesServidoresAtribuidos = servicoEOL.ObterListaNomePorListaRF(new List<string> { supervisor.SupervisorId }).Result;

                            foreach (var item in nomesServidoresAtribuidos)
                            {
                                listaResponsaveis.Add(new ResponsavelRetornoDto()
                                {
                                    CodigoRfOuLogin = item.CodigoRF,
                                    NomeServidor = item.Nome
                                });
                            }

                            break;
                        }
                }

                var responsavelRetorno = listaResponsaveis.FirstOrDefault(a => a.CodigoRfOuLogin == (supervisor?.SupervisorId ?? string.Empty));

                yield return new ResponsavelEscolasDto()
                {
                    Responsavel = responsavelRetorno.NomeServidor + " - " + responsavelRetorno.CodigoRfOuLogin,
                    ResponsavelId = supervisor.SupervisorId,
                    TipoResponsavel = ObterTipoResponsavelDescricao(supervisor.Tipo),
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

        private static string ObterTipoResponsavelDescricao(int tipo)
        {

            var tipoDescricao = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                .Cast<TipoResponsavelAtribuicao>()
                .Where(w => (int)w == tipo)
                .Select(d => new { descricao = d.Name() })
                .FirstOrDefault()?.descricao;

            return tipoDescricao;
        }

        private static SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
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
                CriadoRF = dto.CriadoRF,
                Tipo = dto.Tipo
            };
        }

        private void TratarRegistrosComResponsaveis(IEnumerable<AbrangenciaUeRetorno> escolasPorDre, IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, List<ResponsavelEscolasDto> listaRetorno)
        {
            if (supervisoresEscolasDres.Any())
            {
                var supervisoresIds = supervisoresEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key);
                var supervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresIds.ToArray());

                if (supervisores != null && supervisores.Any())
                {
                    foreach (var supervisorEscolaDre in supervisoresIds)
                    {
                        var responsavelEscolasDto = new ResponsavelEscolasDto
                        {
                            Responsavel = supervisores.FirstOrDefault(a => a.CodigoRf == supervisorEscolaDre)?.NomeServidor,
                            ResponsavelId = supervisorEscolaDre
                        };

                        var idsEscolasDoSupervisor = supervisoresEscolasDres.Where(a => a.SupervisorId == supervisorEscolaDre)
                            .Select(a => a.EscolaId)
                            .ToList();

                        var escolas = from t in escolasPorDre
                                      where idsEscolasDoSupervisor.Contains(t.Codigo)
                                      select new UnidadeEscolarDto() { Codigo = t.Codigo, Nome = t.NomeSimples };

                        responsavelEscolasDto.Escolas = escolas.ToList();

                        listaRetorno.Add(responsavelEscolasDto);
                    }
                }
            }
        }

        private void RemoverSupervisorSemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres,
            IEnumerable<SupervisoresRetornoDto> supervisoresEol)
        {
            var supervisoresSemAtribuicao = supervisoresEscolasDres;

            if (supervisoresEol != null)
            {
                supervisoresSemAtribuicao = supervisoresEscolasDres
                    .Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.SupervisorEscolar &&
                        !supervisoresEol.Select(e => e.CodigoRf)
                    .Contains(s.SupervisorId));
            }

            if (supervisoresSemAtribuicao != null && supervisoresSemAtribuicao.Any())
            {
                foreach (var supervisor in supervisoresSemAtribuicao)
                {
                    if (supervisor.Tipo == (int)TipoResponsavelAtribuicao.SupervisorEscolar)
                    {
                        var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                        supervisorEntidadeExclusao.Excluir();
                        repositorioSupervisorEscolaDre.Salvar(supervisorEntidadeExclusao);
                    }
                }
            }
        }
    }
}