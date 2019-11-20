import React from 'react';
import { Switch } from 'react-router-dom';
import { setRotas } from '../redux/modulos/navegacao/actions';
import { store } from '../redux';
import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';
import AtribuicaoSupervisorLista from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorLista';
import AtribuicaoSupervisorCadastro from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorCadastro';
import AtribuicaoEsporadicaLista from '../paginas/Gestao/AtribuicaoEsporadica/Lista';
import AtribuicaoEsporadicaForm from '../paginas/Gestao/AtribuicaoEsporadica/Form';
import DetalheNotificacao from '~/paginas/Notificacoes/Detalhes/detalheNotificacao';
import NotificacoesLista from '~/paginas/Notificacoes/Lista/listaNotificacoes';
import RotaAutenticadaEstruturada from './rotaAutenticadaEstruturada';
import RotasTipo from '~/constantes/rotasTipo';
import Login from '~/paginas/Login';
import RecuperarSenha from '~/paginas/RecuperarSenha';
import RedefinirSenha from '~/paginas/RedefinirSenha';
import RotaNaoAutenticadaDesestruturada from './rotaNaoAutenticadaDesestruturada';
import RotaAutenticadaDesestruturada from './rotaAutenticadaDesestruturada';
import RotaMista from './rotaMista';
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

export default function Rotas() {
  const rotas = new Map();

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

  rotas.set('/login/:redirect?/', {
    breadcrumbName: '',
    menu: '',
    parent: '/',
    component: Login,
    exact: true,
    tipo: RotasTipo.DesestruturadaNaoAutenticada,
  });

  rotas.set('/recuperar-senha', {
    breadcrumbName: '',
    menu: '',
    parent: '/',
    component: RecuperarSenha,
    exact: true,
    tipo: RotasTipo.DesestruturadaNaoAutenticada,
  });

  rotas.set('/redefinir-senha/:token?/', {
    breadcrumbName: '',
    menu: '',
    parent: '/',
    component: RedefinirSenha,
    exact: false,
    tipo: RotasTipo.Mista,
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
    // temPermissionamento: true,
    // chavePermissao: RotasDto.ATRIBUICAO_ESPORADICA_LISTA,
  });

  rotas.set('/gestao/atribuicao-esporadica/novo', {
    breadcrumbName: 'Atribuição',
    parent: '/gestao/atribuicao-esporadica',
    component: AtribuicaoEsporadicaForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
    // temPermissionamento: true,
    // chavePermissao: RotasDto.ATRIBUICAO_ESPORADICA_LISTA,
  });

  rotas.set('/gestao/atribuicao-esporadica/editar/:id', {
    breadcrumbName: 'Atribuição',
    parent: '/gestao/atribuicao-esporadica',
    component: AtribuicaoEsporadicaForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
    temPermissionamento: false,
    //chavePermissao: RotasDto.ATRIBUICAO_SUPERVISOR_LISTA,
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
    temPermissionamento: false,
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

  rotas.set('/sem-permissao', {
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

  // TODO - Alterar quando tiver o calendário do professor
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

  rotas.set(`${RotasDto.FREQUENCIA_PLANO_AULA}`, {
    breadcrumbName: 'Frequência/Plano de aula',
    menu: ['Diário de Classe'],
    parent: '/',
    component: FrequenciaPlanoAula,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
    temPermissionamento: false,
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

  return (
    <div className="h-100">
      <Switch>
        {rotasArray.map(rota => {
          switch (rota.tipo) {
            case RotasTipo.EstruturadaAutenticada:
              return (
                <RotaAutenticadaEstruturada
                  path={rota.path}
                  key={rota.path}
                  exact={rota.exact}
                  component={rota.component}
                  temPermissionamento={rota.temPermissionamento}
                  chavePermissao={rota.chavePermissao}
                />
              );

            case RotasTipo.DesestruturadaNaoAutenticada:
              return (
                <RotaNaoAutenticadaDesestruturada
                  path={rota.path}
                  key={rota.path}
                  exact={rota.exact}
                  component={rota.component}
                />
              );

            case RotasTipo.DesestruturadaAutenticada:
              return (
                <RotaAutenticadaDesestruturada
                  path={rota.path}
                  key={rota.path}
                  exact={rota.exact}
                  component={rota.component}
                />
              );

            case RotasTipo.Mista:
              return (
                <RotaMista
                  path={rota.path}
                  key={rota.path}
                  exact={rota.exact}
                  component={rota.component}
                />
              );

            default:
              return (
                <RotaAutenticadaEstruturada
                  path={rota.path}
                  key={rota.path}
                  exact={rota.exact}
                  component={rota.component}
                />
              );
          }
        })}
      </Switch>
    </div>
  );
}
