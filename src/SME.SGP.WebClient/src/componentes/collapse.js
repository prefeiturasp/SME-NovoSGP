import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import { Collapse as PainelCollapse } from 'antd';

const Collapse = ({ idPaineisAbertos, onChange, posicaoIcone, children }) => {
  return (
    <PainelCollapse
      defaultActiveKey={idPaineisAbertos}
      onChange={onChange}
      expandIconPosition="left"
    >
      {children}
    </PainelCollapse>
  );
};

Collapse.propTypes = {
  children: PropTypes.node,
};

Collapse.defaultProps = {
  titulo: '',
  posicaoIcone: 'left',
  children: () => {},
  show: false,
};

export default Collapse;
