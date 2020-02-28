<template>
  <button
    class="common-button"
    :class="{ 'no-text': !(text && text.length), 'no-background': noBackground }"
    @click="$emit('click')"
    v-html="text || html">
    <slot></slot>
  </button>
</template>

<style>
.common-button {
  outline: none;
  background-color: #503f96;
  border: 1px solid #503f96;
  border-radius: 4px;
  box-sizing: border-box;
  height: 64px;
  padding: 23px;
  font-size: 14pt;
  font-family: 'Rubik-Regular';
  color: #ffffff;
  line-height: 0;
  cursor: pointer;
}
.common-button.selected {
  border-color: #6254a3;
  background-color: #6254a3;
}

.common-button.delete-button {
  color: #ff5a20;
}
.common-button.delete-button:hover,
.common-button.delete-button:active {
  color: #ffffff;
}

.common-button:hover {
  border-color: #d49e35;
  background-color: #d49e35;
}

.common-button:active {
  border-color: #b7882e;
  background-color: #b7882e;
}

.common-button[disabled] {
  opacity: 0.5;
  background-color: #2c205e;
  border-color: #3a3066;
  color: #c6bcee;
  cursor: not-allowed;
}

.common-button.no-text {
  width: 64px;
  color: #f1b238;
}

.common-button.no-text:hover {
  color: #ffffff;
}

.common-button.no-text:active {
  color: #ffffff;
}

.common-button.no-text[disabled] {
  color: #503f96;
}

.common-button.no-background {
  border-color: transparent;
  background-color: transparent;
  color: #9084c3;
}

.common-button.no-background:hover {
  color: #d49e35;
}

.common-button.no-background:active {
  color: #b7882e;
}

.common-button.no-background[disabled] {
  color: #2c205d;
}

.common-button .fa {
  line-height: 0;
  text-align: center;
}
</style>


<script lang="ts">
import Vue from 'vue';
export default Vue.extend({
  props: {
    text: String,
    html: String,
    noBackground: Boolean,
  },
});
</script>
