import React, { memo, useCallback, useState } from 'react';
import { Tooltip } from 'antd';
import { useDispatch, useSelector } from 'react-redux';

import { Auditoria, Button, Colors, Editor } from '~/componentes';

import {
  confirmar,
  erros,
  ServicoRegistroIndividual,
  sucesso,
} from '~/servicos';

import {
  excluirRegistroAnteriorId,
  setRegistroAnteriorEmEdicao,
} from '~/redux/modulos/registroIndividual/actions';

import { ContainerBotoes } from './item.css';

const Item = memo(({ dados }) => {
  const [editando, setEditando] = useState(false);
  // const {
  //   dadosPrincipaisRegistroIndividual,
  //   registroAnteriorEmEdicao,
  // } = useSelector(store => store.registroIndividual);

  const { id, auditoria, data, registro } = dados;
  const dispatch = useDispatch();

  const onChange = useCallback(
    valorNovo => {
      // TODO Verificar para salvar dados editados no redux separada do atual para melhorar a performance!
      // const dados = { ...dadosBimestrePlanoAnual };
      // dados.componentes.forEach(item => {
      //   if (
      //     String(item.componenteCurricularId) ===
      //     String(tabAtualComponenteCurricular.codigoComponenteCurricular)
      //   ) {
      //     item.descricao = valorNovo;
      //     item.emEdicao = true;
      //   }
      // });
      // dispatch(setDadosBimestresPlanoAnual(dados));
      dispatch(setRegistroAnteriorEmEdicao(false));
    },
    [dispatch]
  );

  const validarSeTemErro = valorEditado => {
    return !valorEditado;
  };

  const onClickExcluir = async idEscolhido => {
    const confirmado = await confirmar(
      'Excluir',
      '',
      'Você tem certeza que deseja excluir este registro?'
    );

    if (confirmado) {
      const retorno = await ServicoRegistroIndividual.deletarRegistroIndividual(
        {
          id: idEscolhido,
        }
      ).catch(e => erros(e));

      if (retorno?.status === 200) {
        sucesso('Ocorrência excluída com sucesso.');
        dispatch(excluirRegistroAnteriorId(idEscolhido));
      }
    }
  };

  // console.log('emEdicao Aqui=======> ', emEdicao);

  const onClickEditar = useCallback(
    async idEscolhido => {
      setEditando(true);
      dispatch(setRegistroAnteriorEmEdicao(true));
    },
    [dispatch]
  );

  const onClickCancelar = useCallback(() => {
    setEditando(false);
  }, []);

  return (
    <div className="row justify-content-between">
      <div className="p-0 col-12">
        <Editor
          validarSeTemErro={validarSeTemErro}
          mensagemErro="Campo obrigatório"
          label={`Registro - ${window.moment(data).format('DD/MM/YYYY')}`}
          id={`${id}-editor`}
          inicial={registro}
          onChange={onChange}
          desabilitar={!editando}
        />
      </div>
      {auditoria && (
        <div className="mt-1 ml-n3 mb-2">
          <Auditoria
            ignorarMarginTop
            criadoEm={auditoria.criadoEm}
            criadoPor={auditoria.criadoPor}
            criadoRf={auditoria.criadoRF}
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
            alteradoRf={auditoria.alteradoRF}
          />
        </div>
      )}
      <ContainerBotoes className="d-flex">
        {editando ? (
          <div className="d-flex mt-2">
            <Button
              id="btn-cancelar-obs-novo"
              label="Cancelar"
              color={Colors.Roxo}
              border
              bold
              className="mr-3"
              onClick={onClickCancelar}
              height="30px"
            />
            <Button
              id="btn-salvar-obs-novo"
              label="Salvar"
              color={Colors.Roxo}
              border
              bold
              // onClick={onClickSalvar}
              height="30px"
            />
          </div>
        ) : (
          <div className="d-flex mt-2">
            <Tooltip title="Editar">
              <span>
                <Button
                  id="btn-editar"
                  icon="edit"
                  iconType="far"
                  color={Colors.Azul}
                  border
                  className="btn-acao mr-2"
                  onClick={() => onClickEditar(id)}
                  height="30px"
                  width="30px"
                  // disabled={registroAnteriorEmEdicao && !editando}
                />
              </span>
            </Tooltip>
            <Tooltip title="Excluir">
              <span>
                <Button
                  id="btn-excluir"
                  icon="trash-alt"
                  iconType="far"
                  color={Colors.Azul}
                  border
                  className="btn-acao"
                  onClick={() => onClickExcluir(id)}
                  height="30px"
                  width="30px"
                  // disabled={registroAnteriorEmEdicao && !editando}
                />
              </span>
            </Tooltip>
          </div>
        )}
      </ContainerBotoes>
    </div>
  );
});

export default Item;
