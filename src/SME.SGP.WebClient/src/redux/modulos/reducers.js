import { combineReducers } from 'redux';

import notificacoes from './alertas/reducers';

export default combineReducers({
  notificacoes,
});
