import React, { useEffect, useState } from 'react';
import { Breadcrumb } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { Base } from '../componentes/colors';
import styled from 'styled-components';

const BreadcrumbSgp = () => {
  const BreadcrumbBody = styled.div`
    padding: 10px 0 5px 15px !important;
    a,
    a:hover {
      color: ${Base.Roxo};
    }
    i {
      margin-right: 10px;
      margin-left: 10px;
    }
  `;

  const NavegacaoStore = useSelector(store => store.navegacao);

  const rotas = NavegacaoStore.rotas;

  const [itens, setItens] = useState([]);

  const rotaAtual = window.location.pathname;

  useEffect(() => {
    carregaBreadcrumbs();
  }, [NavegacaoStore.rotaAtiva]);

  window.onbeforeunload = () => {
    localStorage.setItem('rota-atual', window.location.pathname);
  };

  const carregaBreadcrumbs = () => {
    const rotaDinamica = localStorage.getItem('rota-dinamica');
    const itemRotaDinamica = rotaDinamica?JSON.parse(rotaDinamica): null;
    const item = rotas.get(rotaAtual);
    if (item) {
      setItensBreadcrumb(item);
    } else if (rotaDinamica && itemRotaDinamica.path === rotaAtual) {
      setItensBreadcrumb(itemRotaDinamica);
    } else {
      const itemHome = rotas.get("/");
      setItensBreadcrumb(itemHome);
    }
  };

  const setItensBreadcrumb = (item) => {
    const newItens = [];
    carregaBreadcrumbsExtra(item, newItens);
    newItens.push(
      criarItemBreadcrumb(item.breadcrumbName, rotaAtual, true, true)
    );
    setItens(newItens);
  }

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
          false
        )
      );
    }

    if (item.menu) {
      newItens.push(
        criarItemBreadcrumb(item.menu, item.path + '-menu', true, false)
      );
    }
  };

  const criarItemBreadcrumb = (
    breadcrumbName,
    path,
    ehEstatico,
    ehRotaAtual
  ) => {
    return { breadcrumbName, path, ehEstatico, ehRotaAtual };
  };

  return (
    <BreadcrumbBody>
      {itens.map(item => {
        return (
          <Breadcrumb.Item key={item.path} separator="">
            <Link hidden={item.ehEstatico} to={item.path}>
              {item.breadcrumbName}
            </Link>
            <span hidden={!item.ehEstatico}>{item.breadcrumbName}</span>
            <i
              hidden={item.ehRotaAtual}
              style={{ color: Base.Roxo }}
              className="fas fa-chevron-circle-right"
            />
          </Breadcrumb.Item>
        );
      })}
    </BreadcrumbBody>
  );
};

export default BreadcrumbSgp;
