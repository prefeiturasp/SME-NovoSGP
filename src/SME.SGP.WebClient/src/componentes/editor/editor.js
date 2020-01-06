import React from 'react';
import CKEditor from '@ckeditor/ckeditor5-react';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import '@ckeditor/ckeditor5-build-classic/build/translations/pt-br';

export default function Editor(props) {
  const { onChange, inicial } = props;
  return (
    <CKEditor
      editor={ClassicEditor}
      config={{
        language: 'pt-br',
      }}
      data={inicial || ''}
      onChange={(event, editor) => {
        const data = editor.getData();
        onChange && onChange(data);
      }}
    />
  );
}
