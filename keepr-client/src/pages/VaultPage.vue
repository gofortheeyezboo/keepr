<template>
  <div class="Vault row ml-2">
    <h1 v-if="state.vault.creator && !state.loading && state.user.email">
      {{ state.vault.name }} <i v-if="state.vault.creator.email == state.user.email && state.loading == false" class="fa fa-trash text-danger hover" aria-hidden="true" @click="deleteVault"></i>
    </h1>
    <h5 class="m-2">Keeps: {{ state.keeps.length }} </h5>
    <div class="row">
      <KeepComponent v-for="k in state.keeps" :key="k.id" :keep-prop="k" />
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { AppState } from '../AppState'
import { keepService } from '../services/KeepService'
import { vaultService } from '../services/VaultService'
export default {
  name: 'VaultPage',
  setup() {
    const router = useRouter()
    const route = useRoute()
    const state = reactive({
      account: computed(() => AppState.account),
      user: computed(() => AppState.user),
      keeps: computed(() => AppState.keeps),
      vault: computed(() => AppState.activeVault),
      loading: true
    })
    onMounted(() => {
      vaultService.getVaultById(route.params.id)
      state.loading = false
    })
    onMounted(() => {
      keepService.getKeepsByVaultId(route.params.id)
      state.loading = false
    })
    return {
      state,
      route,
      router,
      deleteVault() {
        if (window.confirm('Are You Sure?')) {
          vaultService.deleteVault(state.vault.id)
          router.push({ name: 'Home' })
        }
      }
    }
  }
}
</script>
<style scoped>
.hover{
  cursor: pointer;
}
</style>
