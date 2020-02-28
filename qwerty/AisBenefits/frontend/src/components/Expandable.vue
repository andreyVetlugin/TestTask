<template>
  <div @click="onclick" :class="{expanded: value}">
    <slot name="title"></slot>
    <div class="expandable-content">
      <slot name="content"></slot>
    </div>
  </div>
</template>

<style>
.expandable-content {
  display: none;
}
.expanded > .expandable-content {
  display: initial;
}
</style>


<script lang="ts">
import Vue from 'vue';
export default Vue.extend({
  props: {
    value: Boolean,
  },
  methods: {
    onclick(e: any) {
      e.preventDefault();
      e.stopPropagation();
      if (!e.path.find((el: any) => el && el.classList && el.classList.contains('expandable-content'))) {
        this.$emit('input', !this.value);
      }
    },
  },
});
</script>
