import { AppState } from '../AppState'
import { api } from './AxiosService'

class KeepService {
  async getKeeps() {
    try {
      const res = await api.get('api/keeps')
      console.log(res)
      AppState.keeps = res.data
    } catch (error) {
      console.error(error)
    }
  }

  async getKeep(id) {
    try {
      const res = await api.get('api/keeps/' + id)
      AppState.activeKeep = res.data
    } catch (error) {
      console.error(error)
    }
  }

  async getKeepsByVaultId(id) {
    try {
      const res = await api.get('api/vaults/' + id + '/keeps')
      AppState.keeps = res.data
    } catch (error) {
      console.error(error)
    }
  }

  async createKeep(keep) {
    await api.post('api/keeps', keep)
    // this.getKeeps()
  }

  async deleteKeep(id) {
    await api.delete('api/keeps/' + id)
    // this.getKeeps()
  }

  async incrementKeeps(id, keep) {
    await api.put('api/keeps/' + id, keep)
  }

  async incrementViews(id, keep) {
    await api.put('api/keeps/' + id, keep)
  }
}
export const keepService = new KeepService()
