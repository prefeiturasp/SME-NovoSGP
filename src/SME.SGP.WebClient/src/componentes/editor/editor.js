import React, { useState, useRef } from 'react';

import 'jodit';
import 'jodit/build/jodit.min.css';
import JoditEditor from 'jodit-react';

export default function Editor(props) {
  const [content, setContent] = useState();
  let editor = null;
  return (
    <JoditEditor
      value={content}
      config={{
        events: {
          afterInit(instance) {
            editor = instance;
            //setEditor(i => instance);
            console.log('jodit instance', instance);
          },
        },
        readonly: false,
        enableDragAndDropFileToEditor: true,
        uploader: {
          url: 'https://localhost:5001/file/upload',
          isSuccess: resp => {
            return resp;
          },
          process: resp => {
            return {
              files: resp.data.files,
              path: resp.data.path,
              baseurl: resp.data.baseurl,
              error: resp.data.error,
              message: resp.data.message,
            };
          },
          defaultHandlerSuccess: function(data, editor) {
            let i;
            const field = 'files';
            debugger;
            const select = editor;
            if (data[field] && data[field].length) {
              for (i = 0; i < data[field].length; i += 1) {
                this.selection.insertImage(data.baseurl + data[field][i]);
              }
            }
          },
        },
        iframe: true,
        showWordsCounter: false,
        showXPathInStatusbar: false,
        buttons:
          '|,ul,ol,|,outdent,indent,|,font,fontsize,brush,paragraph,|,image,file,table,link,|,align,undo,redo,\n,|',
      }}
      onChange={c => setContent(c)}
    />
  );
}
