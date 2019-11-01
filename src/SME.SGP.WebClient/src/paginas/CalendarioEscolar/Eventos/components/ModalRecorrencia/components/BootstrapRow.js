import React from 'react';
import PropTypes from 'prop-types';

function BootstrapRow({ children, paddingBottom }) {
  const className = `row ${paddingBottom ? `pb-${paddingBottom}` : ''}`;
  return <div className={className}>{children}</div>;
}

BootstrapRow.defaultProps = {
  paddingBottom: 0,
  children: () => {},
};

BootstrapRow.propTypes = {
  paddingBottom: PropTypes.number,
  children: PropTypes.node,
};

export default BootstrapRow;
