import React, {
  useState,
  useEffect,
  useCallback,
  useRef,
  useMemo,
} from 'react';
import PropTypes from 'prop-types';

// Componentes
import { Table } from 'antd';

// Redux
import { useSelector, useDispatch } from 'react-redux';
import { setLoaderTabela } from '~/redux/modulos/loader/actions';

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
  } = props;

  const dispatch = useDispatch();
  const carregando = useSelector(store => store.loader.loaderTabela);

  const [total, setTotal] = useState(0);
  const [linhas, setLinhas] = useState([]);
  const [linhasSelecionadas, setLinhasSelecionadas] = useState([]);
  const [filtroLocal, setFiltroLocal] = useState(null);

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
    if (selecionarItems) {
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

  const filtrar = useCallback(() => {
    dispatch(setLoaderTabela(true));
    api
      .get(urlBusca, { params: filtroLocal })
      .then(resposta => {
        setTotal(resposta.data.totalRegistros);
        setLinhas(resposta.data.items);
        dispatch(setLoaderTabela(false));
      })
      .catch(err => {
        dispatch(setLoaderTabela(false));
        if (
          err.response &&
          err.response.data &&
          err.response.data.mensagens &&
          err.response.data.mensagens.length
        ) {
          if (onErro) onErro(err);
          else erro(err.response.data.mensagens[0]);
        }
      });
  }, [dispatch, filtroLocal, onErro, urlBusca]);

  useEffect(() => {
    setFiltroLocal(filtro);
  }, [filtro, paginaAtual]);

  useEffect(() => {
    if (filtroEhValido) {
      filtrar();
    }
  }, [filtroEhValido, filtrar]);

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
        rowKey="id"
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
};

export default ListaPaginada;
