import { AppState } from '../AppState'
import { logger } from '../utils/Logger'
import { api } from './AxiosService'

class AccountService {
  async getAccount() {
    try {
      const res = await api.get('/account')
      AppState.account = res.data
    } catch (err) {
      logger.error('HAVE YOU STARTED YOUR SERVER YET???', err)
    }
  }

  async getKeepsByAccountId() {
    try {
      const res = await api.get('/account/keeps')
      AppState.keeps = res.data
    } catch (error) {
      logger.log(error)
    }
  }

  async getVaultsByAccountId() {
    try {
      const res = await api.get('/account/vaults')
      AppState.vaults = res.data
    } catch (error) {
      logger.log(error)
    }
  }
}

export const accountService = new AccountService()
