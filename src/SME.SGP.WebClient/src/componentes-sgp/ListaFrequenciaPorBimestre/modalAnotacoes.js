import { Card } from 'antd';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { Colors, DetalhesAluno, ModalConteudoHtml } from '~/componentes';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import SelectComponent from '~/componentes/select';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
} from '~/redux/modulos/listaFrequenciaPorBimestre/actions';
import usuario from '~/redux/modulos/usuario/reducers';
import { ehTurmaInfantil, erros } from '~/servicos';
import ServicoAnotacaoFrequenciaAluno from '~/servicos/Paginas/DiarioClasse/ServicoAnotacaoFrequenciaAluno';

const ModalAnotacoes = () => {
  const dispatch = useDispatch();
  const { turmaSelecionada } = usuario;
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const [dados, setDados] = useState();

  const dadosModalAnotacao = useSelector(
    store => store.listaFrequenciaPorBimestre.dadosModalAnotacao
  );

  const exibirModalAnotacao = useSelector(
    store => store.listaFrequenciaPorBimestre.exibirModalAnotacao
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
        setDados();
      }
    };
    obterAnotacao();
  }, [dadosModalAnotacao, exibirModalAnotacao]);

  const onClose = () => {
    dispatch(setDadosModalAnotacao());
    dispatch(setExibirModalAnotacao(false));
  };
  return dados && dadosModalAnotacao?.id ? (
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
      <div className="row">
        <div className="col-md-12 mb-2">
          <DetalhesAluno
            className="mb-2"
            dados={dados?.aluno}
            exibirBotaoImprimir={false}
            exibirFrequencia={false}
            permiteAlterarImagem={false}
          />
        </div>
        <div className="col-md-12 mb-2">
          <SelectComponent
            id="motivo-ausencia"
            lista={dados?.motivoAusencia?.id ? [dados?.motivoAusencia] : []}
            valueOption="id"
            valueText="descricao"
            disabled
            valueSelect={
              dados?.motivoAusenciaId
                ? String(dados?.motivoAusenciaId)
                : undefined
            }
          />
        </div>
        <div className="col-md-12">
          <JoditEditor value={dados?.anotacao} readonly removerToolbar />
        </div>
        <div className="mb-2">
          {dados?.auditoria?.criadoPor ? (
            <Auditoria
              criadoPor={dados.auditoria.criadoPor}
              criadoEm={dados.auditoria?.criadoEm}
              alteradoPor={dados.auditoria?.alteradoPor}
              alteradoEm={dados.auditoria?.alteradoEm}
              alteradoRf={dados.auditoria?.alteradoRF}
              criadoRf={dados.auditoria?.criadoRF}
            />
          ) : (
            ''
          )}
        </div>
      </div>
      <div className="row">
        <div className="col-md-12 d-flex justify-content-end">
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
      </div>
    </ModalConteudoHtml>
  ) : (
    ''
  );
};

export default ModalAnotacoes;
