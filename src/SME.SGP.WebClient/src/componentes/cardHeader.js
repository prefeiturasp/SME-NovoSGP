import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { Base } from './colors';

const CardHeader = props => {
  const { children, border } = props;

  const Header = styled.div`
    ${border ? `border-left: 8px solid ${Base.AzulBordaCard};` : null}
  `;

  return (
    <Header className="card-header shadow-sm rounded bg-white d-flex align-items-center py-3 fonte-16">
      {children}
    </Header>
  );
};

CardHeader.propTypes = {
  children: PropTypes.node,
  border: PropTypes.bool,
};

export default CardHeader;
