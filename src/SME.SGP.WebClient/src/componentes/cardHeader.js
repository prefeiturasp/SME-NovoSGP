import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';

const CardHeader = props => {
  const { children } = props;

  const Header = styled.div`
    border-left: 4px solid #0a3da3;
  `;

  return (
    <Header className="card-header rounded bg-white d-flex align-items-center">
      {children}
    </Header>
  );
};

CardHeader.propTypes = {
  children: PropTypes.node,
};

export default CardHeader;
