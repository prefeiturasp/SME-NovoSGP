import React, { memo, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { Tooltip } from 'antd';
import { useDispatch, useSelector } from 'react-redux';

import { Auditoria, Button, Colors, JoditEditor } from '~/componentes';

import {
  confirmar,
  erros,
  ServicoRegistroIndividual,
  sucesso,
} from '~/servicos';

import {
  alterarRegistroAnterior,
  excluirRegistroAnteriorId,
  setRegistroAnteriorEmEdicao,
  setRegistroAnteriorId,
} from '~/redux/modulos/registroIndividual/actions';

import { RotasDto } from '~/dtos';

import { ContainerBotoes } from './item.css';

const Item = memo(({ dados, setCarregandoGeral }) => {
  const {
    alunoCodigo,
    auditoria,
    componenteCurricularId,
    data,
    id,
    registro,
    turmaId,
  } = dados;

  const [editando, setEditando] = useState(false);
  const [registroAlterado, setRegistroAlterado] = useState(registro);

  const { registroAnteriorEmEdicao, registroAnteriorId } = useSelector(
    store => store.registroIndividual
  );
  const { permissoes } = useSelector(state => state.usuario);
  const permissoesTela = permissoes[RotasDto.REGISTRO_INDIVIDUAL];

  const dispatch = useDispatch();

  const onChange = valorNovo => {
    setRegistroAlterado(valorNovo);
  };

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
      setCarregandoGeral(true);
      const retorno = await ServicoRegistroIndividual.deletarRegistroIndividual(
        {
          id: idEscolhido,
        }
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoGeral(false));

      if (retorno?.status === 200) {
        sucesso('Registro excluído com sucesso.');
        dispatch(excluirRegistroAnteriorId(idEscolhido));
      }
    }
  };

  const onClickEditar = async idEscolhido => {
    dispatch(setRegistroAnteriorEmEdicao(true));
    dispatch(setRegistroAnteriorId(idEscolhido));
  };

  const resetarInfomacoes = () => {
    setEditando(false);
    setRegistroAlterado(registro);
    dispatch(setRegistroAnteriorEmEdicao(false));
    dispatch(setRegistroAnteriorId({}));
  };

  const onClickCancelar = () => {
    resetarInfomacoes();
  };

  const onClickSalvar = async () => {
    setCarregandoGeral(true);
    const retorno = await ServicoRegistroIndividual.editarRegistroIndividual({
      id,
      turmaId,
      componenteCurricularId,
      alunoCodigo,
      registro: registroAlterado,
      data,
    })
      .catch(e => erros(e))
      .finally(() => setCarregandoGeral(false));

    if (retorno?.status === 200) {
      sucesso('Registro editado com sucesso.');
      dispatch(
        alterarRegistroAnterior({
          id,
          registro: registroAlterado,
          auditoria: retorno.data,
        })
      );
      resetarInfomacoes();
    }
  };

  useEffect(() => {
    const ehMesmoId = registroAnteriorId === id;
    if (ehMesmoId && !editando) {
      setEditando(true);
    }
  }, [registroAnteriorId, editando, id]);

  return (
    <div className="row justify-content-between">
      <div className="p-0 col-12">
        <JoditEditor
          validarSeTemErro={validarSeTemErro}
          mensagemErro="Campo obrigatório"
          label={`Registro - ${window.moment(data).format('DD/MM/YYYY')}`}
          id={`${id}-editor`}
          value={registroAlterado}
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
              onClick={onClickSalvar}
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
                  disabled={
                    (registroAnteriorEmEdicao && !editando) ||
                    (!editando && !permissoesTela.podeIncluir)
                  }
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
                  disabled={
                    (registroAnteriorEmEdicao && !editando) ||
                    (!editando && !permissoesTela.podeIncluir)
                  }
                />
              </span>
            </Tooltip>
          </div>
        )}
      </ContainerBotoes>
    </div>
  );
});

Item.propTypes = {
  dados: PropTypes.checkPropTypes[PropTypes.any],
  setCarregandoGeral: PropTypes.func,
};

Item.defaultProps = {
  dados: {},
  setCarregandoGeral: () => {},
};

export default Item;
