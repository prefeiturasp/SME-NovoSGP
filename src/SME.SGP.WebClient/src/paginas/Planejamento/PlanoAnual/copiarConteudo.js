import React, { useState, useEffect } from 'react';
import {
  ModalConteudoHtml,
  Loader,
  Label,
  SelectComponent,
} from '~/componentes';
import servicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';

const CopiarConteudo = ({
  visivel,
  turmaId,
  componenteCurricularEolId,
  onCloseCopiarConteudo,
  listaBimestresPreenchidos,
}) => {
  const [turmasSelecionadas, setTurmasSelecionadas] = useState([]);
  const [bimestresSelecionados, setBimestresSelecionados] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [loader, setLoader] = useState(false);

  useEffect(() => {
    if (componenteCurricularEolId && turmaId)
      servicoPlanoAnual
        .obterTurmasParaCopia(turmaId, componenteCurricularEolId)
        .then(c => {
          setListaTurmas(c.data);
        })
        .catch(e => {
          debugger;
        });
  }, [componenteCurricularEolId, turmaId]);

  const fecharCopiarConteudo = () => {
    setTurmasSelecionadas([]);
    setBimestresSelecionados([]);
    onCloseCopiarConteudo();
  };

  useEffect(() => {
    console.log(listaBimestresPreenchidos);
  }, [listaBimestresPreenchidos]);

  const copiar = () => {};

  return (
    <ModalConteudoHtml
      key="copiarConteudo"
      visivel={visivel}
      // onConfirmacaoPrincipal={onConfirmarCopiarConteudo}
      onConfirmacaoSecundaria={fecharCopiarConteudo}
      onClose={fecharCopiarConteudo}
      labelBotaoPrincipal="Copiar"
      // tituloAtencao="Atenção"
      // perguntaAtencao="pergunta"
      labelBotaoSecundario="Cancelar"
      titulo="Copiar Conteúdo"
      closable={false}
      loader={loader}
      desabilitarBotaoPrincipal={false}
    >
      <div>
        <label>Copiar para a(s) turma(s)</label>
        <SelectComponent
          id="SelecaoTurma"
          lista={listaTurmas}
          valueOption="codTurma"
          valueText="nomeTurma"
          valueSelect={turmasSelecionadas}
          multiple
          placeholder="Selecione uma ou mais turmas"
          onChange={turmas => setTurmasSelecionadas(turmas)}
        />
        <label>Copiar para o(s) bimestre(s)</label>
        <SelectComponent
          id="bimestres"
          lista={listaBimestresPreenchidos}
          valueOption="valor"
          valueText="nome"
          valueSelect={bimestresSelecionados}
          multiple
          placeholder="Selecione um ou mais bimestres"
          onChange={turmas => setBimestresSelecionados(turmas)}
        />
      </div>
    </ModalConteudoHtml>
  );
};
export default CopiarConteudo;
