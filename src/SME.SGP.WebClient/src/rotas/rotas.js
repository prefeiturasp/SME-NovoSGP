import { setRotas } from '../redux/modulos/navegacao/actions';
import { store } from '../redux';
import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';
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
import ReiniciarSenha from '~/paginas/Configuracoes/Usuarios/reiniciarSenha';
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
import CadastroAula from '~/paginas/CalendarioEscolar/CadastroAula/cadastroAula';
import CalendarioProfessor from '~/paginas/CalendarioProfessor/Calendario';
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

const rotas = new Map();

rotas.set(`${RotasDto.PAP}`, {
  breadcrumbName: 'Resumos e Gráficos PAP',
  menu: ['Relatórios', 'PAP'],
  parent: '/',
  component: ResumosGraficosPAP,
  exact: true,
  tipo: RotasTipo.EstruturadaAutenticada,
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
  component: ReiniciarSenha,
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
  exact: true,
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

rotas.set(`${RotasDto.PERIODO_FECHAMENTO_REABERTURA}/novo`, {
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
  temPermissionamento: false, // chavePermissao: RotasDto.REINICIAR_SENHA,
});

const rotasArray = [];
for (var [key, value] of rotas) {
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
