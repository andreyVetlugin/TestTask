<template>
  <Popup
    :shown="requestResult.showPopup"
    @close="close"
    class="request_result"
    :class="{ error_popup: !requestResult.success }"
    :title="title"
  >
    <template>
      <h2 class="request_message" v-for="(message, i) in messages" :key="i">{{message}}</h2>
      <CommonButton @click="close" text="ОК"></CommonButton>
    </template>
  </Popup>
</template>

<style>
.request_result .popup-background {
  z-index: 87;
}
.request_result.popup-overlay {
  z-index: 88;
}
.request_result .popup {
  z-index: 89;
}
.request_result .popup-content {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}
.request_result .request_message {
  flex: 0 0 100%;
  text-align: center;
  margin: 0 0 24px;
  /* color: darkgreen; */
}
.request_result .popup-content .common-button {
  width: 160px;
}
.request_result.error_popup .request_message {
  color: red;
}
</style>

<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
import Popup from '@/components/Popup.vue';
export default Vue.extend({
  props: {
    requestResult: Object,
  },
  data() {
    return {
      title: '',
    };
  },
  computed: {
    messages(): string[] {
      return typeof this.requestResult.message === 'string'
      ? [this.requestResult.message]
      : this.requestResult.message;
    },
  },
  methods: {
    close() {
      this.$emit('close');
    },
  },
  components: {
    CommonButton,
    Popup,
  },
});
</script>
