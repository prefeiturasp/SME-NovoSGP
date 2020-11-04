import React, { useState, useRef, useEffect } from 'react';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';

function PocEditor() {
  const [conteudoJodit, setContent] = useState('');
  const editor = useRef(null);
  useEffect(() => {
    console.log(conteudoJodit);
  }, [conteudoJodit]);

  return (
    <div className="App">
      <h1>Editor Jodit</h1>
      <br />
      <JoditEditor
        value={conteudoJodit}
        ref={editor}
        onChange={content => setContent(content)}
      />
    </div>
  );
}

export default PocEditor;
