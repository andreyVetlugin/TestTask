<template>
  <div v-show="shown" class="popup-overlay">
    <div class="popup-background" @click.self="close"></div>
    <div class="popup shadow_medium">
      <div class="popup-header">
        <CrossButton noBackground @click="close"></CrossButton>
      </div>
      <h2>{{title}}</h2>
      <div class="popup-content">
        <slot></slot>
      </div>
    </div>
  </div>
</template>

<style>
.popup-background {
  position: absolute;
  background-color: black;
  opacity: 0.5;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: 77;
}
.popup-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: 78;
}
.popup {
  position: absolute;
  top: 160px;
  left: calc(50% - 320px);
  right: calc(50% - 320px);
  background-color: #3f3176;
  border: 1px solid #493c80;
  border-radius: 4px;
  box-sizing: border-box;
  z-index: 79;
  width: 706px;
  min-height: 476px;
}
.popup-header {
  display: flex;
  width: 100%;
  height: 64px;
  justify-content: flex-end;
  align-items: center;
}
.popup h2 {
  margin: 16px 0;
  text-align: center;
  min-height: 26px;
}
.popup-content > h3,
.popup-content > h4,
.popup-content > h5 {
  margin: 16px 0 0;
}
.popup-content {
  padding: 0 32px 80px;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  justify-items: center;
  align-content: space-between;
  align-items: center;
  min-height: 354px;
  box-sizing: border-box;
}
.popup-content > .common-button {
  color: #fff;
  margin-top: 40px;
}
.popup-content .dropdown,
.popup-content .labeled-input {
  margin-top: 16px;
}

.popup-content .popup_br {
  width: 100%;
}
</style>

<script lang="ts">
import Vue from 'vue';
import CrossButton from '@/components/Common/Buttons/CrossButton.vue';
export default Vue.extend({
  props: {
    shown: Boolean,
    title: String,
  },
  data() {
    return {};
  },
  methods: {
    close() {
      this.$emit('close');
    },
  },
  components: {
    CrossButton,
  },
});
</script>
