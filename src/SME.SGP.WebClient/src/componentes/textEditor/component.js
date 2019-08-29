import 'react-quill/dist/quill.snow.css';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import ReactQuill from 'react-quill';

const TextEditor = React.forwardRef((props, ref) => {

  const { value, onBlur } = props;
  const reference = ref;

  const onBlurQuill = () => {
    if (onBlur)
      onBlur(ref.current.state.value, reference);
  }

  return (
    <ReactQuill ref={ref} modules={modules} onBlur={onBlurQuill} value={value || ''} propsPai={props} />
  );
});

TextEditor.propTypes = {
  onBlur: PropTypes.func,
  height: PropTypes.string,
  value: PropTypes.string,
};

TextEditor.defaultProps = {
  onBlur: () => {},
  value: ""
}

export default TextEditor;

const toolbarOptions = [
  ['bold', 'italic', 'underline'],
  [{ list: 'bullet' }, { list: 'ordered' }],
];

const modules = {
  toolbar: toolbarOptions,
};
