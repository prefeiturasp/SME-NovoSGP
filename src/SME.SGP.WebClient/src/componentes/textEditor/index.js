import React, { memo } from 'react';
import Editor from './component';
import Container from './container';

const TextEditor = React.forwardRef((props, ref) => {
  return (
    <Container {...props}>
      <Editor ref={ref} {...props} />
    </Container>
  );
});

export default memo(TextEditor);
