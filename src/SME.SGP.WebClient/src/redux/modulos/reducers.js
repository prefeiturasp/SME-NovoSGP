import { combineReducers } from 'redux';

import notificacoes from './alertas/reducers';
import bimestres from './planoAnual/reducers';

export default combineReducers({
  notificacoes, bimestres
});
