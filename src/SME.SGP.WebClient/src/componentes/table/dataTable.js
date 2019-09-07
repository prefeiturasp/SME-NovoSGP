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
  } = props;

  const rowSelection = { selectedRowKeys };

  const selectRow = record => {
    let selected = [...selectedRowKeys];
    if (selected.indexOf(record.key) >= 0) {
      selected.splice(selected.indexOf(record.key), 1);
    } else if (selectMultipleRows) {
      selected.push(record.key);
    } else {
      selected = [];
      selected.push(record.key);
    }
    onSelectRow(selected);
  };

  return (
    <Container className="table-responsive">
      <Table
        rowSelection={rowSelection}
        columns={columns}
        dataSource={dataSource}
        onRow={record => ({
          onClick: () => {
            selectRow(record);
          },
        })}
        pagination={{ pageSize }}
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
};

DataTable.defaultProps = {
  dataSource: [],
  columns: [],
  selectMultipleRows: false,
  pageSize: 10,
};

export default DataTable;
