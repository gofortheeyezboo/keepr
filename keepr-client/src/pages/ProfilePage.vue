<template>
  <div class="about text-left">
    <div class="row mt-3" v-if="!state.loading && state.profile">
      <img class="rounded mt-3 ml-4" :src="state.profile.picture" alt="" />
      <div class="col">
        <h1>Welcome {{ state.profile.name }}</h1>
        <h1>Vaults: {{ state.vaults.length }} </h1>
        <h1>Keeps: {{ state.keeps.length }} </h1>
      </div>
    </div>
    <div class="row">
      <h1 class="ml-4 mt-4">
        Vaults
      </h1>
    </div>
    <div class="row" v-if="!state.loading && state.vaults">
      <VaultComponent v-for="v in state.vaults" :key="v.id" :vault-prop="v" />
    </div>
    <div class="row">
      <h1 class="ml-4">
        Keeps
      </h1>
    </div>
    <div class="row" v-if="!state.loading && state.keeps">
      <KeepComponent v-for="k in state.keeps" :key="k.id" :keep-prop="k" />
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive } from 'vue'
import { useRoute } from 'vue-router'
import { AppState } from '../AppState'
import { profileService } from '../services/ProfileService'
export default {
  name: 'Profile',
  setup() {
    const route = useRoute()
    const state = reactive({
      account: computed(() => AppState.account),
      user: computed(() => AppState.user),
      profile: computed(() => AppState.activeProfile),
      keeps: computed(() => AppState.keeps),
      vaults: computed(() => AppState.vaults),
      loading: true
    })
    onMounted(() => {
      profileService.getKeepsByProfileId(route.params.id)
      state.loading = false
    })
    onMounted(() => {
      profileService.getVaultsByProfileId(route.params.id)
    })
    onMounted(() => {
      profileService.getProfileById(route.params.id)
    })
    return {
      state,
      route
    }
  }
}
</script>

<style scoped>
img {
  max-width: 100px;
}
</style>
