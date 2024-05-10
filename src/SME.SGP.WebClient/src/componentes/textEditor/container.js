import React from 'react';
import styled from 'styled-components';

const Container = props => {

  const { height, children, maxHeight, id} = props;

  const Retorno = styled.div`
    .ql-container.ql-snow {
      border: 1px solid rgba(0, 0, 0, 0.125) !important;
      border-bottom: none !important;
      border-top-left-radius: 0.25rem;
      border-top-right-radius: 0.25rem;
    }

    .ql-editor {
      min-height: ${height || "100px"};
      max-height: ${maxHeight || "420px"};
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

    .ql-editor li:not(.ql-direction-rtl)::before {
      margin-right: 0.8em;
      margin-left: -2em;
    }
  `;

  return <Retorno id={id || "textEditor"}>{children}</Retorno>;
};

export default Container;
