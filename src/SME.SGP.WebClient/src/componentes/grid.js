import React from 'react';
import PropTypes from 'prop-types';

const Grid = props => {
  const { cols, className, children, style } = props;
  return (
    <div
      style={style}
      className={`col-xl-${cols} col-md-${cols} col-lg-${cols} col-sm-12 col-xs-12 ${className}`}
    >
      {children}
    </div>
  );
};

Grid.propTypes = {
  cols: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  className: PropTypes.string,
  children: PropTypes.node,
  style: PropTypes.oneOfType([PropTypes.any]),
};

Grid.defaultProps = {
  cols: 12,
  className: '',
  children: () => {},
  style: null,
};

export default Grid;
