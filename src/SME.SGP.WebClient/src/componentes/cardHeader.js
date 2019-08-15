import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { Base } from './colors';

const CardHeader = props => {
  const { children } = props;

  const Header = styled.div`
    border-left: 8px solid ${Base.AzulBordaCard};
    font-size: 16px;
  `;

  return (
    <Header className="card-header shadow-sm rounded bg-white d-flex align-items-center py-4">
      {children}
    </Header>
  );
};

CardHeader.propTypes = {
  children: PropTypes.node,
};

export default CardHeader;
