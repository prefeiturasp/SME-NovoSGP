import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Button, Colors, Label } from '~/componentes';
import LocalizadorFuncionario from '~/componentes-sgp/LocalizadorFuncionario';
import { RotasDto } from '~/dtos';
import { setLimparDadosAtribuicaoResponsavel } from '~/redux/modulos/collapseAtribuicaoResponsavel/actions';
import { erros, history, sucesso } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const SecaoDevolutivaResponsavel = () => {
  const [limparCampos, setLimparCampos] = useState(false);
  const [
    funcionarioLocalizadorSelecionado,
    setFuncionarioLocalizadorSelecionado,
  ] = useState();

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);
  const dadosAtribuicaoResponsavel = useSelector(
    store => store.planoAEE.dadosAtribuicaoResponsavel
  );

  const dispatch = useDispatch();

  const onChangeLocalizador = funcionario => {
    setLimparCampos(false);
    if (funcionario?.codigoRF && funcionario?.nomeServidor) {
      setFuncionarioLocalizadorSelecionado({
        codigoRF: funcionario?.codigoRF,
        nomeServidor: funcionario?.nomeServidor,
      });
      return;
    }
    setFuncionarioLocalizadorSelecionado();
    dispatch(setLimparDadosAtribuicaoResponsavel());
  };

  const onClickAtribuirResponsavel = async () => {
    const resposta = await ServicoPlanoAEE.atribuirResponsavel({
      planoAEEId: planoAEEDados.id,
      responsavelRF: funcionarioLocalizadorSelecionado.codigoRF,
    }).catch(e => erros(e));

    if (resposta?.data) {
      history.push(RotasDto.RELATORIO_AEE_PLANO);
      sucesso('Devolutiva registrada com sucesso');
    }
  };

  const onClickCancelar = () => {
    setFuncionarioLocalizadorSelecionado();
    dispatch(setLimparDadosAtribuicaoResponsavel());
    setLimparCampos(true);
  };

  return (
    <>
      <Label text="Responsável" className="mb-3" />
      <div className="row">
        <LocalizadorFuncionario
          id="funcionario"
          onChange={onChangeLocalizador}
          codigoTurma={planoAEEDados?.turma?.codigo}
          limparCampos={limparCampos}
          url="v1/encaminhamento-aee/responsavel/pesquisa"
          valorInicial={{
            codigoRF: dadosAtribuicaoResponsavel?.codigoRF,
          }}
          desabilitado={!!dadosAtribuicaoResponsavel?.codigoRF}
        />
      </div>
      <div className="col-12 d-flex justify-content-end pb-4 mt-2 pr-0">
        <Button
          id="btn-cancelar"
          label="Cancelar"
          color={Colors.Roxo}
          border
          className="mr-3"
          onClick={onClickCancelar}
          disabled={
            !!dadosAtribuicaoResponsavel?.codigoRF ||
            !funcionarioLocalizadorSelecionado?.codigoRF
          }
        />
        <Button
          id="btn-atribuir"
          label="Atribuir responsável"
          color={Colors.Roxo}
          border
          bold
          onClick={onClickAtribuirResponsavel}
          disabled={
            !!dadosAtribuicaoResponsavel?.codigoRF ||
            !funcionarioLocalizadorSelecionado?.codigoRF
          }
        />
      </div>
    </>
  );
};

export default SecaoDevolutivaResponsavel;
