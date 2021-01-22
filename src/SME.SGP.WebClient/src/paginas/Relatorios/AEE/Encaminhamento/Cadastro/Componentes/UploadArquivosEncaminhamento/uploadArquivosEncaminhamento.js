import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import UploadArquivos from '~/componentes-sgp/UploadArquivos/uploadArquivos';
import { setEncaminhamentoAEEEmEdicao } from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros, sucesso } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import { Label } from '~/componentes';

const UploadArquivosEncaminhamento = props => {
  const { dados } = props;
  const { form, questaoAtual, label } = dados;

  const dispatch = useDispatch();

  const onRemoveFile = async arquivo => {
    const codigoArquivo = arquivo.xhr;

    if (arquivo.arquivoId) {
      if (
        form &&
        form?.setFieldValue &&
        form?.values?.[questaoAtual?.id]?.length
      ) {
        const novoMap = form?.values?.[questaoAtual?.id];
        const indice = novoMap.findIndex(
          item => arquivo.arquivoId === item.arquivoId
        );

        if (indice !== -1) {
          novoMap.splice(indice, 1);

          form.setFieldValue(String(questaoAtual?.id), novoMap);
          form.setFieldTouched(String(questaoAtual?.id), true);
          sucesso(`Arquivo ${arquivo.name} removido com sucesso`);
        }
      }
      return true;
    }

    const resposta = await ServicoEncaminhamentoAEE.removerArquivo(
      codigoArquivo
    ).catch(e => erros(e));

    if (resposta && resposta.status === 200) {
      sucesso(`Arquivo ${arquivo.name} removido com sucesso`);
      return true;
    }
    return false;
  };

  return (
    <>
      <div className="col-md-12 mt-2">
        {questaoAtual?.nome ? <Label text={label} /> : ''}
        <UploadArquivos
          form={form}
          name={String(questaoAtual?.id)}
          id={String(questaoAtual?.id)}
          tiposArquivosPermitidos={questaoAtual.opcionais || ''}
          desabilitarUpload={form?.values?.[questaoAtual?.id]?.length > 9}
          onRemove={onRemoveFile}
          urlUpload="v1/encaminhamento-aee/upload"
          defaultFileList={
            form?.values?.[questaoAtual?.id]?.length
              ? form?.values?.[questaoAtual?.id]
              : []
          }
          onChangeListaArquivos={() => {
            dispatch(setEncaminhamentoAEEEmEdicao(true));
          }}
        />
      </div>
    </>
  );
};

UploadArquivosEncaminhamento.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
};

UploadArquivosEncaminhamento.defaultProps = {
  dados: {},
};

export default UploadArquivosEncaminhamento;
