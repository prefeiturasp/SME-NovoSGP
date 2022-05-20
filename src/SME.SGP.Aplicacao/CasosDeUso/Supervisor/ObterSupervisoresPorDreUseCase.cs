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

        public async Task<IEnumerable<SupervisorDto>> Executar(ObterSupervisoresPorDreDto filtro)
        {
            var supervisoresEol = Enumerable.Empty<SupervisoresRetornoDto>().ToList();

            switch (filtro.TipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PAAI:
                    {
                        var funcionariosEol = await mediator.Send(new ObterFuncionariosPorDreECargoQuery(filtro.DreCodigo, 29));

                        foreach (var funcionario in funcionariosEol)
                        {
                            supervisoresEol.Add(new SupervisoresRetornoDto()
                            {
                                Login = funcionario.CodigoRf,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                case TipoResponsavelAtribuicao.Psicopedagogo:
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    {
                        supervisoresEol = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(filtro.DreCodigo,
                            new List<Guid> { Perfis.PERFIL_PSICOLOGO_ESCOLAR, Perfis.PERFIL_PSICOPEDAGOGO, Perfis.PERFIL_ASSISTENTE_SOCIAL }))).ToList();

                        break;
                    }
                default:
                    supervisoresEol = (await mediator.Send(new ObterSupervisoresPorDreEolQuery(filtro.DreCodigo))).ToList();
                    break;
            }

            var supervisoresAtribuidos = (await mediator.Send(new ObterSupervisoresPorDreQuery(filtro.DreCodigo, TipoResponsavelAtribuicao.SupervisorEscolar)))
                .DistinctBy(c => c.SupervisorId);

            var nomeServidoresAtribuidos = await mediator.Send(new ObterListaNomePorListaRFQuery(supervisoresAtribuidos?.Select(c => c.SupervisorId)));

            if (nomeServidoresAtribuidos == null || !nomeServidoresAtribuidos.Any())
                throw new NegocioException("A API/EOL não retornou os nomes dos responsáveis atribuídos.");

            var lstSupervisores = new List<SupervisorDto>();

            if (string.IsNullOrEmpty(filtro.SupervisorNome))
            {
                if (supervisoresEol != null && supervisoresEol.Any())
                    lstSupervisores.AddRange(supervisoresEol?.Select(a => new SupervisorDto() { SupervisorId = a.Login, SupervisorNome = a.NomeServidor }));

                if (supervisoresAtribuidos != null && supervisoresAtribuidos.Any())
                {
                    lstSupervisores.AddRange(supervisoresAtribuidos?.Where(s => !supervisoresEol.Select(se => se.Login).Contains(s.SupervisorId))?
                        .Select(a => new SupervisorDto()
                        {
                            SupervisorId = a.SupervisorId,
                            SupervisorNome = nomeServidoresAtribuidos?.FirstOrDefault(n => n.CodigoRF == a.SupervisorId)?.Nome
                        }));
                }
            }
            else
            {
                if (supervisoresEol != null && supervisoresEol.Any())
                {
                    lstSupervisores.AddRange(
                        from a in supervisoresEol
                        where a.NomeServidor.ToLower().Contains(filtro.SupervisorNome.ToLower())
                        select new SupervisorDto() { SupervisorId = a.Login, SupervisorNome = a.NomeServidor });
                }

                if (supervisoresAtribuidos != null && supervisoresAtribuidos.Any())
                {
                    lstSupervisores.AddRange(
                       from a in supervisoresAtribuidos
                       join b in nomeServidoresAtribuidos on a.SupervisorId equals b.CodigoRF
                       where !supervisoresEol.Select(s => s.Login).Contains(a.SupervisorId) && b.Nome.ToLower().Contains(filtro.SupervisorNome.ToLower())
                       select new SupervisorDto() { SupervisorId = b.CodigoRF, SupervisorNome = b.Nome });
                }
            }

            return lstSupervisores?.OrderBy(s => s.SupervisorNome);
        }
    }
}
