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
import RotaNaoAutenticadaDesestruturada from './rotaNaoAutenticadaDesestruturada';
import RotaAutenticadaDesestruturada from './rotaAutenticadaDesestruturada';
import PrimeiroAcesso from '~/paginas/PrimeiroAcesso';

export default function Rotas() {
  const rotas = new Map();

  rotas.set('/planejamento/plano-ciclo', {
    breadcrumbName: 'Plano de Ciclo',
    menu: 'Planejamento',
    parent: '/',
    component: PlanoCiclo,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/planejamento/plano-anual', {
    breadcrumbName: 'Plano Anual',
    menu: 'Planejamento',
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

  rotas.set('/redefinir-senha', {
    breadcrumbName: '',
    menu: '',
    parent: '/',
    component: PrimeiroAcesso,
    exact: true,
    tipo: RotasTipo.DesestruturadaNaoAutenticada,
  });

  rotas.set('/gestao/atribuicao-supervisor-lista', {
    breadcrumbName: 'Atribuição de Supervisor',
    menu: 'Gestão',
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
    breadcrumbName: 'Notificações',
    parent: '/',
    component: DetalheNotificacao,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/notificacoes', {
    breadcrumbName: 'Notificações',
    parent: '/',
    component: NotificacoesLista,
    exact: true,
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  rotas.set('/:rf?', {
    icone: 'fas fa-home',
    parent: null,
    component: Principal,
    exact: true,
    limpaSelecaoMenu: true,
    paginaInicial: true,
    dicaIcone: 'Página Inicial',
    tipo: RotasTipo.EstruturadaAutenticada,
  });

  const rotasArray = [];
  for (var [key, value] of rotas) {
    const rota = value;
    rota.path = key + (value.params ? value.params : '');
    rotasArray.push(rota);

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
