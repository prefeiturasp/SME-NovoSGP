import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import { Collapse } from 'antd';

const { Panel } = Collapse;

const Painel = ({ titulo, children }) => {
  return (
    <Panel header={titulo} key={shortid.generate()}>
      {children}
    </Panel>
  );
};

Painel.propTypes = {
  titulo: PropTypes.string,
  children: PropTypes.node,
};

Painel.defaultProps = {
  titulo: '',
  children: () => {},
};

export default Painel;
