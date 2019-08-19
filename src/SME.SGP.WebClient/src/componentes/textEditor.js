import 'react-quill/dist/quill.snow.css';
import PropTypes from 'prop-types';
import React from 'react';
import ReactQuill from 'react-quill';
import styled from 'styled-components';

const TextEditor = props => {
  const { modules, height, value } = props;

  const Container = styled.div`
    .ql-container.ql-snow {
      border: 1px solid rgba(0, 0, 0, 0.125) !important;
      border-bottom: none !important;
      border-top-left-radius: 0.25rem;
      border-top-right-radius: 0.25rem;
    }

    .ql-editor {
      min-height: ${height}px;
      max-height: 515px;
      overflow: auto;
    }

    .ql-toolbar.ql-snow {
      border-top: none !important;
      border-bottom-left-radius: 0.25rem;
      border-bottom-right-radius: 0.25rem;
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

  return (
    <Container>
      <ReactQuill modules={modules} value={value} />
    </Container>
  );
};

TextEditor.propTypes = {
  modules: PropTypes.object,
  onChange: PropTypes.func,
  height: PropTypes.number,
  value: PropTypes.string,
};

export default TextEditor;
