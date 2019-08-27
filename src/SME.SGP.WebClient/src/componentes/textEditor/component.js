import 'react-quill/dist/quill.snow.css';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import ReactQuill from 'react-quill';

const TextEditor = React.forwardRef((props, ref) => {

  const { value, onBlur} = props;
  const [textState, setTextState] = useState("");

  useEffect(() => setTextState(value), [value]);

  const onChange = text => {
    setTextState(text);
  };

  const onBlurComponent = () => {
    if (onBlur)
      onBlur(textState)
  };

  return (
    <ReactQuill ref={ref} onBlur={onBlurComponent} onChange={onChange} modules={modules} value={textState || ''} />
  );
});

TextEditor.propTypes = {
  onBlur: PropTypes.func,
  height: PropTypes.string,
  value: PropTypes.string,
};

export default TextEditor;

const toolbarOptions = [
  ['bold', 'italic', 'underline'],
  [{ list: 'bullet' }, { list: 'ordered' }],
];

const modules = {
  toolbar: toolbarOptions,
};
