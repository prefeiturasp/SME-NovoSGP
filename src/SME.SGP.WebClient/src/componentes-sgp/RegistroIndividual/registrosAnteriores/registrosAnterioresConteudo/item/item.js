import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Auditoria, Button, Colors, JoditEditor } from '~/componentes';
import {
  alterarRegistroAnterior,
  excluirRegistroAnteriorId,
  setRegistroAnteriorEmEdicao,
  setRegistroAnteriorId,
} from '~/redux/modulos/registroIndividual/actions';
import {
  confirmar,
  erros,
  ServicoRegistroIndividual,
  sucesso,
} from '~/servicos';
import { ContainerBotoes } from './item.css';

const Item = ({ dados, setCarregandoGeral, permissoesTela, podeEditar }) => {
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
  const [mostrarBotoes, setMostrarBotoes] = useState(false);
  const [registroAlterado, setRegistroAlterado] = useState();

  const registroAnteriorEmEdicao = useSelector(
    store => store.registroIndividual.registroAnteriorEmEdicao
  );
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );
  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);

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

  const onClickEditar = async () => {
    dispatch(setRegistroAnteriorEmEdicao(true));
    setEditando(true);
  };

  const resetarInfomacoes = texto => {
    setEditando(false);
    setRegistroAlterado(texto);
    dispatch(setRegistroAnteriorEmEdicao(false));
    dispatch(setRegistroAnteriorId({}));
  };

  const onClickCancelar = () => {
    resetarInfomacoes(registro);
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
      const dadosPraSalvar = {
        id,
        registro: registroAlterado,
        auditoria: retorno.data,
      };
      dispatch(alterarRegistroAnterior(dadosPraSalvar));
      setRegistroAlterado(registroAlterado);
      resetarInfomacoes(registroAlterado);
    }
  };

  useEffect(() => {
    if (registro) {
      setRegistroAlterado(registro);
    }
  }, [registro]);

  useEffect(() => {
    if (!podeEditar) {
      setMostrarBotoes(false);
      return;
    }

    if (
      permissoesTela.podeIncluir &&
      !dadosAlunoObjectCard.desabilitado &&
      !turmaSelecionada.consideraHistorico
    ) {
      setMostrarBotoes(true);
      return;
    }
    setMostrarBotoes(false);
  }, [
    dadosAlunoObjectCard.desabilitado,
    permissoesTela.podeIncluir,
    turmaSelecionada.consideraHistorico,
    podeEditar,
  ]);

  return (
    <div className="row justify-content-between">
      <div className="p-0 col-12" style={{ minHeight: 200 }}>
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
      {mostrarBotoes && (
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
                    onClick={onClickEditar}
                    height="30px"
                    width="30px"
                    disabled={registroAnteriorEmEdicao && !editando}
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
                    disabled={registroAnteriorEmEdicao && !editando}
                  />
                </span>
              </Tooltip>
            </div>
          )}
        </ContainerBotoes>
      )}
    </div>
  );
};

Item.propTypes = {
  dados: PropTypes.instanceOf(Object),
  setCarregandoGeral: PropTypes.func,
  permissoesTela: PropTypes.instanceOf(Object),
  podeEditar: PropTypes.bool,
};

Item.defaultProps = {
  dados: {},
  setCarregandoGeral: () => {},
  permissoesTela: {},
  podeEditar: true,
};

export default Item;
