import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { Card } from 'antd';
import {
  Colors,
  DetalhesAluno,
  Loader,
  ModalConteudoHtml,
} from '~/componentes';
import Button from '~/componentes/button';
import ServicoAnotacaoFrequenciaAluno from '~/servicos/Paginas/DiarioClasse/ServicoAnotacaoFrequenciaAluno';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
} from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { ehTurmaInfantil, erros } from '~/servicos';
import usuario from '~/redux/modulos/usuario/reducers';

const ModalAnotacoesAcompanhamentoFrequencia = () => {
  const dispatch = useDispatch();
  const { turmaSelecionada } = usuario;
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const [dados, setDados] = useState();
  const [carregandoDados, setCarregandoDados] = useState(false);

  const dadosModalAnotacao = useSelector(
    store => store.acompanhamentoFrequencia.dadosModalAnotacao
  );

  const exibirModalAnotacao = useSelector(
    store => store.acompanhamentoFrequencia.exibirModalAnotacao
  );

  useEffect(() => {
    const obterAnotacao = async () => {
      if (exibirModalAnotacao && dadosModalAnotacao) {
        setCarregandoDados(true);
        const retorno = await ServicoAnotacaoFrequenciaAluno.obterAnotacaoPorId(
          dadosModalAnotacao.id
        ).catch(e => {
          erros(e);
          setCarregandoDados(false);
        });

        if (retorno?.data) {
          setDados(retorno.data);
          setCarregandoDados(false);
        }
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
      titulo={
        !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
          ? 'Anotações do Estudante'
          : 'Anotações da Criança'
      }
      onClose={onClose}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <Loader loading={carregandoDados}>
        {dados && (
          <>
            <DetalhesAluno
              className="mb-2"
              dados={dados?.aluno}
              exibirBotaoImprimir={false}
              permiteAlterarImagem={false}
            />
            <Card
              type="inner"
              className="rounded mt-3 mb-3"
              headStyle={{ borderBottomRightRadius: 0 }}
              bodyStyle={{ borderTopRightRadius: 0 }}
            >
              <strong>{dados?.motivoAusencia?.descricao}</strong>
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
          </>
        )}
      </Loader>
    </ModalConteudoHtml>
  );
};

export default ModalAnotacoesAcompanhamentoFrequencia;
