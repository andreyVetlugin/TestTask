<template>
  <div class="scrollable-container" :class="{ 'scroll-hidden': hideScroll }" @mousewheel="onwheel">
    <div class="scrollable-content" :style="{ top: contentTop + 'px' }">
      <slot></slot>
    </div>
    <div class="scrollbar" @mousedown.prevent @mousemove="ondrag">
      <div v-if="canScroll" class="scroller" :style="{ top: scrollerTop + 'px' }"></div>
      <div v-if="canScroll" class="scrollline"></div>
    </div>
  </div>
</template>

<style>
.scrollable-container {
  overflow: hidden;
}
.scrollable-content {
  position: relative;
  display: inline-block;
  width: calc(100% - 36px);
  transition: top 0.3s linear;
}
.scroller {
  z-index: 2;
  width: 12px;
  height: 16px;
  margin: 2px auto;
  border-radius: 8px;
  position: relative;
  background-color: #503f96;
  -webkit-transition: top 0.3s linear;
  transition: top 0.3s linear;
}
.scrollbar {
  display: inline-block;
  position: relative;
  vertical-align: top;
  border: 1px solid transparent;
  border-radius: 8px;
  width: 0px;
  width: 34px;
  height: calc(100% - 16px);
}
.scrollline {
  display: inline-block;
  position: absolute;
  vertical-align: top;
  margin: 0 16px;
  border: 1px solid #5d4ca3;
  width: 0px;
  height: 100%;
  top: 0;
}
.scroll-hidden .scrollable-content {
  width: 100%;
}
.scroll-hidden .scrollline,
.scroll-hidden .scrollbar,
.scroll-hidden .scroller {
  display: none;
}
</style>

<script lang="ts">
// TODO отрефачить
import Vue from 'vue';
export default Vue.extend({
  props: {
    hideScroll: Boolean,
  },
  data() {
    return {
      canScroll: false, // если контента мало
      contentTop: 0,
      scrollerTop: 0,
    };
  },
  mounted() {
    this.canScroll =
      (this.$el.offsetHeight -
        (this.$el.getElementsByClassName('scrollable-content')[0] as HTMLElement).offsetHeight) < 0;
  },
  updated() {
    this.canScroll =
      (this.$el.offsetHeight -
        (this.$el.getElementsByClassName('scrollable-content')[0] as HTMLElement).offsetHeight) < 0;
  },
  watch: {
    contentTop() {
      this.scrollerTop = (this.$el && this.$el.children && this.$el.children.length)
        ? this.contentTop / (
          this.$el.offsetHeight - (this.$el.children[0] as HTMLElement).offsetHeight
        ) * (this.$el.offsetHeight - 38)
        : 0;
    },
    canScroll(val) {
      if (!val) {
        this.contentTop = 0;
      }
    },
  },
  methods: {
    onwheel(e: WheelEvent) {
      e.preventDefault();
      e.stopPropagation();
      if (!this.canScroll) {
        return;
      }

      this.$emit('mousewheel', e);

      const offset = this.$el.offsetHeight - (this.$el.children[0] as HTMLElement).offsetHeight;
      this.contentTop = Math.min(Math.max(this.contentTop - e.deltaY, offset), 0);
    },
    ondrag(e: MouseEvent) {
      if (e.buttons !== 1) {
        return;
      }
      const target = (e.target as HTMLElement);
      if (target && !target.classList.contains('scroller')) {
        const offset = this.$el.offsetHeight - (this.$el.children[0] as HTMLElement).offsetHeight;
        this.contentTop = Math.min(offset * e.layerY / target.offsetHeight, 0);
      }
    },
  },
});
</script>
