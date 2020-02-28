<template>
  <div class="paginator" v-if="count > 1">
    <CommonButton :disabled="page <= 1" :text="'1'" @click="select(1)"></CommonButton>
    <ArrowButton :disabled="page <= 1" class="left_arrow" @click="select(page - 1)"></ArrowButton>
    <DropDown ref="dropdown" dropup :value="current" @input="select" :list="pages"></DropDown>
    <ArrowButton :disabled="page >= count" class="right_arrow" @click="select(page + 1)"></ArrowButton>
    <CommonButton :disabled="page >= count" :text="count.toString()" @click="select(count)"></CommonButton>
  </div>
</template>

<style>
.paginator {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  /* padding: 24px 24px; */
  box-sizing: border-box;
  height: 64px;
  border-radius: 16px 16px 0 0;
  background-color: #503f96;
}

.paginator h5 {
  flex: 0 0 32px;
  width: 32px;
  text-align: center;
}
.paginator h5.selected,
.paginator h5:hover {
  color: #d49e35;
}
.paginator .dropdown,
.paginator .dropdown ul,
.paginator .dropdown .scrollable-content {
  width: 64px;
  box-sizing: border-box;
}
.paginator .dropdown ul {
  height: 192px;
  margin: 0 -1px;
}
.paginator .dropdown ul li {
  text-align: center;
}
.paginator .dropdown .common-button {
  display: none;
}
.paginator .dropdown .text-input {
  padding-left: 0;
  text-align: center;
  width: 100%;
}
.paginator .common-button {
  width: 64px;
  border-radius: 0;
}
.paginator .common-button:first-child {
  border-radius: 16px 0 0 0;
}
.paginator .common-button:last-child {
  border-radius: 0 16px 0 0;
}
.paginator .common-button.left_arrow {
  transform: rotate(90deg);
}
.paginator .common-button.right_arrow {
  transform: rotate(-90deg);
}
</style>

<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
import ArrowButton from '@/components/Common/Buttons/ArrowButton.vue';
import DropDown from '@/components/DropDown.vue';
export default Vue.extend({
  props: {
    page: Number,
    count: Number,
  },
  data() {
    return {
      current: '1',
    };
  },
  mounted() {
    const self = this;
    const int = setInterval(() => {
      if (self.$refs.dropdown) {
        (self.$refs.dropdown as any).findDisplayInList();
        clearInterval(int);
      }
    }, 100);
  },
  computed: {
    pages() {
      const pages = [];
      for (let i = 1; i <= this.count; i++) {
        const page = i.toString();
        pages.push({ value: page, title: page });
      }
      return pages;
    },
  },
  watch: {
    page() {
      this.current = this.page.toString();
    },
  },
  methods: {
    select(page: number | string) {
      if (typeof page === 'string') {
        page = parseInt(page, 10) || 0;
      }
      if (page > 0) {
        this.$emit('change', page);
      }
    },
  },
  components: {
    DropDown,
    CommonButton,
    ArrowButton,
  },
});
</script>
