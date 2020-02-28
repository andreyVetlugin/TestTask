<template>
  <div class="expandable_title">
    <BubbleButton noBackground></BubbleButton>
    <div v-if="title && title.length" class="expandable_title_title">
      <h4>{{title}}</h4>
    </div>
  </div>
</template>

<style>
.expandable_title {
  display: inline-block;
}
.expandable_title .common-button {
  width: 16px;
  height: 16px;
  padding: 0;
  margin-left: 8px;
}
.expandable_title_title {
  display: none;
  position: absolute;
  padding: 0;
  margin: 0;
  width: 240px;
  border-radius: 4px;
  border: 1px solid #6253a2;
  background-color: #503f96;
  z-index: 10;
}
.expandable_title_title h4 {
  margin: 24px;
}
.expandable_title:hover .expandable_title_title {
  display: block;
}
</style>

<script lang="ts">
import Vue from 'vue';
import BubbleButton from '@/components/Common/Buttons/BubbleButton.vue';
export default Vue.extend({
  props: {
    title: String,
  },
  data() {
    return {
      expanded: false,
    };
  },
  methods: {
    onblur() {
      setTimeout(() => {
        this.$data.expanded = false;
      }, 200);
    },
  },
  components: {
    BubbleButton,
  },
});
</script>
