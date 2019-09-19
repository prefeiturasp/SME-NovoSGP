import React, { useEffect, useState } from 'react';
import { Breadcrumb } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { Base } from '../componentes/colors';
import { store } from '../redux';
import styled from 'styled-components';
import { rotaAtiva } from '../redux/modulos/navegacao/actions';

const BreadcrumbSgp = () => {
  const BreadcrumbBody = styled.div`
    padding: 10px 0 5px 15px !important;
    font-size: 10px;
    a, a:hover {
      color: ${Base.AzulBreadcrumb};
    }

    a:hover{
      text-decoration: underline;
    }

    .icone-seta {
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

  useEffect(window.onbeforeunload = () => {
    depoisDeCarregar()
  }, []);

  const depoisDeCarregar = () => {
    localStorage.setItem('rota-atual', window.location.pathname);
    store.dispatch(rotaAtiva(window.location.pathname));
  }

  const carregaBreadcrumbs = () => {
    const rotaDinamica = localStorage.getItem('rota-dinamica');
    const itemRotaDinamica = rotaDinamica ? JSON.parse(rotaDinamica) : null;
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
      criarItemBreadcrumb(item.breadcrumbName, rotaAtual, true, true, item.icone, item.dicaIcone)
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
          false,
          itemParent.icone,
          itemParent.dicaIcone
        )
      );
    }

    if (item.menu) {
      newItens.push(
        criarItemBreadcrumb(item.menu, item.path + '-menu', true, false, item.icone, item.dicaIcone)
      );
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

  return (
    <BreadcrumbBody>

      {itens.map(item => {
        return (
          <Breadcrumb.Item key={item.path} separator="" hidden={itens.length === 1 && item.path === '/'}>
            <Link hidden={item.ehEstatico} to={item.path}>
              <i className={item.icone} title={item.breadcrumbName} title={item.dicaIcone}/>
              <span hidden={item.path === '/'}>{item.breadcrumbName}</span>
            </Link>
            <i hidden={!item.ehEstatico} className={item.icone} title={item.dicaIcone} />
            <span hidden={!item.ehEstatico || item.path === '/'}>{item.breadcrumbName}</span>
            <i
              hidden={item.ehRotaAtual}
              style={{ color: Base.AzulBreadcrumb }}
              className="fas fa-chevron-circle-right icone-seta"
            />
          </Breadcrumb.Item>
        );
      })}
    </BreadcrumbBody>
  );
};

export default BreadcrumbSgp;
