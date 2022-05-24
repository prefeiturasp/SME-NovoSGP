using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreUseCase : AbstractUseCase, IObterSupervisoresPorDreUseCase
    {
        public ObterSupervisoresPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterNomesResponsaveisAtribuidos(IEnumerable<SupervisorEscolasDreDto> responsaveisAtribuidos,
            TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            var listaNomesResponsaveisAtribuidos = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            if (responsaveisAtribuidos == null || !responsaveisAtribuidos.Any())
                return listaNomesResponsaveisAtribuidos;

            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                case TipoResponsavelAtribuicao.Psicopedagogo:
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    {
                        var nomesFuncionariosAtribuidos = await mediator.Send(new ObterListaNomePorListaLoginQuery(responsaveisAtribuidos?.Select(c => c.SupervisorId)));

                        foreach (var funcionario in nomesFuncionariosAtribuidos)
                        {
                            listaNomesResponsaveisAtribuidos.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.Login,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                default:
                    {
                        var nomesServidoresAtribuidos = await mediator.Send(new ObterListaNomePorListaRFQuery(responsaveisAtribuidos?.Select(c => c.SupervisorId)));

                        foreach (var supervisor in nomesServidoresAtribuidos)
                        {
                            listaNomesResponsaveisAtribuidos.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = supervisor.CodigoRF,
                                NomeServidor = supervisor.Nome
                            });
                        }

                        break;
                    }
            }

            if (listaNomesResponsaveisAtribuidos == null || !listaNomesResponsaveisAtribuidos.Any())
                throw new NegocioException("A API/EOL não retornou os nomes dos responsáveis atribuídos.");

            return await Task.FromResult(listaNomesResponsaveisAtribuidos);
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterResponsaveisEol_CoreSSO(string dreCodigo, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            var listaResponsavel = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PAAI:
                    {
                        var funcionariosEol = await mediator.Send(new ObterFuncionariosPorDreECargoQuery(dreCodigo, 29));

                        foreach (var funcionario in funcionariosEol)
                        {
                            listaResponsavel.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.CodigoRf,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                case TipoResponsavelAtribuicao.Psicopedagogo:
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    {
                        var funcionariosUnidades = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(dreCodigo,
                            new List<Guid> { Perfis.PERFIL_PSICOLOGO_ESCOLAR, Perfis.PERFIL_PSICOPEDAGOGO, Perfis.PERFIL_ASSISTENTE_SOCIAL }))).ToList();

                        foreach (var funcionario in funcionariosUnidades)
                        {
                            listaResponsavel.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.Login,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                default:
                    var supervisoresEol = (await mediator.Send(new ObterSupervisoresPorDreEolQuery(dreCodigo))).ToList();

                    foreach (var supervisor in supervisoresEol)
                    {
                        listaResponsavel.Add(new ResponsavelRetornoDto()
                        {
                            CodigoRf_Login = supervisor.CodigoRf,
                            NomeServidor = supervisor.NomeServidor
                        });
                    }

                    break;
            }

            return await Task.FromResult(listaResponsavel);
        }

        public async Task<IEnumerable<SupervisorDto>> Executar(ObterSupervisoresPorDreDto filtro)
        {
            var responsaveisEol_CoreSSO = await ObterResponsaveisEol_CoreSSO(filtro.DreCodigo, filtro.TipoResponsavelAtribuicao);

            //-> Obtem os resposáveis já atribuidos
            var responsaveisAtribuidos = (await mediator.Send(new ObterSupervisoresPorDreQuery(filtro.DreCodigo, filtro.TipoResponsavelAtribuicao)))
                .DistinctBy(c => c.SupervisorId);

            var nomesResponsaveisAtribuidos = await ObterNomesResponsaveisAtribuidos(responsaveisAtribuidos, filtro.TipoResponsavelAtribuicao);

            //-> Preparando a lista de retorno
            var lstSupervisores = new List<SupervisorDto>();

            if (string.IsNullOrEmpty(filtro.SupervisorNome))
            {
                if (responsaveisEol_CoreSSO != null && responsaveisEol_CoreSSO.Any())
                {
                    lstSupervisores.AddRange(responsaveisEol_CoreSSO?.Select(a => new SupervisorDto()
                    {
                        SupervisorId = a.CodigoRf_Login,
                        SupervisorNome = a.NomeServidor
                    }));
                }

                if (responsaveisAtribuidos != null && responsaveisAtribuidos.Any())
                {
                    lstSupervisores.AddRange(responsaveisAtribuidos?.Where(s => !responsaveisEol_CoreSSO.Select(se => se.CodigoRf_Login).Contains(s.SupervisorId))?
                        .Select(a => new SupervisorDto()
                        {
                            SupervisorId = a.SupervisorId,
                            SupervisorNome = nomesResponsaveisAtribuidos?.FirstOrDefault(n => n.CodigoRf_Login == a.SupervisorId)?.NomeServidor
                        }));
                }
            }
            else
            {
                if (responsaveisEol_CoreSSO != null && responsaveisEol_CoreSSO.Any())
                {
                    lstSupervisores.AddRange(from a in responsaveisEol_CoreSSO
                        where a.NomeServidor.ToLower().Contains(filtro.SupervisorNome.ToLower())
                        select new SupervisorDto() 
                        { 
                            SupervisorId = a.CodigoRf_Login, 
                            SupervisorNome = a.NomeServidor 
                        });
                }

                if (responsaveisAtribuidos != null && responsaveisAtribuidos.Any())
                {
                    lstSupervisores.AddRange(from a in responsaveisAtribuidos
                       join b in nomesResponsaveisAtribuidos on a.SupervisorId equals b.CodigoRf_Login
                       where !responsaveisEol_CoreSSO.Select(s => s.CodigoRf_Login).Contains(a.SupervisorId) && b.NomeServidor.ToLower().Contains(filtro.SupervisorNome.ToLower())
                       select new SupervisorDto() 
                       { 
                           SupervisorId = b.CodigoRf_Login, 
                           SupervisorNome = b.NomeServidor 
                       });
                }
            }

            return lstSupervisores?.OrderBy(s => s.SupervisorNome).ThenBy(c => c.SupervisorId);
        }
    }
}
