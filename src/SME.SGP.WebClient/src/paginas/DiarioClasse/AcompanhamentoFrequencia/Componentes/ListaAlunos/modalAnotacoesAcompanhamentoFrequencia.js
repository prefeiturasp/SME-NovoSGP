import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { Card } from 'antd';
import { Colors, DetalhesAluno, ModalConteudoHtml } from '~/componentes';
import Button from '~/componentes/button';
import ServicoAnotacaoFrequenciaAluno from '~/servicos/Paginas/DiarioClasse/ServicoAnotacaoFrequenciaAluno';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
} from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { erros } from '~/servicos';

const ModalAnotacoesAcompanhamentoFrequencia = () => {
  const dispatch = useDispatch();

  const [dados, setDados] = useState({});

  const dadosModalAnotacao = useSelector(
    store => store.acompanhamentoFrequencia.dadosModalAnotacao
  );

  const exibirModalAnotacao = useSelector(
    store => store.acompanhamentoFrequencia.exibirModalAnotacao
  );

  useEffect(() => {
    const obterAnotacao = async () => {
      if (exibirModalAnotacao && dadosModalAnotacao) {
        const retorno = await ServicoAnotacaoFrequenciaAluno.obterAnotacaoPorId(
          dadosModalAnotacao.id
        ).catch(e => erros(e));

        if (retorno?.data) {
          setDados(retorno.data);
        }
      } else {
        setDados([]);
      }
    };
    obterAnotacao();
  }, [dadosModalAnotacao]);

  const onClose = () => {
    dispatch(setDadosModalAnotacao());
    dispatch(setExibirModalAnotacao(false));
  };
  return (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="anotacao"
      visivel={exibirModalAnotacao}
      titulo="Anotações do Aluno"
      onClose={onClose}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <DetalhesAluno
        className="mb-2"
        dados={dados?.aluno}
        exibirBotaoImprimir={false}
      />
      <Card
        type="inner"
        className="rounded mt-3 mb-3"
        headStyle={{ borderBottomRightRadius: 0 }}
        bodyStyle={{ borderTopRightRadius: 0 }}
      >
        <strong>{dadosModalAnotacao?.motivo}</strong>
      </Card>
      <JoditEditor value={dados?.anotacao} readonly removerToolbar />
      <div className="col-md-12 mt-2 p-0 d-flex justify-content-end">
        <Button
          key="btn-voltar"
          id="btn-voltar"
          label="Voltar"
          icon="arrow-left"
          color={Colors.Azul}
          border
          onClick={onClose}
          className="mt-2"
        />
      </div>
    </ModalConteudoHtml>
  );
};

export default ModalAnotacoesAcompanhamentoFrequencia;
