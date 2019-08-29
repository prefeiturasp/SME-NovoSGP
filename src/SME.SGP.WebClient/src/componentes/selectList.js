import { Transfer } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';

import { Base } from './colors';

const Container = styled.div`
  .ant-transfer-list ::-webkit-scrollbar-track {
    background-color: #f4f4f4 !important;
  }

  .ant-transfer-list ::-webkit-scrollbar {
    width: 4px !important;
    background-color: rgba(229, 237, 244, 0.71) !important;
    border-radius: 2.5px !important;
  }

  .ant-transfer-list ::-webkit-scrollbar-thumb {
    background: #a8a8a8 !important;
    border-radius: 3px !important;
  }

  .ant-checkbox-checked .ant-checkbox-inner {
    background-color: ${Base.Roxo} !important;
    border-color: ${Base.Roxo} !important;
  }

  ul li:hover {
    background-color: ${Base.Roxo} !important;
    color: #ffffff;
  }

  .ant-transfer-list {
    border: none;
    height: 60vh;
    @media (max-width: 768px) {
      width: 100% !important;
    }
  }

  .ant-transfer-list-content-item {
    -webkit-transition: none;
    transition: none;
  }

  .ant-transfer-list-body {
    border-radius: 2px;
    box-shadow: 0 3px 9px 0 rgba(0, 0, 0, 0.15);
    background-color: #ffffff;
  }

  .ant-transfer-list-header {
    border-bottom: none;
  }

  .ant-transfer-list {
    width: 47%;
    flex: none;
  }

  .ant-transfer-operation {
    width: 6%;
    flex: none;
    margin: 0;
    padding-left: 10px;

    @media (max-width: 768px) {
      width: 100% !important;
      padding: 15px;
    }
  }

  .ant-transfer-list-header-title {
    font-family: Roboto;
    font-weight: normal;
    font-style: normal;
    font-stretch: normal;
    line-height: normal;
    letter-spacing: normal;
    color: #686868;
  }

  .ant-transfer-list-header-selected span:first-child {
    display: none !important;
  }
  .ant-transfer-list-header-selected span:last-child {
    display: contents !important;
  }

  .ant-transfer-operation button {
    background-color: ${Base.Roxo};
    border-color: ${Base.Roxo};
    font-family: FontAwesome;
    height: 40px;
    width: 40px;
    border-radius: 50%;
    padding-right: 0px;
    padding-left: 5px;

    i {
      font-style: normal;
      font-size: 26px !important;
      svg {
        display: none;
      }
    }

    &[disabled] {
      background-color: #ffffff !important;
      border-color: ${Base.Roxo} !important;
      i {
        color: transparent;
        -webkit-text-stroke-width: 1px;
        -webkit-text-stroke-color: ${Base.Roxo};
      }
    }
  }

  .ant-transfer-operation button:first-child {
    i::after {
      content: '\f054' !important;
    }
    padding-right: 3px !important;
    margin-bottom: 20px;
  }

  .ant-transfer-operation button:last-child {
    i::after {
      content: '\f053' !important;
    }
    padding-left: 3px !important;
  }

  .ant-btn-primary[disabled] {
    color: rgba(0, 0, 0, 0.25) !important;
    background-color: #f5f5f5 !important;
    border-color: #d9d9d9 !important;
    text-shadow: none !important;
    -webkit-box-shadow: none !important;
    box-shadow: none !important;
  }
`;

const SelectList = props => {
  const { mockData, handleChange, targetKeys, titulos, height } = props;

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12">
      <Container>
        <Transfer
          titles={titulos}
          showSelectAll={false}
          notFoundContent=""
          dataSource={mockData}
          targetKeys={targetKeys}
          onChange={handleChange}
          render={item => `${item.titulo}-${item.descricao}`}
        />
      </Container>
    </div>
  );
};

SelectList.propTypes = {
  mockData: PropTypes.array,
  targetKeys: PropTypes.array,
  handleChange: PropTypes.func,
  titulos: PropTypes.array,
  height: PropTypes.string,
};

export default SelectList;
