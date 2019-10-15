import { Table, Pagination } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
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

  const [paginaAtual, setPaginaAtual] = useState({
    defaultPageSize: 5,
    pageSize: 5,
    total: dataSource.totalRegistros,
    showSizeChanger: true,
    pageSizeOptions: ['10', '20', '50', '100'],
    locale: { items_per_page: '' },
    current: 1,
  });

  const executaPaginacao = pagina => {
    setPaginaAtual({ ...paginaAtual, ...pagina });
    onPaginacao({
      numeroPagina: pagina.current,
      numeroRegistros: pagina.pageSize,
    });
  };

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
        pagination={{
          defaultPageSize: 5,
          pageSize: 5,
          total: dataSource.totalRegistros,
          showSizeChanger: true,
          pageSizeOptions: ['10', '20', '50', '100'],
          locale: { items_per_page: '' },
          current: paginaAtual.current,
        }}
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
  selectedRowKeys: PropTypes.array,
  onSelectRow: PropTypes.func,
  dataSource: PropTypes.array,
  columns: PropTypes.array,
  selectMultipleRows: PropTypes.bool,
  pageSize: PropTypes.number,
  paginacao: PropTypes.object,
  onClickRow: PropTypes.func,
  onPaginacao: PropTypes.func,
  locale: PropTypes.object,
};

DataTable.defaultProps = {
  dataSource: [],
  columns: [],
  selectMultipleRows: false,
  pageSize: 10,
  paginacao: {
    defaultPageSize: 5,
    pageSize: 5,
    total: 0,
    showSizeChanger: true,
    pageSizeOptions: ['10', '20', '50', '100'],
    locale: { items_per_page: '' },
    current: 1,
  },
  onRowClick: () => {},
  locale: { emptyText: 'Sem dados' },
  onPaginacao: () => {},
};

export default DataTable;
