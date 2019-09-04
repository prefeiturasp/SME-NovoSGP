import { Table } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';

import { Container } from './dataTabe.css';

export class DataTable extends React.Component {
  state = {
    selectedRowKeys: this.props.selectedRowKeys,
    setSelectedRowKeys: this.props.setSelectedRowKeys,
    dataSource: this.props.dataSource,
    columns: this.props.columns,
    selectMultipleRows: this.props.selectMultipleRows,
    pageSize: this.props.pageSize,
  };

  selectRow = record => {
    let selectedRowKeys = [...this.state.selectedRowKeys];
    if (selectedRowKeys.indexOf(record.key) >= 0) {
      selectedRowKeys.splice(selectedRowKeys.indexOf(record.key), 1);
    } else if (this.state.selectMultipleRows) {
      selectedRowKeys.push(record.key);
    } else {
      selectedRowKeys = [];
      selectedRowKeys.push(record.key);
    }
    this.setState({ selectedRowKeys });
    this.state.setSelectedRowKeys(selectedRowKeys);
  };

  render() {
    const { selectedRowKeys, columns, dataSource } = this.state;
    const rowSelection = {
      selectedRowKeys,
    };

    return (
      <Container>
        <Table
          rowSelection={rowSelection}
          columns={columns}
          dataSource={dataSource}
          onRow={record => ({
            onClick: () => {
              this.selectRow(record);
            },
          })}
          pagination={{ pageSize: 2 }}
          bordered
          size="middle"
        />
      </Container>
    );
  }
}

DataTable.propTypes = {
  selectedRowKeys: PropTypes.array,
  setSelectedRowKeys: PropTypes.func,
  dataSource: PropTypes.array,
  columns: PropTypes.array,
  selectMultipleRows: PropTypes.bool,
  pageSize: PropTypes.number,
};

DataTable.defaultProps = {
  selectedRowKeys: [],
  dataSource: [],
  columns: [],
  selectMultipleRows: false,
  pageSize: 2, // TODO
};

export default DataTable;
