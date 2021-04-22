<template>
  <div class="col-4 card bg-dark text-white hover">
    <router-link :to="{ name: 'Vault', params: { id: vaultProp.id } }">
      <div class="card-body">
        <h5 class="card-title text-center text-white" style="position:absolute;bottom:0">
          {{ vaultProp.name }}
        </h5>
      </div>
    </router-link>
  </div>
</template>

<script>
import { reactive } from 'vue'
export default {
  name: 'VaultComponent',
  props: {
    vaultProp: {
      type: Object,
      required: true
    }
  },
  setup() {
    const state = reactive({
    })
    return { state }
  }
}
</script>

<style scoped>
.hover{
  cursor: pointer;
}
</style>
