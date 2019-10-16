import { Table } from 'antd';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { Container } from './dataTabe.css';

const DataTable = props => {
  const {
    selectedRowKeys,
    columns,
    dataSource,
    onSelectRow,
    onClickRow,
    selectMultipleRows,
    pageSize,
    paginacao,
    locale,
    onPaginacao,
  } = props;

  const rowSelection = {
    selectedRowKeys,
    onChange: ids => {
      onSelectRow(ids);
    },
  };

  const selectRow = row => {
    let selected = [...selectedRowKeys];
    if (selected.indexOf(row.id) >= 0) {
      selected.splice(selected.indexOf(row.id), 1);
    } else if (selectMultipleRows) {
      selected.push(row.id);
    } else {
      selected = [];
      selected.push(row.id);
    }
    onSelectRow(selected);
  };

  const clickRow = row => {
    if (onClickRow) {
      onClickRow(row);
    }
  };

  const retornaPaginacaoInicial = () => {
    return {
      defaultPageSize: 10,
      pageSize: 10,
      total: dataSource.totalRegistros,
      showSizeChanger: true,
      pageSizeOptions: ['10', '20', '50', '100'],
      locale: { items_per_page: 'Linhas' },
      current: 1,
    };
  };

  const [paginaAtual, setPaginaAtual] = useState(retornaPaginacaoInicial());

  const executaPaginacao = pagina => {
    const novaPagina = { ...paginaAtual, ...pagina };
    if (pagina.total < pagina.pageSize) {
      novaPagina.current = 1;
    }
    setPaginaAtual(novaPagina);
    onPaginacao({
      totalRegistros: pagina.total,
      numeroPagina: novaPagina.current,
      numeroRegistros: pagina.pageSize,
    });
  };

  useEffect(() => {
    executaPaginacao(paginaAtual);
  }, []);

  useEffect(() => {
    executaPaginacao(paginaAtual);
  }, [dataSource]);

  return (
    <Container className="table-responsive">
      <Table
        className={selectMultipleRows ? '' : 'ocultar-coluna-multi-selecao'}
        rowKey="id"
        rowSelection={rowSelection}
        columns={columns}
        dataSource={dataSource.items}
        onRow={row => ({
          onClick: colunaClicada => {
            if (
              colunaClicada &&
              colunaClicada.target &&
              colunaClicada.target.className == 'ant-table-selection-column'
            ) {
              selectRow(row);
            } else {
              clickRow(row);
            }
          },
        })}
        pagination={
          paginacao && {
            defaultPageSize: paginaAtual.defaultPageSize,
            pageSize: paginaAtual.pageSize,
            total: dataSource.totalRegistros,
            showSizeChanger: true,
            pageSizeOptions: ['10', '20', '50', '100'],
            locale: { items_per_page: '' },
            current: paginaAtual.current,
          }
        }
        pageSize={{ pageSize }}
        bordered
        size="middle"
        locale={locale}
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

DataTable.propTypes = {
  selectedRowKeys: PropTypes.oneOfType([PropTypes.array, PropTypes.func]),
  onSelectRow: PropTypes.func,
  onRowClick: PropTypes.func,
  dataSource: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
  columns: PropTypes.array,
  selectMultipleRows: PropTypes.bool,
  pageSize: PropTypes.number,
  paginacao: PropTypes.bool,
  onClickRow: PropTypes.func,
  onPaginacao: PropTypes.func,
  locale: PropTypes.object,
};

DataTable.defaultProps = {
  dataSource: {},
  columns: [],
  selectMultipleRows: false,
  pageSize: 10,
  paginacao: false,
  selectedRowKeys: () => {},
  onRowClick: () => {},
  onClickRow: () => {},
  onSelectRow: () => {},
  locale: { emptyText: 'Sem dados' },
  onPaginacao: () => {},
};

export default DataTable;
