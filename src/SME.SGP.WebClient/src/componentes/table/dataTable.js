import { Table } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import { Container } from './dataTabe.css';

const DataTable = props => {
  const {
    selectedRowKeys,
    columns,
    dataSource,
    onSelectRow,
    selectMultipleRows,
    pageSize,
    pagination,
    onRowClick,
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

  return (
    <Container className="table-responsive">
      <Table
        className={selectMultipleRows ? '' : 'ocultar-coluna-multi-selecao'}
        rowKey="id"
        rowSelection={rowSelection}
        columns={columns}
        dataSource={dataSource}
        onRow={row => ({
          onClick: () => {
            selectRow(row);
            onRowClick();
          },
        })}
        pagination={pagination}
        pageSize={{ pageSize }}
        bordered
        size="middle"
        locale={{ emptyText: 'Sem dados' }}
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
  pagination: PropTypes.bool,
  onRowClick: PropTypes.func,
};

DataTable.defaultProps = {
  dataSource: [],
  columns: [],
  selectMultipleRows: false,
  pageSize: 10,
  pagination: false,
  onRowClick: () => {},
};

export default DataTable;
