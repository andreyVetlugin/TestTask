<template>
  <div id="app">
    <LeftMenu v-if="isAuthorized"></LeftMenu>
    <div class="content">
      <router-view/>
    </div>
    <Loader></Loader>
    <RequestResultPopup @close="requestResult.showPopup = false" :requestResult="requestResult"></RequestResultPopup>
  </div>
</template>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  display: flex;
  height: 100%;
}

.content {
  flex: 1 3 100%;
  /* padding: 24px; */
  box-sizing: border-box;
  width: auto;
  height: 100%;
  overflow-y: hidden;
}
</style>

<script lang="ts">
import Vue from 'vue';
import LeftMenu from './components/LeftMenu.vue';
import Loader from './components/Loader.vue';
import RequestResultPopup from './components/Common/Popups/RequestResultPopup.vue';
import RequestSender from '@/requestSender';
export default Vue.extend({
  computed: {
    isAuthorized(): boolean {
      return this.$store.getters.isAuthorized;
    },
    requestResult(): any {
      return this.$store.getters.requestSender.requestResult;
    },
  },
  components: {
    RequestResultPopup,
    LeftMenu,
    Loader,
  },
});
</script>
