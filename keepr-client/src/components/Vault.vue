<template>
  <div class="card bg-dark text-white hover">
    <router-link :to="{ name: 'Vault', params: { id: vaultProp.id } }">
      <div class="card-body">
        <img class="card-img mb-4" src="https://res.cloudinary.com/teepublic/image/private/s--cuZ-0nFl--/c_crop,x_10,y_10/c_fit,h_830/c_crop,g_north_west,h_1038,w_1038,x_-104,y_-104/l_upload:v1565806151:production:blanks:vdbwo35fw6qtflw9kezw/fl_layer_apply,g_north_west,x_-215,y_-215/b_rgb:343d76/c_limit,f_jpg,h_630,q_90,w_630/v1542632140/production/designs/3534359_0.jpg" alt="Card image">
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
