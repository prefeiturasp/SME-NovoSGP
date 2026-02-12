using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;


namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler : IRequestHandler<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery, IEnumerable<AbrangenciaTurmaRetorno>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> Handle(ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery request,CancellationToken cancellationToken)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            var anosInfantilDesconsiderar = !request.ConsideraNovosAnosInfantil
                ? await mediator.Send(
                    new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(request.AnoLetivo,
                        Modalidade.EducacaoInfantil))
                : null;

            var result = await repositorioAbrangencia.ObterTurmasPorTipos(request.CodigoUe, login, perfil,
                request.Modalidade, request.Tipos.NaoEhNulo() && request.Tipos.Any() ? request.Tipos : null, request.Periodo,
                request.ConsideraHistorico, request.AnoLetivo, anosInfantilDesconsiderar);

            // Com base no codigo das turmas listada, é feito uma busca na Api Eol que está atualizada. 
            var codigosTurmas = result?.Select(t => t.Codigo.ToString())?.ToList();
            var listaTurmaEOL = await mediator.Send(new ObterTurmasApiEolQuery(codigosTurmas));

            // Transforma codigo em string e remove os repetidos.
            var codigosValidos = new HashSet<string>(
                listaTurmaEOL.Select(x => x.Codigo.ToString())
            );

            // Dados mocados
            var turmasFakes = new[]
            {
                new AbrangenciaTurmaRetorno
                {
                    Ano = "2025",
                    AnoLetivo = 2025,
                    Codigo = "0002 TURMA123",
                    CodigoModalidade = (int)Modalidade.Fundamental,
                    Nome = "Turma Teste 1",
                    Semestre = 1,
                    EnsinoEspecial = false,
                    Id = 1,
                    TipoTurma = 10,
                    NomeFiltro = "0 ATurma Teste 1"
                },
                new AbrangenciaTurmaRetorno
                {
                    Ano = "2025",
                    AnoLetivo = 2025,
                    Codigo = "0003 TURMA456",
                    CodigoModalidade = (int)Modalidade.Medio,
                    Nome = "Turma Teste 2",
                    Semestre = 2,
                    EnsinoEspecial = false,
                    Id = 2,
                    TipoTurma = 20,
                    NomeFiltro = "0 ATurma Teste 2"
                }
            };

            //result = (result ?? Enumerable.Empty<AbrangenciaTurmaRetorno>()).Concat(turmasFakes);

            // Filtrando as turmas com base nas turmas atualizada.
            var resultatualizado = result
                .Where(x => !string.IsNullOrEmpty(x.Codigo) && codigosValidos.Contains(x.Codigo))
                .ToList();

            //return OrdernarTurmasItinerario(result);
            return OrdernarTurmasItinerario(resultatualizado);            
        }  
 
        private IEnumerable<AbrangenciaTurmaRetorno> OrdernarTurmasItinerario(IEnumerable<AbrangenciaTurmaRetorno> result)
        {
            List<AbrangenciaTurmaRetorno> turmasOrdenadas = new List<AbrangenciaTurmaRetorno>();

            var turmasItinerario = result.Where(t => t.TipoTurma == (int)TipoTurma.Itinerarios2AAno || t.TipoTurma == (int)TipoTurma.ItinerarioEnsMedio);
            var turmas = result.Where(t => !turmasItinerario.Any(ti => ti.Id == t.Id));

            turmasOrdenadas.AddRange(turmas.OrderBy(a => a.ModalidadeTurmaNome));
            turmasOrdenadas.AddRange(turmasItinerario.OrderBy(a => a.ModalidadeTurmaNome));

            return turmasOrdenadas;
        }
    }
}
