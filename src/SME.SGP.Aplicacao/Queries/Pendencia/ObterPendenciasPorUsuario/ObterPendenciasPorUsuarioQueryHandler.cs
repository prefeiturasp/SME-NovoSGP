using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasPorUsuarioQueryHandler : ConsultasBase, IRequestHandler<ObterPendenciasPorUsuarioQuery, PaginacaoResultadoDto<PendenciaDto>>
    {
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IMediator mediator;

        public ObterPendenciasPorUsuarioQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioPendencia repositorioPendencia) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<PaginacaoResultadoDto<PendenciaDto>> Handle(ObterPendenciasPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            PaginacaoResultadoDto<Pendencia> pendenciaPaginada;
            List<long> listaPendenciasUsuario;

            if (ParametrosValidos(request))
            {
                var tiposPendenciasGruposFiltrar = ObterTiposPendenciasGrupos(request.TipoPendencia);
                pendenciaPaginada = await repositorioPendencia.ListarPendenciasUsuarioComFiltro(request.UsuarioId,
                                                                                                tiposPendenciasGruposFiltrar.ToArray(),
                                                                                                request.TituloPendencia,
                                                                                                request.TurmaCodigo,
                                                                                                Paginacao);
                
                if (!string.IsNullOrEmpty(request.TurmaCodigo) && request.TipoPendencia == 0)
                {
                    listaPendenciasUsuario = (await repositorioPendencia.FiltrarListaPendenciasUsuario(request.TurmaCodigo,
                                                                                                       pendenciaPaginada.Items.ToList())).ToList();
                    pendenciaPaginada.Items = pendenciaPaginada.Items.Where(pendencia => listaPendenciasUsuario.Any(c => c == pendencia.Id));
                }
                return await MapearParaDtoPaginado(pendenciaPaginada);
            }

            pendenciaPaginada = await repositorioPendencia.ListarPendenciasUsuarioSemFiltro(request.UsuarioId, Paginacao);
            listaPendenciasUsuario = (await repositorioPendencia.FiltrarListaPendenciasUsuario(request.TurmaCodigo, pendenciaPaginada.Items.ToList())).ToList();
            pendenciaPaginada.Items = pendenciaPaginada.Items.Where(pendencia => listaPendenciasUsuario.Any(c => c == pendencia.Id));
           
            return await MapearParaDtoPaginado(pendenciaPaginada);
        }

        private bool ParametrosValidos(ObterPendenciasPorUsuarioQuery request) =>
                    !string.IsNullOrEmpty(request.TurmaCodigo) || 
                    !string.IsNullOrEmpty(request.TituloPendencia) ||
                    request.TipoPendencia > 0;

        private static IEnumerable<int> ObterTiposPendenciasGrupos(int? tipoPendenciaGrupo)
        {
            if (tipoPendenciaGrupo > 0)
                return ObterTiposPendenciasGrupos((TipoPendenciaGrupo)tipoPendenciaGrupo.GetValueOrDefault()).ToArray();
            return Array.Empty<int>();

        }
        private static IEnumerable<int> ObterTiposPendenciasGrupos(TipoPendenciaGrupo tipoPendenciaGrupo)
        {
            var tiposPendencias = Enum.GetValues(typeof(TipoPendenciaGrupo))
                .Cast<TipoPendenciaGrupo>()
                .Select(d => new { codigo = (int)d, descricao = d.ObterNome() })
                .Where(d => d.descricao == tipoPendenciaGrupo.Name());

            return tiposPendencias.Select(tp => tp.codigo);
        }

        private async Task<PaginacaoResultadoDto<PendenciaDto>> MapearParaDtoPaginado(PaginacaoResultadoDto<Pendencia> pendenciasPaginadas)
        {
            return new PaginacaoResultadoDto<PendenciaDto>()
            {
                Items = await MapearParaDto(pendenciasPaginadas.Items),
                TotalPaginas = pendenciasPaginadas.TotalPaginas,
                TotalRegistros = pendenciasPaginadas.TotalRegistros
            };
        }

        private async Task<IEnumerable<PendenciaDto>> MapearParaDto(IEnumerable<Pendencia> pendencias)
        {
            var listaPendenciasDto = new List<PendenciaDto>();
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            foreach (var pendencia in pendencias)
            {
                var pendenciasDto = await ObterPendencias(pendencia, usuarioLogado.CodigoRf);

                foreach (var dto in pendenciasDto)
                {
                    var criadoEmUtc = DateTime.SpecifyKind(pendencia.CriadoEm, DateTimeKind.Utc);
                    var dias = (DateTime.UtcNow - criadoEmUtc).TotalDays;

                    dto.MensagemTooltip = ObterMensagemTooltip(pendencia, dias);
                }

                listaPendenciasDto.AddRange(pendenciasDto);
            }

            return listaPendenciasDto;
        }

        private string ObterMensagemTooltip(Pendencia pendencia, double dias)
        {
            if (dias <= 10) return null;

            return pendencia.Tipo switch
            {
                (TipoPendencia)7 => "Esta aula está sem registro de frequência há mais de 10 dias",
                (TipoPendencia)3 => "Esta aula está sem plano de aula registrado há mais de 10 dias.",
                (TipoPendencia)9 => "Esta aula está sem registro no diário de bordo há mais de 10 dias.",
                _ => null
            };
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendencias(Pendencia pendencia, string codigoRf)
        {
            switch (true)
            {
                case var _ when pendencia.EhPendenciaAula() || pendencia.EhPendenciaCadastroEvento() || pendencia.EhPendenciaCalendarioUe():
                    return await ObterPendenciasAulasFormatadas(pendencia);
                case var _ when pendencia.EhPendenciaAusenciaAvaliacaoProfessor():
                    return await ObterPendenciaAusenciaAvaliacaoProfessor(pendencia);
                case var _ when pendencia.EhPendenciaAusenciaAvaliacaoCP():
                    return await ObterDescricaoPendenciaAusenciaAvaliacaoCP(pendencia);
                case var _ when pendencia.EhPendenciaFechamento():
                    return await ObterPendenciasFechamentoFormatadas(pendencia);
                case var _ when pendencia.EhAusenciaFechamento():
                    return await ObterPendenciasProfessorFormatadas(pendencia);
                case var _ when pendencia.EhPendenciaDiarioBordo():
                    return await ObterPendenciasDiarioBordoFormatadas(pendencia, codigoRf);
                case var _ when pendencia.EhPendenciaAEE() || pendencia.EhPendenciaDevolutiva() || pendencia.EhPendenciaProfessor():
                    return await ObterPendenciasProfessorFormatadas(pendencia);
                default:
                    return Enumerable.Empty<PendenciaDto>();
            }
        }
        
        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasProfessorFormatadas(Pendencia pendencia)
        {
            var turma = await mediator.Send(new ObterTurmaDaPendenciaProfessorQuery(pendencia.Id));

            return new List<PendenciaDto>
            {
                new()
                {
                    Tipo = pendencia.Tipo.GroupName(),
                    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                    Detalhe = pendencia.Descricao,
                    Turma = ObterNomeTurma(turma),
                    Bimestre = await ObterBimestreTurma(pendencia)
                }
            };
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasFechamentoFormatadas(Pendencia pendencia)
        {
            var turma = await mediator.Send(new ObterTurmaDaPendenciaFechamentoQuery(pendencia.Id));

            var descricao = string.Empty;

            if (pendencia.EhPendenciaCadastroEvento())
                descricao = await ObterDescricaoPendenciaEvento(pendencia);

            if (pendencia.EhPendenciaAusenciaDeRegistroIndividual())
                descricao = await ObterDescricaoPendenciaAusenciaRegistroIndividualAsync(pendencia);

            if (string.IsNullOrEmpty(descricao) && !string.IsNullOrEmpty(pendencia.Descricao))
                descricao = pendencia.Descricao;

            return new List<PendenciaDto>
            {
                new()
                {
                    Tipo = pendencia.Tipo.GroupName(),
                    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                    Detalhe = descricao,
                    Turma = ObterNomeTurma(turma),
                    Bimestre = await ObterBimestreTurma(pendencia)
                }
            };
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciaAusenciaAvaliacaoProfessor(Pendencia pendencia)
        {
            var pendenciasProfessorBimestre = (await mediator.Send(new ObterPendenciasProfessorPorPendenciaIdQuery(pendencia.Id))).ToList();

            var pendenciasProfessor = new List<PendenciaDto>();

            if (!pendenciasProfessorBimestre.Any())
                return pendenciasProfessor;

            var pendenciaProfessor = pendenciasProfessorBimestre.FirstOrDefault();

            if (pendenciaProfessor.EhNulo())
                return pendenciasProfessor;

            var pendenciaFormatada = new PendenciaDto
            {
                Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                Tipo = pendencia.Tipo.GroupName(),
                Bimestre = ObterNomeBimestre(pendenciaProfessor.Bimestre),
                Turma = $"{((Modalidade)pendenciaProfessor.ModalidadeCodigo).ShortName()} - {pendenciaProfessor.NomeTurma}"
            };

            var descricao = new StringBuilder(pendencia.Descricao);
            
            descricao.AppendLine("<br /><ul>");

            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            pendenciaFormatada.Detalhe = descricao.ToString();
            pendenciasProfessor.Add(pendenciaFormatada);

            return pendenciasProfessor;
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasAulasFormatadas(Pendencia pendencia)
        {
            var pendenciasAulas = await mediator.Send(new ObterPendenciasAulasPorPendenciaQuery(pendencia.Id));

            var agrupamentoPendenciasBimestres = pendenciasAulas.GroupBy(g => new { g.Bimestre,g.DisciplinaId, g.ModalidadeCodigo, g.NomeTurma }, (key, group) =>
                                                        new PendenciaAgrupamentoDto()
                                                        {
                                                            Bimestre = key.Bimestre,
                                                            ModalidadeCodigo = key.ModalidadeCodigo,
                                                            NomeTurma = key.NomeTurma,
                                                            PendenciaDetalhes = group.Select(s => new PendenciaDetalheDto()
                                                            {
                                                                DataAula = s.DataAula,
                                                                EhReposicao = s.EhReposicao
                                                            })
                                                        });

            return ObterPendenciasFormatadas(pendencia, agrupamentoPendenciasBimestres);
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasDiarioBordoFormatadas(Pendencia pendencia, string codigoRf)
        {
            var pendenciasDiarios = await mediator.Send(new ObterPendenciasDiarioPorPendenciaIdEProfessorQuery(pendencia.Id, codigoRf));

            var agrupamentoPendenciasBimestres = pendenciasDiarios.GroupBy(g => new { g.Bimestre, g.ModalidadeCodigo, g.NomeTurma }, (key, group) =>
                                                        new PendenciaAgrupamentoDto()
                                                        {
                                                            Bimestre = key.Bimestre,
                                                            ModalidadeCodigo = key.ModalidadeCodigo,
                                                            NomeTurma = key.NomeTurma,
                                                            PendenciaDetalhes = group.Select(s => new PendenciaDetalheDto()
                                                            {
                                                                DataAula = s.DataAula,
                                                                EhReposicao = s.EhReposicao
                                                            })
                                                        });

            return ObterPendenciasFormatadas(pendencia, agrupamentoPendenciasBimestres);
        }

        private static IEnumerable<PendenciaDto> ObterPendenciasFormatadas(Pendencia pendencia, IEnumerable<PendenciaAgrupamentoDto> agrupamentoPendenciasBimestres)
        {
            var pendenciasDiarioBordo = new List<PendenciaDto>();

            foreach (var pendenciaBimestre in agrupamentoPendenciasBimestres)
            {
                var descricao = new StringBuilder(pendencia.Descricao);

                descricao.AppendLine("<br /><ul>");

                var pendenciaFormatada = new PendenciaDto
                {
                    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                    Tipo = pendencia.Tipo.GroupName(),
                    Bimestre = ObterNomeBimestre(pendenciaBimestre.Bimestre),
                    Turma = $"{((Modalidade)pendenciaBimestre.ModalidadeCodigo).ShortName()} - {pendenciaBimestre.NomeTurma}"
                };

                foreach (var pendenciaDetalhe in pendenciaBimestre.PendenciaDetalhes)
                    descricao.AppendLine($"<li>{pendenciaDetalhe.DataAula:dd/MM/yyyy} {ObterComplementoReposicao(pendenciaDetalhe.EhReposicao)}</li>");

                descricao.AppendLine("</ul>");
                descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

                pendenciaFormatada.Detalhe = descricao.ToString();
                pendenciasDiarioBordo.Add(pendenciaFormatada);

            }

            return pendenciasDiarioBordo;
        }

        private async Task<string> ObterBimestreTurma(Pendencia pendencia)
        {
            if (pendencia.EhPendenciaFechamento())
            {
                var pendenciaCompleto =
                    await mediator.Send(new ObterTurmaDaPendenciaFechamentoCompletoQuery(pendencia.Id));
                
               return pendenciaCompleto.NaoEhNulo() ? ObterNomeBimestre(pendenciaCompleto.Bimestre) : string.Empty;
            }

            var turma = await ObterTurmaPorPendencia(pendencia);

            if (turma.EhNulo())
                return "";
            
            return await ObterDescricaoBimestrePendencia(pendencia.Id, turma.Id, pendencia.CriadoEm);
        }

        private Task<Turma> ObterTurmaPorPendencia(Pendencia pendencia)
        {
            var lista = new List<(bool executar, Func<Task<Turma>> funcaoPendenciaTurma)>()
            {
                (pendencia.EhPendenciaAula(), async () => await mediator.Send(new ObterTurmaDaPendenciaAulaQuery(pendencia.Id))),
                (pendencia.EhPendenciaProfessor(), async () => await mediator.Send(new ObterTurmaDaPendenciaProfessorQuery(pendencia.Id))),
                (pendencia.EhPendenciaDiarioBordo(), async () => await mediator.Send(new ObterTurmaDaPendenciaDiarioQuery(pendencia.Id))),
                (pendencia.EhPendenciaDevolutiva(), async () => await mediator.Send(new ObterTurmaDaPendenciaDevolutivaQuery(pendencia.Id)))
            };

            foreach(var item in lista)
            {
                if (item.executar)
                    return item.funcaoPendenciaTurma();
            }

            return Task.FromResult<Turma>(null);
        }

        private async Task<string> ObterDescricaoBimestrePendencia(long pendenciaId, long turmaId, DateTime dataPendenciaCriada)
        {
            var bimestre = await mediator.Send(new ObterModalidadePorPendenciaQuery(pendenciaId, turmaId, dataPendenciaCriada));
            return ObterNomeBimestre(bimestre);
        }

        private static string ObterNomeTurma(Turma turma)
            => turma.NaoEhNulo() ? $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}" : "";

        private static string ObterNomeBimestre(int bimestre)
            => bimestre == 0 ? "Final" : $"{bimestre}º Bimestre";

        private async Task<IEnumerable<PendenciaDto>> ObterDescricaoPendenciaAusenciaAvaliacaoCP(Pendencia pendencia)
        {
            var pendenciasProfessor = (await mediator.Send(new ObterPendenciasProfessorPorPendenciaIdQuery(pendencia.Id))).ToList();

            var pendenciasCP = new List<PendenciaDto>();

            if (!pendenciasProfessor.Any())
                return pendenciasCP;

            var pendenciaProfessor = pendenciasProfessor.FirstOrDefault();

            if (pendenciaProfessor.EhNulo())
                return pendenciasCP;

            var pendenciaFormatada = new PendenciaDto
            {
                Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                Tipo = pendencia.Tipo.GroupName(),
                Bimestre = ObterNomeBimestre(pendenciaProfessor.Bimestre),
                Turma = $"{((Modalidade)pendenciaProfessor.ModalidadeCodigo).ShortName()} - {pendenciaProfessor.NomeTurma}"
            };

            var descricao = new StringBuilder(pendencia.Descricao);
            
            descricao.Append("<br/><table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            descricao.Append("<tr>");
            descricao.Append("<td style='padding: 5px;'><b>Componente curricular</b></td>");
            descricao.Append("<td style='padding: 5px;'><b>Professor titular</b></td>");
            descricao.Append("</tr>");
            
            foreach (var itemPendenciaProfessor in pendenciasProfessor)
            {
                descricao.Append("<tr style='padding:5px'>");
                descricao.Append($"<td style='padding: 5px;'>{itemPendenciaProfessor.ComponenteCurricular}</td>");
                descricao.Append($"<td style='padding: 5px;'>{itemPendenciaProfessor.Professor}({itemPendenciaProfessor.ProfessorRf})</td>");
                descricao.Append("</tr>");
            }
            
            descricao.Append("</table><br/>");
            descricao.Append($"<b>{pendencia.Instrucao}</b>");

            pendenciaFormatada.Detalhe = descricao.ToString();
            pendenciasCP.Add(pendenciaFormatada);

            return pendenciasCP;
        }

        private async Task<string> ObterDescricaoPendenciaEvento(Pendencia pendencia)
        {
            var pendenciasEventos = await mediator.Send(new ObterPendenciasParametroEventoPorPendenciaQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul>");

            foreach (var pendenciaEvento in pendenciasEventos)
            {
                descricao.AppendLine($"<li>{pendenciaEvento.Descricao} ({pendenciaEvento.Valor})</li>");
            }

            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }

        private static string ObterComplementoReposicao(bool ehReposicao)
        {
            return ehReposicao ? " - Reposição" : "";
        }

        private async Task<string> ObterDescricaoPendenciaAusenciaRegistroIndividualAsync(Pendencia pendencia)
        {
            var alunos = await mediator.Send(new ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul style='padding-top: 20px; padding-left: 20px';>");

            foreach (var aluno in alunos)
                descricao.AppendLine($"<li style='padding-top: 2px;'>{aluno.NomeValido()} ({aluno.CodigoAluno})</li>");
            
            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }
    }
}