import React from 'react';
import { Switch } from 'react-router-dom';
import { setRotas } from '../redux/modulos/navegacao/actions';
import { store } from '../redux';
import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';
import AtribuicaoSupervisorLista from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorLista';
import AtribuicaoSupervisorCadastro from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorCadastro';
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
import TipoEventosLista from '~/paginas/CalendarioEscolar/TipoEventos/tipoEventosLista';
import TipoEventosForm from '~/paginas/CalendarioEscolar/TipoEventos/tipoEventosForm';
import { useSelector } from 'react-redux';

export default function Rotas() {
  const rotas = new Map();
  const permissoes = useSelector(state => state.usuario.permissoes);

  rotas.set('/calendario-escolar/tipo-eventos', {
    breadcrumbName: 'Tipo de Eventos',
    menu: ['Calendário Escolar'],
    parent: '/',
    component: TipoEventosLista,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-eventos/novo', {
    breadcrumbName: 'Cadastro de Tipo de Eventos',
    parent: '/calendario-escolar/tipo-eventos',
    component: TipoEventosForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-eventos/editar/:id', {
    breadcrumbName: 'Cadastro de Tipo de Eventos',
    parent: '/calendario-escolar/tipo-eventos',
    component: TipoEventosForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/planejamento/plano-ciclo', {
    breadcrumbName: 'Plano de Ciclo',
    menu: ['Planejamento'],
    parent: '/',
    component: PlanoCiclo,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/planejamento/plano-anual', {
    breadcrumbName: 'Plano Anual',
    menu: ['Planejamento'],
    parent: '/',
    component: PlanoAnual,
    exact: false,
    tipo: RotasTipo.EstruturadaAutenticada,
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

  rotas.set('/gestao/atribuicao-supervisor-lista', {
    breadcrumbName: 'Atribuição de Supervisor',
    menu: ['Gestão'],
    parent: '/',
    component: AtribuicaoSupervisorLista,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/gestao/atribuicao-supervisor', {
    breadcrumbName: 'Nova Atribuição',
    parent: '/gestao/atribuicao-supervisor-lista',
    component: AtribuicaoSupervisorCadastro,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/gestao/atribuicao-supervisor/:dreId/', {
    breadcrumbName: 'Atribuição de Supervisor',
    parent: '/gestao/atribuicao-supervisor-lista',
    component: AtribuicaoSupervisorCadastro,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/gestao/atribuicao-supervisor/:dreId/:supervisorId', {
    breadcrumbName: 'Atribuição de Supervisor',
    parent: '/gestao/atribuicao-supervisor-lista',
    component: AtribuicaoSupervisorCadastro,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/notificacoes/:id', {
    breadcrumbName: ['Notificações'],
    parent: '/',
    component: DetalheNotificacao,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/notificacoes', {
    breadcrumbName: ['Notificações'],
    parent: '/',
    component: NotificacoesLista,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/meus-dados', {
    breadcrumbName: 'Perfil',
    parent: '/',
    component: MeusDados,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/periodos-escolares', {
    breadcrumbName: 'Períodos Escolares',
    menu: ['Calendário Escolar'],
    parent: '/',
    component: PeriodosEscolares,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/usuarios/reiniciar-senha', {
    breadcrumbName: 'Reiniciar Senha',
    menu: ['Configurações', 'Usuários'],
    parent: '/',
    component: ReiniciarSenha,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-calendario-escolar', {
    breadcrumbName: 'Tipo de Calendário Escolar',
    menu: ['Calendário Escolar'],
    parent: '/',
    component: TipoCalendarioEscolarLista,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-calendario-escolar/novo', {
    breadcrumbName: 'Cadastro do Tipo de Calendário Escolar',
    parent: '/calendario-escolar/tipo-calendario-escolar',
    component: TipoCalendarioEscolarForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-calendario-escolar/editar/:id', {
    breadcrumbName: 'Cadastro do Tipo de Calendário Escolar',
    parent: '/calendario-escolar/tipo-calendario-escolar',
    component: TipoCalendarioEscolarForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/', {
    icone: 'fas fa-home',
    parent: null,
    component: Principal,
    exact: true,
    limpaSelecaoMenu: true,
    paginaInicial: true,
    dicaIcone: 'Página Inicial',
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-feriado', {
    breadcrumbName: 'Lista de Tipo de Feriado',
    menu: ['Tipo Feriado'],
    parent: '/',
    component: TipoFeriadoLista,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-feriado/novo', {
    breadcrumbName: 'Cadastro de Tipo de Feriado',
    parent: '/calendario-escolar/tipo-feriado',
    component: TipoFeriadoForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/calendario-escolar/tipo-feriado/editar/:id', {
    breadcrumbName: 'Alterar Tipo de Feriado',
    parent: '/calendario-escolar/tipo-feriado',
    component: TipoFeriadoForm,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  const rotasArray = [];
  for (var [key, value] of rotas) {
    const rota = value;
    rota.path = key + (value.params ? value.params : '');
    if (permissoes[rota.path] || (rota.tipo !==RotasTipo.EstruturadaAutenticada|| rota.path === '/')) {
      rotasArray.push(rota);
    }

    const rotaRedux = {
      path: value.paginaInicial ? '/' : key,
      icone: value.icone,
      dicaIcone: value.dicaIcone,
      params: value.params,
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
