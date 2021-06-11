import { setRotas } from '../redux/modulos/navegacao/actions';
import { store } from '../redux';
import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/Anual/planoAnual';
import AtribuicaoSupervisorLista from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorLista';
import AtribuicaoSupervisorCadastro from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorCadastro';
import AtribuicaoEsporadicaLista from '../paginas/Gestao/AtribuicaoEsporadica/Lista';
import AtribuicaoEsporadicaForm from '../paginas/Gestao/AtribuicaoEsporadica/Form';
import AtribuicaoCJLista from '../paginas/Gestao/AtribuicaoCJ/Lista';
import AtribuicaoCJForm from '../paginas/Gestao/AtribuicaoCJ/Form';
import DetalheNotificacao from '~/paginas/Notificacoes/Detalhes/detalheNotificacao';
import NotificacoesLista from '~/paginas/Notificacoes/Lista/listaNotificacoes';
import RotasTipo from '~/constantes/rotasTipo';
import MeusDados from '~/paginas/Perfil/meusDados';
import PeriodosEscolares from '~/paginas/CalendarioEscolar/PeriodosEscolares/PeriodosEscolares';
import TabsReiniciarSenha from '~/paginas/Configuracoes/Usuarios/TabsReiniciarSenha';
import TipoCalendarioEscolarLista from '~/paginas/CalendarioEscolar/TipoCalendarioEscolar/tipoCalendarioEscolarLista';
import TipoCalendarioEscolarForm from '~/paginas/CalendarioEscolar/TipoCalendarioEscolar/tipoCalendarioEscolarForm';
import TipoFeriadoLista from '~/paginas/CalendarioEscolar/TipoFeriado/tipoFeriadoLista';
import TipoFeriadoForm from '~/paginas/CalendarioEscolar/TipoFeriado/tipoFeriadoForm';
import CalendarioEscolar from '~/paginas/CalendarioEscolar/Calendario';
import EventosLista from '~/paginas/CalendarioEscolar/Eventos/eventosLista';
import EventosForm from '~/paginas/CalendarioEscolar/Eventos/eventosForm';
import TipoEventosLista from '~/paginas/CalendarioEscolar/TipoEventos/tipoEventosLista';
import TipoEventosForm from '~/paginas/CalendarioEscolar/TipoEventos/tipoEventosForm';
import SemPermissao from '~/paginas/SemPermissao/sem-permissao';
import RotasDto from '~/dtos/rotasDto';
import CadastroAula from '~/paginas/CalendarioProfessor/CadastroAula/cadastroAula';
import FrequenciaPlanoAula from '~/paginas/DiarioClasse/FrequenciaPlanoAula/frequenciaPlanoAula';
import AvaliacaoForm from '~/paginas/CalendarioEscolar/Avaliacao/avaliacaoForm';
import Notas from '~/paginas/DiarioClasse/Notas/notas';
import TipoAvaliacaoLista from '~paginas/Configuracoes/TipoAvaliacao/tipoAvaliacaoLista';
import TipoAvaliacaoForm from '~paginas/Configuracoes/TipoAvaliacao/tipoAvaliacaoForm';
import AulaDadaAulaPrevista from '~/paginas/DiarioClasse/AulaDadaAulaPrevista/aulaDadaAulaPrevista';
import RegistroPOALista from '~/paginas/DiarioClasse/RegistroPOA/Lista';
import RegistroPOAForm from '~/paginas/DiarioClasse/RegistroPOA/Form';
import CompensacaoAusenciaLista from '~/paginas/DiarioClasse/CompensacaoAusencia/compensacaoAusenciaLista';
import CompensacaoAusenciaForm from '~/paginas/DiarioClasse/CompensacaoAusencia/compensacaoAusenciaForm';
import FechamentoBismestre from '~/paginas/Fechamento/FechamentoBimestre/fechamento-bimestre';
import PeriodoFechamentoAbertura from '~/paginas/CalendarioEscolar/PeriodoFechamentoAbertura/periodo-fechamento-abertura';
import ResumosGraficosPAP from '~/paginas/Relatorios/PAP/ResumosGraficos';
import PaginaComErro from '~/paginas/Erro/pagina-com-erro';
import PeriodoFechamentoReaberturaLista from '~/paginas/CalendarioEscolar/PeriodoFechamentoReabertura/periodoFechamentoReaberturaLista';
import PeriodoFechamentoReaberturaForm from '~/paginas/CalendarioEscolar/PeriodoFechamentoReabertura/periodoFechamentoReaberturaForm';
import RelatorioPAPAcompanhamento from '~/paginas/Relatorios/PAP/Acompanhamento';
import PendenciasFechamentoLista from '~/paginas/Fechamento/PendenciasFechamento/pendenciasFechamentoLista';
import PendenciasFechamentoForm from '~/paginas/Fechamento/PendenciasFechamento/pendenciasFechamentoForm';
import ComunicadosLista from '~/paginas/AcompanhamentoEscolar/Comunicados/Lista';
import ComunicadosCadastro from '~/paginas/AcompanhamentoEscolar/Comunicados/Cadastro';
import ConselhoClasse from '~/paginas/Fechamento/ConselhoClasse/conselhoClasse';
import RelatorioSemestral from '~/paginas/Relatorios/PAP/RelatorioSemestral/relatorioSemestral';

import CalendarioProfessor from '~/paginas/CalendarioEscolar/CalendarioProfessor';
import TerritorioSaber from '~/paginas/Planejamento/TerritorioSaber';
import HistoricoEscolar from '~/paginas/Relatorios/HistoricoEscolar/historicoEscolar';
import AtaFinalResultados from '~/paginas/Relatorios/Atas/AtaFinalResultados/ataFinalResultados';

import BoletimSimples from '~/paginas/Relatorios/DiarioClasse/BoletimSimples';
import FaltasFrequencia from '~/paginas/Relatorios/Frequencia/faltasFrequencia';
import ListaDiarioBordo from '~/paginas/DiarioClasse/DiarioBordo/listaDiarioBordo';
import DiarioBordo from '~/paginas/DiarioClasse/DiarioBordo/diarioBordo';
import RelatorioPendencias from '~/paginas/Relatorios/Pendencias/relatorioPendencias';
import CartaIntencoes from '~/paginas/Planejamento/CartaIntencoes/cartaIntencoes';
import RelatorioParecerConclusivo from '~/paginas/Relatorios/ParecerConclusivo/relatorioParecerConclusivo';
import DevolutivasLista from '~/paginas/DiarioClasse/Devolutivas/devolutivasLista';
import RegistroIndividual from '~/paginas/DiarioClasse/RegistroIndividual/registroIndividual';
import DevolutivasForm from '~/paginas/DiarioClasse/Devolutivas/devolutivasForm';
import RelatorioNotasConceitosFinais from '~/paginas/Relatorios/NotasConceitosFinais/relatorioNotasConceitosFinais';
import RelatorioCompensacaoAusencia from '~/paginas/Relatorios/CompensacaoAusencia/relatorioCompensacaoAusencia';
import DashboardEscolaAqui from '~/paginas/Dashboard/DashboardEscolaAqui/dashboardEscolaAqui';
import ControleGrade from '~/paginas/Relatorios/DiarioClasse/ControleGrade/controleGrade';
import Sondagem from '~/paginas/Sondagem/sondagem';
import PocUploadArquivos from '~/componentes-sgp/UploadArquivos/pocUploadArquivos';
import DocumentosPlanosTrabalhoLista from '~/paginas/Gestao/DocumentosPlanosTrabalho/documentosPlanosTrabalhoLista';
import DocumentosPlanosTrabalhoCadastro from '~/paginas/Gestao/DocumentosPlanosTrabalho/documentosPlanosTrabalhoCadastro';
import HistoricoNotificacoes from '~/paginas/Relatorios/Notificacoes/HistoricoNotificacoes/historicoNotificacoes';
import RelatorioUsuarios from '~/paginas/Relatorios/Gestao/Usuarios/relatorioUsuarios';
import AtribuicaoCJ from '~/paginas/Relatorios/Gestao/AtribuicaoCJ/atribuicaoCJ';
import RelatorioHistoricoAlteracoesNotas from '~/paginas/Relatorios/Fechamento/HistoricoAlteracoesNotas/relatorioHistoricoAlteracoesNotas';
import relatorioEscolaAquiAdesao from '~/paginas/Relatorios/EscolaAqui/Adesao/relatorioEscolaAquiAdesao';
import RelatorioLeitura from '~/paginas/Relatorios/EscolaAqui/Leitura/relatorioLeitura';
import ListaOcorrencias from '~/paginas/Gestao/Ocorrencia/ListaOcorrencias';
import CadastroOcorrencias from '~/paginas/Gestao/Ocorrencia/CadastroOcorrencias';
import RelatorioPlanejamentoDiario from '~/paginas/Relatorios/DiarioClasse/PlanejamentoDiario/relatorioPlanejamentoDiario';
import EncaminhamentoAEELista from '~/paginas/AEE/Encaminhamento/Lista/encaminhamentoAEELista';
import EncaminhamentoAEECadastro from '~/paginas/AEE/Encaminhamento/Cadastro/encaminhamentoAEECadastro';
import RegistroItineranciaAEECadastro from '~/paginas/AEE/RegistroItinerancia/Cadastro/registroItineranciaAEECadastro';
import AcompanhamentoFrequencia from '~/paginas/DiarioClasse/AcompanhamentoFrequencia/acompanhamentoFrequencia';
import PlanoAEELista from '~/paginas/AEE/Plano/Lista/planoAEELista';
import PlanoAEECadastro from '~/paginas/AEE/Plano/Cadastro/planoAEECadastro';
import RegistroItineranciaAEELista from '~/paginas/AEE/RegistroItinerancia/Lista/registroItineranciaAEELista';
import AcompanhamentoAprendizagem from '~/paginas/Fechamento/AcompanhamentoAprendizagem/acompanhamentoAprendizagem';
import RelatorioDevolutivas from '~/paginas/Relatorios/Planejamento/Devolutivas/relatorioDevolutivas';
import DashboardAEE from '~/paginas/Dashboard/AEE/dashboardAEE';
import DashboardRegistroItinerancia from '~/paginas/Dashboard/DashboardRegistroItinerancia/dashboardRegistroItinerancia';
import DashboardFrequencia from '~/paginas/Dashboard/DashboardFrequencia/dashboardFrequencia';

const rotas = new Map();

rotas.set(RotasDto.RELATORIO_BOLETIM_SIMPLES, {
  breadcrumbName: ['Boletim'],
  menu: ['Fechamento'],
  parent: '/',
  component: BoletimSimples,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_BOLETIM_SIMPLES,
});

rotas.set(RotasDto.ACOMPANHAMENTO_COMUNICADOS, {
  breadcrumbName: 'Comunicados',
  menu: ['Gestão'],
  parent: '/',
  component: ComunicadosLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ACOMPANHAMENTO_COMUNICADOS,
});

rotas.set(`${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/novo`, {
  menu: ['Gestão'],
  parent: RotasDto.ACOMPANHAMENTO_COMUNICADOS,
  component: ComunicadosCadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ACOMPANHAMENTO_COMUNICADOS,
});

rotas.set(`${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/editar/:id`, {
  breadcrumbName: 'Comunicados',
  menu: ['Gestão'],
  parent: RotasDto.ACOMPANHAMENTO_COMUNICADOS,
  component: ComunicadosCadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ACOMPANHAMENTO_COMUNICADOS,
});

rotas.set(`${RotasDto.PAP}`, {
  breadcrumbName: 'Resumos e Gráficos PAP',
  menu: ['Relatórios', 'PAP'],
  parent: '/',
  component: ResumosGraficosPAP,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PAP,
});

rotas.set(RotasDto.CALENDARIO_ESCOLAR, {
  breadcrumbName: 'Calendário Escolar',
  parent: '/',
  component: CalendarioEscolar,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
});

rotas.set(RotasDto.TIPO_EVENTOS, {
  breadcrumbName: 'Tipo de Eventos',
  menu: ['Calendário Escolar'],
  parent: '/',
  component: TipoEventosLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_EVENTOS,
});

rotas.set('/calendario-escolar/tipo-eventos/novo', {
  breadcrumbName: 'Cadastro de Tipo de Eventos',
  parent: '/calendario-escolar/tipo-eventos',
  component: TipoEventosForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_EVENTOS,
});

rotas.set('/calendario-escolar/tipo-eventos/editar/:id', {
  breadcrumbName: 'Cadastro de Tipo de Eventos',
  parent: '/calendario-escolar/tipo-eventos',
  component: TipoEventosForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_EVENTOS,
});

rotas.set(RotasDto.PLANO_CICLO, {
  breadcrumbName: 'Plano de Ciclo',
  menu: ['Planejamento'],
  parent: '/',
  component: PlanoCiclo,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PLANO_CICLO,
});

rotas.set(RotasDto.PLANO_ANUAL, {
  breadcrumbName: 'Plano Anual',
  menu: ['Planejamento'],
  parent: '/',
  component: PlanoAnual,
  exact: false,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PLANO_ANUAL,
});

rotas.set(RotasDto.TERRITORIO_SABER, {
  breadcrumbName: 'Territórios do Saber',
  menu: ['Planejamento'],
  parent: '/',
  component: TerritorioSaber,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TERRITORIO_SABER,
});

rotas.set(RotasDto.ATRIBUICAO_SUPERVISOR_LISTA, {
  breadcrumbName: 'Atribuição de Supervisor',
  menu: ['Gestão'],
  parent: '/',
  component: AtribuicaoSupervisorLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_SUPERVISOR_LISTA,
});

rotas.set('/gestao/atribuicao-supervisor', {
  breadcrumbName: 'Nova Atribuição',
  parent: '/gestao/atribuicao-supervisor-lista',
  component: AtribuicaoSupervisorCadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_SUPERVISOR_LISTA,
});

rotas.set('/gestao/atribuicao-supervisor/:dreId/', {
  breadcrumbName: 'Atribuição de Supervisor',
  parent: '/gestao/atribuicao-supervisor-lista',
  component: AtribuicaoSupervisorCadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_SUPERVISOR_LISTA,
});

rotas.set('/gestao/atribuicao-supervisor/:dreId/:supervisorId', {
  breadcrumbName: 'Atribuição de Supervisor',
  parent: '/gestao/atribuicao-supervisor-lista',
  component: AtribuicaoSupervisorCadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_SUPERVISOR_LISTA,
});

rotas.set('/gestao/atribuicao-esporadica', {
  breadcrumbName: 'Atribuição Esporádica',
  menu: ['Gestão'],
  parent: '/',
  component: AtribuicaoEsporadicaLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_ESPORADICA_LISTA,
});

rotas.set('/gestao/atribuicao-esporadica/novo', {
  breadcrumbName: 'Atribuição',
  parent: '/gestao/atribuicao-esporadica',
  component: AtribuicaoEsporadicaForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_ESPORADICA_LISTA,
});

rotas.set('/gestao/atribuicao-esporadica/editar/:id', {
  breadcrumbName: 'Atribuição',
  parent: '/gestao/atribuicao-esporadica',
  component: AtribuicaoEsporadicaForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_ESPORADICA_LISTA,
});

rotas.set('/gestao/atribuicao-cjs', {
  breadcrumbName: 'Atribuição de CJ',
  menu: ['Gestão'],
  parent: '/',
  component: AtribuicaoCJLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_CJ_LISTA,
});

rotas.set('/gestao/atribuicao-cjs/novo', {
  breadcrumbName: 'Atribuição',
  parent: '/gestao/atribuicao-cjs',
  component: AtribuicaoCJForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_CJ_LISTA,
});

rotas.set('/gestao/atribuicao-cjs/editar', {
  breadcrumbName: 'Atribuição',
  parent: '/gestao/atribuicao-cjs',
  component: AtribuicaoCJForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATRIBUICAO_CJ_LISTA,
});

rotas.set('/notificacoes/:id', {
  breadcrumbName: ['Notificações'],
  parent: '/',
  component: DetalheNotificacao,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.NOTIFICACOES,
});

rotas.set(RotasDto.NOTIFICACOES, {
  breadcrumbName: ['Notificações'],
  parent: '/',
  component: NotificacoesLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.NOTIFICACOES,
});

rotas.set(RotasDto.MEUS_DADOS, {
  breadcrumbName: 'Perfil',
  parent: '/',
  component: MeusDados,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.MEUS_DADOS,
});

rotas.set(RotasDto.PERIODOS_ESCOLARES, {
  breadcrumbName: 'Períodos Escolares',
  menu: ['Calendário Escolar'],
  parent: '/',
  component: PeriodosEscolares,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PERIODOS_ESCOLARES,
});

rotas.set(RotasDto.REINICIAR_SENHA, {
  breadcrumbName: 'Reiniciar Senha',
  menu: ['Configurações', 'Usuários'],
  parent: '/',
  component: TabsReiniciarSenha,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.REINICIAR_SENHA,
});

rotas.set(RotasDto.TIPO_CALENDARIO_ESCOLAR, {
  breadcrumbName: 'Tipo de Calendário Escolar',
  menu: ['Calendário Escolar'],
  parent: '/',
  component: TipoCalendarioEscolarLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_CALENDARIO_ESCOLAR,
});

rotas.set('/calendario-escolar/tipo-calendario-escolar/novo', {
  breadcrumbName: 'Cadastro do Tipo de Calendário Escolar',
  parent: '/calendario-escolar/tipo-calendario-escolar',
  component: TipoCalendarioEscolarForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_CALENDARIO_ESCOLAR,
});

rotas.set('/calendario-escolar/tipo-calendario-escolar/editar/:id', {
  breadcrumbName: 'Cadastro do Tipo de Calendário Escolar',
  parent: '/calendario-escolar/tipo-calendario-escolar',
  component: TipoCalendarioEscolarForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_CALENDARIO_ESCOLAR,
});

rotas.set(RotasDto.PRINCIPAL, {
  icone: 'fas fa-home',
  parent: null,
  component: Principal,
  exact: true,
  limpaSelecaoMenu: true,
  paginaInicial: true,
  dicaIcone: 'Página Inicial',
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});

rotas.set(RotasDto.TIPO_FERIADO, {
  breadcrumbName: 'Lista de Tipo de Feriado',
  menu: ['Tipo Feriado'],
  parent: '/',
  component: TipoFeriadoLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_FERIADO,
});

rotas.set('/calendario-escolar/tipo-feriado/novo', {
  breadcrumbName: 'Cadastro de Tipo de Feriado',
  parent: '/calendario-escolar/tipo-feriado',
  component: TipoFeriadoForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_FERIADO,
});

rotas.set('/calendario-escolar/tipo-feriado/editar/:id', {
  breadcrumbName: 'Alterar Tipo de Feriado',
  parent: '/calendario-escolar/tipo-feriado',
  component: TipoFeriadoForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.TIPO_FERIADO,
});

rotas.set(RotasDto.SEM_PERMISSAO, {
  breadcrumbName: 'Sem permissão',
  parent: '/',
  component: SemPermissao,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});

rotas.set(RotasDto.EVENTOS, {
  breadcrumbName: 'Evento do Calendário Escolar',
  menu: ['Calendário Escolar'],
  parent: '/',
  component: EventosLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.EVENTOS,
});

rotas.set(`${RotasDto.EVENTOS}/:tipoCalendarioId`, {
  breadcrumbName: 'Evento do Calendário Escolar',
  menu: ['Calendário Escolar'],
  parent: '/',
  component: EventosLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.EVENTOS,
});

rotas.set('/calendario-escolar/eventos/novo/:tipoCalendarioId', {
  breadcrumbName: 'Cadastro de Eventos no Calendário Escolar',
  parent: '/calendario-escolar/eventos',
  component: EventosForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.EVENTOS,
});

rotas.set('/calendario-escolar/eventos/editar/:id', {
  breadcrumbName: 'Cadastro de Eventos no Calendário Escolar',
  parent: '/calendario-escolar/eventos',
  component: EventosForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.EVENTOS,
});

rotas.set('/calendario-escolar/eventos/editar/:id/:tipoCalendarioId', {
  breadcrumbName: 'Cadastro de Eventos no Calendário Escolar',
  parent: '/calendario-escolar/eventos',
  component: EventosForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.EVENTOS,
});

rotas.set(RotasDto.CALENDARIO_PROFESSOR, {
  breadcrumbName: 'Calendário do Professor',
  menu: ['Calendário Escolar'],
  parent: '/',
  component: CalendarioProfessor,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CALENDARIO_PROFESSOR,
});

rotas.set(RotasDto.CADASTRO_DE_AULA, {
  breadcrumbName: 'Cadastro de Aula',
  parent: '/calendario-escolar/calendario-professor',
  component: CadastroAula,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CALENDARIO_PROFESSOR,
});

rotas.set(`${RotasDto.CADASTRO_DE_AULA}/novo/:tipoCalendarioId`, {
  breadcrumbName: 'Cadastro de Aula',
  parent: RotasDto.CADASTRO_DE_AULA,
  component: CadastroAula,
  exact: false,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CALENDARIO_PROFESSOR,
});

rotas.set(`${RotasDto.CADASTRO_DE_AULA}/editar/:id`, {
  breadcrumbName: 'Cadastro de Aula',
  parent: RotasDto.CADASTRO_DE_AULA,
  component: CadastroAula,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CALENDARIO_PROFESSOR,
});

rotas.set(`${RotasDto.CADASTRO_DE_AVALIACAO}/novo`, {
  breadcrumbName: 'Cadastro de Avaliação',
  parent: RotasDto.CALENDARIO_PROFESSOR,
  component: AvaliacaoForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CALENDARIO_PROFESSOR,
});

rotas.set(`${RotasDto.CADASTRO_DE_AVALIACAO}/editar/:id`, {
  breadcrumbName: 'Cadastro de Avaliação',
  parent: RotasDto.CALENDARIO_PROFESSOR,
  component: AvaliacaoForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CALENDARIO_PROFESSOR,
});

rotas.set(`${RotasDto.FREQUENCIA_PLANO_AULA}`, {
  breadcrumbName: 'Frequência/Plano de aula',
  menu: ['Diário de Classe'],
  parent: '/',
  component: FrequenciaPlanoAula,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.FREQUENCIA_PLANO_AULA,
});

rotas.set(`${RotasDto.ACOMPANHAMENTO_FREQUENCIA}`, {
  breadcrumbName: 'Acompanhamento de Frequência',
  menu: ['Diário de Classe'],
  parent: '/',
  component: AcompanhamentoFrequencia,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ACOMPANHAMENTO_FREQUENCIA,
});

rotas.set(`${RotasDto.NOTAS}/:disciplinaId/:bimestre`, {
  breadcrumbName: 'Notas',
  menu: ['Diário de Classe'],
  parent: '/',
  component: Notas,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.NOTAS,
});

rotas.set(`${RotasDto.NOTAS}`, {
  breadcrumbName: 'Notas',
  menu: ['Diário de Classe'],
  parent: '/',
  component: Notas,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.NOTAS,
});

rotas.set(`${RotasDto.TIPO_AVALIACAO}`, {
  breadcrumbName: 'Configurações/Tipo Avalição',
  menu: ['Configurações', 'Tipo Avaliação'],
  parent: '/',
  component: TipoAvaliacaoLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});
rotas.set(`${RotasDto.TIPO_AVALIACAO}/novo`, {
  breadcrumbName: 'Configurações/Tipo Avalição Novo',
  menu: ['Configurações', 'Tipo Avaliação'],
  parent: '/',
  component: TipoAvaliacaoForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});
rotas.set(`${RotasDto.TIPO_AVALIACAO}/editar/:id`, {
  breadcrumbName: 'Configurações/Tipo Avalição Novo',
  menu: ['Configurações', 'Tipo Avaliação'],
  parent: '/',
  component: TipoAvaliacaoForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});

rotas.set(`${RotasDto.AULA_DADA_AULA_PREVISTA}`, {
  breadcrumbName: 'Aula prevista X Aula dada',
  menu: ['Diário de Classe'],
  parent: '/',
  component: AulaDadaAulaPrevista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});

rotas.set(`${RotasDto.REGISTRO_POA}`, {
  breadcrumbName: 'Registro POA',
  menu: ['Planejamento'],
  parent: '/',
  component: RegistroPOALista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});

rotas.set(`${RotasDto.REGISTRO_POA}/novo`, {
  breadcrumbName: 'Registro',
  parent: RotasDto.REGISTRO_POA,
  component: RegistroPOAForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.REGISTRO_POA,
});

rotas.set(`${RotasDto.REGISTRO_POA}/editar/:id`, {
  breadcrumbName: 'Registro',
  parent: RotasDto.REGISTRO_POA,
  component: RegistroPOAForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.REGISTRO_POA,
});

rotas.set(`${RotasDto.COMPENSACAO_AUSENCIA}`, {
  breadcrumbName: 'Compensação de Ausência',
  menu: ['Diário de Classe'],
  parent: '/',
  component: CompensacaoAusenciaLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.COMPENSACAO_AUSENCIA,
});

rotas.set(`${RotasDto.COMPENSACAO_AUSENCIA}/novo`, {
  breadcrumbName: 'Cadastrar Compensação de Ausência',
  parent: RotasDto.COMPENSACAO_AUSENCIA,
  component: CompensacaoAusenciaForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.COMPENSACAO_AUSENCIA,
});

rotas.set(`${RotasDto.COMPENSACAO_AUSENCIA}/editar/:id`, {
  breadcrumbName: 'Editar Compensação de Ausência',
  parent: RotasDto.COMPENSACAO_AUSENCIA,
  component: CompensacaoAusenciaForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.COMPENSACAO_AUSENCIA,
});

rotas.set(`${RotasDto.FECHAMENTO_BIMESTRE}`, {
  breadcrumbName: 'Fechamento de Bimestre',
  menu: ['Fechamento'],
  parent: '/',
  component: FechamentoBismestre,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.FECHAMENTO_BIMESTRE,
});

rotas.set(`${RotasDto.PERIODO_FECHAMENTO_ABERTURA}`, {
  breadcrumbName: 'Abertura',
  menu: ['Calendário Escolar', 'Período de Fechamento'],
  parent: '/',
  component: PeriodoFechamentoAbertura,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PERIODO_FECHAMENTO_ABERTURA,
});

rotas.set(`${RotasDto.PERIODO_FECHAMENTO_REABERTURA}`, {
  breadcrumbName: 'Reabertura',
  menu: ['Calendário Escolar', 'Período de Fechamento'],
  parent: '/',
  component: PeriodoFechamentoReaberturaLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PERIODO_FECHAMENTO_REABERTURA,
});

rotas.set(`${RotasDto.PERIODO_FECHAMENTO_REABERTURA}/novo/:tipoCalendarioId`, {
  breadcrumbName: 'Períodos',
  parent: RotasDto.PERIODO_FECHAMENTO_REABERTURA,
  component: PeriodoFechamentoReaberturaForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PERIODO_FECHAMENTO_REABERTURA,
});

rotas.set(`${RotasDto.PERIODO_FECHAMENTO_REABERTURA}/editar/:id`, {
  breadcrumbName: 'Períodos',
  parent: RotasDto.PERIODO_FECHAMENTO_REABERTURA,
  component: PeriodoFechamentoReaberturaForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PERIODO_FECHAMENTO_REABERTURA,
});

rotas.set(`${RotasDto.PENDENCIAS_FECHAMENTO}`, {
  breadcrumbName: 'Pendências do Fechamento',
  menu: ['Fechamento'],
  parent: '/',
  component: PendenciasFechamentoLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PENDENCIAS_FECHAMENTO,
});
rotas.set(
  `${RotasDto.PENDENCIAS_FECHAMENTO}/:bimestre/:codigoComponenteCurricular`,
  {
    breadcrumbName: 'Pendências do Fechamento',
    menu: ['Fechamento'],
    parent: '/',
    component: PendenciasFechamentoLista,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
    temPermissionamento: true,
    chavePermissao: RotasDto.PENDENCIAS_FECHAMENTO,
  }
);

rotas.set(`${RotasDto.PENDENCIAS_FECHAMENTO}/:id`, {
  breadcrumbName: 'Pendências do Fechamento',
  menu: ['Fechamento'],
  parent: RotasDto.PENDENCIAS_FECHAMENTO,
  component: PendenciasFechamentoForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.PENDENCIAS_FECHAMENTO,
});

rotas.set('/erro', {
  breadcrumbName: 'Erro',
  parent: '/',
  component: PaginaComErro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
});

rotas.set(RotasDto.RELATORIO_PAP_ACOMPANHAMENTO, {
  breadcrumbName: 'Acompanhamento',
  menu: ['Relatórios', 'PAP'],
  parent: '/',
  component: RelatorioPAPAcompanhamento,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_PAP_ACOMPANHAMENTO,
});

rotas.set(RotasDto.CONSELHO_CLASSE, {
  breadcrumbName: 'Conselho de Classe',
  menu: ['Fechamento'],
  parent: '/',
  component: ConselhoClasse,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CONSELHO_CLASSE,
});

rotas.set(RotasDto.RELATORIO_SEMESTRAL, {
  breadcrumbName: 'Relatório Semestral',
  menu: ['Relatórios', 'PAP'],
  parent: '/',
  component: RelatorioSemestral,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_SEMESTRAL,
});
rotas.set(RotasDto.ATA_FINAL_RESULTADOS, {
  breadcrumbName: 'Ata final de resultados',
  menu: ['Relatórios', 'Atas'],
  parent: '/',
  component: AtaFinalResultados,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ATA_FINAL_RESULTADOS,
});

rotas.set(RotasDto.HISTORICO_ESCOLAR, {
  breadcrumbName: 'Histórico Escolar',
  menu: ['Relatórios'],
  parent: '/',
  component: HistoricoEscolar,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  // temPermissionamento: true,
  // chavePermissao: RotasDto.HISTORICO_ESCOLAR,
});

rotas.set(RotasDto.FALTAS_FREQUENCIA, {
  breadcrumbName: 'Faltas e frequência',
  menu: ['Relatórios', 'Frequência'],
  parent: '/',
  component: FaltasFrequencia,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.FALTAS_FREQUENCIA,
});

rotas.set(RotasDto.DIARIO_BORDO, {
  breadcrumbName: 'Diário de Bordo (Intencionalidade docente)',
  menu: ['Diário de Classe'],
  parent: '/',
  component: ListaDiarioBordo,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DIARIO_BORDO,
});

rotas.set(`${RotasDto.DIARIO_BORDO}/novo`, {
  breadcrumbName: 'Cadastrar',
  menu: [],
  parent: RotasDto.DIARIO_BORDO,
  component: DiarioBordo,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DIARIO_BORDO,
});

rotas.set(`${RotasDto.DIARIO_BORDO}/detalhes/:aulaId`, {
  breadcrumbName: 'Diário de Bordo (Intencionalidade docente)',
  menu: ['Diário de Classe'],
  parent: '/',
  component: DiarioBordo,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DIARIO_BORDO,
});

rotas.set(RotasDto.RELATORIO_PENDENCIAS, {
  breadcrumbName: 'Relatório de pendências',
  menu: ['Relatórios'],
  parent: '/',
  component: RelatorioPendencias,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_PENDENCIAS,
});

rotas.set(RotasDto.RELATORIO_PARECER_CONCLUSIVO, {
  breadcrumbName: 'Parecer conclusivo',
  menu: ['Relatórios', 'Fechamento'],
  parent: '/',
  component: RelatorioParecerConclusivo,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_PARECER_CONCLUSIVO,
});

rotas.set(RotasDto.RELATORIO_NOTAS_CONCEITOS_FINAIS, {
  breadcrumbName: 'Notas e conceitos finais',
  menu: ['Relatórios', 'Fechamento'],
  parent: '/',
  component: RelatorioNotasConceitosFinais,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_NOTAS_CONCEITOS_FINAIS,
});

rotas.set(RotasDto.CARTA_INTENCOES, {
  breadcrumbName: 'Carta de intenções',
  menu: ['Planejamento '],
  parent: '/',
  component: CartaIntencoes,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.CARTA_INTENCOES,
});

rotas.set(RotasDto.DEVOLUTIVAS, {
  breadcrumbName: 'Devolutivas',
  menu: ['Diário de Classe '],
  parent: '/',
  component: DevolutivasLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DEVOLUTIVAS,
});

rotas.set(`${RotasDto.DEVOLUTIVAS}/novo`, {
  breadcrumbName: 'Cadastrar Devolutiva',
  parent: RotasDto.DEVOLUTIVAS,
  component: DevolutivasForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DEVOLUTIVAS,
});

rotas.set(`${RotasDto.DEVOLUTIVAS}/editar/:id`, {
  breadcrumbName: 'Alterar Devolutiva',
  parent: RotasDto.DEVOLUTIVAS,
  component: DevolutivasForm,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DEVOLUTIVAS,
});

rotas.set(RotasDto.REGISTRO_INDIVIDUAL, {
  breadcrumbName: 'Registro individual',
  menu: ['Diário de Classe '],
  parent: '/',
  component: RegistroIndividual,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.REGISTRO_INDIVIDUAL,
});

rotas.set(RotasDto.RELATORIO_COMPENSACAO_AUSENCIA, {
  breadcrumbName: 'Compensação de ausência',
  menu: ['Relatórios', 'Frequência'],
  parent: '/',
  component: RelatorioCompensacaoAusencia,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.RELATORIO_COMPENSACAO_AUSENCIA,
});

rotas.set(RotasDto.DASHBOARD_ESCOLA_AQUI, {
  breadcrumbName: 'Escola aqui',
  menu: ['Dashboard'],
  parent: '/',
  component: DashboardEscolaAqui,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.DASHBOARD_ESCOLA_AQUI,
});

rotas.set(RotasDto.CONTROLE_GRADE, {
  breadcrumbName: 'Controle de grade',
  menu: ['Relatórios', 'Diário de classe'],
  parent: '/',
  component: ControleGrade,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.CONTROLE_GRADE,
});

rotas.set(RotasDto.RELATORIO_PLANEJAMENTO_DIARIO, {
  breadcrumbName: 'Controle de planejamento diário',
  menu: ['Relatórios', 'Diário de classe'],
  parent: '/',
  component: RelatorioPlanejamentoDiario,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_PLANEJAMENTO_DIARIO,
});

rotas.set(RotasDto.SONDAGEM, {
  breadcrumbName: 'Sistema Sondagem',
  parent: '/',
  component: Sondagem,
  exact: false,
  tipo: RotasTipo.EstruturadaAutenticada,
});

rotas.set(RotasDto.POC_UPLOAD_ARQUIVOS, {
  breadcrumbName: 'Poc Upload Arquivos',
  parent: '/',
  component: PocUploadArquivos,
  exact: false,
  tipo: RotasTipo.EstruturadaAutenticada,
});

rotas.set(RotasDto.HISTORICO_NOTIFICACOES, {
  breadcrumbName: 'Histórico de notificações',
  menu: ['Relatórios', 'Notificações'],
  parent: '/',
  component: HistoricoNotificacoes,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.HISTORICO_NOTIFICACOES,
});

rotas.set(RotasDto.DOCUMENTOS_PLANOS_TRABALHO, {
  breadcrumbName: 'Documentos e planos de trabalho',
  menu: ['Gestão'],
  parent: '/',
  component: DocumentosPlanosTrabalhoLista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DOCUMENTOS_PLANOS_TRABALHO,
});

rotas.set(`${RotasDto.DOCUMENTOS_PLANOS_TRABALHO}/novo`, {
  breadcrumbName: 'Upload do arquivo',
  parent: RotasDto.DOCUMENTOS_PLANOS_TRABALHO,
  component: DocumentosPlanosTrabalhoCadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DOCUMENTOS_PLANOS_TRABALHO,
});

rotas.set(`${RotasDto.DOCUMENTOS_PLANOS_TRABALHO}/editar/:id`, {
  breadcrumbName: 'Upload do arquivo',
  parent: RotasDto.DOCUMENTOS_PLANOS_TRABALHO,
  component: DocumentosPlanosTrabalhoCadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DOCUMENTOS_PLANOS_TRABALHO,
});

rotas.set(RotasDto.RELATORIO_USUARIOS, {
  breadcrumbName: 'usuários',
  menu: ['Relatórios', 'Gestão', 'Usuários'],
  parent: '/',
  component: RelatorioUsuarios,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_USUARIOS,
});

rotas.set(RotasDto.RELATORIO_ATRIBUICAO_CJ, {
  breadcrumbName: 'Atribuições',
  menu: ['Relatórios', 'Gestão'],
  parent: '/',
  component: AtribuicaoCJ,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.RELATORIO_ATRIBUICAO_CJ,
});

rotas.set(RotasDto.RELATORIO_ALTERACAO_NOTAS, {
  breadcrumbName: 'Histórico de alterações de notas',
  menu: ['Relatórios', 'Fechamento'],
  parent: '/',
  component: RelatorioHistoricoAlteracoesNotas,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.RELATORIO_ALTERACAO_NOTAS,
});

rotas.set(RotasDto.RELATORIO_DEVOLUTIVAS, {
  breadcrumbName: 'Devolutivas',
  menu: ['Relatórios', 'Planejamento'],
  parent: '/',
  component: RelatorioDevolutivas,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.RELATORIO_DEVOLUTIVAS,
});

rotas.set(RotasDto.RELATORIO_LEITURA, {
  breadcrumbName: 'Leitura',
  menu: ['Relatórios', 'Escola aqui'],
  parent: '/',
  component: RelatorioLeitura,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_LEITURA,
});

rotas.set(RotasDto.RELATORIO_ESCOLA_AQUI_ADESAO, {
  breadcrumbName: 'Adesão',
  menu: ['Relatórios', 'Escola aqui'],
  parent: '/',
  component: relatorioEscolaAquiAdesao,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.RELATORIO_ESCOLA_AQUI_ADESAO,
});

rotas.set(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO, {
  breadcrumbName: 'Encaminhamento',
  menu: ['AEE'],
  parent: '/',
  component: EncaminhamentoAEELista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_ENCAMINHAMENTO,
});

rotas.set(`${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}/novo`, {
  breadcrumbName: 'Cadastrar',
  parent: `${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}`,
  component: EncaminhamentoAEECadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_ENCAMINHAMENTO,
});

rotas.set(`${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}/editar/:id`, {
  breadcrumbName: 'Editar',
  parent: `${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}`,
  component: EncaminhamentoAEECadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_ENCAMINHAMENTO,
});

rotas.set(`${RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA}`, {
  breadcrumbName: 'Registro de itinerância',
  menu: ['AEE'],
  parent: '/',
  component: RegistroItineranciaAEELista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA,
});

rotas.set(`${RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA}/novo`, {
  breadcrumbName: 'Cadastro',
  parent: RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA,
  component: RegistroItineranciaAEECadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA,
});

rotas.set(`${RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA}/editar/:id`, {
  breadcrumbName: 'Registro de itinerância',
  menu: ['AEE'],
  parent: '/',
  component: RegistroItineranciaAEECadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA,
});

rotas.set(RotasDto.RELATORIO_AEE_PLANO, {
  breadcrumbName: 'Plano',
  menu: ['AEE'],
  parent: '/',
  component: PlanoAEELista,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_PLANO,
});

rotas.set(`${RotasDto.RELATORIO_AEE_PLANO}/novo`, {
  breadcrumbName: 'Cadastro',
  parent: RotasDto.RELATORIO_AEE_PLANO,
  component: PlanoAEECadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_PLANO,
});

rotas.set(`${RotasDto.RELATORIO_AEE_PLANO}/editar/:id`, {
  breadcrumbName: 'Editar',
  parent: `${RotasDto.RELATORIO_AEE_PLANO}`,
  component: PlanoAEECadastro,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.RELATORIO_AEE_PLANO,
});

rotas.set(RotasDto.OCORRENCIAS, {
  breadcrumbName: 'Ocorrências',
  menu: ['Gestão'],
  parent: '/',
  component: ListaOcorrencias,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.OCORRENCIAS,
});

rotas.set(`${RotasDto.OCORRENCIAS}/novo`, {
  breadcrumbName: 'Cadastro',
  parent: RotasDto.OCORRENCIAS,
  component: CadastroOcorrencias,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.OCORRENCIAS,
});

rotas.set(`${RotasDto.OCORRENCIAS}/editar/:id`, {
  breadcrumbName: 'Cadastro',
  parent: RotasDto.OCORRENCIAS,
  component: CadastroOcorrencias,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: false,
  chavePermissao: RotasDto.OCORRENCIAS,
});

rotas.set(RotasDto.ACOMPANHAMENTO_APRENDIZAGEM, {
  breadcrumbName: 'Relatório do Acompanhamento da Aprendizagem',
  menu: ['Fechamento'],
  parent: '/',
  component: AcompanhamentoAprendizagem,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.ACOMPANHAMENTO_APRENDIZAGEM,
});

rotas.set(RotasDto.DASHBOARD_AEE, {
  breadcrumbName: 'AEE',
  menu: ['Dashboard'],
  parent: '/',
  component: DashboardAEE,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DASHBOARD_AEE,
});

rotas.set(RotasDto.DASHBOARD_REGISTRO_ITINERANCIA, {
  breadcrumbName: 'Registro de Itinerância',
  menu: ['Dashboard'],
  parent: '/',
  component: DashboardRegistroItinerancia,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DASHBOARD_REGISTRO_ITINERANCIA,
});

rotas.set(RotasDto.DASHBOARD_FREQUENCIA, {
  breadcrumbName: 'Frequência',
  menu: ['Dashboard'],
  parent: '/',
  component: DashboardFrequencia,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
  temPermissionamento: true,
  chavePermissao: RotasDto.DASHBOARD_FREQUENCIA,
});

const rotasArray = [];
for (const [key, value] of rotas) {
  const rota = value;
  rota.path = key;
  rotasArray.push(rota);

  const rotaRedux = {
    path: value.paginaInicial ? '/' : key,
    icone: value.icone,
    dicaIcone: value.dicaIcone,
    breadcrumbName: value.breadcrumbName,
    menu: value.menu,
    parent: value.parent,
    limpaSelecaoMenu: value.limpaSelecaoMenu,
  };

  store.dispatch(setRotas(rotaRedux));
}

export default rotasArray;
