import 'react-quill/dist/quill.snow.css';

import PropTypes from 'prop-types';
import React from 'react';
import ReactQuill from 'react-quill';
import styled from 'styled-components';

const Container = styled.div`
  .ql-container.ql-snow {
    border: 1px solid #ccc !important;
    border-bottom: none !important;
  }

  .ql-editor {
    min-height: 515px;
    max-height: 515px;
    overflow: auto;
  }

  .ql-toolbar.ql-snow {
    border-top: none !important;
  }

  .ql-snow .ql-stroke {
    stroke: #a4a4a4;
  }

  .ql-snow .ql-fill {
    fill: #a4a4a4;
  }

  .quill {
    display: flex;
    flex-direction: column-reverse;
  }
`;

const TextEditor = props => {
  const { modules, onChange } = props;

  return (
    <Container>
      <ReactQuill onChange={onChange} modules={modules} />
    </Container>
  );
};

TextEditor.propTypes = {
  modules: PropTypes.object,
  onChange: PropTypes.func,
};

export default TextEditor;
