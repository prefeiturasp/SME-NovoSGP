import React from 'react';
import PropTypes from 'prop-types';
import Editor from './component';
import Container from './container';

const TextEditor = React.forwardRef((props, ref) => {

  return (
    <Container {...props}>
      <Editor ref={ref} {...props} />
    </Container>
  );
});

TextEditor.propTypes = {
  onBlur: PropTypes.func,
  maxHeight: PropTypes.string,
  height: PropTypes.string,
  value: PropTypes.string,
  disabled: PropTypes.bool,
  onClick: PropTypes.func,
  alt: PropTypes.string,
  estadoAdicional: PropTypes.object
};

export default TextEditor;