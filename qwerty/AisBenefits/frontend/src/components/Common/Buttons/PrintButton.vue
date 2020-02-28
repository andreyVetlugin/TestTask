<template>
  <CommonButton
    :html="'<span class=\'print_button\'></span>'"
    :noBackground="noBackground"
    @click="$emit('click')"
  ></CommonButton>
</template>

<style>
.common-button .print_button {
  width: 16px;
  height: 16px;
  display: inline-block;
  background-image: url("/images/print_16.png");
}
.common-button:hover .print_button,
.common-button:active .print_button {
  background-image: url("/images/print_16_white.png");
}
</style>


<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
export default Vue.extend({
  props: {
    noBackground: Boolean,
  },
  components: {
    CommonButton,
  },
});
</script>
