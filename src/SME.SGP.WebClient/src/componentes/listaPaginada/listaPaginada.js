import { Table } from 'antd';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { Container } from './listaPaginada.css';
import api from '~/servicos/api';

const ListaPaginada = props => {
  const {
    url,
    filtro,
    colunaChave,
    colunas,
    onClick,
    multiSelecao,
    aoSelecionarLinhas,
    linhasSelecionadas,
  } = props;

  const [total, setTotal] = useState(0);
  const [linhas, setLinhas] = useState([]);

  const [paginaAtual, setPaginaAtual] = useState({
    defaultPageSize: 10,
    pageSize: 10,
    total: 0,
    showSizeChanger: true,
    pageSizeOptions: ['10', '20', '50', '100'],
    locale: { items_per_page: 'Linhas' },
    current: 1,
  });

  const selecaoLinha = {
    selectedRowKeys: linhasSelecionadas,
    onChange: ids => aoSelecionarLinhas(ids),
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
    aoSelecionarLinhas(selecionadas);
  };

  const clicarLinha = row => {
    if (onClick) {
      onClick(row);
    }
  };

  const obterPaginacao = () => {
    return `numeroPagina=${paginaAtual.current}&numeroRegistros=${paginaAtual.pageSize}`;
  };

  const filtrar = () => {
    api.get(`${url}?${obterPaginacao()}`, { params: filtro }).then(resposta => {
      setTotal(resposta.data.totalRegistros);
      setLinhas(resposta.data.items);
    });
  };

  useEffect(() => {
    filtrar();
  }, [paginaAtual]);

  useEffect(() => {
    const novaPagina = { ...paginaAtual, current: 1 };
    setPaginaAtual(novaPagina);
    filtrar();
  }, [filtro]);

  const executaPaginacao = pagina => {
    const novaPagina = { ...paginaAtual, ...pagina };
    if (pagina.total < pagina.pageSize) {
      novaPagina.current = 1;
    }
    setPaginaAtual(novaPagina);
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
      />
    </Container>
  );
};

ListaPaginada.propTypes = {
  colunas: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  multiSelecao: PropTypes.oneOfType([PropTypes.bool]),
  onClick: PropTypes.oneOfType([PropTypes.func]),
  url: PropTypes.string,
  colunaChave: PropTypes.string,
  filtro: PropTypes.oneOfType([PropTypes.object]),
  aoSelecionarLinhas: PropTypes.oneOfType([PropTypes.func]),
  linhasSelecionadas: PropTypes.oneOfType([PropTypes.array]),
};

ListaPaginada.defaultProps = {
  colunas: [],
  multiSelecao: false,
  onClick: () => {},
  url: '',
  colunaChave: 'id',
  filtro: {},
  aoSelecionarLinhas: () => {},
  linhasSelecionadas: [],
};

export default ListaPaginada;
