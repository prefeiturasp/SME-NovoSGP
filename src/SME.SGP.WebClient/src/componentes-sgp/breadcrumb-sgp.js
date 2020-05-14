import React, { useEffect, useState } from 'react';
import { Breadcrumb } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { Base } from '../componentes/colors';
import { store } from '../redux';
import styled from 'styled-components';
import { rotaAtiva } from '../redux/modulos/navegacao/actions';
import modalidade from '~/dtos/modalidade';

const BreadcrumbBody = styled.div`
  padding: 10px 0 5px 15px !important;
  font-size: 12px;
  a,
  a:hover {
    color: ${Base.Roxo};
  }
  a:hover {
    text-decoration: underline;
  }
  .icone-seta {
    margin-right: 10px;
    margin-left: 10px;
    color: ${Base.Roxo};
  }
`;
const BreadcrumbSgp = () => {
  const NavegacaoStore = useSelector(store => store.navegacao);

  const UsuarioStrore = useSelector(store => store.usuario);

  const rotas = NavegacaoStore.rotas;

  const [itens, setItens] = useState([]);

  const rotaAtual = window.location.pathname;
  const itemRotaAtual = rotas.get(rotaAtual);

  const rotaDinamica = localStorage.getItem('rota-dinamica');
  const itemRotaDinamica = rotaDinamica ? JSON.parse(rotaDinamica) : null;

  const verificaTrocaNomesBreadcrumb = () => {
    if (
      UsuarioStrore &&
      UsuarioStrore.turmaSelecionada &&
      UsuarioStrore.turmaSelecionada.length &&
      UsuarioStrore.turmaSelecionada[0].codModalidade == modalidade.EJA
    ) {
      rotas.get('/planejamento/plano-ciclo').breadcrumbName = 'Plano de Etapa';
      rotas.get('/planejamento/plano-anual').breadcrumbName = 'Plano Semestral';
    } else {
      rotas.get('/planejamento/plano-ciclo').breadcrumbName = 'Plano de Ciclo';
      rotas.get('/planejamento/plano-anual').breadcrumbName = 'Plano Anual';
    }
  };

  useEffect(() => {
    carregaBreadcrumbs();
  }, [NavegacaoStore.rotaAtiva]);

  useEffect(() => {
    carregaBreadcrumbs();
  }, [UsuarioStrore.turmaSelecionada]);

  useEffect(
    (window.onbeforeunload = () => {
      depoisDeCarregar();
    }),
    []
  );

  const depoisDeCarregar = () => {
    localStorage.setItem('rota-atual', window.location.pathname);
    store.dispatch(rotaAtiva(window.location.pathname));
  };

  const carregaBreadcrumbs = () => {
    verificaTrocaNomesBreadcrumb();

    if (itemRotaAtual) {
      setItensBreadcrumb(itemRotaAtual);
    } else if (rotaDinamica && itemRotaDinamica.path === rotaAtual) {
      setItensBreadcrumb(itemRotaDinamica);
    } else {
      const itemHome = rotas.get('/');
      setItensBreadcrumb(itemHome);
    }
  };

  const setItensBreadcrumb = item => {
    const newItens = [];
    if ((!item.breadcrumbName || !item.breadcrumbName === '') && item.parent) {
      item = rotas.get(item.parent);
    }
    carregaBreadcrumbsExtra(item, newItens);
    newItens.push(
      criarItemBreadcrumb(
        item.breadcrumbName,
        rotaAtual,
        true,
        true,
        item.icone,
        item.dicaIcone
      )
    );
    setItens(newItens);
  };

  const carregaBreadcrumbsExtra = (item, newItens) => {
    const itemParent = rotas.get(item.parent);
    if (itemParent && itemParent.parent) {
      carregaBreadcrumbsExtra(itemParent, newItens);
    }

    if (itemParent) {
      newItens.push(
        criarItemBreadcrumb(
          itemParent.breadcrumbName,
          item.parent,
          false,
          false,
          itemParent.icone,
          itemParent.dicaIcone
        )
      );
    }

    if (item.menu && item.menu.length) {
      item.menu.forEach((menu, i) => {
        newItens.push(
          criarItemBreadcrumb(
            menu,
            item.path + '-menu' + i,
            true,
            false,
            item.icone,
            item.dicaIcone
          )
        );
      });
    }
  };

  const criarItemBreadcrumb = (
    breadcrumbName,
    path,
    ehEstatico,
    ehRotaAtual,
    icone,
    dicaIcone
  ) => {
    return { breadcrumbName, path, ehEstatico, ehRotaAtual, icone, dicaIcone };
  };

  const ocultarBreadcrumb = () => {
    const path = itens[0].path;
    if (itens.length === 1) {
      return (
        path === '/' ||
        (itemRotaAtual && itemRotaAtual.path !== path) ||
        (itemRotaDinamica && itemRotaDinamica.path !== path)
      );
    }
    return false;
  };

  return (
    <BreadcrumbBody>
      {itens.map(item => {
        return (
          <Breadcrumb.Item
            key={item.path}
            separator=""
            hidden={ocultarBreadcrumb()}
          >
            <Link hidden={item.ehEstatico} to={item.path}>
              <i
                className={item.icone}
                title={item.breadcrumbName}
                title={item.dicaIcone}
              />
              <span hidden={item.path === '/'}>{item.breadcrumbName}</span>
            </Link>
            <i
              hidden={!item.ehEstatico}
              className={item.icone}
              title={item.dicaIcone}
            />
            <span hidden={!item.ehEstatico || item.path === '/'}>
              {item.breadcrumbName}
            </span>
            <i
              hidden={item.ehRotaAtual}
              style={{ color: Base.Roxo }}
              className="fas fa-chevron-circle-right icone-seta"
            />
          </Breadcrumb.Item>
        );
      })}
    </BreadcrumbBody>
  );
};

export default BreadcrumbSgp;
