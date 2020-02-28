<template>
  <h4 class="checkbox" :class="{ selected: value }" @click="select">
    <span v-if="single" class="fa"></span>
    <i v-else class="fa fa-check"></i>
    {{title}}
  </h4>
</template>

<style>
.checkbox {
  display: inline-block;
  width: 240px;
  flex: 0 0 100%;
}

.checkbox .fa {
  color: transparent;
  font-size: 10px;
  padding: 4px;
  margin: 2px 8px 2px 0;
  height: 18px;
  width: 18px;
  border-radius: 50%;
  background-color: #503f96;
  box-sizing: border-box;
}
.checkbox:hover .fa {
  background-color: #8a7ec0;
}
.checkbox:active .fa {
  background-color: #6253a2;
  color: #ffffff;
}
.checkbox.selected .fa-check {
  color: #ffffff;
}
.checkbox.selected span.fa {
  background-color: #ffffff;
}
</style>

<script lang="ts">
import Vue from 'vue';
export default Vue.extend({
  props: {
    value: Boolean,
    title: String,
    single: Boolean,
  },
  methods: {
    select() {
      this.$emit('input', !this.value);
    },
  },
});
</script>
