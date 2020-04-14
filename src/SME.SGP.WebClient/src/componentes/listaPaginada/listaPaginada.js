import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import queryString from 'query-string';

// Componentes
import { Table } from 'antd';

import { Container } from './listaPaginada.css';
import api from '~/servicos/api';
import { erro } from '~/servicos/alertas';

const ListaPaginada = props => {
  const {
    url,
    filtro,
    colunaChave,
    colunas,
    onClick,
    multiSelecao,
    onSelecionarLinhas,
    selecionarItems,
    filtroEhValido,
    onErro,
    paramArrayFormat,
  } = props;

  const [carregando, setCarregando] = useState(false);

  const [total, setTotal] = useState(0);
  const [linhas, setLinhas] = useState([]);
  const [linhasSelecionadas, setLinhasSelecionadas] = useState([]);

  const [paginaAtual, setPaginaAtual] = useState({
    defaultPageSize: 10,
    pageSize: 10,
    total: 0,
    showSizeChanger: true,
    pageSizeOptions: ['10', '20', '50', '100'],
    locale: { items_per_page: 'Linhas' },
    current: 1,
  });

  const [urlBusca, setUrlBusca] = useState(url);

  const selecionaItems = selecionadas => {
    if (selecionarItems && linhas) {
      const items = linhas.filter(
        item => selecionadas.indexOf(item[colunaChave]) >= 0
      );
      selecionarItems(items);
    }
  };

  const selecionar = ids => {
    setLinhasSelecionadas(ids);
    if (onSelecionarLinhas) onSelecionarLinhas(ids);
    selecionaItems(ids);
  };

  const selecaoLinha = {
    selectedRowKeys: linhasSelecionadas,
    onChange: ids => selecionar(ids),
  };

  const selecionarLinha = linha => {
    let selecionadas = [...linhasSelecionadas];
    if (selecionadas.indexOf(linha[colunaChave]) >= 0) {
      selecionadas.splice(selecionadas.indexOf(linha[colunaChave]), 1);
    } else if (multiSelecao) {
      selecionadas.push(linha[colunaChave]);
    } else {
      selecionadas = [];
      selecionadas.push(linha[colunaChave]);
    }
    setLinhasSelecionadas(selecionadas);
    if (onSelecionarLinhas) onSelecionarLinhas(selecionadas);
    selecionaItems(selecionadas);
  };

  const clicarLinha = row => {
    if (onClick) {
      onClick(row);
    }
  };

  const defineUrlBusca = pagina => {
    setUrlBusca(
      `${url}?numeroPagina=${pagina.current}&numeroRegistros=${pagina.pageSize}`
    );
  };

  const filtrar = () => {
    selecionar([]);
    setCarregando(true);
    api
      .get(urlBusca, {
        params: filtro,
        paramsSerializer(params) {
          return queryString.stringify(params, {
            arrayFormat: paramArrayFormat,
          });
        },
      })
      .then(resposta => {
        setTotal(resposta.data.totalRegistros);
        setLinhas(resposta.data.items);
      })
      .catch(err => {
        if (
          err.response &&
          err.response.data &&
          err.response.data.mensagens &&
          err.response.data.mensagens.length
        ) {
          if (onErro) onErro(err);
          else erro(err.response.data.mensagens[0]);
        }
      })
      .finally(() => setCarregando(false));
  };

  useEffect(() => {
    if (filtroEhValido) {
      filtrar();
    }
  }, [filtroEhValido, filtro, paginaAtual]);

  const executaPaginacao = pagina => {
    const novaPagina = { ...paginaAtual, ...pagina };
    if (pagina.total < pagina.pageSize) {
      novaPagina.current = 1;
    }
    setPaginaAtual(novaPagina);
    defineUrlBusca(novaPagina);
  };

  return (
    <Container className="table-responsive">
      <Table
        className={multiSelecao ? '' : 'ocultar-coluna-multi-selecao'}
        rowKey={colunaChave}
        rowSelection={selecaoLinha}
        columns={colunas}
        dataSource={linhas}
        onRow={row => ({
          onClick: colunaClicada => {
            if (
              colunaClicada &&
              colunaClicada.target &&
              colunaClicada.target.className === 'ant-table-selection-column'
            ) {
              selecionarLinha(row);
            } else {
              clicarLinha(row);
            }
          },
        })}
        pagination={{
          defaultPageSize: paginaAtual.defaultPageSize,
          pageSize: paginaAtual.pageSize,
          total,
          showSizeChanger: true,
          pageSizeOptions: ['10', '20', '50', '100'],
          locale: { items_per_page: '' },
          current: paginaAtual.current,
        }}
        bordered
        size="middle"
        locale={{ emptyText: 'Sem dados' }}
        onHeaderRow={() => {
          return {
            onClick: colunaClicada => {
              if (
                colunaClicada &&
                colunaClicada.target &&
                colunaClicada.target.className === 'ant-table-selection-column'
              ) {
                const checkboxSelecionarTodos = document
                  .getElementsByClassName('ant-table-selection')[0]
                  .getElementsByClassName('ant-checkbox-wrapper')[0]
                  .getElementsByClassName('ant-checkbox')[0]
                  .getElementsByClassName('ant-checkbox-input')[0];

                checkboxSelecionarTodos.click();
              }
            },
          };
        }}
        onChange={executaPaginacao}
        loading={carregando}
      />
    </Container>
  );
};

ListaPaginada.propTypes = {
  colunas: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  multiSelecao: PropTypes.oneOfType([PropTypes.bool]),
  onClick: PropTypes.oneOfType([PropTypes.func]),
  onSelecionarLinhas: PropTypes.oneOfType([PropTypes.func]),
  selecionarItems: PropTypes.oneOfType([PropTypes.func]),
  url: PropTypes.string,
  colunaChave: PropTypes.string,
  filtro: PropTypes.oneOfType([PropTypes.object]),
  filtroEhValido: PropTypes.bool,
  onErro: PropTypes.oneOfType([PropTypes.func]),
  paramArrayFormat: PropTypes.oneOfType([PropTypes.func]),
};

ListaPaginada.defaultProps = {
  colunas: [],
  multiSelecao: false,
  onClick: () => {},
  onSelecionarLinhas: () => {},
  selecionarItems: () => {},
  url: '',
  colunaChave: 'id',
  filtro: null,
  filtroEhValido: true,
  onErro: () => {},
  paramArrayFormat: 'brackets',
};

export default ListaPaginada;
