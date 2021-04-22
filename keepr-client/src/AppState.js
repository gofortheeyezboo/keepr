import { reactive } from 'vue'

export const AppState = reactive({
  user: {},
  account: {},
  keeps: [],
  activeKeep: {},
  activeProfile: {},
  activeVault: {},
  vaults: []
})
